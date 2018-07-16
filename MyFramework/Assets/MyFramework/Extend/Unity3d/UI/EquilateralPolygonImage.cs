using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyFramework
{
    public class EquilateralPolygonImage: Graphic
    {
        public int EdgesNumber;
        public float Radius;
        private List<Vector2> Vectors;

        void Update()
        {
            SetAllDirty();
        }

        public void CreateMesh()
        {
            Vectors = new List<Vector2>();
            Vectors.Add(Vector2.zero);
            for (int i = 0; i < EdgesNumber; i++)
            {
                Vector3 v = new Vector3(Radius*Mathf.Cos((Mathf.PI*2) * 72*i/360.0f), Radius * Mathf.Sin(Mathf.PI * 2 * 72 * i / 360.0f));
                Vectors.Add(v);
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            Color32 color32 = color;
            for (int i = 0; i < Vectors.Count; i++)
            {
                vh.AddVert(Vectors[i], color32, Vectors[i] + Vector2.one * 0.5f);
            }
            for (int i = 0; i < Vectors.Count; i++)
            {
                vh.AddTriangle(0, i, (i + 1)% Vectors.Count);
            }
        }
    }
}
