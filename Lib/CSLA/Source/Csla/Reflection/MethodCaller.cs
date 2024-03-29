//-----------------------------------------------------------------------
// <copyright file="MethodCaller.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>Provides methods to dynamically find and call methods.</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Csla.Properties;
using Csla.Server;
using Csla;
using System.Globalization;

namespace Csla.Reflection
{
  /// <summary>
  /// Provides methods to dynamically find and call methods.
  /// </summary>
  public static class MethodCaller
  {
    private const BindingFlags allLevelFlags 
      = BindingFlags.FlattenHierarchy 
      | BindingFlags.Instance 
      | BindingFlags.Public 
      | BindingFlags.NonPublic
      ;

    private const BindingFlags oneLevelFlags 
      = BindingFlags.DeclaredOnly 
      | BindingFlags.Instance 
      | BindingFlags.Public 
      | BindingFlags.NonPublic
      ;

    private const BindingFlags ctorFlags 
      = BindingFlags.Instance 
      | BindingFlags.Public 
      | BindingFlags.NonPublic
      ;

    private const BindingFlags factoryFlags =
      BindingFlags.Static |
      BindingFlags.Public |
      BindingFlags.FlattenHierarchy;

    private const BindingFlags privateMethodFlags =
      BindingFlags.Public |
      BindingFlags.NonPublic |
      BindingFlags.Instance |
      BindingFlags.FlattenHierarchy;

#if !WINDOWS_PHONE
    #region Dynamic Method Cache

    private static Dictionary<MethodCacheKey, DynamicMethodHandle> _methodCache = new Dictionary<MethodCacheKey, DynamicMethodHandle>();

    private static DynamicMethodHandle GetCachedMethod(object obj, System.Reflection.MethodInfo info, params object[] parameters)
    {
      var key = new MethodCacheKey(obj.GetType().FullName, info.Name, GetParameterTypes(parameters));
      DynamicMethodHandle mh = null;
      if (!_methodCache.TryGetValue(key, out mh))
      {
        lock (_methodCache)
        {
          if (!_methodCache.TryGetValue(key, out mh))
          {
            mh = new DynamicMethodHandle(info, parameters);
            _methodCache.Add(key, mh);
          }
        }
      }
      return mh;
    }

    private static DynamicMethodHandle GetCachedMethod(object obj, string method, params object[] parameters)
    {
      var key = new MethodCacheKey(obj.GetType().FullName, method, GetParameterTypes(parameters));
      DynamicMethodHandle mh = null;
      if (!_methodCache.TryGetValue(key, out mh))
      {
        lock (_methodCache)
        {
          if (!_methodCache.TryGetValue(key, out mh))
          {
            var info = GetMethod(obj.GetType(), method, parameters);
            mh = new DynamicMethodHandle(info, parameters);
            _methodCache.Add(key, mh);
          }
        }
      }
      return mh;
    }

    #endregion

    #region Dynamic Constructor Cache

    private static Dictionary<Type, DynamicCtorDelegate> _ctorCache = new Dictionary<Type, DynamicCtorDelegate>();

    private static DynamicCtorDelegate GetCachedConstructor(Type objectType)
    {
      DynamicCtorDelegate result = null;
      if (!_ctorCache.TryGetValue(objectType, out result))
      {
        lock (_ctorCache)
        {
          if (!_ctorCache.TryGetValue(objectType, out result))
          {
            ConstructorInfo info = objectType.GetConstructor(ctorFlags, null, Type.EmptyTypes, null);
            if (info == null)
              throw new NotSupportedException(string.Format(
                CultureInfo.CurrentCulture,
                "Cannot create instance of Type '{0}'. No public parameterless constructor found.",
                objectType));

            result = DynamicMethodHandlerFactory.CreateConstructor(info);
            _ctorCache.Add(objectType, result);
          }
        }
      }
      return result;
    }

    #endregion
#endif

    #region GetType

