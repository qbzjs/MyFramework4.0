using LuaInterface;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace MyFramework
{
    /// <summary>
    /// Lua帮助类
    /// </summary>
    public static class LuaHelpTools
    {
        public static Manager_ManagerModel GetManager_ManagerModel
        {
            get { return Manager_ManagerModel.Instance; }
        }
        public static ViewManagerModel GetViewManagerModel
        {
            get { return ViewManagerModel.Instance; }
        }
        public static GameObject Find(this GameObject Target, string target)
        {
            Transform targetran = Target.transform.Find(target);
            if (targetran != null)
            {
                return targetran.gameObject;
            }
            else
            {
                return null;
            }
        }
        public static Component OnSubmit(this GameObject Target, string target, string type)
        {

            Transform obj = Target.transform.Find(target);
            if (obj != null)
            {
                Component comp = obj.GetComponent(type);
                return comp;
            }
            return null;
        }
        public static void AddClick(this GameObject Target,string target,LuaFunction fun)
        {
            GameObject targetobj = Target.transform.Find(target).gameObject;
            if (targetobj == null) return;
            if (targetobj.GetComponent<Button>() != null)
            {
                targetobj.GetComponent<Button>().onClick.AddListener(
                    delegate () {
                        fun.Call(targetobj);
                    }
                );
                return;
            }
            if (targetobj.GetComponent<Toggle>() != null)
            {
                targetobj.GetComponent<Toggle>().onValueChanged.AddListener(
                    delegate (bool isOn) {
                        fun.Call(isOn);
                    }
                );
                return;
            }
        }
        public static void AddEvent(this GameObject Target,string target, EventTriggerType EventType, LuaFunction fun)
        {
            GameObject targetobj = Target.transform.Find(target).gameObject;
            if (targetobj == null) return;
            EventTrigger Trigger = targetobj.GetComponent<EventTrigger> ();
            if (Trigger != null)
            {
                UnityAction<BaseEventData> _BackCall = new UnityAction<BaseEventData>(delegate(BaseEventData data) { fun.Call((PointerEventData)data); });
                EventTrigger.Entry MyEvent = new EventTrigger.Entry();
                MyEvent.eventID = EventType;
                MyEvent.callback.AddListener(_BackCall);
                Trigger.triggers.Add(MyEvent);
            }
        }
    }
}
