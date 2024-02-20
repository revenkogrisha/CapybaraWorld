using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class VersionText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmp;

        [Space]
        [SerializeField] private string _prefix = "версия ";

        private void Start()
        {
            string version = Application.version;
            _tmp.text = $"{_prefix}{version}";
        }
    }
}