    /// <summary>
    /// Gets a Type object based on the type name.
    /// </summary>
    /// <param name="typeName">Type name including assembly name.</param>
    /// <param name="throwOnError">true to throw an exception if the type can't be found.</param>
    /// <param name="ignoreCase">true for a case-insensitive comparison of the type name.</param>
    public static Type GetType(string typeName, bool throwOnError, bool ignoreCase)
    {
      string fullTypeName;
#if SILVERLIGHT
      if (typeName.Contains("Version="))
        fullTypeName = typeName;
      else
        fullTypeName = typeName + ", Version=..., Culture=neutral, PublicKeyToken=null";
#else
        fullTypeName = typeName;
#endif
      return Type.GetType(fullTypeName, throwOnError, ignoreCase);
    }

    /// <summary>
    /// Gets a Type object based on the type name.
    /// </summary>
    /// <param name="typeName">Type name including assembly name.</param>
    /// <param name="throwOnError">true to throw an exception if the type can't be found.</param>
    public static Type GetType(string typeName, bool throwOnError)
    {
      return GetType(typeName, throwOnError, false);
    }

    /// <summary>
    /// Gets a Type object based on the type name.
    /// </summary>
    /// <param name="typeName">Type name including assembly name.</param>
    public static Type GetType(string typeName)
    {
      return GetType(typeName, true, false);
    }

    #endregion

    #region Create Instance

    /// <summary>
    /// Uses reflection to create an object using its 
    /// default constructor.
    /// </summary>
    /// <param name="objectType">Type of object to create.</param>
    public static object CreateInstance(Type objectType)
    {
#if WINDOWS_PHONE
      if (objectType.IsValueType)
        throw new NotSupportedException(string.Format("CreateInstance {0}", objectType.Name));
      return Activator.CreateInstance(objectType);
#else
      var ctor = GetCachedConstructor(objectType);
      if (ctor == null)
        throw new NotImplementedException(Resources.DefaultConstructor + Resources.MethodNotImplemented);
      return ctor.Invoke();
#endif
    }

    #endregion

    private const BindingFlags propertyFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
    private const BindingFlags fieldFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

#if !WINDOWS_PHONE
    private static readonly Dictionary<MethodCacheKey, DynamicMemberHandle> _memberCache = new Dictionary<MethodCacheKey, DynamicMemberHandle>();

    internal static DynamicMemberHandle GetCachedProperty(Type objectType, string propertyName)
    {
      var key = new MethodCacheKey(objectType.FullName, propertyName, GetParameterTypes(null));
      DynamicMemberHandle mh = null;
      if (!_memberCache.TryGetValue(key, out mh))
      {
        lock (_memberCache)
        {
          if (!_memberCache.TryGetValue(key, out mh))
          {
            PropertyInfo info = objectType.GetProperty(propertyName, propertyFlags);
            if (info == null)
              throw new InvalidOperationException(
                string.Format(Resources.MemberNotFoundException, propertyName));
            mh = new DynamicMemberHandle(info);
            _memberCache.Add(key, mh);
          }
        }
      }
      return mh;
    }

    internal static DynamicMemberHandle GetCachedField(Type objectType, string fieldName)
    {
      var key = new MethodCacheKey(objectType.FullName, fieldName, GetParameterTypes(null));
      DynamicMemberHandle mh = null;
      if (!_memberCache.TryGetValue(key, out mh))
      {
        lock (_memberCache)
        {
          if (!_memberCache.TryGetValue(key, out mh))
          {
            FieldInfo info = objectType.GetField(fieldName, fieldFlags);
            if (info == null)
              throw new InvalidOperationException(
                string.Format(Resources.MemberNotFoundException, fieldName));
            mh = new DynamicMemberHandle(info);
            _memberCache.Add(key, mh);
          }
        }
      }
      return mh;
    }
#endif

