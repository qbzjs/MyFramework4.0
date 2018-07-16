using System;
using System.Collections.Generic;


namespace MyFramework
{
    /// <summary>
    /// 事件模块数据管理组件
    /// </summary>
    public class EventModelDataComp : ModelCompBase<EventModel>
    {
        #region 框架构造
        public override void Load(ModelContorlBase _ModelContorl, params object[] _Agr)
        {
           
            base.Load(_ModelContorl);
            base.LoadEnd();
        }
        #endregion

        private Dictionary<string, EventDelegates> m_events;


        #region 通用接口
        public void Add(string eid, EventDelegate1 edlg)
        {
            if (!this.m_events.ContainsKey(eid))
            {
                this.m_events[eid] = new EventDelegates(eid);
            }
            this.m_events[eid].Add(edlg);
        }

        public void Remove(string eid, EventDelegate1 edlg)
        {
            if (this.m_events.ContainsKey(eid) && this.m_events[eid].Remove(edlg))
            {
                this.m_events.Remove(eid);
            }
        }

        public void Add(string eid, EventDelegate2 edlg)
        {
            if (!this.m_events.ContainsKey(eid))
            {
                this.m_events[eid] = new EventDelegates(eid);
            }
            this.m_events[eid].Add(edlg);
        }

        public void Remove(string eid, EventDelegate2 edlg)
        {
            if (this.m_events.ContainsKey(eid) && this.m_events[eid].Remove(edlg))
            {
                this.m_events.Remove(eid);
            }
        }

        public void Add(string eid, EventDelegate3 edlg)
        {
            if (!this.m_events.ContainsKey(eid))
            {
                this.m_events[eid] = new EventDelegates(eid);
            }
            this.m_events[eid].Add(edlg);
        }


        public void Remove(string eid, EventDelegate3 edlg)
        {
            if (this.m_events.ContainsKey(eid) && this.m_events[eid].Remove(edlg))
            {
                this.m_events.Remove(eid);
            }
        }
        #endregion

        #region 泛型接口
        public void Add<T>(string eid, EventDelegate<T> edlg)
        {
            if (!this.m_events.ContainsKey(eid))
            {
                this.m_events[eid] = new EventDelegates(eid);
            }
            this.m_events[eid].Add(edlg);
        }


        public void Remove<T>(string eid, EventDelegate<T> edlg)
        {
            if (this.m_events.ContainsKey(eid) && this.m_events[eid].Remove(edlg))
            {
                this.m_events.Remove(eid);
            }
        }
        #endregion

        #region 触发器
        public void OnTrigger(string eid, EventArg arg)
        {
            EventDelegates edlgs;
            if (this.m_events.TryGetValue(eid, out edlgs))
            {
                edlgs.Call(arg);
            }
        }
        public void OnTrigger(string eid)
        {
            this.OnTrigger(eid, new EventArg());
        }


        public void OnTrigger<T>(string eid, T arg)
        {
            EventDelegates edlgs;
            if (this.m_events.TryGetValue(eid, out edlgs))
            {
                edlgs.Call<T>(arg);
            }
        }
        #endregion

    }

    /// <summary>
    /// 事件消息体
    /// </summary>
    public class EventArg
    {
        private object[] m_args;
        public EventArg(params object[] args)
        {
            this.m_args = args;
        }

        public object this[int index]
        {
            get { return this.m_args[index]; }
        }

        public object[] Args
        {
            get { return m_args; }
        }
    }

    /// <summary>
    /// 通用事件，装箱拆箱效率有所消耗
    /// </summary>
    public delegate void EventDelegate1();
    public delegate void EventDelegate2(EventArg arg);
    public delegate void EventDelegate3(string eid, EventArg arg);

    /// <summary>
    /// 泛型委托，尽量减少装箱和拆箱的消耗
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arg"></param>
    public delegate void EventDelegate<T>(T arg);
    public delegate void EventDelegate<T, U>(T arg1, U arg2);
    public delegate void EventDelegate<T, U, V>(T arg1, U arg2, V arg3);

    class EventDelegates
    {
        private string m_eid;
        private List<Delegate> m_edlgs;
        private List<Delegate> m_current;

        public EventDelegates(string eid)
        {
            this.m_eid = eid;
            this.m_edlgs = new List<Delegate>();
            this.m_current = new List<Delegate>();
        }

        public void Add(Delegate edlg)
        {
            this.m_edlgs.Add(edlg);
        }

        public bool Remove(Delegate edlg)
        {
            if (this.m_edlgs.Contains(edlg))
            {
                this.m_edlgs.Remove(edlg);
            }
            return this.m_edlgs.Count == 0;
        }

        public void Call(EventArg earg)
        {
            m_current.Clear();
            m_current.AddRange(m_edlgs);

            foreach (Delegate edlg in this.m_current)
            {
                if (edlg.GetType() == typeof(EventDelegate1))
                    ((EventDelegate1)edlg)();
                else if (edlg.GetType() == typeof(EventDelegate2))
                    ((EventDelegate2)edlg)(earg);
                else
                    ((EventDelegate3)edlg)(this.m_eid, earg);
            }
        }

        /// <summary>
        /// 泛型触发接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="earg"></param>
        public void Call<T>(T earg)
        {
            m_current.Clear();
            m_current.AddRange(m_edlgs);

            foreach (Delegate edlg in this.m_current)
            {
                if (edlg.GetType() == typeof(EventDelegate<T>))
                    ((EventDelegate<T>)edlg)(earg);
            }
        }
    }

}
