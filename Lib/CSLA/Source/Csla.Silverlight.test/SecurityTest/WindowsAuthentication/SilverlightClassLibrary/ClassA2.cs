﻿using System;
using System.Net;
using Csla;
using Csla.Security;
using Csla.Serialization;

namespace ClassLibrary
{
  [Serializable()]
  public class ClassA2 : BusinessBase<ClassA2>
  {
    private static PropertyInfo<string> AProperty = RegisterProperty(new PropertyInfo<string>("A"));
    public string A
    {
      get { return GetProperty<string>(AProperty); }
      set { SetProperty<string>(AProperty, value); }
    }
    private static PropertyInfo<string> BProperty = RegisterProperty(new PropertyInfo<string>("B"));
    public string B
    {
      get { return GetProperty<string>(BProperty); }
      set { SetProperty<string>(BProperty, value); }
    }

    protected override void AddAuthorizationRules()
    {
      AuthorizationRules.AllowWrite(AProperty, "Users");
      AuthorizationRules.AllowRead(AProperty, "Users");
    }

    protected static void AddObjectAuthorizationRules()
    {
      AuthorizationRules.AllowCreate(typeof(ClassA2), "Users");
      AuthorizationRules.AllowEdit(typeof(ClassA2), "Users");
      AuthorizationRules.AllowDelete(typeof(ClassA2), "Users");
    }
  }
}
