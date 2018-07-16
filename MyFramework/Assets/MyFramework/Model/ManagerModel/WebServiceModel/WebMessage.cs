using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework
{
    public delegate void MsgRequestBack(string ReturnData);

    /// <summary>
    /// web消息体
    /// </summary>
    public class WebMessage
    {
        /// <summary>
        /// 消息重复请求次数
        /// </summary>
        public int RepeatRequesNum;
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl;
        /// <summary>
        /// 是否显示等待界面
        /// </summary>
        public bool IsShowWaitView = false;

        /// <summary>
        /// 请求的json数据
        /// </summary>
        public JSONClass RequestData;
        /// <summary>
        /// 请求返回
        /// </summary>
        public MsgRequestBack MsgRequestBack;

        /// <summary>
        /// 创建web请求对象
        /// </summary>
        /// <param name="_Service"></param>
        /// <param name="_RequestData"></param>
        /// <param name="_BackCall"></param>
        public WebMessage(string _RequestUrl, JSONClass _RequestData, MsgRequestBack _BackCall)
        {
            RequestUrl = _RequestUrl;
            RequestData = _RequestData;
            MsgRequestBack = _BackCall;
        }
    }
}
