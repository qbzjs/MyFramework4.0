using UnityEngine;

namespace MyFramework
{
    /// <summary>
    /// 扩展工具类 用于Transform 数学计算
    /// </summary>
    public static class TransformMath
    {
        /// <summary>
        /// 判定攻击目标是否在攻击者圆形攻击范围类
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static bool IsInRound(Transform source, Transform target, float radius)
        {
            float STDistance = Vector3.Distance(source.position, target.position);
            if (STDistance > radius)
                return false;
            else
                return true;
        }


        /// <summary>
        /// 判定攻击目标是否在攻击者扇形攻击范围类
        /// </summary>
        /// <param name="source">攻击者</param>
        /// <param name="target">被击者</param>
        /// <param name="radius">扇形半径</param>
        /// <param name="angle">扇形角度</param>
        /// <returns></returns>
        public static bool IsInSector(Transform source, Transform target, float radius, float angle)
        {
            float STDistance = Vector3.Distance(source.position, target.position);
            if (STDistance > radius)
                return false;
            float _angle = Vector3.Angle(source.forward, (target.position - source.position));
            if (_angle > angle / 2.0f)
                return false;
            return true;
        }
    }
}
