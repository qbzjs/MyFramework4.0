using System;
using UnityEngine;

namespace MyFramework
{
    /// <summary>
    /// N阶贝塞尔曲线实现
    /// </summary>
    public class BezierBase : MonoBehaviour
    {
        public Vector3[] KeyPoint;
        public int LineLeng = 10;
        public Vector3[] LinePoints;

        public void Init()
        {
            if (KeyPoint == null || KeyPoint.Length <= 0)
                return;
            LinePoints = Calculate(KeyPoint, LineLeng);
        }

        /// <summary>
        /// 计算费塞尔曲线
        /// </summary>
        /// <param name="poss">曲线控制点</param>
        /// <param name="precision">曲线生成精度</param>
        /// <returns></returns>
        public Vector3[] Calculate(Vector3[] poss, int precision)
        {
            int number = poss.Length;
            if (number < 2 )
                return null;
            Vector3[] result = new Vector3[precision+1];
            int[] mi = new int[number];
            mi[0] = mi[1] = 1;
            for (int i = 3; i <= number; i++)
            {

                int[] t = new int[i - 1];
                for (int j = 0; j < t.Length; j++)
                {
                    t[j] = mi[j];
                }

                mi[0] = mi[i - 1] = 1;
                for (int j = 0; j < i - 2; j++)
                {
                    mi[j + 1] = t[j] + t[j + 1];
                }
            }
            for (int i = 0; i < precision; i++)
            {
                float t = (float)i / precision;
                for (int j = 0; j < 3; j++)
                {
                    float temp = 0.0f;
                    for (int k = 0; k < number; k++)
                    {
                        temp += (float)(Math.Pow(1 - t, number - k - 1) * poss[k][j] * Math.Pow(t, k) * mi[k]);
                    }
                    result[i][j] = temp;
                }
            }
            result[precision] = poss[number - 1];
            return result;
        }

    }
}
