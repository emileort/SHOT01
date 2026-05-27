using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class IllusionaryWall : MonoBehaviour
    {
        public bool wallHasBeenHit;
        public Material illusionaryWallMaterial;
        public float alpha;
        public float fadeTimer = 2.5f;
        public Collider wallCollider;

        public AudioSource audioSource;
        public AudioClip illusuonaryWallSound;


        private void Update()
        {
            if (wallHasBeenHit)
            {
                FadeIllusionaryWall();
            }
        }

        public void FadeIllusionaryWall()
        {
            alpha = illusionaryWallMaterial.color.a;
            alpha = alpha - Time.deltaTime / fadeTimer;
            Color fadeWallColor = new Color(1, 1, 1, alpha);
            illusionaryWallMaterial.color = fadeWallColor;

            if (wallCollider.enabled)
            {
                wallCollider.enabled = false;
                audioSource.PlayOneShot(illusuonaryWallSound);
            }

            if (alpha <= 0)
            {
                Destroy(this);
            }
        }
    }
}
