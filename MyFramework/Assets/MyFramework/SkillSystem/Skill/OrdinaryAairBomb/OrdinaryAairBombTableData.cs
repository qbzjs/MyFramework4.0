using UnityEngine;

namespace MyFramework.SkillSystem
{
    [System.Serializable]
    public class OrdinaryAairBombData : SkillDataBase<int>
    {
        public float CDTime;                //技能CD时间
        public GameObject BulletPrefab;     //子弹预制对象
        public float BulletSpeed;           //子弹运动速度
        public float Range;                 //子弹射程
        public float Injury;                //技能伤害值
    }

    public class OrdinaryAairBombTableData: SkillTableDataBase<int, OrdinaryAairBombData>
    {

    }
}
