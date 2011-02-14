//-----------------------------------------------------------------------
// <copyright file="ValidationTests.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using UnitDriven;
using Csla.Serialization;

#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#elif MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Csla.Test.ValidationRules
{
  [TestClass()]
  public class ValidationTests : TestBase
  {
#if SILVERLIGHT
    [TestInitialize]
    public void Setup()
    {
      Csla.DataPortal.ProxyTypeName = "Local";
    }
#endif

    [TestMethod()]
    public void TestValidationRulesWithPrivateMember()
    {
      //works now because we are calling ValidationRules.CheckRules() in DataPortal_Create
      UnitTestContext context = GetContext();
      Csla.ApplicationContext.GlobalContext.Clear();
      HasRulesManager.NewHasRulesManager((o, e) =>
      {
        HasRulesManager root = e.Object;
        context.Assert.AreEqual("<new>", root.Name);
        context.Assert.AreEqual(true, root.IsValid, "should be valid on create");
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);

        root.BeginEdit();
        root.Name = "";
        root.CancelEdit();

        context.Assert.AreEqual("<new>", root.Name);
        context.Assert.AreEqual(true, root.IsValid, "should be valid after CancelEdit");
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);

        root.BeginEdit();
        root.Name = "";
        root.ApplyEdit();

        context.Assert.AreEqual("", root.Name);
        context.Assert.AreEqual(false, root.IsValid);
        context.Assert.AreEqual(1, root.BrokenRulesCollection.Count);
        context.Assert.AreEqual("Name required", root.BrokenRulesCollection[0].Description);
        context.Assert.Success();
      });

      context.Complete();
    }

    [TestMethod()]
    public void TestValidationRulesWithPublicProperty()
    {
      UnitTestContext context = GetContext();
      //should work since ValidationRules.CheckRules() is called in DataPortal_Create
      Csla.ApplicationContext.GlobalContext.Clear();
      HasRulesManager2.NewHasRulesManager2((o, e) =>
      {
        HasRulesManager2 root = e.Object;
        context.Assert.AreEqual("<new>", root.Name);
        context.Assert.AreEqual(true, root.IsValid, "should be valid on create");
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);

        root.BeginEdit();
        root.Name = "";
        root.CancelEdit();

        context.Assert.AreEqual("<new>", root.Name);
        context.Assert.AreEqual(true, root.IsValid, "should be valid after CancelEdit");
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);

        root.BeginEdit();
        root.Name = "";
        root.ApplyEdit();

        context.Assert.AreEqual("", root.Name);
        context.Assert.AreEqual(false, root.IsValid);
        context.Assert.AreEqual(1, root.BrokenRulesCollection.Count);
        context.Assert.AreEqual("Name required", root.BrokenRulesCollection[0].Description);
        context.Assert.AreEqual("Name required", root.BrokenRulesCollection.GetFirstMessage(HasRulesManager2.NameProperty).Description);
        context.Assert.AreEqual("Name required", root.BrokenRulesCollection.GetFirstBrokenRule(HasRulesManager2.NameProperty).Description);
        context.Assert.Success();
      });

      context.Complete();
    }

    [TestMethod()]
    public void TestValidationAfterEditCycle()
    {
      //should work since ValidationRules.CheckRules() is called in DataPortal_Create
      Csla.ApplicationContext.GlobalContext.Clear();
      UnitTestContext context = GetContext();
      HasRulesManager.NewHasRulesManager((o, e) =>
      {
        HasRulesManager root = e.Object;
        context.Assert.AreEqual("<new>", root.Name);
        context.Assert.AreEqual(true, root.IsValid, "should be valid on create");
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);

        bool validationComplete = false;
        root.ValidationComplete += (vo, ve) => { validationComplete = true; };

        root.BeginEdit();
        root.Name = "";
        context.Assert.AreEqual("", root.Name);
        context.Assert.AreEqual(false, root.IsValid);
        context.Assert.AreEqual(1, root.BrokenRulesCollection.Count);
        context.Assert.AreEqual("Name required", root.BrokenRulesCollection[0].Description);
        context.Assert.IsTrue(validationComplete, "ValidationComplete should have run");
        root.BeginEdit();
        root.Name = "Begin 1";
        context.Assert.AreEqual("Begin 1", root.Name);
        context.Assert.AreEqual(true, root.IsValid);
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);
        root.BeginEdit();
        root.Name = "Begin 2";
        context.Assert.AreEqual("Begin 2", root.Name);
        context.Assert.AreEqual(true, root.IsValid);
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);
        root.BeginEdit();
        root.Name = "Begin 3";
        context.Assert.AreEqual("Begin 3", root.Name);
        context.Assert.AreEqual(true, root.IsValid);
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);

        HasRulesManager hrmClone = root.Clone();

        //Test validation rule cancels for both clone and cloned
        root.CancelEdit();
        context.Assert.AreEqual("Begin 2", root.Name);
        context.Assert.AreEqual(true, root.IsValid);
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);
        hrmClone.CancelEdit();
        context.Assert.AreEqual("Begin 2", hrmClone.Name);
        context.Assert.AreEqual(true, hrmClone.IsValid);
        context.Assert.AreEqual(0, hrmClone.BrokenRulesCollection.Count);
        root.CancelEdit();
        context.Assert.AreEqual("Begin 1", root.Name);
        context.Assert.AreEqual(true, root.IsValid);
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);
        hrmClone.CancelEdit();
        context.Assert.AreEqual("Begin 1", hrmClone.Name);
        context.Assert.AreEqual(true, hrmClone.IsValid);
        context.Assert.AreEqual(0, hrmClone.BrokenRulesCollection.Count);
        root.CancelEdit();
        context.Assert.AreEqual("", root.Name);
        context.Assert.AreEqual(false, root.IsValid);
        context.Assert.AreEqual(1, root.BrokenRulesCollection.Count);
        context.Assert.AreEqual("Name required", root.BrokenRulesCollection[0].Description);
        hrmClone.CancelEdit();
        context.Assert.AreEqual("", hrmClone.Name);
        context.Assert.AreEqual(false, hrmClone.IsValid);
        context.Assert.AreEqual(1, hrmClone.BrokenRulesCollection.Count);
        context.Assert.AreEqual("Name required", hrmClone.BrokenRulesCollection[0].Description);
        root.CancelEdit();
        context.Assert.AreEqual("<new>", root.Name);
        context.Assert.AreEqual(true, root.IsValid);
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);
        hrmClone.CancelEdit();
        context.Assert.AreEqual("<new>", hrmClone.Name);
        context.Assert.AreEqual(true, hrmClone.IsValid);
        context.Assert.AreEqual(0, hrmClone.BrokenRulesCollection.Count);
        context.Assert.Success();
      });

      context.Complete();
    }

    [TestMethod()]
    public void TestValidationRulesAfterClone()
    {
      //this test uses HasRulesManager2, which assigns criteria._name to its public
      //property in DataPortal_Create.  If it used HasRulesManager, it would fail
      //the first assert, but pass the others
      Csla.ApplicationContext.GlobalContext.Clear();
      UnitTestContext context = GetContext();
      HasRulesManager2.NewHasRulesManager2((o, e) =>
      {
        context.Assert.Try(() =>
          {
            HasRulesManager2 root = e.Object;
            context.Assert.AreEqual(true, root.IsValid);
            root.BeginEdit();
            root.Name = "";
            root.ApplyEdit();

            context.Assert.AreEqual(false, root.IsValid);
            HasRulesManager2 rootClone = root.Clone();
            context.Assert.AreEqual(false, rootClone.IsValid);

            rootClone.Name = "something";
            context.Assert.AreEqual(true, rootClone.IsValid);
            context.Assert.Success();
          });
      });
      context.Complete();
    }

    [TestMethod()]
    public void BreakRequiredRule()
    {
      Csla.ApplicationContext.GlobalContext.Clear();
      UnitTestContext context = GetContext();
      HasRulesManager.NewHasRulesManager((o, e) =>
      {
        context.Assert.Try(() =>
          {
            HasRulesManager root = e.Object;
            root.Name = "";

            context.Assert.AreEqual(false, root.IsValid, "should not be valid");
            context.Assert.AreEqual(1, root.BrokenRulesCollection.Count);
            context.Assert.AreEqual("Name required", root.BrokenRulesCollection[0].Description);
            context.Assert.Success();
          });
      });

      context.Complete();
    }

    [TestMethod()]
    public void BreakLengthRule()
    {
      Csla.ApplicationContext.GlobalContext.Clear();
      UnitTestContext context = GetContext();
      HasRulesManager.NewHasRulesManager((o, e) =>
      {
        HasRulesManager root = e.Object;
        root.Name = "12345678901";
        context.Assert.AreEqual(false, root.IsValid, "should not be valid");
        context.Assert.AreEqual(1, root.BrokenRulesCollection.Count);
        //Assert.AreEqual("Name too long", root.GetBrokenRulesCollection[0].Description);
        Assert.AreEqual("Name can not exceed 10 characters", root.BrokenRulesCollection[0].Description);

        root.Name = "1234567890";
        context.Assert.AreEqual(true, root.IsValid, "should be valid");
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);
        context.Assert.Success();
      });
      context.Complete();
    }

    [TestMethod()]
    public void BreakLengthRuleAndClone()
    {
      Csla.ApplicationContext.GlobalContext.Clear();
      UnitTestContext context = GetContext();
      HasRulesManager.NewHasRulesManager((o, e) =>
      {
        HasRulesManager root = e.Object;
        root.Name = "12345678901";
        context.Assert.AreEqual(false, root.IsValid, "should not be valid before clone");
        context.Assert.AreEqual(1, root.BrokenRulesCollection.Count);
        //Assert.AreEqual("Name too long", root.GetBrokenRulesCollection[0].Description;
        Assert.AreEqual("Name can not exceed 10 characters", root.BrokenRulesCollection[0].Description);

        root = (HasRulesManager)(root.Clone());
        context.Assert.AreEqual(false, root.IsValid, "should not be valid after clone");
        context.Assert.AreEqual(1, root.BrokenRulesCollection.Count);
        //Assert.AreEqual("Name too long", root.GetBrokenRulesCollection[0].Description;
        context.Assert.AreEqual("Name can not exceed 10 characters", root.BrokenRulesCollection[0].Description);

        root.Name = "1234567890";
        context.Assert.AreEqual(true, root.IsValid, "Should be valid");
        context.Assert.AreEqual(0, root.BrokenRulesCollection.Count);
        context.Assert.Success();
      });
      context.Complete();
    }

    [TestMethod()]
    public void RegExSSN()
    {
      Csla.ApplicationContext.GlobalContext.Clear();
      UnitTestContext context = GetContext();

      HasRegEx root = new HasRegEx();

      root.Ssn = "555-55-5555";
      root.Ssn2 = "555-55-5555";
      context.Assert.IsTrue(root.IsValid, "Ssn should be valid");

      root.Ssn = "555-55-5555d";
      context.Assert.IsFalse(root.IsValid, "Ssn should not be valid");

      root.Ssn = "555-55-5555";
      root.Ssn2 = "555-55-5555d";
      context.Assert.IsFalse(root.IsValid, "Ssn should not be valid");

      context.Assert.Success();
      context.Complete();
    }

    [TestMethod]
    public void MergeBrokenRules()
    {
      UnitTestContext context = GetContext();
      var root = new BrokenRulesMergeRoot();
      root.Validate();
      Csla.Rules.BrokenRulesCollection list = root.BrokenRulesCollection;
      context.Assert.AreEqual(2, list.Count, "Should have 2 broken rules");
      context.Assert.AreEqual("rule://csla.test.validationrules.brokenrulesmergeroot-rulebroken/Test1", list[0].RuleName);

      context.Assert.Success();
      context.Complete();
    }

    [TestMethod]
    public void VerifyUndoableStateStackOnClone()
    {
      Csla.ApplicationContext.GlobalContext.Clear();
      using (UnitTestContext context = GetContext())
      {
        HasRulesManager2.NewHasRulesManager2((o, e) =>
        {
          context.Assert.IsNull(e.Error);
          HasRulesManager2 root = e.Object;

          string expected = root.Name;
          root.BeginEdit();
          root.Name = "";
          HasRulesManager2 rootClone = root.Clone();
          rootClone.CancelEdit();

          string actual = rootClone.Name;
          context.Assert.AreEqual(expected, actual);
          context.Assert.Try(rootClone.ApplyEdit);

          context.Assert.Success();
        });
        context.Complete();
      }
    }

    [TestMethod()]
    public void ListChangedEventTrigger()
    {
      Csla.ApplicationContext.GlobalContext.Clear();
      UnitTestContext context = GetContext();
      HasChildren.NewObject((o, e) =>
      {
        try
        {
          HasChildren root = e.Object;
          context.Assert.AreEqual(false, root.IsValid);
          root.BeginEdit();
          root.ChildList.Add(Csla.DataPortal.CreateChild<Child>());
          context.Assert.AreEqual(true, root.IsValid);

          root.CancelEdit();
          context.Assert.AreEqual(false, root.IsValid);

          context.Assert.Success();
        }
        finally
        {
          context.Complete();
        }
      });
    }

    [TestMethod]
    public void RuleThrowsException()
    {
      UnitTestContext context = GetContext();
      var root = new HasBadRule();
      root.Validate();
      context.Assert.IsFalse(root.IsValid);
      context.Assert.AreEqual(1, root.GetBrokenRules().Count);
      context.Assert.AreEqual("rule://csla.test.validationrules.hasbadrule-badrule/null:Operation is not valid due to the current state of the object.", root.GetBrokenRules()[0].Description);
      context.Assert.Success();
      context.Complete();
    }

    [TestMethod]
    public void PrivateField()
    {
      UnitTestContext context = GetContext();
      var root = new HasPrivateFields();
      root.Validate();
      context.Assert.IsFalse(root.IsValid);
      root.Name = "abc";
      context.Assert.IsTrue(root.IsValid);
      context.Assert.Success();
      context.Complete();
    }

