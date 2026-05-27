using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;
        
        public Transform playerStandingPosition;
        public GameObject itemSpawner;
        public WeaponItem itemInChest;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
        }
        public override void Interact(PlayerManager playerManager)
        {
            // 向著玩家的寶箱，面對才能打開
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            // 看玩家座標是否能與寶箱互動
            playerManager.OpenChestInteraction(playerStandingPosition);

            // 開啟寶箱，並播放玩家的開啟寶箱動畫
            animator.Play("Chest Open");
            StartCoroutine(SpawnItemInChest());

            // 撿取寶箱中的物品，且寶箱維持

            WeaponPickUP weaponPickUP = itemSpawner.GetComponent<WeaponPickUP>();

            if (weaponPickUP != null)
            {
                weaponPickUP.weapon = itemInChest;
            }

        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
        }
    }
}
