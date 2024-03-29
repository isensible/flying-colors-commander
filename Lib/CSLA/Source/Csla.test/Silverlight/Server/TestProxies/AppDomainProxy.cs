﻿//-----------------------------------------------------------------------
// <copyright file="AppDomainProxy.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>This is a test proxy used to test any type of calls that have to</summary>
//-----------------------------------------------------------------------
using System;
using System.Reflection;
using System.Threading;
using Csla.Server;
using DataPortalContext=Csla.Server.DataPortalContext;
using IDataPortalServer=Csla.Server.IDataPortalServer;

namespace Csla.Testing.Business.TestProxies
{
  /// <summary>
  /// This is a test proxy used to test any type of calls that have to
  /// be send accross to a different AppDomain (non local styff)
  /// </summary>
  [Serializable]
  public class AppDomainProxy : DataPortalClient.IDataPortalProxy
  {
    private const int TIMEOUT = 70000;

    AppDomain _appDomain;
    IDataPortalServer _portal;

    IDataPortalServer Portal
    {
      get
      {
        try
        {
          if (_appDomain == null)
          {
            var current = AppDomain.CurrentDomain;
            var setup = current.SetupInformation;
            setup.ApplicationName = "Csla.DataPortal";
            _appDomain =
              AppDomain.CreateDomain("ServerDomain", null, setup);
            current.DomainUnload += UnloadDomain;
            SerializationWorkaround(AppDomain.CurrentDomain);
            SerializationWorkaround(_appDomain);
            _appDomain.UnhandledException += new UnhandledExceptionEventHandler(_appDomain_UnhandledException);
          }
          if (_portal == null)
            _portal = (IDataPortalServer)_appDomain.CreateInstanceAndUnwrap(
              "Csla", "Csla.Server.Hosts.RemotingPortal");
        }
        catch (Exception ex)
        {
          UnloadDomain(null, null);
          Console.WriteLine(ex);
          throw;
        }
        return _portal;
      }
    }

    void _appDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      Console.WriteLine(e.ExceptionObject);
    }

    void UnloadDomain(object sender, EventArgs e)
    {
      if (_appDomain != null)
      {
        AppDomain.CurrentDomain.DomainUnload -= UnloadDomain;
        AppDomain.Unload(_appDomain);
      }
    }

    #region Create

    private class CreateTask
    {
      public Type ObjectType;
      public object Criteria;
      public DataPortalContext Context;
      public DataPortalResult Result;
      public Exception ResultException;
    }

    public DataPortalResult Create(Type objectType, object criteria, DataPortalContext context)
    {
      var t = new Thread(DoCreate);
      var task = new CreateTask{
            ObjectType = objectType, 
            Criteria = criteria, 
            Context = context
          };
      t.Start(task);
      t.Join(TIMEOUT);
      if (task.ResultException != null)
        throw task.ResultException;
      return task.Result;
    }

    void DoCreate(object state)
    {
      CreateTask task = null;
      try
      {
        task = state as CreateTask;
        if (task != null)
          task.Result = Portal.Create(task.ObjectType, task.Criteria, task.Context);
      }
      catch (Exception ex)
      {
        if (task != null)
          task.ResultException = ex;
      }
    }

    #endregion

    #region Fetch

    private class FetchTask
    {
      public Type ObjectType;
      public object Criteria;
      public DataPortalContext Context;
      public DataPortalResult Result;
      public Exception ResultException;
    }

    public DataPortalResult Fetch(Type objectType, object criteria, DataPortalContext context)
    {
      var t = new Thread(DoFetch);
      var task = 
        new FetchTask
          {
            ObjectType = objectType, 
            Criteria = criteria, 
            Context = context
          };
      t.Start(task);
      t.Join(TIMEOUT);
      if (task.ResultException != null)
        throw task.ResultException;
      return task.Result;
    }

    void DoFetch(object state)
    {
      FetchTask task = null;
      try
      {
        task = state as FetchTask;
        if (task != null)
          task.Result = Portal.Fetch(task.ObjectType, task.Criteria, task.Context);
      }
      catch (Exception ex)
      {
        if (task != null)
          task.ResultException = ex;
      }
    }

    #endregion

    #region Update

    private class UpdateTask
    {
      public object Obj;
      public DataPortalContext Context;
      public DataPortalResult Result;
      public Exception ResultException;
    }

    public DataPortalResult Update(object obj, DataPortalContext context)
    {
      //Security.BusinessPrincipal
      //var temp = new DataPortalContext(new UnauthenticatedPrincipal(), context.IsRemotePortal);

      var t = new Thread(DoUpdate);
      var task = 
        new UpdateTask
          {
            Obj = obj, 
            Context = context
          };

      t.Start(task);
      t.Join(TIMEOUT);
      if (task.ResultException != null)
      {
          t.Abort();
          throw task.ResultException;
      }
        return task.Result;
    }

    void DoUpdate(object state)
    {
      UpdateTask task = null;
      try
      {
        task = (UpdateTask)state;

        if (task != null)
          task.Result = Portal.Update(task.Obj, task.Context);
      }
      catch (Exception ex)
      {
        if (task != null)
          task.ResultException = ex;
      }
    }

    #endregion

    #region Delete

    public DataPortalResult Delete(Type objectType, object criteria, DataPortalContext context)
    {
      var t = new Thread(DoDelete);
      var task = 
        new DeleteTask
          {
            Criteria = criteria, 
            Context = context, 
            ObjectType = objectType
          };

      t.Start(task);
      t.Join(TIMEOUT);
      if (task.ResultException != null)
        throw task.ResultException;
      return task.Result;
    }

    private class DeleteTask
    {
      public object Criteria;
      public DataPortalContext Context;
      public DataPortalResult Result;
      public Type ObjectType;
      public Exception ResultException;
    }

    void DoDelete(object state)
    {
      DeleteTask task = null;
      try
      {
        task = state as DeleteTask;
        if (task != null)
          task.Result = Portal.Delete(task.ObjectType, task.Criteria, task.Context);
      }
      catch (Exception ex)
      {
        if (task != null)
          task.ResultException = ex;
      }
    }

    #endregion

    #region Serialization bug workaround

    private static void SerializationWorkaround(AppDomain domain)
    {
      // hook up the AssemblyResolve
      // event so deep serialization works properly
      // this is a workaround for a bug in the .NET runtime
      AppDomain currentDomain = AppDomain.CurrentDomain;

      domain.AssemblyResolve +=
        new ResolveEventHandler(ResolveEventHandler);
    }

    private static Assembly ResolveEventHandler(
      object sender, ResolveEventArgs args)
    {
      var domain = sender as AppDomain;

      // get a list of all the assemblies loaded in our appdomain
      Assembly[] list = domain.GetAssemblies();

      // search the list to find the assembly that was not found automatically
      // and return the assembly from the list

      foreach (Assembly asm in list)
        if (asm.FullName == args.Name)
          return asm;

      // can't find it...
      return null;
    }

    #endregion

    public bool IsServerRemote
    {
      get { return true; }
    }
  }
}