#if !SILVERLIGHT
    [TestMethod]
    public void MinMaxValue()
    {
      var context = GetContext();
      var root = Csla.DataPortal.Create<UsesCommonRules>();
      context.Assert.AreEqual(1, root.Data);

      context.Assert.IsFalse(root.IsValid);
      context.Assert.IsTrue(root.BrokenRulesCollection[0].Description.Length > 0);


      root.Data = 0;
      context.Assert.IsFalse(root.IsValid);

      root.Data = 20;
      context.Assert.IsFalse(root.IsValid);

      root.Data = 15;
      context.Assert.IsTrue(root.IsValid);

      context.Assert.Success();
      context.Complete();
    }

    [TestMethod]
    public void MinMaxLength()
    {
      var context = GetContext();

      var root = Csla.DataPortal.Create<UsesCommonRules>();
      root.Data = 15;
      context.Assert.IsTrue(root.IsValid, "Should start valid");

      root.MinCheck = "a";
      context.Assert.IsFalse(root.IsValid, "Min too short");

      root.MinCheck = "123456";
      context.Assert.IsTrue(root.IsValid, "Min OK");

      root.MaxCheck = "a";
      context.Assert.IsTrue(root.IsValid, "Max OK");

      root.MaxCheck = "123456";
      context.Assert.IsFalse(root.IsValid, "Max too long");

      context.Assert.Success();
      context.Complete();
    }
