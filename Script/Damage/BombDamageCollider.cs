using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class BombDamageCollider : DamageCollider
    {
        [Header("Ãz¬µ¶Ë®`&½d³ò")]
        public int explosiveRadius = 1;
        public int explosionDamage;
        public int explosionSplashDamage;
        // Å]ªk¶Ë®`
        // ¥ú©ú¶Ë®`

        public Rigidbody bombRigidbody;
        private bool hasCollided = false;
        public GameObject impactParticles;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            bombRigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasCollided)
            {
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
                Explode();

                CharacterStatsManager character = collision.transform.GetComponent<CharacterStatsManager>();

                if (character != null)
                {
                    //½T»{Ãz¬µ¤õµK
                    if (character.teamIDNumber != teamIDNumber)
                    {
                        character.TakeDamage(0, explosionDamage, currentDamageAnimation);
                    }
                }
                Destroy(impactParticles, 3f);
                Destroy(transform.parent.gameObject);
            }
        }

        private void Explode()
        {
            Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);

            foreach(Collider objectInExplosion in characters)
            {
                CharacterStatsManager character = objectInExplosion.GetComponent<CharacterStatsManager>();

                if (character != null)
                {
                    // ¤õµK¶Ë®`
                    if (character.teamIDNumber != teamIDNumber)
                    {
                        character.TakeDamage(0, explosionSplashDamage, currentDamageAnimation);
                    }
                }
            }
        }
    }
}
