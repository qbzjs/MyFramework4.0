using UnityEngine;
using System;
using System.Collections.Generic;

namespace MyFramework
{
    /// <summary>
    /// 场景模块控制器
    /// </summary>
    public class SceneModel : ManagerContorBase<SceneModel>
    {
        private SceneChedulerComp  ChangeSceneComp;
        public override void Load(params object[] _Agr)
        {
            if (_Agr.Length != 1 || !(_Agr[0] is IScenesChedulerBase))
            {
                LoggerHelper.Error("SceneChedulerComp 加载参数不对!");
                return;
            }
            IScenesChedulerBase Cheduler = _Agr[0] as IScenesChedulerBase;
            CoroutineComp = AddComp<Model_CoroutineComp>();
            ResourceComp = AddComp<Model_ResourceComp>();
            ChangeSceneComp = AddComp<SceneChedulerComp>(Cheduler);
            base.Load(_Agr);
        }
        public override void Start(params object[] _Agr)
        {
            base.Start(_Agr);
        }


        /// <summary>
        /// 跳转场景
        /// </summary>
        /// <param name="SceneId"></param>
        /// <param name="CallBack"></param>
        public void ChangeScene (ISceneLoadCompBase SceneLoadComp)
        {
            ChangeSceneComp.ChangeScene(SceneLoadComp);
        }
    }
}
