using UnityEngine;

namespace Interacting
{
    public class HeavyAttack : MonoBehaviour
    {
        private static readonly int AbilityAnimatorString = Animator.StringToHash("Ability");

        public MeleeAttack meleeAttack;
        public int attackDamage = 15;
        public Animator animator;
        public float rate = 1.1f;
        public float energyCost = 10f;
        public Energy energy;

        private bool _activated;
        private float _nextPerformTime;

        private void Start()
        {
            enabled = meleeAttack != null && energy != null;
            _activated = false;
        }

        private void FixedUpdate()
        {
            if (!_activated) return;
            _activated = false;
            if (!(Time.time > _nextPerformTime) || !meleeAttack.IsAbleToAttack()) return;
            if (!energy.ConsumeEnergyIfPossible(energyCost)) return;
            animator.SetTrigger(AbilityAnimatorString);
        }

        public void Cancel()
        {
            animator.ResetTrigger(AbilityAnimatorString);
            _activated = false;
        }

        public void ApplyAbilityDamage()
        {
            var collider2Ds = new Collider2D[10];
            var size = Physics2D.OverlapCircleNonAlloc(meleeAttack.attackPoint.position, meleeAttack.attackRadius, collider2Ds, meleeAttack.affects);
            for (var i = 0; i < size; i++)
            {
                var durable = collider2Ds[i].GetComponent<Durable>();
                if (durable != null && durable.enabled)
                {
                    durable.ApplyDamage(attackDamage);
                }
            }

            _nextPerformTime = Time.time + rate;
        }

        public void Perform()
        {
            _activated = true;
        }
    
    }
}