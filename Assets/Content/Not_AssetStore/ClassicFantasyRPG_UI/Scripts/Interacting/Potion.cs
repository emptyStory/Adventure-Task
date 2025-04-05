using System;
using UnityEngine;
using UnityEngine.UI;

namespace Interacting
{
    public class Potion : MonoBehaviour
    {
        public float potionCooldown = 1f;
        public float restoreHealthValue = 30f;
        public int maxPotionCount = 3;
        public Durable durable;
        public Text textCounter;
        private bool _textSet;
        private int _potionsLeft;
        private float _ready;

        private void Start()
        {
            enabled = durable != null;
            _textSet = textCounter != null;
            _potionsLeft = maxPotionCount;
            if (_textSet)
            {
                textCounter.text = Convert.ToString(_potionsLeft);
            }
        }

        public void Use()
        {
            if (_potionsLeft <= 0 || Time.time < _ready)
            {
                return;
            }

            _potionsLeft--;
            durable.Restore(restoreHealthValue);
            _ready = Time.time + potionCooldown;
            if (_textSet)
            {
                textCounter.text = Convert.ToString(_potionsLeft);
            }
        }
    }
}