using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework
{
    public enum WebRootAddress
    {
        DebugWeb,   //调试模式
        LocalWeb,   //本地模式
        FormalWeb,  //正式模式
    }

    public static class AppWebConfig
    {
        private static string DebugWebRootUrl = "http://localhost:56470/Swevices/";
        private static string LocalWebRootUrl = "http://localhost:9567/Swevices/";
        private static string FormalWebRootUrl = "http://139.199.221.159:8082/";
        public static WebRootAddress WebRoot = WebRootAddress.LocalWeb;

        /// <summary>
        /// 当前服务端根地址
        /// </summary>
        public static string CurrWebRootUrl
        {
            get
            {
                switch (WebRoot)
                {
                    case WebRootAddress.DebugWeb:
                        return DebugWebRootUrl;
                    case WebRootAddress.LocalWeb:
                        return LocalWebRootUrl;
                    case WebRootAddress.FormalWeb:
                        return FormalWebRootUrl;
                    default:
                        return LocalWebRootUrl;
                }
            }
        }
    }
}
