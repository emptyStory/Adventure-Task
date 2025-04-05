using UnityEngine;

namespace Interacting
{
    public abstract class IAttack : MonoBehaviour
    {
        public abstract void Attack();
        public abstract void CancelAttack();
        public abstract void ApplyDamage();
    }
}