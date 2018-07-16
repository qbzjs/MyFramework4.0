using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework
{
    /// <summary>
    /// 版本下载组件
    /// </summary>
    public class VersionDownlooadComp : ModelCompBase<VersionManagerModel>
    {
        public override void Load(ModelContorlBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
        }

        public void StartDownload(List<ResBuileInfo> Fileinfos)
        {
            if (Fileinfos != null && Fileinfos.Count > 0)
            {
                DownloadTask[] tsdks = new DownloadTask[Fileinfos.Count];
                for (int i = 0; i < Fileinfos.Count; i++)
                {
                    tsdks[i] = new DownloadTask(Fileinfos[i]);
                }
                DownloadTaskGroup Task = new DownloadTaskGroup(tsdks, UpdataDownload, TaskCompleted);
                DownloadModel.Instance.StartTasks(Task);
            }
            else
            {
                LoadEnd();
            }
        }

        private void UpdataDownload(DownloadTaskGroup TaskGroup, float Progress)
        {
            if (!TaskGroup.IsComp)
            {
                MyCentorl.RefreshProgress("下载资源文件", TaskGroup.CurrTask.Url, Progress, TaskGroup.CurrTask.Progress);
            }
            else
            {
                MyCentorl.RefreshProgress("下载资源文件", "下载完毕", Progress);
            }
        }

        public void TaskCompleted(DownloadTaskGroup TaskGroup, DownloadTask Task)
        {
            string ResId = Task.Id;
            string ResPath = AppPathConfig.AppAssetBundleTemp + "/" + ResId;
            string TargetPath = AppPathConfig.AppAssetBundleAddress + "/" + ResId;
            FilesTools.CopyFile(ResPath, TargetPath);
            MyCentorl.GetLocalAssetInfo.AppResInfo[ResId] = MyCentorl.GetServiceAssetInfo.AppResInfo[ResId];
            string AssetInfoStr = JsonTools.ObjectToJsonStr(MyCentorl.GetLocalAssetInfo);
            FilesTools.WriteStrToFile(AppPathConfig.AppAssetBundleAddress + "/AssetInfo.json", AssetInfoStr);
            if (TaskGroup.IsComp)
            {
                string VersionStr = JsonTools.DictionaryToJsonStr<string, int>(MyCentorl.GetServiceVersion);
                FilesTools.WriteStrToFile(AppPathConfig.AppAssetBundleAddress + "/VersionInfo.json", VersionStr);
                FilesTools.ClearDirectory(AppPathConfig.AppAssetBundleTemp);
                LoadEnd();
            }
        }

    }
}
