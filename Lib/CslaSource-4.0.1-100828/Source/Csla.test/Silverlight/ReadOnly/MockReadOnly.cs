﻿//-----------------------------------------------------------------------
// <copyright file="MockReadOnly.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Csla;
using Csla.Serialization;

namespace cslalighttest.ReadOnly
{
  [Serializable]
  public class MockReadOnly : BusinessBase<MockReadOnly>
  {
    public MockReadOnly() { }

    private static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(
      typeof(MockReadOnly),
      new PropertyInfo<int>("Id"));

    public int Id
    {
      get { return GetProperty<int>(IdProperty); }
    }
  }
}