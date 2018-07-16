using UnityEngine;
using UnityEditor;

namespace MyFramework
{
    [CustomEditor(typeof(EquilateralPolygonImage))]
    public class EquilateralPolygonImageEditor : Editor
    {
        EquilateralPolygonImage Image;

        /// <summary>
         /// 初始化,绑定各变量
         /// </summary>
        private void OnEnable()
        {
            Image = (EquilateralPolygonImage) target;
        }

        public override void OnInspectorGUI()
        {
            Image.color = EditorGUILayout.ColorField("颜色",Image.color);
            Image.EdgesNumber = EditorGUILayout.IntField("边数", Image.EdgesNumber);
            Image.Radius = EditorGUILayout.FloatField("半径", Image.Radius);
            Image.CreateMesh();
        }
    }
}


