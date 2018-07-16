using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework
{
    /// <summary>
    /// App版本对比组件 只对比 资源校验模式为AppStartCheck的资源
    /// </summary>
    public class AppVersionContrastComp : ModelCompBase<VersionManagerModel>
    {
        public Dictionary<string, int> ServiceVersion;
        public AppBuileInfo ServiceAssetInfo;

        public override void Load(ModelContorlBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
        }


        public void ContrastVersion()
        {
            RequestVersionInfo(()=> {
                Dictionary<string, int> localversion = MyCentorl.GetLocalVersion;
                if (ServiceVersion["AppVersion"] == localversion["AppVersion"] && ServiceVersion["AssetVersion"] > localversion["AssetVersion"])
                {
                    AppBuileInfo Localassetinfo = MyCentorl.GetLocalAssetInfo;
                    RequestAssetVersionInfo(()=> {
                        List<ResBuileInfo> UpdataAssetinfo = ContrastAssetInfo(Localassetinfo, ServiceAssetInfo);
                        MyCentorl.VersionDownlooad(UpdataAssetinfo);
                        LoadEnd();
                    });
                }
                else
                {
                    MyCentorl.VersionDownlooad(null);
                    LoadEnd();
                }
            });
        }


        private void RequestVersionInfo(Action CallBack)
        {
            string Url = AppWebConfig.CurrWebRootUrl + "/Files/" + AppConfig.TargetPlatform.ToString() + "/VersionInfo.json";
            WebMessage msg = new WebMessage(Url, null, (data) => {
                ServiceVersion = JsonTools.JsonStrToDictionary<string, int>(data);
                CallBack();
            });
            WebServiceModel.Instance.SendMsg(msg);
        }

        private void RequestAssetVersionInfo(Action CallBack)
        {
            string Url = AppWebConfig.CurrWebRootUrl + "/Files/" + AppConfig.TargetPlatform.ToString() + "/AssetInfo.json";
            WebMessage msg = new WebMessage(Url, null, (data) => {
                ServiceAssetInfo = JsonTools.JsonStrToObject<AppBuileInfo>(data);
                CallBack();
            });
            WebServiceModel.Instance.SendMsg(msg);
        }

        /// <summary>
        /// 对比资源信息文件
        /// </summary>
        /// <param name="LocalAssetInfo"></param>
        /// <param name="ServiceAssetInfo"></param>
        /// <returns></returns>
        private List<ResBuileInfo> ContrastAssetInfo(AppBuileInfo LocalAssetInfo, AppBuileInfo ServiceAssetInfo)
        {
            List<ResBuileInfo> UpdataAssetinfo = new List<ResBuileInfo>();
            List<string> ServiceKeys = new List<string>(ServiceAssetInfo.AppResInfo.Keys);
            List<string> LocalKeys = new List<string>(LocalAssetInfo.AppResInfo.Keys);
            for (int i = 0; i < ServiceKeys.Count; i++)
            {
                bool IsNewAsset = true;
                for (int j = 0; j < LocalKeys.Count; j++)
                {
                    if (ServiceKeys[i] == LocalKeys[j])
                    {
                        IsNewAsset = false;
                        if (ServiceAssetInfo.AppResInfo[ServiceKeys[i]].CheckModel == AssetCheckMode.AppStartCheck)
                        {
                            if (!LocalAssetInfo.AppResInfo[LocalKeys[j]].IsNeedUpdata)
                            {
                                if (ServiceAssetInfo.AppResInfo[ServiceKeys[i]].Md5 != LocalAssetInfo.AppResInfo[LocalKeys[j]].Md5)
                                {
                                    LocalAssetInfo.AppResInfo[LocalKeys[j]].IsNeedUpdata = true;
                                    UpdataAssetinfo.Add(ServiceAssetInfo.AppResInfo[ServiceKeys[i]]);
                                }
                            }
                            else
                            {
                                UpdataAssetinfo.Add(LocalAssetInfo.AppResInfo[LocalKeys[j]]);
                            }
                        }
                        else
                        {
                            if (!LocalAssetInfo.AppResInfo[LocalKeys[j]].IsNeedUpdata)
                            {
                                if (ServiceAssetInfo.AppResInfo[ServiceKeys[i]].Md5 != LocalAssetInfo.AppResInfo[LocalKeys[j]].Md5)
                                {
                                    LocalAssetInfo.AppResInfo[LocalKeys[j]].IsNeedUpdata = true;
                                }
                            }
                        }
                        continue;
                    }
                }
                if (IsNewAsset)
                {
                    if (ServiceAssetInfo.AppResInfo[ServiceKeys[i]].CheckModel == AssetCheckMode.AppStartCheck)
                    {
                        UpdataAssetinfo.Add(ServiceAssetInfo.AppResInfo[ServiceKeys[i]]);
                    }
                    LocalAssetInfo.AppResInfo[ServiceKeys[i]] = ServiceAssetInfo.AppResInfo[ServiceKeys[i]];
                    LocalAssetInfo.AppResInfo[ServiceKeys[i]].IsNeedUpdata = true;
                }
            }
            return UpdataAssetinfo;
        }
    }
}
