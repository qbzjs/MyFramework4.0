using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyFramework
{
    [CustomEditor(typeof(BezierBase))]
    public class BezierBaseEditor : Editor
    {
        BezierBase Bezier;
        bool KeyPointFoldou = false;
        List<Vector3> KeyPoint = new List<Vector3>();
        int KeyPointCount = 0;
  

        /// <summary>
        /// 初始化,绑定各变量
        /// </summary>
        private void OnEnable()
        {
            Bezier = (BezierBase)target;
            KeyPoint = new List<Vector3>(Bezier.KeyPoint);
        }

        public override void OnInspectorGUI()
        {
            KeyPointFoldou = EditorGUILayout.Foldout(KeyPointFoldou, "曲线控制点");
            if (KeyPointFoldou)
            {
                KeyPointCount = EditorGUILayout.IntField("Size ：", KeyPoint.Count);
                int i = 0;
                for (i = 0; i < KeyPointCount; i++)
                {
                    if (i >= KeyPoint.Count)
                    {
                        KeyPoint.Add(new Vector3());
                    }
                    KeyPoint[i] = EditorGUILayout.Vector3Field("控制点:", KeyPoint[i]);
                }
                if (i < KeyPoint.Count)
                {
                    KeyPoint.RemoveRange(i, KeyPoint.Count-i);
                }
                Bezier.KeyPoint = KeyPoint.ToArray();
            }
            Bezier.LineLeng = EditorGUILayout.IntField("曲线平滑度 ：", Bezier.LineLeng);
            Bezier.Init();
        }

        void OnSceneGUI()
        {
            Handles.color = Color.blue;
            if (Bezier.LinePoints != null && Bezier.LinePoints.Length > 0)
            {
                for (int i = 0; i < Bezier.LinePoints.Length - 1; i++)
                {
                    Handles.DrawLine(Bezier.LinePoints[i], Bezier.LinePoints[i + 1]);
                }
            }
 
            if (KeyPoint != null || KeyPoint.Count > 0)
            {
                for (int i = 0; i < KeyPoint.Count; i++)
                {
                    Handles.color = Color.white;
                    KeyPoint[i] = Handles.FreeMoveHandle(KeyPoint[i], Quaternion.identity, 0.1f, Vector3.zero,Handles.SphereHandleCap);
                    Handles.Label(KeyPoint[i]+new Vector3(0,-0.1f,0), i.ToString());
                }
                Bezier.KeyPoint = KeyPoint.ToArray();
            }
            Bezier.Init();
        }
    }
}
