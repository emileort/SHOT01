using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        PlayerStatsManager playerStatsManager;

        [Header("裝備模型切換")]
        HelmetModelChanger helmetModelChanger;
        TorsoModelChanger torsoModelChanger;
        HandModelChanger handModelChanger;
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;

        [Header("軀幹防禦裝備")]
        public string nakeHeadModel;
        public string nakeTorsoModel;
        public string nakeHipModel;
        public string nakeHandModel;
        public string nakeLeftLegModel;
        public string nakeRightLegModel;



        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            playerInventory = GetComponent<PlayerInventory>();
            playerStatsManager = GetComponent<PlayerStatsManager>();

            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            handModelChanger = GetComponentInChildren<HandModelChanger>();
        }

        private void Start()
        {
            EquipAllEquipmentModelsOnStart();
        }

        public void EquipAllEquipmentModelsOnStart()
        {
            helmetModelChanger.UnEquipAllHelmetModels();
            if (playerInventory.currentHelmetEquipment != null)
            {
                helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
                playerStatsManager.physicalDamageAbsorptionHead = playerInventory.currentHelmetEquipment.physicalDefense;
                Debug.Log("頭部吸收" + playerStatsManager.physicalDamageAbsorptionHead + "%");
            }
            else
            {
                helmetModelChanger.EquipHelmetModelByName(nakeHeadModel);
                playerStatsManager.physicalDamageAbsorptionHead = 0;
            }
            
            torsoModelChanger.UnEquipAllTorsoModels();

            if (playerInventory.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
                playerStatsManager.physicalDamageAbsorptionBody = playerInventory.currentTorsoEquipment.physicalDefense;
                Debug.Log("盔甲吸收" + playerStatsManager.physicalDamageAbsorptionBody + "%");
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakeTorsoModel);
                playerStatsManager.physicalDamageAbsorptionBody = 0;
            }

            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLeftLegModels();
            rightLegModelChanger.UnEquipAllRightLegModels();

            if (playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLeftLegModelByName(playerInventory.currentLegEquipment.leftLegName);
                rightLegModelChanger.EquipRightLegModelByName(playerInventory.currentLegEquipment.rightLegName);
                playerStatsManager.physicalDamageAbsorptionLegs = playerInventory.currentLegEquipment.physicalDefense;
                Debug.Log("腿甲吸收" + playerStatsManager.physicalDamageAbsorptionLegs + "%");
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakeHipModel);
                leftLegModelChanger.EquipLeftLegModelByName(nakeLeftLegModel);
                rightLegModelChanger.EquipRightLegModelByName(nakeRightLegModel);
                playerStatsManager.physicalDamageAbsorptionLegs = 0;
            }

            handModelChanger.UnEquipAllHandModels();

            if (playerInventory.currentHandEquipment != null)
            {
                handModelChanger.EquipHandModelByName(playerInventory.currentHandEquipment.handModelName);
                playerStatsManager.physicalDamageAbsorptionHand = playerInventory.currentHandEquipment.physicalDefense;
                Debug.Log("手甲吸收" + playerStatsManager.physicalDamageAbsorptionHand + "%");
            }
            else
            {
                handModelChanger.EquipHandModelByName(nakeHandModel);
                playerStatsManager.physicalDamageAbsorptionHand = 0;
            }
            
        }

        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsarption(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsarption(playerInventory.leftWeapon);
            }

            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
