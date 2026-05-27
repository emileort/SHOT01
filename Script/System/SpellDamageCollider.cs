using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        bool hasCollided = false;

        CharacterStatsManager spellTarget;
        Rigidbody rigidbody;

        Vector3 impactNormal; // 用旋轉的子彈

        protected override void Awake()
        {
            damageCollider = GetComponent<SphereCollider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
            projectileParticles.transform.parent = transform;

            if (muzzleParticles)
            {
                //子彈存在多久
                muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
                Destroy(muzzleParticles, 2f);
            }

        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!hasCollided)
            {
                spellTarget = collision.transform.GetComponent<CharacterStatsManager>();

                if (spellTarget != null && spellTarget.teamIDNumber != teamIDNumber)
                {
                    spellTarget.TakeDamage(0, fireDamage, currentDamageAnimation);
                }

                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 2f);
                Destroy(gameObject, 1f);
            }
        }
    }

}
