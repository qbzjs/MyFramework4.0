using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.SkillSystem
{
    /// <summary>
    /// 普通伤害行为 
    /// </summary>
    public class OrdinaryInjuryBehavior: BehaviorBase
    {
        public float Injury
        {
            get;
            private set;
        }

        public OrdinaryInjuryBehavior(BulletBase Parent, float _Injury)
            :base(Parent)
        {
            Injury = _Injury;
        }
    }
}
