﻿//-----------------------------------------------------------------------
// <copyright file="AuthorizationContext.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>Context information provided to an authorization</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Csla.Rules
{
  /// <summary>
  /// Context information provided to an authorization
  /// rule when it is invoked.
  /// </summary>
  public class AuthorizationContext
  {
    /// <summary>
    /// Gets the rule object.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IAuthorizationRule Rule { get; internal set; }
    /// <summary>
    /// Gets a reference to the target business object.
    /// </summary>
    public object Target { get; internal set; }
    /// <summary>
    /// Gets the type of the target business class.
    /// </summary>
    public Type TargetType { get; internal set; }
    /// <summary>
    /// Gets or sets a value indicating whether the
    /// current user has permission to perform the requested
    /// action.
    /// </summary>
    public bool HasPermission { get; set; }
  }
}