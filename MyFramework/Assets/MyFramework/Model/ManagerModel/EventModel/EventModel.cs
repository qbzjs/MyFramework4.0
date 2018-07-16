using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework
{
    /// <summary>
    /// 事件模块控制器
    /// </summary>
    public class EventModel: ManagerContorBase<EventModel>
    {
        private EventModelDataComp DataComp;
        public override void Load(params object[] _Agr)
        {
            base.Load(_Agr);
            DataComp = AddComp<EventModelDataComp>();
        }

        #region 通用接口
        public void Add(string eid, EventDelegate1 edlg)
        {
            DataComp.Add(eid, edlg);
        }

        public void Remove(string eid, EventDelegate1 edlg)
        {
            DataComp.Remove(eid, edlg);
        }

        public void Add(string eid, EventDelegate2 edlg)
        {
            DataComp.Add(eid, edlg);
        }

        public void Remove(string eid, EventDelegate2 edlg)
        {
            DataComp.Add(eid, edlg);
        }

        public void Add(string eid, EventDelegate3 edlg)
        {
            DataComp.Add(eid, edlg);
        }


        public void Remove(string eid, EventDelegate3 edlg)
        {
            DataComp.Add(eid, edlg);
        }
        #endregion

        #region 泛型接口
        public void Add<T>(string eid, EventDelegate<T> edlg)
        {
            DataComp.Add<T>(eid, edlg);
        }


        public void Remove<T>(string eid, EventDelegate<T> edlg)
        {
            DataComp.Remove<T>(eid, edlg);
        }
        #endregion

        #region 触发器
        public void OnTrigger(string eid, EventArg arg)
        {
            DataComp.OnTrigger(eid, arg);
        }
        public void OnTrigger(string eid)
        {
            DataComp.OnTrigger(eid);
        }

        public void OnTrigger<T>(string eid, T arg)
        {
            DataComp.OnTrigger<T>(eid, arg);
        }
        #endregion

    }
}
