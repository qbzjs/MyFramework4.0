using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework
{
    /// <summary>
    /// 版本校验组件
    /// </summary>
    public class VersionCheckeComp : ModelCompBase<VersionManagerModel>
    {
        public Dictionary<string, int> LocalVersion;
        public AppBuileInfo LocalAssetInfo;

        public override void Load(ModelContorlBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
        }

        /// <summary>
        /// 校验本地环境
        /// </summary>
        public void CheckeLocalVersion(Action CallBack)
        {
            string localVersionPath = AppPathConfig.AppAssetBundleAddress + "/VersionInfo.json";
            string localAssetPath = AppPathConfig.AppAssetBundleAddress + "/AssetInfo.json";
            bool IsSucc = FilesTools.IsKeepFileOrDirectory(localVersionPath);
            if (!IsSucc) //外部资源不存在 进行版本迁移处理
            {
                AssemblyLocalVersion(() =>
                {
                    string versionstr = FilesTools.ReadFileToStr(localVersionPath);
                    string assetinfostr = FilesTools.ReadFileToStr(localAssetPath);
                    LocalVersion = JsonTools.JsonStrToDictionary<string, int>(versionstr);
                    LocalAssetInfo = JsonTools.JsonStrToObject<AppBuileInfo>(assetinfostr);
                    CallBack();
                    LoadEnd();
                });
            }
            else
            {
                string versionstr = FilesTools.ReadFileToStr(localVersionPath);
                string assetinfostr = FilesTools.ReadFileToStr(localAssetPath);
                LocalVersion = JsonTools.JsonStrToDictionary<string, int>(versionstr);
                LocalAssetInfo = JsonTools.JsonStrToObject<AppBuileInfo>(assetinfostr);
                CheckeAppInside(()=> {
                    CallBack();
                    LoadEnd();
                });
            }

        }

        /// <summary>
        /// 对比App版本
        /// </summary>
        public void CheckeAppInside(Action CallBack)
        {
            float AppVersion = float.Parse(Application.version);
            if (LocalVersion["AppVersion"] < AppVersion)        //外部资源为上一个版本的资源文件，需要重新覆盖
            {
                AssemblyLocalVersion(() =>
                {
                    string localVersionPath = AppPathConfig.AppAssetBundleAddress + "/VersionInfo.json";
                    string localAssetPath = AppPathConfig.AppAssetBundleAddress + "/AssetInfo.json";
                    string versionstr = FilesTools.ReadFileToStr(localVersionPath);
                    string assetinfostr = FilesTools.ReadFileToStr(localAssetPath);
                    LocalVersion = JsonTools.JsonStrToDictionary<string, int>(versionstr);
                    LocalAssetInfo = JsonTools.JsonStrToObject<AppBuileInfo>(assetinfostr);
                    CallBack();
                });
            }
            else
            {
                CallBack();
            }
        }

        private void AssemblyLocalVersion(Action CallBack)
        {
            MyCentorl.RefreshProgress("初次运行解压资源文件", "开始解压资源文件", 0.0f);
            MyCentorl.StartCoroutine(AssemblyAsset(CallBack));
        }

        IEnumerator AssemblyAsset(Action CallBack)
        {
            WWW www = new WWW(AppPathConfig.GetstreamingAssetsPath + "/Asset.zip");
            yield return www;
            if (www.error != null)
                Debug.Log(www.error);
            else
            {
                yield return ZipTools.UnzipFile(www.bytes, AppPathConfig.AppAssetBundleAddress, "1152",(string DescribeStr, float Progress)=>{
                    MyCentorl.RefreshProgress("初次运行解压资源文件", DescribeStr, Progress);
                });
                if (CallBack != null)
                {
                    CallBack();
                }
                www.Dispose();
            }
        }

    }
}
