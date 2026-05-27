using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class WeaponFX : MonoBehaviour
    {
        [Header("ªZ¾¹¯S®Ä")]
        public ParticleSystem normalWeaponTrail;

        public void PlayWeaponFX()
        {
            normalWeaponTrail.Stop();
            if (normalWeaponTrail.isStopped)
            {
                normalWeaponTrail.Play();
            }
        }
    }
}
