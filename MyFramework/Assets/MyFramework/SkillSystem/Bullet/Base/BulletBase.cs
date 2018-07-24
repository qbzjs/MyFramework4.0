using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.SkillSystem
{
    /// <summary>
    /// 技能子弹 1运动轨迹 2行为载体 3触发检测 4自我销毁
    /// </summary>
    public abstract class BulletBase
    {
        public SkillBase ParentSkill
        {
            get;
            protected set;
        }
        protected List<BehaviorBase> Behaviors;

        /// <summary>
        /// 子弹生成方法 多态变化
        /// </summary>
        public BulletBase(SkillBase Parent)
        {
            ParentSkill = Parent;
        }

        /// <summary>
        /// 组装子弹
        /// </summary>
        public virtual void BuildBehaviors(List<BehaviorBase> _Behaviors)
        {
            Behaviors = _Behaviors;
        }


        /// <summary>
        /// 子弹发射接口 多态变化 根据具体表现实现多态接口
        /// </summary>
        public virtual void Launch(params object[] _Agr)
        {

        }

        protected virtual void OnDextory()
        {
            ParentSkill.DextoryBullet(this);
        }

    }
}
