using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework
{
    public enum Language
    {
        Chinese,
    }

    public enum AppPlatform
    {
        IOS,
        Android,
        Windows,
    }

    public enum AppResModel
    {
        DebugModel,
        AssetBundleModel,
    }

    /// <summary>
    /// 应用主要配置 可变动配置
    /// </summary>
    public static class AppConfig
    {

        public static string ResFileSuffix = ".1dao";
        /// <summary>
        /// app资源模型
        /// </summary>
        public static AppResModel AppResModel = AppResModel.DebugModel;
        /// <summary>
        /// App当前使用语言
        /// </summary>
        public static Language AppLanguage = Language.Chinese;

        /// <summary>
        /// 当前App目标平台
        /// </summary>
        public static AppPlatform TargetPlatform = AppPlatform.Windows;


        public static void SetAppConfig(AppResModel _AppResModel, Language _AppLanguage, AppPlatform _TargetPlatform, WebRootAddress WebRoot)
        {
            AppResModel = _AppResModel;
            AppLanguage = _AppLanguage;
            TargetPlatform = _TargetPlatform;
            AppWebConfig.WebRoot = WebRoot;
        }
    }
}
