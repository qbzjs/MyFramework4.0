﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class MyFramework_LuaManagerModelWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(MyFramework.LuaManagerModel), typeof(MyFramework.ManagerContorBase<MyFramework.LuaManagerModel>));
		L.RegFunction("DetectionLocalVersion", DetectionLocalVersion);
		L.RegFunction("Load", Load);
		L.RegFunction("OpenLibs", OpenLibs);
		L.RegFunction("Start", Start);
		L.RegFunction("Close", Close);
		L.RegFunction("AddLuaModel", AddLuaModel);
		L.RegFunction("RemoveLuaModel", RemoveLuaModel);
		L.RegFunction("FindFile", FindFile);
		L.RegFunction("ReadFile", ReadFile);
		L.RegFunction("DoFile", DoFile);
		L.RegFunction("GetTable", GetTable);
		L.RegFunction("GetFunction", GetFunction);
		L.RegFunction("New", _CreateMyFramework_LuaManagerModel);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateMyFramework_LuaManagerModel(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				MyFramework.LuaManagerModel obj = new MyFramework.LuaManagerModel();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: MyFramework.LuaManagerModel.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DetectionLocalVersion(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			obj.DetectionLocalVersion();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Load(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			object[] arg0 = ToLua.CheckObjectArray(L, 2);
			obj.Load(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenLibs(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			obj.OpenLibs();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Start(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			object[] arg0 = ToLua.CheckObjectArray(L, 2);
			obj.Start(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Close(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			obj.Close();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddLuaModel(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			obj.AddLuaModel(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveLuaModel(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			obj.RemoveLuaModel(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindFile(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			string o = obj.FindFile(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReadFile(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			byte[] o = obj.ReadFile(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoFile(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			string arg1 = ToLua.CheckString(L, 3);
			obj.DoFile(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTable(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			LuaInterface.LuaTable o = obj.GetTable(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFunction(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MyFramework.LuaManagerModel obj = (MyFramework.LuaManagerModel)ToLua.CheckObject<MyFramework.LuaManagerModel>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			LuaInterface.LuaFunction o = obj.GetFunction(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}