    /// <summary>
    /// Invokes a property getter using dynamic
    /// method invocation.
    /// </summary>
    /// <param name="obj">Target object.</param>
    /// <param name="property">Property to invoke.</param>
    /// <returns></returns>
    public static object CallPropertyGetter(object obj, string property)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");
      if (string.IsNullOrEmpty(property))
        throw new ArgumentException("Argument is null or empty.", "property");

#if WINDOWS_PHONE
      System.Reflection.PropertyInfo p = obj.GetType().GetProperty(property);
      if (p == null)
        throw new NotSupportedException(string.Format(
          CultureInfo.CurrentCulture,
          "The property '{0}' on Type '{1}' does not have a public getter.",
          property,
          obj.GetType()));
      try
      {
        return p.GetValue(obj, new object[] { });
      }
      catch (ArgumentException ex)
      {
        throw new NotSupportedException("CallPropertyGetter", ex);
      }
#else
      var mh = GetCachedProperty(obj.GetType(), property);
      if (mh.DynamicMemberGet == null)
      {
        throw new NotSupportedException(string.Format(
          CultureInfo.CurrentCulture,
          "The property '{0}' on Type '{1}' does not have a public getter.",
          property,
          obj.GetType()));
      }

      return mh.DynamicMemberGet(obj);
#endif
    }

    /// <summary>
    /// Invokes a property setter using dynamic
    /// method invocation.
    /// </summary>
    /// <param name="obj">Target object.</param>
    /// <param name="property">Property to invoke.</param>
    /// <param name="value">New value for property.</param>
    public static void CallPropertySetter(object obj, string property, object value)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");
      if (string.IsNullOrEmpty(property))
        throw new ArgumentException("Argument is null or empty.", "property");

#if WINDOWS_PHONE
      System.Reflection.PropertyInfo p = obj.GetType().GetProperty(property);
      if (p == null)
        throw new NotSupportedException(string.Format(
          CultureInfo.CurrentCulture,
          "The property '{0}' on Type '{1}' does not have a public setter.",
          property,
          obj.GetType()));
      try
      {
        p.SetValue(obj, value, null);
      }
      catch (ArgumentException ex)
      {
        throw new NotSupportedException("CallPropertySetter", ex);
      }
#else
      var mh = GetCachedProperty(obj.GetType(), property);
      if (mh.DynamicMemberSet == null)
      {
        throw new NotSupportedException(string.Format(
          CultureInfo.CurrentCulture,
          "The property '{0}' on Type '{1}' does not have a public setter.",
          property,
          obj.GetType()));
      }

