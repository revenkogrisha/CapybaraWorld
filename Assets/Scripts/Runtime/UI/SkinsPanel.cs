using System;
using System.Collections.Generic;
using System.Linq;
using Core.Player;
using NTC.Pool;
using UniRx;
using UnityEngine;

namespace Core.UI
{
    public class SkinsPanel : MonoBehaviour
    {
        [SerializeField] private SkinItemView _itemPrefab;

        [Space]
        [SerializeField] private Transform _itemsRoot;
        
        private List<SkinItemView> _items;
        
        public readonly ReactiveCommand<SkinName> ItemDisplayCommand = new();

        public void CreateItems(IEnumerable<SkinPreset> presets)
        {
            if (_items == null)
                _items = new(presets.Count());
            else if (_items.Count > 0)
                ClearItems();

            foreach (SkinPreset preset in presets)
            {
                SkinItemView item = NightPool.Spawn(_itemPrefab, _itemsRoot);

                item.Initialize(this, preset.Name, preset.MenuItem);
                _items.Add(item);
            }
        }

        public void InvokeItemDisplay(SkinName skinName) =>
            ItemDisplayCommand.Execute(skinName);

        private void ClearItems()
        {
            foreach (SkinItemView item in _items)
                NightPool.Despawn(item);

            _items.Clear();
        }
    }
}