#endif

    [TestMethod]
    public void TwoRules()
    {
      var context = GetContext();

      var root = new TwoPropertyRules();
      var rule = new TwoProps(TwoPropertyRules.Value1Property, TwoPropertyRules.Value2Property);

      var ctx = new Csla.Rules.RuleContext(null, rule, root,
        new Dictionary<Core.IPropertyInfo, object> { 
          { TwoPropertyRules.Value1Property, "a" },
          { TwoPropertyRules.Value2Property, "b" } 
        });
      ((Csla.Rules.IBusinessRule)rule).Execute(ctx);
      context.Assert.AreEqual(0, ctx.Results.Count);

      ctx = new Csla.Rules.RuleContext(null, rule, root,
        new Dictionary<Core.IPropertyInfo, object> { 
          { TwoPropertyRules.Value1Property, "" },
          { TwoPropertyRules.Value2Property, "a" } 
        });
      ((Csla.Rules.IBusinessRule)rule).Execute(ctx);
      context.Assert.AreEqual(1, ctx.Results.Count);

      context.Assert.Success();
      context.Complete();
    }

#if SILVERLIGHT && !WINDOWS_PHONE
    [TestMethod]
    public void NotifyDataErrorInfo()
    {
      var context = GetContext();

      var root = new TwoPropertyRules();

      int errorsChanged = 0;
      root.ErrorsChanged += (o, e) =>
        {
          errorsChanged++;
        };

      root.Value2 = "b";
      context.Assert.AreEqual(2, errorsChanged);
      var indei = root as System.ComponentModel.INotifyDataErrorInfo;
      context.Assert.IsTrue(indei.HasErrors);
      int count = 0;
      foreach (var item in indei.GetErrors("Value2"))
        count++;
      context.Assert.IsTrue(count > 0);

      root.Value1 = "a";

      context.Assert.AreEqual(4, errorsChanged);

      context.Assert.Success();
      context.Complete();
    }
