using System;
using Core.UI;
using UnityTools;

namespace Core.Level
{
    public class AreaLabelsService : IDisposable
    {
        private const string LabelFormat = "< {0} {1} >";
        private const float LabelDisplayDuration = 5f;
        
        private readonly AreaLabelsCollection _labels;
        private readonly AreaLabelContainer _container;
        private float _offset;
        private int _nextLandNumber = 1;

        public AreaLabelsService(AreaLabelsCollection labels, AreaLabelContainer container)
        {
            _labels = labels;
            _offset = Platform.Length * 0.5f;
            _container = container;
        }

        public void Dispose() => 
            _nextLandNumber = 1;

        public void CheckDistance(float positionX, int landPlatformNumber)
        {
            float nextLandPosition = landPlatformNumber * Platform.Length * _nextLandNumber;
                
            if (positionX + _offset <= nextLandPosition)
                return;

            string label = GetLabel();
            DisplayLabel(label);

            _nextLandNumber++;
        }

        private string GetLabel()
        {
            string firstPart = _labels.FirstParts.GetRandomItem<string>();
            string lastPart = _labels.LastParts.GetRandomItem<string>();
            return string.Format(LabelFormat, firstPart, lastPart);
        }

        private void DisplayLabel(string label)
        {
            _container.SetLabel(label);
            _container.TweenDisplay(LabelDisplayDuration);
        }
    }
}