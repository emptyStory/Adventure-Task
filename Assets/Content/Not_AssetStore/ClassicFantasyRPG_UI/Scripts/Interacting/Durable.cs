using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Interacting
{
    public class Durable : MonoBehaviour
    {
        private static readonly int DeadAnimatorString = Animator.StringToHash("Dead");
        private static readonly int HurtAnimatorString = Animator.StringToHash("Hurt");
        private static readonly int HealAnimatorString = Animator.StringToHash("Heal");
    
        public float maxHealth = 10;
        public float blockDamageMultiplier = 0.1f;
        public MonoBehaviour[] componentsToDisableOnDeath;
        public GameObject[] gameObjectsToDisableOnDeath;
        public Animator animator;
        public Slider healthSlider;
        public Block block;
        public UnityEvent deadEvent;
        public bool Dead { get; private set; }

        public float CurrentHealth { get; private set; }

        private bool _sliderSet;
        private bool _couldBlock;

        public void ApplyDamage(float damage)
        {
            animator.SetTrigger(HurtAnimatorString);
            damage = ReduceDamageIfBlocking(damage);
            CurrentHealth = CurrentHealth - damage;
            if (_sliderSet)
            {
                healthSlider.value = CurrentHealth;
            }
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void Restore(float value)
        {
            animator.SetTrigger(HealAnimatorString);
            CurrentHealth = Mathf.Min(CurrentHealth + value, maxHealth);
            if (_sliderSet)
            {
                healthSlider.value = CurrentHealth;
            }
        }

        private float ReduceDamageIfBlocking(float damage)
        {
            if (_couldBlock && block.Blocking)
            {
                damage = damage * blockDamageMultiplier;
            }

            return damage;
        }

        private void Die()
        {
            foreach (var component in componentsToDisableOnDeath)
            {
                component.enabled = false;
            }
            
            foreach (var component in gameObjectsToDisableOnDeath)
            {
                component.SetActive(false);
            }

            animator.SetBool(DeadAnimatorString, true);
            GetComponent<Collider2D>().enabled = false;
            enabled = false;

            Dead = true;
            deadEvent?.Invoke();
        }

        private void Start()
        {
            CurrentHealth = maxHealth;
            _couldBlock = block != null;
        
            InitHealthSlider();
            Dead = false;
        }

        private void InitHealthSlider()
        {
            _sliderSet = healthSlider != null;
            if (!_sliderSet) return;
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }
}