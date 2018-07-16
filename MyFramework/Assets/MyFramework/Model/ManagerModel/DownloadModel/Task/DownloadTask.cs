using System.Collections.Generic;
using System.IO;

namespace MyFramework
{
    public delegate void DownloadProgress(DownloadTaskGroup TasksGroup, float Progress);
    public delegate void TaskCompleted(DownloadTaskGroup TasksGroup, DownloadTask Task);

    /// <summary>
    /// 单个下载任务
    /// </summary>
    public class DownloadTask
    {
        public string Id;                                                               //资源Id
        public string Url;                                                              //下载链接
        public string FileName;                                                         //文件名称
        public float Szie { get; private set; }                                         //文件大小 单位kb
        public float Progress { get; private set; }                                     //下载进度
        public bool IsComp { get; set; }                                                //是否下载完成

        public DownloadTask(string _Url,string _SavePath,float  _Szie)
        {
            IsComp = false;
            Url = _Url;
            FileName = _SavePath;
            Szie = _Szie;
        }

        public DownloadTask(ResBuileInfo Info)
        {
            Id = Info.Id;
            Url = AppWebConfig.CurrWebRootUrl + "/Files/" + AppConfig.TargetPlatform.ToString()+"/"+ Info.Id;
            FileName = AppPathConfig.AppAssetBundleTemp + "/" + Info.Id;
            Szie = Info.Size;
            IsComp = false;
        }

        public void UpdateDownloadProgress(long DownloadBytes)
        {
            Progress =  (DownloadBytes / 1204.0f) / Szie;
        }
    }


    /// <summary>
    /// 一组下载任务
    /// </summary>
    public class DownloadTaskGroup
    {
        private DownloadTask[] AllTask;                                                 //所有下载任务
        private Queue<DownloadTask> NoCompTasks;                                        //未完成任务
        private Queue<DownloadTask> CompTasks;                                          //完成任务
        private float Size = 0;
        private float CompSize = 0;
        private DownloadProgress DownloadBack;
        private TaskCompleted CompTaskBack;
        public bool IsComp { get; private set; }                                        //是否下载完成
        public float Progress { get; private set; }                                     //下载进度
        public DownloadTask CurrTask { get { return NoCompTasks.Peek(); } }             //当前任务

        public DownloadTaskGroup(DownloadTask[] _TaskQueues, DownloadProgress _DownloadBack = null, TaskCompleted _CompTaskBack = null)
        {
            IsComp = false;
            AllTask = _TaskQueues;
            NoCompTasks = new Queue<DownloadTask>(_TaskQueues);
            CompTasks = new Queue<DownloadTask>();
            for (int i = 0; i < _TaskQueues.Length; i++)
            {
                Size += _TaskQueues[i].Szie;
            }
            CompSize = 0;
            DownloadBack = _DownloadBack;
            CompTaskBack = _CompTaskBack;
        }

        public DownloadTask NextTask()
        {
            if (NoCompTasks.Count > 0)
            {
               return NoCompTasks.Peek();
            }
            return null;
        }
        
        public virtual void UpdateDownloadProgress(long DownloadBytes)
        {
            if (!IsComp)
            {
                CurrTask.UpdateDownloadProgress(DownloadBytes);
                Progress = (CompSize + DownloadBytes / 1204.0f) / Size;
                if (DownloadBack != null)
                {
                    DownloadBack(this, Progress);
                }
            }
            else
            {
                DownloadBack(this, 1);
            }
        }

        /// <summary>
        /// 任务完成
        /// </summary>
        public virtual void TaskCompleted()
        {
            DownloadTask Task = CurrTask;
            Task.IsComp = true;
            CompSize += Task.Szie;
            CompTasks.Enqueue(NoCompTasks.Dequeue());
            if (NoCompTasks.Count == 0)
            {
                IsComp = true;
            }
            if (CompTaskBack != null)
            {
                CompTaskBack(this,Task);
            }
        }
    }
}
