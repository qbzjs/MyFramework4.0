using UnityEngine;
using System.Collections.Generic;

namespace MyFramework.SkillSystem
{
    /// <summary>
    /// 实体_直线_单体作用子弹
    /// </summary>
    public class Entity_StraightLine_Monomer : BulletBase
    {
        private MGameObject Entity;      //子弹实体
        private float Speed;            //子弹速度
        private float Range;            //子弹射程
        private Vector3 Pos;            //发射坐标
        private Vector3 Direction;      //方向

        public Entity_StraightLine_Monomer(SkillBase Parent, MGameObject _Entity,float _Speed, float _Range)
            :base(Parent)
        {
            Entity = _Entity;
        }

        /// <summary>
        /// 发射接口
        /// </summary>
        public override void Launch(params object[] _Agr)
        {
            if (_Agr.Length != 2 || _Agr[0] is Vector3 || _Agr[1] is Vector3)
            {
                LoggerHelper.Error("子弹发射参数错误 Bullet:Entity_StraightLine_Monomer");
                return;
            }
            Pos = (Vector3)_Agr[0];
            Direction = (Vector3)_Agr[1];
            Entity.FixedUpdateDelegate += FixedUpdate;
            Entity.OnTriggerEnterDelegate += OnTriggerEnter;
        }

        private void FixedUpdate()
        {
            Entity.transform.position = Entity.transform.position + Direction * Speed * Time.fixedDeltaTime;
        }

        /// <summary>
        /// 子弹碰撞事件
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Wall")
            {
                OnDextory();
            }
            else if (other.tag == "Actor")
            {
                OnDextory();
            }
        }

        protected override void OnDextory()
        {
            base.OnDextory();
            MGameObject.Destroy(Entity);
        }

    }
}