#endif
  }

  [Serializable]
  public class HasBadRule : BusinessBase<HasBadRule>
  {
    public static PropertyInfo<int> IdProperty = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(IdProperty); }
      set { SetProperty(IdProperty, value); }
    }

    public void Validate()
    {
      BusinessRules.CheckRules();
    }

    public Rules.BrokenRulesCollection GetBrokenRules()
    {
      return BusinessRules.GetBrokenRules();
    }

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new BadRule());
    }

    private class BadRule : Rules.BusinessRule
    {
      protected override void Execute(Rules.RuleContext context)
      {
        throw new InvalidOperationException();
      }
    }
  }

  [Serializable]
  public class HasPrivateFields : BusinessBase<HasPrivateFields>
  {
    public static PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name, RelationshipTypes.PrivateField);
    private string _name = NameProperty.DefaultValue;
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    public void Validate()
    {
      BusinessRules.CheckRules();
    }

    protected override object ReadProperty(Core.IPropertyInfo propertyInfo)
    {
      if (ReferenceEquals(propertyInfo, NameProperty))
        return _name;
      else
        return base.ReadProperty(propertyInfo);
    }

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(NameProperty));
    }
  }

  [Serializable]
  public class UsesCommonRules : BusinessBase<UsesCommonRules>
  {
    private static PropertyInfo<int> DataProperty = RegisterProperty<int>(c => c.Data, null, 1);
    public int Data
    {
      get { return GetProperty(DataProperty); }
      set { SetProperty(DataProperty, value); }
    }

    private static PropertyInfo<string> MinCheckProperty = RegisterProperty<string>(c => c.MinCheck, null, "123456");
    public string MinCheck
    {
      get { return GetProperty(MinCheckProperty); }
      set { SetProperty(MinCheckProperty, value); }
    }

    private static PropertyInfo<string> MaxCheckProperty = RegisterProperty<string>(c => c.MaxCheck);
    public string MaxCheck
    {
      get { return GetProperty(MaxCheckProperty); }
      set { SetProperty(MaxCheckProperty, value); }
    }

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MinValue<int>(DataProperty, 5));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxValue<int>(DataProperty, 15));

      BusinessRules.AddRule(new Csla.Rules.CommonRules.MinLength(MinCheckProperty, 5));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(MaxCheckProperty, 5));
    }
  }

  [Serializable]
  public class TwoPropertyRules : BusinessBase<TwoPropertyRules>
  {
    public static PropertyInfo<string> Value1Property = RegisterProperty<string>(c => c.Value1);
    public string Value1
    {
      get { return GetProperty(Value1Property); }
      set { SetProperty(Value1Property, value); }
    }

    public static PropertyInfo<string> Value2Property = RegisterProperty<string>(c => c.Value2);
    public string Value2
    {
      get { return GetProperty(Value2Property); }
      set { SetProperty(Value2Property, value); }
    }

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new TwoProps(Value1Property, Value2Property));
      BusinessRules.AddRule(new TwoProps(Value2Property, Value1Property));
    }
  }

  public class TwoProps : Csla.Rules.BusinessRule
  {
    public Csla.Core.IPropertyInfo SecondaryProperty { get; set; }
    public TwoProps(Csla.Core.IPropertyInfo primaryProperty, Csla.Core.IPropertyInfo secondProperty)
      : base(primaryProperty)
    {
      SecondaryProperty = secondProperty;
      AffectedProperties.Add(SecondaryProperty);
      InputProperties = new List<Core.IPropertyInfo> { PrimaryProperty, SecondaryProperty };
    }

    protected override void Execute(Rules.RuleContext context)
    {
      var v1 = (string)context.InputPropertyValues[PrimaryProperty];
      var v2 = (string)context.InputPropertyValues[SecondaryProperty];
      if (string.IsNullOrEmpty(v1) || string.IsNullOrEmpty(v2))
        context.AddErrorResult(string.Format("v1:{0}, v2:{1}", v1, v2));
    }
  }
}