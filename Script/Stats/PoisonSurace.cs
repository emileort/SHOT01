using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PoisonSurace : MonoBehaviour
    {
        public float poisonBuildUpAmount = 7;

        public List<CharacterEffectsManager> characterInsidePoisonSurface;

        private void OnTriggerEnter(Collider other)
        {
            CharacterEffectsManager character = other.GetComponent<CharacterEffectsManager>();
            if (character != null)
            {
                characterInsidePoisonSurface.Add(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterEffectsManager character = other.GetComponent<CharacterEffectsManager>();
            if (character != null)
            {
                characterInsidePoisonSurface.Remove(character);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            foreach(CharacterEffectsManager character in characterInsidePoisonSurface)
            {
                if (character.isPoisoned)
                    return;
                character.poisonBuildup = character.poisonBuildup + poisonBuildUpAmount * Time.deltaTime;
            }
        }

    }
}
