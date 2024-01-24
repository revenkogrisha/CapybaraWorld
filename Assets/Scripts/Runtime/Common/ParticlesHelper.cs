using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Editor.Debugger;
using Core.Other;
using Cysharp.Threading.Tasks;
using NTC.Pool;
using UnityEngine;

namespace Core.Common
{
    public class ParticlesHelper : IDisposable
    {
        private const float Duration = 5f;

        private readonly Dictionary<ParticlesName, ParticleSystem> _particles;
        private readonly List<ParticleSystem> _spawnedParticles = new();
        private CancellationTokenSource _commonCTS;

        public ParticlesHelper(ParticlesCollection collection)
        {
            _particles = collection.Particles
                .ToDictionary(item => item.Name, item => item.ParticlePrefab);
        }

        public void Initialize() =>
            _commonCTS = new();

        public void Dispose()
        {
            _commonCTS.Clear();

            foreach (ParticleSystem particles in _spawnedParticles.ToList())
                Despawn(particles);
        }

        public async UniTaskVoid Spawn(ParticlesName name,
            Vector3 position, 
            float duration = Duration)
        {
            try
            {
                ParticleSystem particle = NightPool.Spawn(_particles[name],
                    position,
                    Quaternion.identity);

                particle.Play();

                _spawnedParticles.Add(particle);

                await UniTaskUtility.Delay(duration, _commonCTS.Token);
                
                Despawn(particle);
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(ParticlesHelper)}::{nameof(Spawn)}: {ex.Message} \n {ex.StackTrace}");
            }
        }

        private void Despawn(ParticleSystem particle)
        {
            particle.Stop();
            particle.Clear();

            NightPool.Despawn(particle);

            _spawnedParticles.Remove(particle);
        }
    }
}