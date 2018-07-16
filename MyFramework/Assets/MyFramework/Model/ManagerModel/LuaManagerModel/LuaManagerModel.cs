using System.IO;
using LuaInterface;
using UnityEngine;
using System;

namespace MyFramework
{
    public class LuaManagerModel : ManagerContorBase<LuaManagerModel>
    {
        private LuaState lua;
        private LuaManagerModelFileComp FileComp;

        #region 框架接口
        public LuaManagerModel()
        {
            DetectionLocalVersion();
            lua = new LuaState();
            FileComp = AddComp<LuaManagerModelFileComp>();
        }

        /// <summary>
        /// 校验App版本是否有效
        /// </summary>
        public void DetectionLocalVersion()
        {
            if (AppConfig.AppResModel == AppResModel.AssetBundleModel)
            {
                if (!PlayerPrefs.HasKey("Configuration" + Application.version))
                {
                    FilesTools.CopyDirFile(Application.streamingAssetsPath, AppPathConfig.AppAssetBundleAddress);
                    PlayerPrefs.SetInt("Configuration" + Application.version, 1);
                }
            }
        }

        public override void Load(params object[] _Agr)
        {
            lua.LuaSetTop(0);
            LuaBinder.Bind(lua);
            OpenLibs();
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, Manager_ManagerModel.Instance);
            base.Load(_Agr);
        }
        #region lua第三方插件

        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        public void OpenLibs()
        {
            lua.OpenLibs(LuaDLL.luaopen_pb);
            lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.OpenLibs(LuaDLL.luaopen_socket_core);

            lua.BeginPreLoad();
            lua.RegFunction("socket.core", LuaOpen_Socket_Core);
            lua.RegFunction("cjson", LuaOpen_Cjson);
            lua.EndPreLoad();
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Socket_Core(IntPtr L)
        {
            return LuaDLL.luaopen_socket_core(L);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Cjson(IntPtr L)
        {
            return LuaDLL.luaopen_cjson(L);
        }
        #endregion

        public override void Start(params object[] _Agr)
        {
            AddLuaModel("LuaManagerModel");
            DoFile("ToLua/tolua");
            lua.Start();
            base.Start(_Agr);
            DoFile("Main");
        }
        public override void Close()
        {
            RemoveLuaModel("LuaManagerModel");
            lua.LuaClose();
            base.Close();
        }
        #endregion

        public void AddLuaModel(string ModelName)
        {
            FileComp.AddLuaModel(ModelName);
        }

        public void RemoveLuaModel(string ModelName)
        {
            FileComp.AddLuaModel(ModelName);
        }

        public string FindFile(string ModelFileName)
        {
            return FileComp.FindFile(ModelFileName);
        }

        public byte[] ReadFile(string ModelFileName)
        {
            return FileComp.ReadFile(ModelFileName);
        }

        public void DoFile(string ModelName,string FileName)
        {
            if (State == ModelBaseState.Start)
            {
                byte[] buffer = FileComp.ReadFile(ModelName, FileName);
                if (buffer != null)
                {
                    lua.LuaLoadBuffer(buffer, Path.Combine(ModelName, FileName));
                }
            }
            else
            {
                LoggerHelper.Error("LuaManagerModelControl No Start:" +Path.Combine(ModelName, FileName));
            }
        }

        private void DoFile(string FileName)
        {
            byte[] buffer = FileComp.ReadFile("LuaManagerModel", FileName);
            if (buffer != null)
            {
                lua.LuaLoadBuffer(buffer, Path.Combine("LuaManagerModel", FileName));
            }
        }


        public LuaTable GetTable(string fullPath)
        {
            return lua.GetTable(fullPath);
        }
        public LuaFunction GetFunction(string fullPath)
        {
            return lua.GetFunction(fullPath);
        }

    }
}
