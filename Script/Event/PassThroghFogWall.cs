using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PassThroghFogWall : Interactable
    {
        WorldEventManager worldEventManager;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            playerManager.PassThroghFogWallInteraction(transform);
            worldEventManager.ActivateBossFight();
        }

    }
}