      mh.DynamicMemberSet(obj, value);
#endif
    }


    #region Call Method

    /// <summary>
    /// Uses reflection to dynamically invoke a method
    /// if that method is implemented on the target object.
    /// </summary>
    /// <param name="obj">
    /// Object containing method.
    /// </param>
    /// <param name="method">
    /// Name of the method.
    /// </param>
    /// <param name="parameters">
    /// Parameters to pass to method.
    /// </param>
    public static object CallMethodIfImplemented(object obj, string method, params object[] parameters)
    {
#if WINDOWS_PHONE
      System.Reflection.MethodInfo info = GetMethod(obj.GetType(), method, parameters);
      if (info != null)
        return CallMethod(obj, info, parameters);
      else
        return null;
#else
      var mh = GetCachedMethod(obj, method, parameters);
      if (mh == null || mh.DynamicMethod == null)
        return null;
      return CallMethod(obj, mh, parameters);
#endif
    }

    /// <summary>
    /// Detects if a method matching the name and parameters is implemented on the provided object.
    /// </summary>
    /// <param name="obj">The object implementing the method.</param>
    /// <param name="method">The name of the method to find.</param>
    /// <param name="parameters">The parameters matching the parameters types of the method to match.</param>
    /// <returns>True obj implements a matching method.</returns>
    public static bool IsMethodImplemented(object obj, string method, params object[] parameters)
    {
#if WINDOWS_PHONE
      System.Reflection.MethodInfo info = GetMethod(obj.GetType(), method, parameters);
      return info != null;
#else
      var mh = GetCachedMethod(obj, method, parameters);
      return mh != null && mh.DynamicMethod != null;
#endif
    }

    /// <summary>
    /// Uses reflection to dynamically invoke a method,
    /// throwing an exception if it is not
    /// implemented on the target object.
    /// </summary>
    /// <param name="obj">
    /// Object containing method.
    /// </param>
    /// <param name="method">
    /// Name of the method.
    /// </param>
    /// <param name="parameters">
    /// Parameters to pass to method.
    /// </param>
    public static object CallMethod(object obj, string method, params object[] parameters)
    {
#if WINDOWS_PHONE
      System.Reflection.MethodInfo info = GetMethod(obj.GetType(), method, parameters);
      if (info == null)
        throw new NotImplementedException(method + " " + Resources.MethodNotImplemented);

      return CallMethod(obj, info, parameters);
#else
      var mh = GetCachedMethod(obj, method, parameters);
      if (mh == null || mh.DynamicMethod == null)
        throw new NotImplementedException(method + " " + Resources.MethodNotImplemented);
      return CallMethod(obj, mh, parameters);
#endif
    }

    /// <summary>
    /// Uses reflection to dynamically invoke a method,
    /// throwing an exception if it is not
    /// implemented on the target object.
    /// </summary>
    /// <param name="obj">
    /// Object containing method.
    /// </param>
    /// <param name="info">
    /// System.Reflection.MethodInfo for the method.
    /// </param>
    /// <param name="parameters">
    /// Parameters to pass to method.
    /// </param>
    public static object CallMethod(object obj, System.Reflection.MethodInfo info, params object[] parameters)
    {
#if WINDOWS_PHONE
      var infoParams = info.GetParameters();
      var infoParamsCount = infoParams.Length;
      bool hasParamArray = infoParamsCount > 0 && infoParams[infoParamsCount - 1].GetCustomAttributes(typeof(ParamArrayAttribute), true).Length > 0;
      bool specialParamArray = false;
      if (hasParamArray && infoParams[infoParamsCount - 1].ParameterType.Equals(typeof(string[])))
        specialParamArray = true;
      if (hasParamArray && infoParams[infoParamsCount - 1].ParameterType.Equals(typeof(object[])))
        specialParamArray = true;
      object[] par = null;
      if (infoParamsCount == 1 && specialParamArray)
      {
        par = new object[] { parameters };
      }
      else if (infoParamsCount > 1 && hasParamArray && specialParamArray)
      {
        par = new object[infoParamsCount];
        for (int i = 0; i < infoParamsCount - 1; i++)
          par[i] = parameters[i];
        par[infoParamsCount - 1] = parameters[infoParamsCount - 1];
      }
      else
      {
        par = parameters;
      }

      object result = null;
      try
      {
        result = info.Invoke(obj, par);
      }
      catch (Exception e)
      {
        Exception inner = null;
        if (e.InnerException == null)
          inner = e;
        else
          inner = e.InnerException;
        throw new CallMethodException(info.Name + " " + Resources.MethodCallFailed, inner);
      }
      return result;
#else
      var mh = GetCachedMethod(obj, info, parameters);
      if (mh == null || mh.DynamicMethod == null)
        throw new NotImplementedException(info.Name + " " + Resources.MethodNotImplemented);
      return CallMethod(obj, mh, parameters);
#endif
    }

