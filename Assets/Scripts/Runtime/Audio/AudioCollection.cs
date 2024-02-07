using System.Linq;
using TriInspector;
using UnityEngine;

namespace Core.Audio
{
    [CreateAssetMenu(fileName = "Audio Collection", menuName = "Collections/Audio")]
    public class AudioCollection : ScriptableObject
    {
        [ValidateInput(nameof(ValidateDublicates)), ListDrawerSettings(AlwaysExpanded = true)]
        [SerializeField] private NamedAudio[] _audio;

        public NamedAudio[] Audio => _audio;

        private TriValidationResult ValidateDublicates()
        {
            bool isUnique = _audio.GroupBy(x => x.Name).All(grouping => grouping.Count() == 1);

            if (isUnique == true)
                return TriValidationResult.Valid;
            else
                return TriValidationResult.Warning("Duplicates found!");
        }
    }
}
