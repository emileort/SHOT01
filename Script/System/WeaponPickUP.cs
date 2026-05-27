using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMO
{
    public class WeaponPickUP : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        public void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerCn playerCn;
            PlayerAnimatorManager animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerCn = playerManager.GetComponent<PlayerCn>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            playerCn.rigidbody.velocity = Vector3.zero; // 玩家在撿東西時停止移動。
            animatorHandler.PlayTargetAnimation("Pick Up Item", true); //播放動畫。
            playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);

        }
    }

}