#if !WINDOWS_PHONE
    /// <summary>
    /// Uses reflection to dynamically invoke a method,
    /// throwing an exception if it is not implemented
    /// on the target object.
    /// </summary>
    /// <param name="obj">
    /// Object containing method.
    /// </param>
    /// <param name="methodHandle">
    /// MethodHandle for the method.
    /// </param>
    /// <param name="parameters">
    /// Parameters to pass to method.
    /// </param>
    private static object CallMethod(object obj, DynamicMethodHandle methodHandle, params object[] parameters)
    {
      object result = null;
      var method = methodHandle.DynamicMethod;

      object[] inParams = null;
      if (parameters == null)
        inParams = new object[] { null };
      else
        inParams = parameters;

      if (methodHandle.HasFinalArrayParam)
      {
        // last param is a param array or only param is an array
        var pCount = methodHandle.MethodParamsLength;
        var inCount = inParams.Length;
        if (inCount == pCount - 1)
        {
          // no paramter was supplied for the param array
          // copy items into new array with last entry null
          object[] paramList = new object[pCount];
          for (var pos = 0; pos <= pCount - 2; pos++)
            paramList[pos] = parameters[pos];
          paramList[paramList.Length - 1] = null;

          // use new array
          inParams = paramList;
        }
        else if ((inCount == pCount && !inParams[inCount - 1].GetType().IsArray) || inCount > pCount)
        {
          // 1 or more params go in the param array
          // copy extras into an array
          var extras = inParams.Length - (pCount - 1);
          object[] extraArray = GetExtrasArray(extras, methodHandle.FinalArrayElementType);
          Array.Copy(inParams, pCount - 1, extraArray, 0, extras);

          // copy items into new array
          object[] paramList = new object[pCount];
          for (var pos = 0; pos <= pCount - 2; pos++)
            paramList[pos] = parameters[pos];
          paramList[paramList.Length - 1] = extraArray;

          // use new array
          inParams = paramList;
        }
      }
      try
      {
        result = methodHandle.DynamicMethod(obj, inParams);
      }
      catch (Exception ex)
      {
        throw new CallMethodException(methodHandle.MethodName + " " + Resources.MethodCallFailed, ex);
      }
      return result;
    }

    private static object[] GetExtrasArray(int count, Type arrayType)
    {
      return (object[])(System.Array.CreateInstance(arrayType.GetElementType(), count));
    }
