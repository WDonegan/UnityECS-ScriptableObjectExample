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
        }

        public void Clear()
        {
            colors = null;
        }

    }
}
