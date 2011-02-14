//-----------------------------------------------------------------------
// <copyright file="AuthTests.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using Csla.Test.Security;
using UnitDriven;
using System.Diagnostics;

#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#elif MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Csla.Test.Authorization
{
#if TESTING
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
#endif
  [TestClass()]
    public class AuthTests
    {
        private DataPortal.DpRoot root = DataPortal.DpRoot.NewRoot();

        [TestMethod()]
        public void TestAuthCloneRules()
        {
            ApplicationContext.GlobalContext.Clear();

            Security.TestPrincipal.SimulateLogin();

          Assert.AreEqual(true, Csla.ApplicationContext.User.IsInRole("Admin"));

            #region "Pre Cloning Tests" 

            //Is it denying read properly?
            Assert.AreEqual("[DenyReadOnProperty] Can't read property", root.DenyReadOnProperty,
                "Read should have been denied 1");

            //Is it denying write properly?
            root.DenyWriteOnProperty = "DenyWriteOnProperty"; 

            Assert.AreEqual("[DenyWriteOnProperty] Can't write variable", root.Auth,
                "Write should have been denied 2");

            //Is it denying both read and write properly?
            Assert.AreEqual("[DenyReadWriteOnProperty] Can't read property", root.DenyReadWriteOnProperty,
                "Read should have been denied 3");

            root.DenyReadWriteOnProperty = "DenyReadWriteONproperty";

            Assert.AreEqual("[DenyReadWriteOnProperty] Can't write variable", root.Auth,
                "Write should have been denied 4");

            //Is it allowing both read and write properly?
            Assert.AreEqual(root.AllowReadWriteOnProperty, root.Auth,
                "Read should have been allowed 5");

            root.AllowReadWriteOnProperty = "No value";
            Assert.AreEqual("No value", root.Auth,
                "Write should have been allowed 6");

            #endregion 

            #region "After Cloning Tests"

            //Do they work under cloning as well?
            DataPortal.DpRoot NewRoot = root.Clone();

            ApplicationContext.GlobalContext.Clear();

            //Is it denying read properly?
            Assert.AreEqual("[DenyReadOnProperty] Can't read property", NewRoot.DenyReadOnProperty,
                "Read should have been denied 7");

            //Is it denying write properly?
            NewRoot.DenyWriteOnProperty = "DenyWriteOnProperty";

            Assert.AreEqual("[DenyWriteOnProperty] Can't write variable", NewRoot.Auth,
                "Write should have been denied 8");

            //Is it denying both read and write properly?
            Assert.AreEqual("[DenyReadWriteOnProperty] Can't read property", NewRoot.DenyReadWriteOnProperty,
                "Read should have been denied 9");

            NewRoot.DenyReadWriteOnProperty = "DenyReadWriteONproperty";

            Assert.AreEqual("[DenyReadWriteOnProperty] Can't write variable", NewRoot.Auth,
                "Write should have been denied 10");

            //Is it allowing both read and write properly?
            Assert.AreEqual(NewRoot.AllowReadWriteOnProperty, NewRoot.Auth,
                "Read should have been allowed 11");

            NewRoot.AllowReadWriteOnProperty = "AllowReadWriteOnProperty";
            Assert.AreEqual("AllowReadWriteOnProperty", NewRoot.Auth,
                "Write should have been allowed 12");

            #endregion 

            Security.TestPrincipal.SimulateLogout();
        }

        [TestMethod()]
        public void TestAuthBeginEditRules()
        {
            ApplicationContext.GlobalContext.Clear();

            Security.TestPrincipal.SimulateLogin();

#if SILVERLIGHT
          Assert.AreEqual(true, Csla.ApplicationContext.User.IsInRole("Admin"));
#else
            Assert.AreEqual(true, System.Threading.Thread.CurrentPrincipal.IsInRole("Admin"));
#endif

            root.Data = "Something new";

            root.BeginEdit();

            #region "Pre-Testing"

            root.Data = "Something new 1";

            //Is it denying read properly?
            Assert.AreEqual("[DenyReadOnProperty] Can't read property", root.DenyReadOnProperty,
                "Read should have been denied");

            //Is it denying write properly?
            root.DenyWriteOnProperty = "DenyWriteOnProperty";

            Assert.AreEqual("[DenyWriteOnProperty] Can't write variable", root.Auth,
                "Write should have been denied");

            //Is it denying both read and write properly?
            Assert.AreEqual("[DenyReadWriteOnProperty] Can't read property", root.DenyReadWriteOnProperty,
                "Read should have been denied");

            root.DenyReadWriteOnProperty = "DenyReadWriteONproperty";

            Assert.AreEqual("[DenyReadWriteOnProperty] Can't write variable", root.Auth,
                "Write should have been denied");

            //Is it allowing both read and write properly?
            Assert.AreEqual(root.AllowReadWriteOnProperty, root.Auth,
                "Read should have been allowed");

            root.AllowReadWriteOnProperty = "No value";
            Assert.AreEqual("No value", root.Auth,
                "Write should have been allowed");

            #endregion 

            #region "Cancel Edit"

            //Cancel the edit and see if the authorization rules still work
            root.CancelEdit();

            //Is it denying read properly?
            Assert.AreEqual("[DenyReadOnProperty] Can't read property", root.DenyReadOnProperty,
                "Read should have been denied");

            //Is it denying write properly?
            root.DenyWriteOnProperty = "DenyWriteOnProperty";

            Assert.AreEqual("[DenyWriteOnProperty] Can't write variable", root.Auth,
                "Write should have been denied");

            //Is it denying both read and write properly?
            Assert.AreEqual("[DenyReadWriteOnProperty] Can't read property", root.DenyReadWriteOnProperty,
                "Read should have been denied");

            root.DenyReadWriteOnProperty = "DenyReadWriteONproperty";

            Assert.AreEqual("[DenyReadWriteOnProperty] Can't write variable", root.Auth,
                "Write should have been denied");

            //Is it allowing both read and write properly?
            Assert.AreEqual(root.AllowReadWriteOnProperty, root.Auth,
                "Read should have been allowed");

            root.AllowReadWriteOnProperty = "No value";
            Assert.AreEqual("No value", root.Auth,
                "Write should have been allowed");

            #endregion

            #region "Apply Edit"

            //Apply this edit and see if the authorization rules still work
            //Is it denying read properly?
            Assert.AreEqual("[DenyReadOnProperty] Can't read property", root.DenyReadOnProperty,
                "Read should have been denied");

            //Is it denying write properly?
            root.DenyWriteOnProperty = "DenyWriteOnProperty";

            Assert.AreEqual("[DenyWriteOnProperty] Can't write variable", root.Auth,
                "Write should have been denied");

            //Is it denying both read and write properly?
            Assert.AreEqual("[DenyReadWriteOnProperty] Can't read property", root.DenyReadWriteOnProperty,
                "Read should have been denied");

            root.DenyReadWriteOnProperty = "DenyReadWriteONproperty";

            Assert.AreEqual("[DenyReadWriteOnProperty] Can't write variable", root.Auth,
                "Write should have been denied");

            //Is it allowing both read and write properly?
            Assert.AreEqual(root.AllowReadWriteOnProperty, root.Auth,
                "Read should have been allowed");

            root.AllowReadWriteOnProperty = "No value";
            Assert.AreEqual("No value", root.Auth,
                "Write should have been allowed");


            #endregion

            Security.TestPrincipal.SimulateLogout();
        }

        [TestMethod()]
        public void TestAuthorizationAfterEditCycle()
        {
            Csla.ApplicationContext.GlobalContext.Clear();
            Csla.Test.Security.PermissionsRoot pr = Csla.Test.Security.PermissionsRoot.NewPermissionsRoot();

            Csla.Test.Security.TestPrincipal.SimulateLogin();
            pr.FirstName = "something";

            pr.BeginEdit();
            pr.FirstName = "ba";
            pr.CancelEdit();
            Csla.Test.Security.TestPrincipal.SimulateLogout();

            Csla.Test.Security.PermissionsRoot prClone = pr.Clone();
            Csla.Test.Security.TestPrincipal.SimulateLogin();
            prClone.FirstName = "somethiansdfasdf";
            Csla.Test.Security.TestPrincipal.SimulateLogout();
        }

        [ExpectedException(typeof(System.Security.SecurityException))]
        [TestMethod]
        public void TestUnauthorizedAccessToGet()
        {
            Csla.ApplicationContext.GlobalContext.Clear();

            PermissionsRoot pr = PermissionsRoot.NewPermissionsRoot();

            //this should throw an exception, since only admins have access to this property
            string something = pr.FirstName;
        }

        [ExpectedException(typeof(System.Security.SecurityException))]
        [TestMethod]
        public void TestUnauthorizedAccessToSet()
        {
            PermissionsRoot pr = PermissionsRoot.NewPermissionsRoot();

            //will cause an exception, because only admins can write to property
            pr.FirstName = "test";
        }

        [TestMethod]
        public void TestAuthorizedAccess()
        {
            Csla.ApplicationContext.GlobalContext.Clear();
            Csla.Test.Security.TestPrincipal.SimulateLogin();

            PermissionsRoot pr = PermissionsRoot.NewPermissionsRoot();
            //should work, because we are now logged in as an admin
            pr.FirstName = "something";
            string something = pr.FirstName;

#if SILVERLIGHT
          Assert.AreEqual(true, Csla.ApplicationContext.User.IsInRole("Admin"));
#else
            Assert.AreEqual(true, System.Threading.Thread.CurrentPrincipal.IsInRole("Admin"));
#endif
            //set to null so the other testmethods continue to throw exceptions
            Csla.Test.Security.TestPrincipal.SimulateLogout();

#if SILVERLIGHT
          Assert.AreEqual(false, Csla.ApplicationContext.User.IsInRole("Admin"));
#else
            Assert.AreEqual(false, System.Threading.Thread.CurrentPrincipal.IsInRole("Admin"));
#endif
        }

        [TestMethod]
        public void TestAuthExecute()
        {
          Csla.ApplicationContext.GlobalContext.Clear();
          Csla.Test.Security.TestPrincipal.SimulateLogin();

          PermissionsRoot pr = PermissionsRoot.NewPermissionsRoot();
          //should work, because we are now logged in as an admin
          pr.DoWork();

#if SILVERLIGHT
          Assert.AreEqual(true, Csla.ApplicationContext.User.IsInRole("Admin"));
#else
          Assert.AreEqual(true, System.Threading.Thread.CurrentPrincipal.IsInRole("Admin"));
#endif
          //set to null so the other testmethods continue to throw exceptions
          Csla.Test.Security.TestPrincipal.SimulateLogout();

#if SILVERLIGHT
          Assert.AreEqual(false, Csla.ApplicationContext.User.IsInRole("Admin"));
#else
          Assert.AreEqual(false, System.Threading.Thread.CurrentPrincipal.IsInRole("Admin"));
#endif
        }

        [TestMethod]
        [ExpectedException(typeof(System.Security.SecurityException))]
        public void TestUnAuthExecute()
        {
          Csla.ApplicationContext.GlobalContext.Clear();

          Assert.AreEqual(false, Csla.ApplicationContext.User.IsInRole("Admin"));

          PermissionsRoot pr = PermissionsRoot.NewPermissionsRoot();
          //should fail, because we're not an admin
          pr.DoWork();

        }

        //[TestMethod()]
        //public void TestAuthorizationAfterClone()
        //{
        //    Csla.ApplicationContext.GlobalContext.Clear();
        //    Csla.Test.Security.TestPrincipal.SimulateLogin();

        //    PermissionsRoot pr = PermissionsRoot.NewPermissionsRoot();

        //    //should work because we are now logged in as an admin
        //    pr.FirstName = "something";
        //    string something = pr.FirstName;

        //    //The permissions should persist across a cloning
        //    PermissionsRoot prClone = pr.Clone();
        //    pr.FirstName = "something";
        //    something = pr.FirstName;
        //}


    }
}