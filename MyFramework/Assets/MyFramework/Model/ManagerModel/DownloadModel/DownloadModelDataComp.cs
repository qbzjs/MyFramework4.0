using System;
using System.IO;
using System.Net;
using System.Collections.Generic;


namespace MyFramework
{ 
    /// <summary>
    /// 下载模块数据组件
    /// </summary>
    public class DownloadModelDataComp : ModelCompBase<DownloadModel>
    {
        #region 框架构造
        public override void Load(ModelContorlBase _ModelContorl, params object[] _Agr)
        {
            IsDownloading = false;
            webClient = new WebClient();
            TasksQueues = new Queue<DownloadTaskGroup>();
            webClient.DownloadFileCompleted += DownloadFileCompleted;
            webClient.DownloadProgressChanged += DownloadProgressChanged;
            base.Load(_ModelContorl);
            base.LoadEnd();
        }
        #endregion

        private WebClient webClient;
        private Queue<DownloadTaskGroup> TasksQueues;
        private DownloadTaskGroup CurrDownloadTask;
        private bool IsDownloading = false;


        /// <summary>
        /// 添加下载任务
        /// </summary>
        /// <param name="_DownloadTask"></param>
        public void AddTask(DownloadTaskGroup _DownloadTask)
        {
            TasksQueues.Enqueue(_DownloadTask);
            if (!IsDownloading)
                StartNextTask();
        }

        private void StartNextTask()
        {
            if (CurrDownloadTask != null)
            {
                if (CurrDownloadTask.IsComp)
                {
                    if (TasksQueues.Count > 0)
                    {
                        IsDownloading = true;
                        CurrDownloadTask = TasksQueues.Dequeue();
                        StartTask(CurrDownloadTask.NextTask());
                    }
                    else
                    {
                        IsDownloading = false;
                    }
                }
                else
                {
                    StartTask(CurrDownloadTask.NextTask());
                }
            }
            else
            {
                if (TasksQueues.Count > 0)
                {
                    IsDownloading = true;
                    CurrDownloadTask = TasksQueues.Dequeue();
                    StartTask(CurrDownloadTask.NextTask());
                }
                else
                {
                    IsDownloading = false;
                }
            }
        }

        private void StartTask(DownloadTask _Task)
        {
            string filPath = _Task.FileName.Substring(0, _Task.FileName.LastIndexOf("/"));
            if (!Directory.Exists(filPath))
            {
                Directory.CreateDirectory(filPath);
            }
            webClient.DownloadFileAsync(new Uri(_Task.Url), _Task.FileName, _Task.Url);
        }

        #region 下载事件
        /// <summary>
        /// 下载进度更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            MyCentorl.VP(0, () => {
                CurrDownloadTask.UpdateDownloadProgress(e.BytesReceived);
            });
        }
        /// <summary>
        /// 下载完成回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MyCentorl.VP(0, () =>
                 {
                     CurrDownloadTask.TaskCompleted();
                     StartNextTask();
                 });
            }
            else
            {
                StartNextTask();
            }
        }
        #endregion
    }
}
