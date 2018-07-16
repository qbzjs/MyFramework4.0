using UnityEditor;
using UnityEngine;

namespace MyFramework
{
    [CustomEditor(typeof(BezierPath))]
    public class BezierPathEditor : Editor
    {
        BezierPath Path;
        bool KeyPointFoldou = false;
        int KeyPointCount = 0;
        bool IsRefresh = false;
        bool IsInit = false;
        int RefreshIndex = -1;
        Vector3 KeyPoint;
        Vector3 KeyPointMove;
        private void OnEnable()
        {
            Path = (BezierPath)target;
        }


        public override void OnInspectorGUI()
        {
            KeyPointFoldou = EditorGUILayout.Foldout(KeyPointFoldou, "曲线控制点");
            if (KeyPointFoldou)
            {
                KeyPointCount = EditorGUILayout.IntField("Size ：", Path.KeyPoint.Count);
                int i = 0;
               
                for (i = 0; i < KeyPointCount; i++)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    if (i >= Path.KeyPoint.Count)
                    {
                        Path.KeyPoint.Add(new Vector3());
                         if (Path.KeyPoint.Count > 1)
                         {
                            Path.Lines.Add(new BezierPath.BezierLine(Path.KeyPoint[i - 1], Path.KeyPoint[i], Path.LineLeng));
                            IsInit = true;
                         }
                    }
                    KeyPoint = EditorGUILayout.Vector3Field("控制点:", Path.KeyPoint[i]);
                    if (KeyPoint != Path.KeyPoint[i])
                    {
                        KeyPointMove = KeyPoint - Path.KeyPoint[i];
                        Path.KeyPoint[i] = KeyPoint;
                        RefreshIndex = i;
                        IsRefresh = true;
                        IsInit = true;
                    }
                    GUILayout.EndVertical();
                }
                if (i < Path.KeyPoint.Count)
                {
                    Path.KeyPoint.RemoveRange(i, Path.KeyPoint.Count - i);
                    Path.Lines.RemoveRange(i, Path.KeyPoint.Count - i);
                    IsInit = true;
                }
            }
            int  _LineLeng = EditorGUILayout.IntField("曲线平滑度 ：", Path.LineLeng);
            if (Path.LineLeng != _LineLeng)
            {
                Path.LineLeng = _LineLeng;
                IsInit = true;
            }
        }

        void OnSceneGUI()
        {
            if (Path.KeyPoint != null && Path.KeyPoint.Count > 0)
            {
                for (int i = 0; i < Path.KeyPoint.Count; i++)
                {
                    Handles.color = Color.white;
                    KeyPoint = Handles.FreeMoveHandle(Path.KeyPoint[i], Quaternion.identity, 1.0f, Vector3.zero, Handles.SphereHandleCap);
                    Handles.Label(Path.KeyPoint[i] + new Vector3(0, -0.1f, 0), i.ToString());
 
                    if (KeyPoint != Path.KeyPoint[i])
                    {
                        KeyPointMove = KeyPoint - Path.KeyPoint[i];
                        Path.KeyPoint[i] = KeyPoint;
                        IsRefresh = true;
                        IsInit = true;
                        RefreshIndex = i;
                    }
                    if (i > 0 && i <= Path.Lines.Count)
                    {
                        BezierPath.BezierLine lastline = Path.Lines[i - 1];
                        Handles.color = Color.green;
                        Vector3 SAnchor = Handles.FreeMoveHandle(lastline.SatrtAnchor, Quaternion.identity, 0.5f, Vector3.zero, Handles.CubeHandleCap);
                        Handles.Label(SAnchor + new Vector3(0, -0.1f, 0), "S");
                        Vector3 EAnchor = Handles.FreeMoveHandle(lastline.EndAnchor, Quaternion.identity, 0.5f, Vector3.zero, Handles.CubeHandleCap);
                        Handles.Label(EAnchor + new Vector3(0, -0.1f, 0), "E");
                        if (IsInit || (IsRefresh && RefreshIndex == i) || SAnchor != lastline.SatrtAnchor || EAnchor != lastline.EndAnchor)
                        {
                            if (IsRefresh && RefreshIndex == i)
                            {
                                EAnchor += KeyPointMove;
                            }
                            lastline.UpdataLine(Path.KeyPoint[i - 1], SAnchor, Path.KeyPoint[i], EAnchor, Path.LineLeng);
                            IsInit = true;
                        }
                        Handles.color = Color.blue;
                        Handles.DrawLine(SAnchor, lastline.Satrt);
                        Handles.DrawLine(EAnchor, lastline.End);
                    }
                    if (i < (Path.KeyPoint.Count - 1) && i < Path.Lines.Count)
                    {
                        BezierPath.BezierLine nextline = Path.Lines[i];
                        Handles.color = Color.green;
                        Vector3 SAnchor = Handles.FreeMoveHandle(nextline.SatrtAnchor, Quaternion.identity, 0.5f, Vector3.zero, Handles.CubeHandleCap);
                        Handles.Label(SAnchor + new Vector3(0, -0.1f, 0), "S");
                        Vector3 EAnchor = Handles.FreeMoveHandle(nextline.EndAnchor, Quaternion.identity, 0.5f, Vector3.zero, Handles.CubeHandleCap);
                        Handles.Label(EAnchor + new Vector3(0, -0.1f, 0), "E");
                        if (IsInit || (IsRefresh && RefreshIndex == i) || SAnchor != nextline.SatrtAnchor || EAnchor != nextline.EndAnchor)
                        {
                            if (IsRefresh && RefreshIndex == i)
                            {
                                SAnchor += KeyPointMove;
                            }
                            nextline.UpdataLine(Path.KeyPoint[i], SAnchor, Path.KeyPoint[i + 1], EAnchor, Path.LineLeng);
                            IsInit = true;
                        }
                        Handles.color = Color.blue;
                        Handles.DrawLine(SAnchor, nextline.Satrt);
                        Handles.DrawLine(EAnchor, nextline.End);
                    }
                }
                KeyPointMove = Vector3.zero;
                RefreshIndex = -1;
                IsRefresh = false;
                if (IsInit)
                {
                    Path.Init();
                    IsInit = false;
                }
                Handles.color = Color.yellow;
                for (int n = 0; n < Path.PathPoints.Count - 1; n++)
                {
                    Handles.DrawLine(Path.PathPoints[n], Path.PathPoints[n + 1]);
                }
            }
        }
    }
}
