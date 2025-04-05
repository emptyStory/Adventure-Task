using System;
using UnityEngine;

namespace Interacting
{
    public class RangeAttack : IAttack
    {
        private static readonly int AttackAnimatorString = Animator.StringToHash("Attack");
        public Animator animator;
        public float attackRate = 0.8f;
        public int attackDamage = 10;
        public GameObject bulletPrefab;
        public Transform attackPoint;
        public Transform gameObjectToDetectFacing;
    
        private float _nextAttackTime;
        public override void Attack()
        {
            if (Time.time < _nextAttackTime) return;
            animator.SetTrigger(AttackAnimatorString);
            _nextAttackTime = Time.time + attackRate;
            var rightFacing = gameObjectToDetectFacing.localScale.x > 0;
            var bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.Euler(0f, 0f, rightFacing? 0f : 180f));
            bullet.GetComponent<Projectile>().AttackDamage = attackDamage;
        }

        public override void CancelAttack()
        {
            animator.ResetTrigger(AttackAnimatorString);
        }

        public override void ApplyDamage()
        {
            throw new NotImplementedException();
        }
    }
}