using UnityEngine;

namespace Interacting
{
    public class MeleeAttack : IAttack
    {
        private static readonly int AttackAnimatorString = Animator.StringToHash("Attack");
    
        public Transform attackPoint;
        public float attackRadius = 1.6f;
        public LayerMask affects;
        public int attackDamage = 5;
        public Animator animator;
        public float attackRate = 0.8f;
        public Rigidbody2D rb;

        private bool _attack;
        private float _nextAttackTime;

        private void FixedUpdate()
        {
            if (!_attack || !(Time.time > _nextAttackTime) || !IsAbleToAttack()) return;
            animator.SetTrigger(AttackAnimatorString);
            _attack = false;
        }

        public override void CancelAttack()
        {
            animator.ResetTrigger(AttackAnimatorString);
            _attack = false;
        }

        public override void ApplyDamage()
        {
            var collider2Ds = new Collider2D[10];
            var size = Physics2D.OverlapCircleNonAlloc(attackPoint.position, attackRadius, collider2Ds, affects);
            for (var i = 0; i < size; i++)
            {
                var durable = collider2Ds[i].GetComponent<Durable>();
                if (durable != null && durable.enabled)
                {
                    durable.ApplyDamage(attackDamage);
                }
            }

            _nextAttackTime = Time.time + attackRate;
        }

        public bool IsAbleToAttack()
        {
            return Mathf.Abs(rb.velocity.y) < 0.04f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        public override void Attack()
        {
            _attack = true;
        }
    
    }
}