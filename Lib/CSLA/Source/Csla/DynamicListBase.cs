﻿//-----------------------------------------------------------------------
// <copyright file="DynamicListBase.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>This is the base class from which collections</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Csla.Core;
using Csla.Reflection;
using Csla.Serialization;
using Csla.Serialization.Mobile;

namespace Csla
{
  /// <summary>
  /// This is the base class from which collections
  /// of editable root business objects should be
  /// derived.
  /// </summary>
  /// <typeparam name="T">
  /// Type of editable root object to contain within
  /// the collection.
  /// </typeparam>
  /// <remarks>
  /// <para>
  /// Your subclass should implement a factory method
  /// and should override or overload
  /// DataPortal_Fetch() to implement data retrieval.
  /// </para><para>
  /// Saving (inserts or updates) of items in the collection
  /// should be handled through the SaveItem() method on
  /// the collection. 
  /// </para><para>
  /// Removing an item from the collection
  /// through Remove() or RemoveAt() causes immediate deletion
  /// of the object, by calling the object's Delete() and
  /// Save() methods.
  /// </para>
  /// </remarks>
  [Serializable()]
  public abstract class DynamicListBase<T> :
#if SILVERLIGHT
    ExtendedBindingList<T>,
#else
    ObservableBindingList<T>,
#endif
    Core.IParent, Csla.Server.IDataPortalTarget, ISerializationNotification
    where T : Core.IEditableBusinessObject, Core.IUndoableObject, Core.ISavable, IMobileObject
  {
    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    public DynamicListBase()
    {
      Initialize();
      AllowNew = true;
    }

    #region Initialize

    /// <summary>
    /// Override this method to set up event handlers so user
    /// code in a partial class can respond to events raised by
    /// generated code.
    /// </summary>
    protected virtual void Initialize()
    { /* allows subclass to initialize events before any other activity occurs */ }

    #endregion

    #region  SaveItem Methods

    /// <summary>
    /// Event raised when an object in the list has been saved.
    /// </summary>
    public event EventHandler<Csla.Core.SavedEventArgs> Saved;

    /// <summary>
    /// Raises the Saved event.
    /// </summary>
    /// <param name="newObject">Object returned as a result
    /// of the save operation.</param>
    /// <param name="error">Exception returned as a result
    /// of the save operation.</param>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected void OnSaved(T newObject, Exception error)
    {
      if (Saved != null)
        Saved(this, new SavedEventArgs(newObject, error, null));
    }

    /// <summary>
    /// Saves the specified item in the list.
    /// </summary>
    /// <param name="item">
    /// Reference to the item to be saved.
    /// </param>
    /// <remarks>
    /// This method properly saves the child item,
    /// by making sure the item in the collection
    /// is properly replaced by the result of the
    /// Save() method call.
    /// </remarks>
    public void SaveItem(T item)
    {
      SaveItem(IndexOf(item));
    }

    /// <summary>
    /// Saves the specified item in the list.
    /// </summary>
    /// <param name="index">
    /// Index of the item to be saved.
    /// </param>
    /// <remarks>
    /// This method properly saves the child item,
    /// by making sure the item in the collection
    /// is properly replaced by the result of the
    /// Save() method call.
    /// </remarks>
    public virtual void SaveItem(int index)
    {

      T item = this[index];
      var handleBusy = false;
      if (item.IsDeleted || (item.IsValid && item.IsDirty))
      {
        T savable = item;

        // attempt to clone object
        ICloneable cloneable = savable as ICloneable;
        if (cloneable != null)
        {
          savable = (T)cloneable.Clone();
          MethodCaller.CallMethodIfImplemented(item, "MarkBusy");
          handleBusy = true;
        }

        // commit all changes
        int editLevel = savable.EditLevel;
        for (int tmp = 1; tmp <= editLevel; tmp++)
          savable.AcceptChanges(editLevel - tmp, false);

        // save object
        DataPortal<T> dp = new DataPortal<T>();
        dp.UpdateCompleted += (o, e) =>
        {
          if (handleBusy)
            MethodCaller.CallMethodIfImplemented(item, "MarkIdle");
          if (e.Error == null)
          {
            T result = e.Object;
            if (item.IsDeleted)
            {
              //SafeRemoveItem  will raise INotifyCollectionChanged event
              SafeRemoveItem(index);
            }
            else
            {
              for (int tmp = 1; tmp <= editLevel; tmp++)
                result.CopyState(tmp, false);

              SafeSetItem(index, result);
#if SILVERLIGHT
              //Because SL Data Grid does not support replace action.
              // we have to artificially raise remove/insert events
              OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
              OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this[index], index));
#else
              OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, result, item));
#endif
            }
#if SILVERLIGHT
            item.SaveComplete(result, null, null);
#else
            item.SaveComplete(result);
#endif
            OnSaved(result, null);
          }
          else
          {
#if SILVERLIGHT
            item.SaveComplete(item, e.Error, null);
#else
            item.SaveComplete(item);
#endif
            OnSaved(item, e.Error);
          }
        };
        dp.BeginUpdate(savable);
      }
    }

    private void SafeSetItem(int index, T newObject)
    {
      //This is needed because we cannot call base.SetItem from lambda expression
      newObject.SetParent(this);
      base.SetItem(index, newObject);
    }

    private void SafeRemoveItem(int index)
    {
      this[index].SetParent(null);
      base.RemoveItem(index);
    }

    #endregion

    #region  Insert, Remove, Clear

