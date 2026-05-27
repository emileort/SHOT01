using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PlayerCn : MonoBehaviour
    {
        CameraHandler cameraHandler;
        
        PlayerManager playerManager;
        PlayerStatsManager playerStats;
        Transform cameraOdject;

        InputHandler inputHandler;

        public Vector3 moveDirection;

        [HideInInspector]

        public Transform myTransform;

        [HideInInspector]

        public PlayerAnimatorManager playerAnimatorManager;
        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        
        //起點到預計落下點的開始點
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        //落下的最小距離計算，並播放動畫所需
        [SerializeField]
        float minmumDistanceNeededToBeGinFall = 1f;
        //光線投射偏移需要距離
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        public LayerMask groundLayer;
        public float inAirTimer;


        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float walkingSpeed = 1;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 45;

        [Header("Stamina Costs")]
        [SerializeField]
        int rollStaminaCost = 15;
        int backstepStaminaCost = 12;
        int sprintStaminaCost = 1;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStatsManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

        }
        void Start()
        {
            cameraOdject = Camera.main.transform;
            myTransform = transform;
            playerManager.isGrounded = true;
            // groundLayer = ~(1 << 8 | 1 << 11);
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }



        #region Movement

        Vector3 normalVector;

        Vector3 targetPosition;


        // 控制轉向
        public void HandleRotation(float delta)
        {

            if (playerAnimatorManager.canRotate)
            {
                if (inputHandler.lockOnFlag)
                {
                    if (inputHandler.sprintFlag || inputHandler.rollFlag)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                        targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }
                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }

                }
                else
                {
                    Vector3 targetDir = Vector3.zero;

                    float moveOverride = inputHandler.moveAmount;


                    // 相機跟隨目標
                    targetDir = cameraOdject.forward * inputHandler.vertical;
                    targetDir += cameraOdject.right * inputHandler.horizontal;

                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero)
                        targetDir = myTransform.forward;

                    float rs = rotationSpeed;

                    Quaternion tr = Quaternion.LookRotation(targetDir);

                    Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

                    myTransform.rotation = targetRotation;
                }
            }

        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag) 
                return;

            if (playerManager.isInteracting) 
                return;

            moveDirection = cameraOdject.forward * inputHandler.vertical;
            moveDirection += cameraOdject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;


            // 跑步與走路的移動速度與移動距離判定

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerStats.TakeStaminaDamage(sprintStaminaCost);
            }
            else
            {
                if (inputHandler.moveAmount <= 0.5)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
                
            }

            // moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                playerAnimatorManager.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                playerAnimatorManager.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }

        }

        public void HandRollingAndSprinting(float delta)
        {
            if (playerAnimatorManager.animator.GetBool("isInteracting"))
                return;

            //確認我們有多少耐力，假如沒有足夠的，就返回
            if (playerStats.currentStamina <= 0)
                return;

            // 操作翻滾與退後判定
            if (inputHandler.rollFlag)
            {
                moveDirection = cameraOdject.forward * inputHandler.vertical;
                moveDirection += cameraOdject.right * inputHandler.horizontal;


                // 當角色移動量>0，判定翻滾
                if (inputHandler.moveAmount > 0)
                {
                    playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                    playerAnimatorManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                    playerStats.TakeStaminaDamage(rollStaminaCost);
                }
                
                // 除此以外都是退後
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Backstep", true);
                    playerAnimatorManager.EraseHandIKForWeapon();
                    playerStats.TakeStaminaDamage(backstepStaminaCost);
                }

            }
        }

        public void HandleFalling(float delta,Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if(Physics.Raycast(origin,myTransform.forward,out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 5f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minmumDistanceNeededToBeGinFall, Color.red, 0.1f, false);
            if(Physics.Raycast(origin,-Vector3.up,out hit, minmumDistanceNeededToBeGinFall, groundLayer))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;


                // 角色狀態判定isGrounded為常態，是否isInAir與isInAir的frame數，判定是否落下動作或做空狀態。
                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for" + inAirTimer);
                        playerAnimatorManager.PlayTargetAnimation("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        playerAnimatorManager.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }

            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if(playerManager.isInteracting == false)
                    {
                        playerAnimatorManager.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }
            //判斷玩家目標位置
            if (playerManager.isGrounded)
            {
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting)
                return;

            //確認我們有多少耐力，假如沒有足夠的，就返回
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.jump_Input)
            {
                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraOdject.forward * inputHandler.vertical;
                    moveDirection += cameraOdject.right * inputHandler.horizontal;
                    playerAnimatorManager.PlayTargetAnimation("Jump", true);
                    playerAnimatorManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                }
            }
        }

        #endregion
    }

}
   
