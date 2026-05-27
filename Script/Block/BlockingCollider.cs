using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockingCollider;

        public float blockingPhysicalDamageAbsarption;

        public float blockingFireDamageAbsorption;

        private void Awake()
        {
            blockingCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsarption(WeaponItem weapon)
        {
            if (weapon != null)
            {
                blockingPhysicalDamageAbsarption = weapon.physicalDamageAbsorption;
            }
        }
        public void EnableBlockingCollider()
        {
            blockingCollider.enabled = true;
        }

        public void DisableBlockingCollider()
        {
            blockingCollider.enabled = false;
        }
    }
}