#endif
    #endregion

    #region Get/Find Method

    /// <summary>
    /// Uses reflection to locate a matching method
    /// on the target object.
    /// </summary>
    /// <param name="objectType">
    /// Type of object containing method.
    /// </param>
    /// <param name="method">
    /// Name of the method.
    /// </param>
    /// <param name="parameters">
    /// Parameters to pass to method.
    /// </param>
    public static System.Reflection.MethodInfo GetMethod(Type objectType, string method, params object[] parameters)
    {

      System.Reflection.MethodInfo result = null;

      object[] inParams = null;
      if (parameters == null)
        inParams = new object[] { null };
      else
        inParams = parameters;

      // try to find a strongly typed match

      // first see if there's a matching method
      // where all params match types
      result = FindMethod(objectType, method, GetParameterTypes(inParams));

      if (result == null)
      {
        // no match found - so look for any method
        // with the right number of parameters
        try
        {
          result = FindMethod(objectType, method, inParams.Length);
        }
        catch (AmbiguousMatchException)
        {
          // we have multiple methods matching by name and parameter count
          result = FindMethodUsingFuzzyMatching(objectType, method, inParams);
        }
      }

      // no strongly typed match found, get default based on name only
      if (result == null)
      {
        result = objectType.GetMethod(method, allLevelFlags);
      }
      return result;
    }

    private static System.Reflection.MethodInfo FindMethodUsingFuzzyMatching(Type objectType, string method, object[] parameters)
    {
      System.Reflection.MethodInfo result = null;
      Type currentType = objectType;
      do
      {
        System.Reflection.MethodInfo[] methods = currentType.GetMethods(oneLevelFlags);
        int parameterCount = parameters.Length;
        // Match based on name and parameter types and parameter arrays
        foreach (System.Reflection.MethodInfo m in methods)
        {
          if (m.Name == method)
          {
            var infoParams = m.GetParameters();
            var pCount = infoParams.Length;
            if (pCount > 0)
            {
              if (pCount == 1 && infoParams[0].ParameterType.IsArray)
              {
                // only param is an array
                if (parameters.GetType().Equals(infoParams[0].ParameterType))
                {
                  // got a match so use it
                  result = m;
                  break;
                }
              }
              if (infoParams[pCount - 1].GetCustomAttributes(typeof(ParamArrayAttribute), true).Length > 0)
              {
                // last param is a param array
                if (parameterCount == pCount && parameters[pCount - 1].GetType().Equals(infoParams[pCount - 1].ParameterType))
                {
                  // got a match so use it
                  result = m;
                  break;
                }
              }
            }
          }
        }
        if (result == null)
        {
          // match based on parameter name and number of parameters
          foreach (System.Reflection.MethodInfo m in methods)
          {
            if (m.Name == method && m.GetParameters().Length == parameterCount)
            {
              result = m;
              break;
            }
          }
        }
        if (result != null)
          break;
        currentType = currentType.BaseType;
      } while (currentType != null);

      
      return result;
    }

    /// <summary>
    /// Returns information about the specified
    /// method, even if the parameter types are
    /// generic and are located in an abstract
    /// generic base class.
    /// </summary>
    /// <param name="objectType">
    /// Type of object containing method.
    /// </param>
    /// <param name="method">
    /// Name of the method.
    /// </param>
    /// <param name="types">
    /// Parameter types to pass to method.
    /// </param>
    public static System.Reflection.MethodInfo FindMethod(Type objectType, string method, Type[] types)
    {
      System.Reflection.MethodInfo info = null;
      do
      {
        // find for a strongly typed match
        info = objectType.GetMethod(method, oneLevelFlags, null, types, null);
        if (info != null)
        {
          break; // match found
        }

        objectType = objectType.BaseType;
      } while (objectType != null);

      return info;
    }

    /// <summary>
    /// Returns information about the specified
    /// method, finding the method based purely
    /// on the method name and number of parameters.
    /// </summary>
    /// <param name="objectType">
    /// Type of object containing method.
    /// </param>
    /// <param name="method">
    /// Name of the method.
    /// </param>
    /// <param name="parameterCount">
    /// Number of parameters to pass to method.
    /// </param>
    public static System.Reflection.MethodInfo FindMethod(Type objectType, string method, int parameterCount)
    {
      // walk up the inheritance hierarchy looking
      // for a method with the right number of
      // parameters
      System.Reflection.MethodInfo result = null;
      Type currentType = objectType;
      do
      {
        System.Reflection.MethodInfo info = currentType.GetMethod(method, oneLevelFlags);
        if (info != null)
        {
          var infoParams = info.GetParameters();
          var pCount = infoParams.Length;
          if (pCount > 0 &&
             ((pCount == 1 && infoParams[0].ParameterType.IsArray) ||
             (infoParams[pCount - 1].GetCustomAttributes(typeof(ParamArrayAttribute), true).Length > 0)))
          {
            // last param is a param array or only param is an array
            if (parameterCount >= pCount - 1)
            {
              // got a match so use it
              result = info;
              break;
            }
          }
          else if (pCount == parameterCount)
          {
            // got a match so use it
            result = info;
            break;
          }
        }
        currentType = currentType.BaseType;
      } while (currentType != null);

      return result;
    }

    #endregion

    /// <summary>
    /// Returns an array of Type objects corresponding
    /// to the type of parameters provided.
    /// </summary>
    /// <param name="parameters">
    /// Parameter values.
    /// </param>
    public static Type[] GetParameterTypes(object[] parameters)
    {
      List<Type> result = new List<Type>();

      if (parameters == null)
      {
        result.Add(typeof(object));

      }
      else
      {
        foreach (object item in parameters)
        {
          if (item == null)
          {
            result.Add(typeof(object));
          }
          else
          {
            result.Add(item.GetType());
          }
        }
      }
      return result.ToArray();
    }

#if !SILVERLIGHT
    /// <summary>
    /// Gets a property type descriptor by name.
    /// </summary>
    /// <param name="t">Type of object containing the property.</param>
    /// <param name="propertyName">Name of the property.</param>
    public static PropertyDescriptor GetPropertyDescriptor(Type t, string propertyName)
    {
      var propertyDescriptors = TypeDescriptor.GetProperties(t);
      PropertyDescriptor result = null;
      foreach (PropertyDescriptor desc in propertyDescriptors)
        if (desc.Name == propertyName)
        {
          result = desc;
          break;
        }
      return result;
    }
