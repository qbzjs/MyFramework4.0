using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyFramework
{
    public class VariantImage : Graphic
    {
        public Vector2 CommonVect;
        public List<Vector2> Pos;

        void Update()
        {
            SetAllDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (Pos.Count < 3)
            {
                return;
            }
            Color32 color32 = color;
            vh.Clear();
            List<Vector2> Verts = Initialize(CommonVect, Pos);
            for (int i = 0; i < Verts.Count; i++)
            {
                vh.AddVert(Verts[i], color32, new Vector2(0f, 0f));
            }

            for (int i = 1; i < Verts.Count - 1; i++)
            {
                vh.AddTriangle(0, i , i + 1);
            }
        }

        /// <summary>
        /// 点集合封闭成简单多边形
        /// </summary>
        /// <param name="commonVect">公共顶点</param>
        /// <param name="Vects">除公共顶点以外的顶点集合</param>
        /// <returns></returns>
        public static List<Vector2> Initialize(Vector2 commonVect, List<Vector2> Vects)
        {
            if (Vects.Count < 2)
            {
                LoggerHelper.Error("简单多边形创建 参数无效");
                return null;
            }
            List<Vector2> tmp = new List<Vector2>(Vects);
            tmp.Sort((Vector2 a, Vector2 b) =>
            {
                float ret = PointCmp(a, b, commonVect);
                if (ret > 0)
                {
                    return 1;
                }
                else if (ret < 0)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            });
            for (int i = 0; i < Vects.Count; i++)
            {
                Debug.Log("i=" + i + "pos = " + Vects[i].ToString());
            }
            List<Vector2> Polygon = new List<Vector2>();
            Polygon.Add(commonVect);
            Polygon.AddRange(tmp);
            return Polygon;
        }

        //若点a大于点b,即点a在点b顺时针方向,返回true,否则返回false
        static float PointCmp(Vector2 a, Vector2 b, Vector2 center)
        {
            Vector2 vect1 = (a - center);
            Vector2 vect2 = (b - center);
            float p1 = Vector2.Angle(Vector2.up, vect1);
            float p2 = Vector2.Angle(Vector2.up, vect2);
            return p1 - p2;
        }
    }
}


