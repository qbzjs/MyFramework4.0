using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyFramework
{
    /// <summary>
    /// web服务器模块数据组件
    /// </summary>
    public class WebServiceModelDataComp : ModelCompBase<WebServiceModel>
    {
        #region 框架构造
        public override void Load(ModelContorlBase _ModelContorl, params object[] _Agr)
        {
            MsgQueues = new Queue<WebMessage>();
            IsWorking = false;
            base.Load(_ModelContorl);
            base.LoadEnd();
        }
        #endregion
        private bool IsWorking = false;
        private Queue<WebMessage> MsgQueues;

        public void SendMsg(WebMessage _Msg)
        {

            if (!MyCentorl.DetectNetwork())
            {
                return;
            }
            MsgQueues.Enqueue(_Msg);
            if (!IsWorking)
            {
                SendNextMsg();
            }
        }

        private void SendNextMsg()
        {
            if (MsgQueues.Count > 0)
            {
                IsWorking = true;
                WebMessage msg = MsgQueues.Dequeue();
                MyCentorl.StartCoroutine(RequestMsg(msg));
            }
            else
            {
                IsWorking = false;
            }
        }

        IEnumerator RequestMsg(WebMessage _Msg)
        {
            _Msg.RepeatRequesNum--;
            WWW www = null;
             www = new WWW(_Msg.RequestUrl);
            yield return www;
            if (www.error != null)
            {
                LoggerHelper.Error(www.error+"请求" + _Msg.RequestUrl);
                if (_Msg.RepeatRequesNum >= 0)      //重复请求
                {
                    MyCentorl.StartCoroutine(RequestMsg(_Msg));
                }
                else
                {
                    SendNextMsg();
                }
            }
            else
            {
                DealRequestMsg(www.text, _Msg);
                SendNextMsg();
            }
        }

        private void DealRequestMsg(string return_msg, WebMessage _Msg)
        {
            if (_Msg.MsgRequestBack != null)
            {
                _Msg.MsgRequestBack(return_msg);
            }
        }

    }
}
