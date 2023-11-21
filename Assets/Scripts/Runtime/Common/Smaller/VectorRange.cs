using System;
using UnityEngine;

namespace Core.Other
{
    [Serializable]
    public class VectorRange
    {
        public Vector2 Minimum { get; set; }
        public Vector2 Maximum { get; set; }

        public VectorRange(Vector2 minimum, Vector2 maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }
    }
}
