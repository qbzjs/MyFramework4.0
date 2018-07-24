namespace MyFramework.SkillSystem
{
    /// <summary>
    /// 技能表现行为 最小的表现单位 技能的最终释放表现就是这些最小行为的集合表现
    /// </summary>
    public abstract class BehaviorBase{

        public BulletBase ParentBullet{
            get;
            private set;
        }

        public BehaviorBase(BulletBase Parent)
        {
            ParentBullet = Parent;
        }
    }
}
