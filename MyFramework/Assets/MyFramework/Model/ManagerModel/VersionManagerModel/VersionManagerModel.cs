using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework
{
    /// <summary>
    /// 版本检测模块
    /// </summary>
    public class VersionManagerModel : ManagerContorBase<VersionManagerModel>
    {
        private VersionCheckeComp LocalCheckeComp;              //本地环境检测组件
        private AppVersionContrastComp VersionContrastComp;     //App版本对比组件
        private VersionDownlooadComp DownlooadComp;             //下载组件
        private VersionManageViewCompBase ViewComp;            

        public override void Load(params object[] _Agr)
        {
            ResourceComp = AddComp<Model_ResourceComp>();
            CoroutineComp = AddComp<Model_CoroutineComp>();
            if (_Agr != null && _Agr[0] is ModelCompBase)
            {
                ViewComp = AddComp(_Agr[0] as ModelCompBase) as VersionManageViewCompBase;
            }
            LocalCheckeComp = AddComp<VersionCheckeComp>();
            VersionContrastComp = AddComp<AppVersionContrastComp>();
                        DownlooadComp = AddComp<VersionDownlooadComp>();
            base.Load(_Agr);
            StartLocalChecke();
        }

        public Dictionary<string, int> GetLocalVersion
        {
            get {return LocalCheckeComp.LocalVersion;}
        }
        public AppBuileInfo GetLocalAssetInfo
        {
            get { return LocalCheckeComp.LocalAssetInfo; }
        }
        public Dictionary<string, int> GetServiceVersion
        {
            get { return VersionContrastComp.ServiceVersion; }
        }
        public AppBuileInfo GetServiceAssetInfo
        {
            get { return VersionContrastComp.ServiceAssetInfo; }
        }


        public void StartLocalChecke()
        {
            LocalCheckeComp.CheckeLocalVersion(()=> {
                VersionContrast();
            });
        }

        public void VersionContrast()
        {

            VersionContrastComp.ContrastVersion();
        }

        public void VersionDownlooad(List<ResBuileInfo> ResFiles)
        {
            DownlooadComp.StartDownload(ResFiles);
        }


        /// <summary>
        /// 版本检测刷新
        /// </summary>
        /// <param name="TitleStr"></param>
        /// <param name="DescribeStr"></param>
        /// <param name="Progress"></param>
        public void RefreshProgress(string TitleStr, string DescribeStr, float Progress)
        {
            if (ViewComp != null)
            {
                ViewComp.UpdataView(TitleStr, DescribeStr, Progress);
            }
        }
        public void RefreshProgress(string TitleStr, string DescribeStr, float Progress01,float Progress02)
        {
            if (ViewComp != null)
            {
                ViewComp.UpdataView(TitleStr, DescribeStr, Progress01, Progress02);
            }
        }

        /// <summary>
        /// 版本校验完毕
        /// </summary>
        public void VersionCheckeFinish()
        {
            ViewComp.Hide();
        }
    }
}
