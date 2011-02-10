//-----------------------------------------------------------------------
// <copyright file="LegacySplit.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace Csla.Test.DataPortalTest
{
  [Serializable]
  class LegacySplit : LegacySplitBase<LegacySplit>
  {
    private LegacySplit()
    { /* Require use of factory methods */ }
  }
}