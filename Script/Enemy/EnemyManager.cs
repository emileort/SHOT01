using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EMO
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStatsManager enemyStatsManager;
        EnemyEffectsManager enemyEffectsManager;

        public NavMeshAgent navMeshAgent;
        public State currentState;
        public CharacterStatsManager currentTarget;
        public Rigidbody enemyRigidBody;

        public bool isPreformingAction;

        public float distanceFromTarget;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        [Header("AI設定")]
        public float detectionRadius = 20;
        // 最高與最低，可視的角度與距離範圍
        public float maximumDirectionAngle = 50;
        public float minimumDirectionAngle = -50;
        public float viewableAngle;
        public float currentRecoveryTime = 0;

        [Header("AI姿態設定")]

        public bool allowAIToPerformCombos;
        public bool isPhaseShifting;
        public float comboLikelyHood;

        protected override void Awake()
        {
            base.Awake();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            enemyRigidBody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
        }

        private void Start()
        {
            enemyRigidBody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();

            isUsingLeftHand = enemyAnimatorManager.animator.GetBool("isUsingLeftHand");
            isUsingRightHand = enemyAnimatorManager.animator.GetBool("isUsingRightHand");
            isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");

            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
            isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
            enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);
            canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
            canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
        }

        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            enemyEffectsManager.HandleAllBuildUpEffects();
        }

        // 死後不旋轉與站起來
        private void HandleStateMachine()
        {
            if (enemyStatsManager.isDead)
                return;

            else if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwithToNextState(nextState);
                }
            }
        }

        private void SwithToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPreformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPreformingAction = false;
                }
            }

        }

    }
}
