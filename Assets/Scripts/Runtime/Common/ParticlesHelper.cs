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

        private Dictionary<ParticlesName, ParticleSystem> _particles;
        private CancellationTokenSource _cts;

        public ParticlesHelper(ParticlesCollection collection)
        {
            _particles = collection.Particles
                .ToDictionary(item => item.Name, item => item.ParticlePrefab);
        }

        public void Dispose() => 
            _cts.Clear();

        public async UniTaskVoid Spawn(ParticlesName name,
            Vector3 position, 
            float duration = Duration)
        {
            try
            {
                _cts = new();
                ParticleSystem particle = NightPool.Spawn(_particles[name],
                    position,
                    Quaternion.identity);

                particle.Play();

                await UniTaskUtility.Delay(duration, _cts.Token);
                
                particle.Stop();
                particle.Clear();

                NightPool.Despawn(particle);
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(ParticlesHelper)}::{nameof(Spawn)}: {ex.Message} \n {ex.StackTrace}");
            }
            finally
            {
                _cts.Clear();
                _cts = null;
            }
        }
    }
}