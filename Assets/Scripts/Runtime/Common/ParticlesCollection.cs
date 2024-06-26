using System.Linq;
using TriInspector;
using UnityEngine;

namespace Core.Common
{
    [CreateAssetMenu(fileName = "Particles Collection", menuName = "Collections/Particles")]
    public class ParticlesCollection : ScriptableObject
    {
        [ValidateInput(nameof(ValidateDublicates)), ListDrawerSettings(AlwaysExpanded = true)]
        [SerializeField] private NamedParticle[] _particles;

        public NamedParticle[] Particles => _particles;

        private TriValidationResult ValidateDublicates()
        {
            bool isUnique = _particles.GroupBy(x => x.Name).All(grouping => grouping.Count() == 1);

            if (isUnique == true)
                return TriValidationResult.Valid;
            else
                return TriValidationResult.Warning("Duplicates found!");
        }
    }
}