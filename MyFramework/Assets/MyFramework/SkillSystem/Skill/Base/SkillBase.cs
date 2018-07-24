using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.SkillSystem
{
    public enum SkillStatus
    {
        Loading,            //加载中
        Sleeping,           //休眠中
        Releaseding,        //释放中
        CDing,              //冷却中
    }

    public enum SkillReleaseStatus
    {
        Success,
        Failure
    }

    public abstract class SkillBase
    {
        protected ISkillDataBase Config;
        public SkillStatus Status { get; protected set; }
        protected List<BulletBase> Bullets { get; private set; }
        public SkillBase(ISkillDataBase _Config)
        {
            Config = _Config;
            Bullets = new List<BulletBase>();
        }

        /// <summary>
        /// 技能释放接口 拥有多态变化 基类只实现不参数定义
        /// </summary>
        /// <returns>释放状态返回</returns>
        public virtual SkillReleaseStatus Release()
        {
            if(Status == SkillStatus.Sleeping)
                return SkillReleaseStatus.Success;
            else
                return SkillReleaseStatus.Failure;
        }


        /// <summary>
        /// 技能释放过程 通过携程管理模块执行
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator ReleaseProcess();

        public virtual void DextoryBullet(BulletBase Bullet)
        {
            if (Bullets.Contains(Bullet))
            {
                Bullets.Remove(Bullet);
            }
        }
    }
}
