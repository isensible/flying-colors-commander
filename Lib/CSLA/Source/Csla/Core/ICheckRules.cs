﻿//-----------------------------------------------------------------------
// <copyright file="ICheckRules.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>  Defines the common methods for any business object which exposes means to supress and check business rules.</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Csla.Core
{
  /// <summary>
  /// Defines the common methods for any business object which exposes means
  /// to supress and check business rules.
  /// </summary>
  public interface ICheckRules
  {
    /// <summary>
    /// Sets value indicating no rule methods will be invoked.
    /// </summary>
    void SuppressRuleChecking();

    /// <summary>
    /// Resets value indicating all rule methods will be invoked.
    /// </summary>
    void ResumeRuleChecking();

    /// <summary>
    /// Invokes all rules for the business type.
    /// </summary>
    void CheckRules();
  }
}