﻿//-----------------------------------------------------------------------
// <copyright file="FieldTests.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitDriven;
#if !SILVERLIGHT
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Csla.Reflection;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif
#endif

namespace Csla.Test.MethodCaller
{
  [TestClass]
  public class FieldTests
  {

    private class Test1 { 
      private string _f1 = "private";
      public string _f2 = "public";
    }

    [TestMethod]
    public void PrivateFieldGetSuccess()
    {
      var instance = new Test1();
      var expected = "private";
      var actual = Csla.Data.DataMapper.GetFieldValue(instance, "_f1");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void PrivateFieldSetFail()
    {
      var instance = new Test1();
      var expected = "success";
      Csla.Data.DataMapper.SetFieldValue(instance, "_f1", expected);
      var actual = Csla.Data.DataMapper.GetFieldValue(instance, "_f1");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void PublicFieldGetSuccess()
    {
      var instance = new Test1();
      var expected = "public";
      var actual = Csla.Data.DataMapper.GetFieldValue(instance, "_f2");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void PublicFieldSetSuccess()
    {
      var instance = new Test1();
      var expected = "one";
      Csla.Data.DataMapper.SetFieldValue(instance, "_f2", expected);

      var actual = instance._f2;
      Assert.AreEqual(expected, actual);
    }
  }
}