﻿//-----------------------------------------------------------------------
// <copyright file="AddressBase.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using Csla;
using Csla.Serialization;

namespace cslalighttest.Serialization
{
  [Serializable]
  public class AddressBase : BusinessBase<AddressBase>
  {
    private static readonly PropertyInfo<string> CityProperty = RegisterProperty<string>(
      typeof(AddressBase),
      new PropertyInfo<string>("City"));

    public string City
    {
      get { return GetProperty<string>(CityProperty); }
      set { SetProperty<string>(CityProperty, value); }
    }
  }
}