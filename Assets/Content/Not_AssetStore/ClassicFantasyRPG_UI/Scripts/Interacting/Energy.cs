using UnityEngine;
using UnityEngine.UI;

namespace Interacting
{
    public class Energy : MonoBehaviour
    {
        public Slider energySlider;
        public float maxEnergy = 20f;
        public float initialEnergy = 20f;
        public float restoreRate = 1f;

        private float _currentEnergy;
        private bool _sliderSet;

        private void Start()
        {
            _currentEnergy = Mathf.Min(initialEnergy, maxEnergy);
            _sliderSet = energySlider != null;
            if (!_sliderSet) return;
            energySlider.maxValue = maxEnergy;
            energySlider.minValue = 0f;
            energySlider.value = _currentEnergy;
        }

        private void Update()
        {
            if (!(_currentEnergy < maxEnergy)) return;
            _currentEnergy = Mathf.Min(maxEnergy, _currentEnergy + Time.deltaTime * restoreRate);
            if (!_sliderSet) return;
            energySlider.value = _currentEnergy;
        }

        public bool ConsumeEnergyIfPossible(float energyAmount)
        {
            if (energyAmount > _currentEnergy)
            {
                return false;
            }

            _currentEnergy = _currentEnergy - energyAmount;
            if (!_sliderSet) return true;
            energySlider.value = _currentEnergy;
            return true;
        }
    }
}