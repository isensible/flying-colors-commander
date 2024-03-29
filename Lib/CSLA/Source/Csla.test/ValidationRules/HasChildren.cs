﻿//-----------------------------------------------------------------------
// <copyright file="HasChildren.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;
using Csla.Serialization.Mobile;
using Csla.Core;
using System.Runtime.Serialization;

namespace Csla.Test.ValidationRules
{
  [Serializable]
  public class HasChildren : BusinessBase<HasChildren>
  {
    private static PropertyInfo<int> IdProperty = RegisterProperty(new PropertyInfo<int>("Id", "Id"));
    public int Id
    {
      get { return GetProperty(IdProperty); }
      set { SetProperty(IdProperty, value); }
    }

    private static PropertyInfo<ChildList> ChildListProperty = RegisterProperty(new PropertyInfo<ChildList>("ChildList", "Child list"));
    public ChildList ChildList
    {
      get 
      {
        if (!FieldManager.FieldExists(ChildListProperty))
          LoadProperty(ChildListProperty, ChildList.NewList());
        return GetProperty(ChildListProperty); 
      }
    }

    protected override void AddBusinessRules()
    {
      BusinessRules.AddRule(new OneItem<HasChildren> { PrimaryProperty = ChildListProperty });
    }

    public class OneItem<T> : Rules.BusinessRule
      where T : HasChildren
    {
      protected override void Execute(Rules.RuleContext context)
      {
        var target = (T)context.Target;
        if (target.ChildList.Count < 1)
          context.AddErrorResult("At least one item required");
      }
    }

    protected override void Initialize()
    {
      base.Initialize();
#if (!SILVERLIGHT)
      ChildList.ListChanged += new System.ComponentModel.ListChangedEventHandler(ChildList_ListChanged);
#endif
      this.ChildChanged += new EventHandler<ChildChangedEventArgs>(HasChildren_ChildChanged);
    }

    protected override void OnDeserialized(StreamingContext context)
    {
      base.OnDeserialized(context);
#if !SILVERLIGHT
      ChildList.ListChanged += new System.ComponentModel.ListChangedEventHandler(ChildList_ListChanged);
#endif
      this.ChildChanged += new EventHandler<ChildChangedEventArgs>(HasChildren_ChildChanged);
    }

#if (!SILVERLIGHT)
    void ChildList_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
    {
      //ValidationRules.CheckRules(ChildListProperty);
    }
#endif

    void HasChildren_ChildChanged(object sender, ChildChangedEventArgs e)
    {
      BusinessRules.CheckRules(ChildListProperty);
    }

    public static void NewObject(EventHandler<DataPortalResult<HasChildren>> completed)
    {
      Csla.DataPortal.BeginCreate<HasChildren>(completed);
    }
  }
}