using System;
using System.Collections.Generic;


namespace MyFramework
{
    /// <summary>
    /// 文件下载模块控制器
    /// </summary>
    public class DownloadModel : ManagerContorBase<DownloadModel>
    {
        private DownloadModelDataComp DataComp;
        public override void Load(params object[] _Agr)
        {
            TimerComp = AddComp<Model_TimerComp>();
            DataComp = AddComp<DownloadModelDataComp>();
            base.Load(_Agr);
           
        }

        /// <summary>
        /// 开始下载任务
        /// </summary>
        /// <param name="TaskList"></param>
        public void StartTasks(DownloadTaskGroup Tasks)
        {
            DataComp.AddTask(Tasks);
        }
    }
}
