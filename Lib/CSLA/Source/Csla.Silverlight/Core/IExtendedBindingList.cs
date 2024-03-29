//-----------------------------------------------------------------------
// <copyright file="IExtendedBindingList.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>Extends <see cref="IBindingList" /> by adding extra</summary>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel;

namespace Csla.Core
{
  /// <summary>
  /// Extends <see cref="IBindingList" /> by adding extra
  /// events.
  /// </summary>
  public interface IExtendedBindingList
  {
    /// <summary>
    /// Event indicating that an item is being
    /// removed from the list.
    /// </summary>
    event EventHandler<RemovingItemEventArgs> RemovingItem;
  }
}