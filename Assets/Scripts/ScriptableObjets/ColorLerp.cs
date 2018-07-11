using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SOExample.DataObjects
{ 
    [CreateAssetMenu]
    public class ColorLerp : ScriptableObject
    {
        [SerializeField] Color ColorA, ColorB;
        [SerializeField] int NumberOfSteps;

        [SerializeField] Color[] colors;
        public Color[] Colors { get { return colors; } }

        [SerializeField] Material[] materials;
        public Material[] Materials { get { return materials; } }


        void OnEnable()
        {
            Clear();
            Initialize();
        }

        public void Initialize()
        {
            colors = new Color[NumberOfSteps + 1];
            float step = 1.0f / NumberOfSteps;

            Debug.LogWarning(step);

            Debug.Log(step);
            for (int i = 0; i <= NumberOfSteps; ++i)
            {
                colors[i] = Color.Lerp(ColorA, ColorB, i * step);

            }

            InitializeMaterials();
        }

        public void Clear()
        {
            colors = null;
            materials = null;
        }

        public void InitializeMaterials()
        {
            materials = new Material[colors.Length];

            for (int i = 0; i < materials.Length; ++i)
            {
                materials[i] = new Material(Shader.Find("Standard"))
                {
                    color = colors[i],
                    enableInstancing = true
                };
            }
        }
    }
}
