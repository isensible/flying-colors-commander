//-----------------------------------------------------------------------
// <copyright file="DataPortalResult.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>Returns data from the server-side DataPortal to the </summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Specialized;
using Csla.Core;

namespace Csla.Server
{
  /// <summary>
  /// Returns data from the server-side DataPortal to the 
  /// client-side DataPortal. Intended for internal CSLA .NET
  /// use only.
  /// </summary>
  [Serializable()]
  public class DataPortalResult
  {
    private object _returnObject;
    private ContextDictionary _globalContext;

    /// <summary>
    /// The business object being returned from
    /// the server.
    /// </summary>
    public object ReturnObject
    {
      get { return _returnObject; }
    }

    /// <summary>
    /// The global context being returned from
    /// the server.
    /// </summary>
    public ContextDictionary GlobalContext
    {
      get { return _globalContext; }
    }

    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    public DataPortalResult()
    {
      _globalContext = ApplicationContext.ContextManager.GetGlobalContext();
    }

    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    /// <param name="returnObject">Object to return as part
    /// of the result.</param>
    public DataPortalResult(object returnObject)
    {
      _returnObject = returnObject;
      _globalContext = ApplicationContext.ContextManager.GetGlobalContext();
    }

    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    /// <param name="returnObject">Object to return as part
    /// of the result.</param>
    /// <param name="globalContext">  Global context delivered via current reuest from the server
    /// </param>
    public DataPortalResult(object returnObject, ContextDictionary globalContext)
    {
      _returnObject = returnObject;
      _globalContext = globalContext;
    }
  }
}