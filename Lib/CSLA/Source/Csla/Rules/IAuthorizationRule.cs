﻿//-----------------------------------------------------------------------
// <copyright file="IAuthorizationRule.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>Interface defining an authorization</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csla.Rules
{
  /// <summary>
  /// Interface defining an authorization
  /// rule implementation.
  /// </summary>
  public interface IAuthorizationRule
  {
    /// <summary>
    /// Authorization rule implementation.
    /// </summary>
    /// <param name="context">Rule context object.</param>
    void Execute(AuthorizationContext context);
    /// <summary>
    /// Gets the element (property/method)
    /// to which this rule is associated.
    /// </summary>
    Csla.Core.IMemberInfo Element { get; }
    /// <summary>
    /// Gets the authorization action this rule
    /// will enforce.
    /// </summary>
    AuthorizationActions Action { get; }
  }
}