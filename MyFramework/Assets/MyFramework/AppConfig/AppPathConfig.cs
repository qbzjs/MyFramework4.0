using UnityEngine;
using System.IO;

/// <summary>
/// app相关路径配置 此为硬性配置暂时不可外部修改
/// </summary>
namespace MyFramework
{
    public static class AppPathConfig
    {

        #region 目录定义
        /// <summary>
        /// App外部沙盒目录
        /// </summary>
        public const string mAppExternalAddress = "AppResources";

        /// <summary>
        /// App外部沙盒临时目录
        /// </summary>
        public const string mAppResourcesTemp = "AppResourcesTemp";
        /// <summary>
        /// 版本文件相对路径
        /// </summary>
        public const string mVersionAddress = "Version";

        /// <summary>
        /// 外部资源文件后缀名
        /// </summary>
        public static readonly string ExternalAssetTail = ".unity3d";
        #endregion

        /// <summary>
        /// 平台沙盒存储根目录
        /// </summary>
        public static string PlatformRoot
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                        return Application.persistentDataPath;
                    case RuntimePlatform.Android:
                        return Application.persistentDataPath;
                    case RuntimePlatform.WindowsPlayer:
                        return Application.persistentDataPath;
                    case RuntimePlatform.WindowsEditor:
                        return Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length);
                    default:
                        return Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length);
                }
            }
        }

        public static string GetstreamingAssetsPath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                        return Application.streamingAssetsPath;
                    case RuntimePlatform.Android:
                        return Application.streamingAssetsPath;
                    case RuntimePlatform.WindowsPlayer:
                        return Application.streamingAssetsPath;
                    case RuntimePlatform.WindowsEditor:
                        return Application.streamingAssetsPath;
                    default:
                        return Application.streamingAssetsPath;
                }
            }
        }

        /// <summary>
        /// App外部根目录
        /// </summary>
        public static string AppLuaAddress
        {
            get
            {
#if UNITY_EDITOR
                return Path.Combine(Application.dataPath, "Resources");
#else
                return Path.Combine(PlatformRoot, mAppExternalAddress);
#endif
            }
        }


        public static string AppAssetBundleTemp
        {
            get
            {
                return Path.Combine(PlatformRoot, mAppResourcesTemp);
            }
        }

        public static string AppAssetBundleAddress
        {
            get
            {
                return Path.Combine(PlatformRoot, mAppExternalAddress);
            }
        }

    }
}
