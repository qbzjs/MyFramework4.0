using UnityEngine;
using System.Collections.Generic;

namespace MyFramework
{

    public class BezierPath : MonoBehaviour
    {
        [System.Serializable]
        public class BezierLine
        {
            public Vector3 Satrt;
            public Vector3 End;
            public Vector3 SatrtAnchor;
            public Vector3 EndAnchor;
            public int LineLeng;
            //过滤掉起点与终点
            public Vector3[] LinePoints;

            public BezierLine(Vector3 _Satrt, Vector3 _End,int _LineLeng = 5)
            {
                Satrt = _Satrt;
                SatrtAnchor = _Satrt + (_End - _Satrt) * 0.25f;
                End = _End;
                EndAnchor = _End + (_Satrt - _End) * 0.25f;
                LineLeng = _LineLeng;
                Init();
            }
            public BezierLine(Vector3 _Satrt, Vector3 _SatrtAnchor, Vector3 _End, Vector3 _EndAnchor,int _LineLeng = 5)
            {
                Satrt = _Satrt;
                SatrtAnchor = _SatrtAnchor;
                End = _End;
                EndAnchor = _EndAnchor;
                LineLeng = _LineLeng;
                Init();
            }
            public void UpdataLine(Vector3 _Satrt, Vector3 _SatrtAnchor, Vector3 _End, Vector3 _EndAnchor, int _LineLeng = 5)
            {
                Satrt = _Satrt;
                SatrtAnchor = _SatrtAnchor;
                End = _End;
                EndAnchor = _EndAnchor;
                LineLeng = _LineLeng;
                Init();
            }

            private void Init()
            {
                LinePoints = new Vector3[LineLeng];
                for (int i = 1; i < LinePoints.Length+1; i++)
                {
                    LinePoints[i-1] = BezierCurve(Satrt, SatrtAnchor, EndAnchor, End, i*1.0f / (LinePoints.Length+1));
                }
            }

            private Vector3 BezierCurve(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
            {
                Vector3 B = Vector3.zero;
                float t1 = (1 - t) * (1 - t) * (1 - t);
                float t2 = (1 - t) * (1 - t) * t;
                float t3 = t * t * (1 - t);
                float t4 = t * t * t;
                B = P0 * t1 + 3 * t2 * P1 + 3 * t3 * P2 + P3 * t4;
                return B;
            }
        }

        public int LineLeng = 5;
        public List<Vector3> KeyPoint = new List<Vector3>();
        public List<BezierLine> Lines = new List<BezierLine>();
        public List<Vector3> PathPoints = new List<Vector3>();
        public void Init()
        {
            if (Lines != null)
            {
                PathPoints.Clear();
                for (int i = 0; i < Lines.Count; i++)
                {
                    PathPoints.Add(KeyPoint[i]);
                    PathPoints.AddRange(Lines[i].LinePoints);
                }
                PathPoints.Add(KeyPoint[KeyPoint.Count - 1]);
            }
        }


    }
}
