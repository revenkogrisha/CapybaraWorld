using System;
using TriInspector;
using UnityEngine;

namespace Core.Common
{
    [Serializable]
    public struct NamedParticle
    {
        public ParticlesName Name;
        [Required] public ParticleSystem ParticlePrefab;
    }
}