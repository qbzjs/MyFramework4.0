using UnityEngine;

namespace MyFramework
{
    /// <summary>
    /// web服务器 模块控制器
    /// </summary>
    public class WebServiceModel : ManagerContorBase<WebServiceModel>
    {
        private WebServiceModelDataComp DataComp;
        public override void Load(params object[] _Agr)
        {
            CoroutineComp = AddComp<Model_CoroutineComp>();
            DataComp = AddComp<WebServiceModelDataComp>();
            
            base.Load(_Agr);
        }

        /// <summary>
        /// 检查网络环境
        /// </summary>
        /// <returns></returns>
        public bool DetectNetwork()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)//没有网络
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 请求web服务器
        /// </summary>
        /// <param name="_Msg"></param>
        public void SendMsg(WebMessage _Msg)
        {
            DataComp.SendMsg(_Msg);
        }
    }
}
