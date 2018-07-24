using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyFramework.SkillSystem
{
    /// <summary>
    /// 普通光弹技能
    /// </summary>
    public class OrdinaryAairBomb : SkillBase
    {
        protected new OrdinaryAairBombData Config;
        private CoroutineTask ReleaseCoroutineTask;
        private Vector3 LaunchPos;
        private Vector3 LaunchDirection;
        public OrdinaryAairBomb(OrdinaryAairBombData _Config)
            :base(_Config)
        {
            Config = _Config;
            Status = SkillStatus.Sleeping;
        }

        public SkillReleaseStatus Release(Vector3 Pos,Vector3 _Direction)
        {
            SkillReleaseStatus ReleaseStatus = base.Release();
            if (ReleaseStatus == SkillReleaseStatus.Success)
            {
                Status = SkillStatus.Releaseding;
                LaunchPos = Pos;
                LaunchDirection = _Direction;
                ReleaseCoroutineTask = CoroutineModel.Instance.StartCoroutineTask(ReleaseProcess());
            }
            return ReleaseStatus;
        }

        protected override IEnumerator ReleaseProcess()
        {
            MGameObject Bulletobj = GameObject.Instantiate(Config.BulletPrefab).AddComponent<MGameObject>();
            Entity_StraightLine_Monomer Bullet = new Entity_StraightLine_Monomer(this, Bulletobj, Config.BulletSpeed, Config.Range);
            List<BehaviorBase> Behaviors = new List<BehaviorBase>{ new OrdinaryInjuryBehavior(Bullet, Config.Injury)};
            Bullet.BuildBehaviors(Behaviors);
            Bullet.Launch(LaunchPos, LaunchDirection);
            yield return 1;
            Status = SkillStatus.Sleeping;
        }

    }
}