#endif

    /// <summary>
    /// Gets information about a property.
    /// </summary>
    /// <param name="objectType">Object containing the property.</param>
    /// <param name="propertyName">Name of the property.</param>
    public static PropertyInfo GetProperty(Type objectType, string propertyName)
    {
      return objectType.GetProperty(propertyName, propertyFlags);
    }

    /// <summary>
    /// Gets a property value.
    /// </summary>
    /// <param name="obj">Object containing the property.</param>
    /// <param name="info">Property info object for the property.</param>
    /// <returns>The value of the property.</returns>
    public static object GetPropertyValue(object obj, PropertyInfo info)
    {
      object result = null;
      try
      {
        result = info.GetValue(obj, null);
      }
      catch (Exception e)
      {
        Exception inner = null;
        if (e.InnerException == null)
          inner = e;
        else
          inner = e.InnerException;
        throw new CallMethodException(info.Name + " " + Resources.MethodCallFailed, inner);
      }
      return result;
    }

    /// <summary>
    /// Invokes an instance method on an object.
    /// </summary>
    /// <param name="obj">Object containing method.</param>
    /// <param name="info">Method info object.</param>
    /// <returns>Any value returned from the method.</returns>
    public static object CallMethod(object obj, System.Reflection.MethodInfo info)
    {
      object result = null;
      try
      {
        result = info.Invoke(obj, null);
      }
      catch (Exception e)
      {
        Exception inner = null;
        if (e.InnerException == null)
          inner = e;
        else
          inner = e.InnerException;
        throw new CallMethodException(info.Name + " " + Resources.MethodCallFailed, inner);
      }
      return result;
    }

    /// <summary>
    /// Invokes a static factory method.
    /// </summary>
    /// <param name="objectType">Business class where the factory is defined.</param>
    /// <param name="method">Name of the factory method</param>
    /// <param name="parameters">Parameters passed to factory method.</param>
    /// <returns>Result of the factory method invocation.</returns>
    public static object CallFactoryMethod(Type objectType, string method, params object[] parameters)
    {
      object returnValue;
      System.Reflection.MethodInfo factory = objectType.GetMethod(
           method, factoryFlags, null,
           MethodCaller.GetParameterTypes(parameters), null);

      if (factory == null)
      {
        // strongly typed factory couldn't be found
        // so find one with the correct number of
        // parameters 
        int parameterCount = parameters.Length;
        System.Reflection.MethodInfo[] methods = objectType.GetMethods(factoryFlags);
        foreach (System.Reflection.MethodInfo oneMethod in methods)
          if (oneMethod.Name == method && oneMethod.GetParameters().Length == parameterCount)
          {
            factory = oneMethod;
            break;
          }
      }
      if (factory == null)
      {
        // no matching factory could be found
        // so throw exception
        throw new InvalidOperationException(
          string.Format(Resources.NoSuchFactoryMethod, method));
      }
      try
      {
        returnValue = factory.Invoke(null, parameters);
      }
      catch (Exception ex)
      {
        Exception inner = null;
        if (ex.InnerException == null)
          inner = ex;
        else
          inner = ex.InnerException;
        throw new CallMethodException(factory.Name + " " + Resources.MethodCallFailed, inner);
      }
      return returnValue;
    }

    /// <summary>
    /// Gets a System.Reflection.MethodInfo object corresponding to a
    /// non-public method.
    /// </summary>
    /// <param name="objectType">Object containing the method.</param>
    /// <param name="method">Name of the method.</param>
    public static System.Reflection.MethodInfo GetNonPublicMethod(Type objectType, string method)
    {

      System.Reflection.MethodInfo result = null;

      result = FindMethod(objectType, method, privateMethodFlags);

      return result;
    }

    /// <summary>
    /// Returns information about the specified
    /// method.
    /// </summary>
    /// <param name="objType">Type of object.</param>
    /// <param name="method">Name of the method.</param>
    /// <param name="flags">Flag values.</param>
    public static System.Reflection.MethodInfo FindMethod(Type objType, string method, BindingFlags flags)
    {
      System.Reflection.MethodInfo info = null;
      do
      {
        // find for a strongly typed match
        info = objType.GetMethod(method, flags);
        if (info != null)
          break; // match found
        objType = objType.BaseType;
      } while (objType != null);

      return info;
    }

#if WINDOWS_PHONE
    private static object[] GetExtrasArray(int count, Type arrayType)
    {
      return new object[count];
    //  var array = System.Array.CreateInstance(arrayType.GetElementType(), count);
    //  return (object[])array;
    }
#endif
  }
}