#if SILVERLIGHT
    /// <summary>
    /// Adds a new item to the list.
    /// Uses ProcyModes.LocalOnly as default. 
    /// Override in your own class to use other ProxyModes
    /// </summary>
    protected override void AddNewCore()
    {
      var portal = new Csla.DataPortal<T>(DataPortal.ProxyModes.LocalOnly);
      portal.CreateCompleted += (o, e) =>
      {
        // call OnUnhandledAsyncException if failed
        if (e.Error != null)
        {
          OnUnhandledAsyncException(new ErrorEventArgs(this, e.Error));
        }
        else
        {
          try
          {
            this.Add(e.Object);
            OnAddedNew(e.Object);
          }
          catch (Exception ex)
          {
            OnUnhandledAsyncException(new ErrorEventArgs(this, ex));
          }
        }
      };

      portal.BeginCreate();
    }
#else
    /// <summary>
    /// Adds a new item to the list.
    /// </summary>
    /// <returns>The added object</returns>
    protected override T AddNewCore()
    {
      T item = Csla.DataPortal.Create<T>();
      Add(item);
      return item;
    }
#endif

    /// <summary>
    /// Gives the new object a parent reference to this
    /// list.
    /// </summary>
    /// <param name="index">Index at which to insert the item.</param>
    /// <param name="item">Item to insert.</param>
    protected override void InsertItem(int index, T item)
    {
      item.SetParent(this);
      base.InsertItem(index, item);
    }

    /// <summary>
    /// Removes an item from the list.
    /// </summary>
    /// <param name="index">Index of the item
    /// to be removed.</param>
    protected override void RemoveItem(int index)
    {
      T item = this[index];
      if (item.IsDeleted == false)
      {
        // only delete/save the item if it is not new
        if (!item.IsNew)
        {
          item.Delete();
          SaveItem(index);
        }
        else
        {
          SafeRemoveItem(index);
          OnSaved(item, null);
        }
      }
    }

    /// <summary>
    /// Replaces item in the list.
    /// </summary>
    /// <param name="index">Index of the item
    /// that was replaced.</param>
    /// <param name="item">New item.</param>
    protected override void SetItem(int index, T item)
    {
      item.SetParent(this);
      base.SetItem(index, item);
    }

    #endregion

    #region  IParent Members

    /// <summary>
    /// Gets or sets a value indicating whether the Replace
    /// event should be raised when OnCollectionChanged() is
    /// called.
    /// </summary>
    /// <remarks>
    /// There's a bug in DataGridDataConnection that throws
    /// an exception on the replace action. By default we
    /// disable raising the replace event to avoid that bug, 
    /// but some other datagrid controls require the event.
    /// </remarks>
    protected bool RaiseReplaceEvents { get; set; }

    /// <summary>
    /// Raises the CollectionChanged event.
    /// </summary>
    /// <param name="e">Event args object</param>
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      // SL Data Grid's DataGridDataConnection object does not support replace action.  
      // It throws an excpetioon when this occurs.
      if (this.RaiseListChangedEvents && (e.Action != NotifyCollectionChangedAction.Replace || RaiseReplaceEvents))
        base.OnCollectionChanged(e);
    }

    /// <summary>
    /// Gets a value indicating whether this collection
    /// supports change notification (always returns true).
    /// </summary>
    protected override bool SupportsChangeNotificationCore
    {
      get
      {
        return true;
      }
    }

    void Csla.Core.IParent.ApplyEditChild(Core.IEditableBusinessObject child)
    {
      if (child.EditLevel == 0)
        SaveItem((T)child);
    }

    void Csla.Core.IParent.RemoveChild(Core.IEditableBusinessObject child)
    {
      // do nothing, removal of a child is handled by
      // the RemoveItem override
    }

    #endregion

    #region IsBusy

    /// <summary>
    /// Gets a value indicating whether this object
    /// or any child object is currently executing
    /// an async operation.
    /// </summary>
    public override bool IsBusy
    {
      get
      {
        // run through all the child objects
        // and if any are dirty then then
        // collection is dirty
        foreach (T child in this)
          if (child.IsBusy)
            return true;

        return false;
      }
    }
    #endregion

    #region  Serialization Notification

    /// <summary>
    /// Set parent reference.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected override void OnDeserialized()
    {
      foreach (IEditableBusinessObject child in this)
        child.SetParent(this);

      base.OnDeserialized();
    }

    #endregion

    #region  Data Access

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "criteria")]
    private void DataPortal_Create(object criteria)
    {
      throw new NotSupportedException(Properties.Resources.CreateNotSupportedException);
    }

    /// <summary>
    /// Override this method to allow retrieval of an existing business
    /// object based on data in the database.
    /// </summary>
    /// <param name="criteria">An object containing criteria values to identify the object.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
    protected virtual void DataPortal_Fetch(object criteria)
    {
      throw new NotSupportedException(Properties.Resources.FetchNotSupportedException);
    }

    private void DataPortal_Update()
    {
      throw new NotSupportedException(Properties.Resources.UpdateNotSupportedException);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "criteria")]
    private void DataPortal_Delete(object criteria)
    {
      throw new NotSupportedException(Properties.Resources.DeleteNotSupportedException);
    }

    /// <summary>
    /// Called by the server-side DataPortal prior to calling the 
    /// requested DataPortal_xyz method.
    /// </summary>
    /// <param name="e">The DataPortalContext object passed to the DataPortal.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member"), EditorBrowsable(EditorBrowsableState.Advanced)]
    protected virtual void DataPortal_OnDataPortalInvoke(DataPortalEventArgs e)
    {

    }

    /// <summary>
    /// Called by the server-side DataPortal after calling the 
    /// requested DataPortal_xyz method.
    /// </summary>
    /// <param name="e">The DataPortalContext object passed to the DataPortal.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member"), EditorBrowsable(EditorBrowsableState.Advanced)]
    protected virtual void DataPortal_OnDataPortalInvokeComplete(DataPortalEventArgs e)
    {

    }

    /// <summary>
    /// Called by the server-side DataPortal if an exception
    /// occurs during data access.
    /// </summary>
    /// <param name="e">The DataPortalContext object passed to the DataPortal.</param>
    /// <param name="ex">The Exception thrown during data access.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member"), EditorBrowsable(EditorBrowsableState.Advanced)]
    protected virtual void DataPortal_OnDataPortalException(DataPortalEventArgs e, Exception ex)
    {

    }

    #endregion

    #region ToArray

    /// <summary>
    /// Get an array containing all items in the list.
    /// </summary>
    public T[] ToArray()
    {
      List<T> result = new List<T>();
      foreach (T item in this)
        result.Add(item);
      return result.ToArray();
    }

    #endregion

    #region IDataPortalTarget Members

    void Csla.Server.IDataPortalTarget.CheckRules()
    { }

    void Csla.Server.IDataPortalTarget.MarkAsChild()
    { }

    void Csla.Server.IDataPortalTarget.MarkNew()
    { }

    void Csla.Server.IDataPortalTarget.MarkOld()
    { }

    void Csla.Server.IDataPortalTarget.DataPortal_OnDataPortalInvoke(DataPortalEventArgs e)
    {
      this.DataPortal_OnDataPortalInvoke(e);
    }

    void Csla.Server.IDataPortalTarget.DataPortal_OnDataPortalInvokeComplete(DataPortalEventArgs e)
    {
      this.DataPortal_OnDataPortalInvokeComplete(e);
    }

    void Csla.Server.IDataPortalTarget.DataPortal_OnDataPortalException(DataPortalEventArgs e, Exception ex)
    {
      this.DataPortal_OnDataPortalException(e, ex);
    }

    void Csla.Server.IDataPortalTarget.Child_OnDataPortalInvoke(DataPortalEventArgs e)
    { }

    void Csla.Server.IDataPortalTarget.Child_OnDataPortalInvokeComplete(DataPortalEventArgs e)
    { }

    void Csla.Server.IDataPortalTarget.Child_OnDataPortalException(DataPortalEventArgs e, Exception ex)
    { }

    #endregion
  }
}