﻿//-----------------------------------------------------------------------
// <copyright file="MockList.partial.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Csla.DataPortalClient;
using System.ComponentModel;

namespace Csla.Testing.Business.EditableChildTests
{
  public partial class MockList
  {    
    #region  Data Access

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(LocalProxy<MockList>.CompletedHandler completed)
    {
      // fetch with no filter
      Fetch("", completed);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(SingleCriteria<MockList, string> criteria, LocalProxy<MockList>.CompletedHandler completed)
    {
      Fetch(criteria.Value, completed);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void DataPortal_Update(LocalProxy<MockList>.CompletedHandler completed)
    {
      Child_Update();

      completed(this, null);
    }

    #endregion
  }
}