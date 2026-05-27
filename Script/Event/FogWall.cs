using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class FogWall : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(true);
        }

        public void ActivateFogWall()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateFogWall()
        {
            gameObject.SetActive(false);
        }

    }
}
