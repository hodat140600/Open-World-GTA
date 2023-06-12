using System.Collections;
using System.Linq;
using _GAME._Scripts._Camera;
using _GAME._Scripts._CharacterController._Actions;
using _GAME._Scripts.Game;
using _NEWGAME._Scripts.Game;
using Assets._SDK.Logger;
using UniRx;
using UnityEngine;

namespace _GAME._Scripts._CharacterController
{
    [ClassHeader("THIRD PERSON CONTROLLER", iconName = "ThirdPersonExtensions/controllerIcon")]
    public class ThirdPersonController : ThirdPersonAnimator
    {
        #region Custom code

        private       GenericAnimation _genericAnimation;
        private const string           DANCE_HIGHEST_SCORE = "Dance";
        private const string           END_DAY             = "Victory";
        private const string           OUT_AMMO            = "Sad Idle";

        protected override void Start()
        {
            base.Start();
            this.gameObject.AddComponent<SkinManager>().SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            _genericAnimation = GetComponent<GenericAnimation>();
            GameManager.Instance.CurrentState
                .Where(state => state == GameState.Playing)
                .Subscribe(_ => StartCoroutine(SubscribeGameplay()))
                .AddTo(this);
        }

        public void OutAmmo()
        {
            PlayAnim(OUT_AMMO);
        }

        private ShooterMeleeInput      _input;
        private ThirdPersonCameraState _frontCameraState;
        private ThirdPersonCameraState _defaultCameraState;

        public void PlayAnimHighestScore()
        {
            SfxManager.Instance.Play(Sounds.SeeTinh);
            UpdateCameraToLookAtTommy();
            PlayAnim(DANCE_HIGHEST_SCORE);
        }

        private void UpdateCameraToLookAtTommy()
        {
            RetrieveCameraData();
            _input.ChangeCameraState("Front");
            _frontCameraState.fixedAngle.x = transform.eulerAngles.y + 180;
        }

        private IEnumerator ResetCamera()
        {
            RetrieveCameraData();
            _frontCameraState.fixedAngle.x = transform.eulerAngles.y;
            yield return new WaitForSeconds(.6f);
            _input.ChangeCameraState("Default", false);
        }

        private void RetrieveCameraData()
        {
            _input              ??= GetComponent<ShooterMeleeInput>();
            _frontCameraState   ??= _input.tpCamera.CameraStateList.tpCameraStates.First(state => state.Name == "Front");
        }

        public void PlayAnimEndDay()
        {
            UpdateCameraToLookAtTommy();
            PlayAnim(END_DAY);
        }

        private void PlayAnim(string clip)
        {
            _genericAnimation.animationClip = clip;
            _genericAnimation.PlayAnimation();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            MissionManager.OnScoreUpdated += ActionOnDayEnded;
        }

        private IEnumerator SubscribeGameplay()
        {
            yield return new WaitForSeconds(2f);

            onDead.AddListener(_ => StartCoroutine(DieAfter(2)));
            Gameplay.Instance.MissionManager.CurrentMissionState.Where(state => state == MissionState.Running)
                .Subscribe(_ =>
                {
                    ChangeHealth(100);
                   StartCoroutine( ResetCamera());
                })
                .AddTo(this);

            MessageBroker.Default.Receive<NpcKilledEvent>().Subscribe((enemySkilled) =>
            {
                if (enemySkilled.IsHeadshot)
                    SfxManager.Instance.Play(Sounds.Headshot);
            }).AddTo(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            MissionManager.OnScoreUpdated -= ActionOnDayEnded;
        }

        private void ActionOnDayEnded(ScoreInfo info)
        {
            if (info.IsNewHighscore)
                PlayAnimHighestScore();
            else
                PlayAnimEndDay();
        }

        // to run die animation first
        private IEnumerator DieAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            Gameplay.Instance.MissionManager.Fire(MissionTrigger.Fail);
        }

        #endregion


        /// <summary>
        /// Move the controller to a specific Position, you must Lock the Input first 
        /// </summary>
        /// <param name="targetPosition"></param>
        public virtual void MoveToPosition(Transform targetPosition)
        {
            MoveToPosition(targetPosition.position);
        }

        /// <summary>
        /// Move the controller to a specific Position, you must Lock the Input first 
        /// </summary>
        /// <param name="targetPosition"></param>
        public virtual void MoveToPosition(Vector3 targetPosition)
        {
            Vector3 dir = targetPosition - transform.position;
            dir.y = 0;
            /*dir = dir.normalized * Mathf.Min(1f, dir.magnitude);*/ /*That is to make smootly stop*/

            if (dir.magnitude < 0.1f)
            {
                input         = Vector3.zero;
                moveDirection = Vector3.zero;
            }
            else
            {
                input         = transform.InverseTransformDirection(dir.normalized);
                moveDirection = dir.normalized;
            }
        }

        /// <summary>
        /// Handle RootMotion movement and specific Actions
        /// </summary>       
        public virtual void ControlAnimatorRootMotion()
        {
            if (!enabled)
            {
                return;
            }

            if (isRolling)
            {
                RollBehavior();
                return;
            }

            if (customAction || lockAnimMovement)
            {
                StopCharacterWithLerp();
                transform.position = animator.rootPosition;
                transform.rotation = animator.rootRotation;
            }

            if (useRootMotion)
            {
                MoveCharacter(moveDirection);
            }
        }

        /// <summary>
        /// Set the Controller movement speed (rigidbody, animator and root motion)
        /// </summary>
        public virtual void ControlLocomotionType()
        {
            if (lockAnimMovement || lockMovement || customAction)
            {
                return;
            }

            if (!lockSetMoveSpeed)
            {
                if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing || locomotionType.Equals(LocomotionType.OnlyFree))
                {
                    SetControllerMoveSpeed(freeSpeed);
                    SetAnimatorMoveSpeed(freeSpeed);
                }
                else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing)
                {
                    isStrafing = true;
                    SetControllerMoveSpeed(strafeSpeed);
                    SetAnimatorMoveSpeed(strafeSpeed);
                }
            }

            if (!useRootMotion)
            {
                MoveCharacter(moveDirection);
            }
        }

        /// <summary>
        /// Manage the Control Rotation Type of the Player
        /// </summary>
        public virtual void ControlRotationType()
        {
            if (lockAnimRotation || lockRotation || customAction || isRolling)
            {
                return;
            }

            bool validInput = input != Vector3.zero || (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

            if (validInput)
            {
                if (lockAnimMovement)
                {
                    // calculate input smooth
                    inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                }

                Vector3 dir = (isStrafing && isGrounded && (!isSprinting || sprintOnlyFree == false) || freeSpeed.rotateWithCamera && input == Vector3.zero) && rotateTarget ? rotateTarget.forward : moveDirection;

                //RotationTest(dir);

                RotateToDirection(dir);
            }
        }

        /// <summary>
        /// Use it to keep the direction the Player is moving (most used with CCV camera)
        /// </summary>
        public virtual void ControlKeepDirection()
        {
            // update oldInput to compare with current Input if keepDirection is true
            if (!keepDirection)
            {
                oldInput = input;
            }
            else if ((input.magnitude < 0.01f || Vector3.Distance(oldInput, input) > 0.9f) && keepDirection)
            {
                keepDirection = false;
            }
        }

        /// <summary>
        /// Determine the direction the player will face based on input and the referenceTransform
        /// </summary>
        /// <param name="referenceTransform"></param>
        public virtual void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (isRolling && !rollControl /*|| input.magnitude <= 0.01*/)
            {
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                return;
            }

            if (referenceTransform && !rotateByWorld)
            {
                //get the right-facing direction of the referenceTransform
                var right = referenceTransform.right;
                right.y = 0;
                //get the forward direction relative to referenceTransform Right
                var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
                // determine the direction the player will face based on input and the referenceTransform's right and forward directions
                moveDirection = inputSmooth.x * right + inputSmooth.z * forward;
            }
            else
            {
                moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
            }
        }

        /// <summary>
        /// Set the isSprinting bool and manage the Sprint Behavior 
        /// </summary>
        /// <param name="value"></param>
        public virtual void Sprint(bool value)
        {
            // EM TESTE FORAM REMOVIDOS ALGUMAS CONDI��ES
            //var sprintConditions = (currentStamina > 0 && hasMovementInput && isGrounded &&
            //    !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f) && !sprintOnlyFree));

            var sprintConditions = (!isCrouching || !inCrouchArea && CanExitCrouch()) && currentStamina > 0 && hasMovementInput &&
                                   !(isStrafing && (horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f) && !sprintOnlyFree);

            if (value && sprintConditions)
            {
                if (currentStamina > (finishStaminaOnSprint ? sprintStamina : 0) && hasMovementInput)
                {
                    finishStaminaOnSprint = false;
                    if (isGrounded && useContinuousSprint)
                    {
                        isCrouching = false;
                        isSprinting = !isSprinting;
                        if (isSprinting)
                        {
                            OnStartSprinting.Invoke();

                            // TESTE PARA SAIR DO WALKONLY QUANDO SPRINTA
                            alwaysWalkByDefault = false;
                        }
                        else
                        {
                            OnFinishSprinting.Invoke();
                        }
                    }
                    else if (!isSprinting)
                    {
                        OnStartSprinting.Invoke();

                        // TESTE PARA SAIR DO WALKONLY QUANDO SPRINTA
                        alwaysWalkByDefault = false;
                        isSprinting         = true;
                    }
                }
                else if (!useContinuousSprint && isSprinting)
                {
                    if (currentStamina <= 0)
                    {
                        finishStaminaOnSprint = true;
                        OnFinishSprintingByStamina.Invoke();
                    }

                    isSprinting = false;
                    OnFinishSprinting.Invoke();
                }
            }
            else if (isSprinting && (!useContinuousSprint || !sprintConditions))
            {
                if (currentStamina <= 0)
                {
                    finishStaminaOnSprint = true;
                    OnFinishSprintingByStamina.Invoke();
                }

                isSprinting = false;
                OnFinishSprinting.Invoke();
            }
        }

        /// <summary>
        /// Manage the isCrouching bool
        /// </summary>
        public virtual void Crouch()
        {
            if (isGrounded && !customAction)
            {
                AutoCrouch();
                if (isCrouching && CanExitCrouch())
                {
                    isCrouching = false;
                }
                else
                {
                    isCrouching = true;
                    isSprinting = false;
                }
            }
        }

        /// <summary>
        /// Set the isStrafing bool
        /// </summary>
        public virtual void Strafe()
        {
            isStrafing = !isStrafing;
        }

        /// <summary>
        /// Triggers the Jump Animation and set the necessary variables to make the Jump behavior in the <seealso cref="ThirdPersonMotor"/>
        /// </summary>
        /// <param name="consumeStamina">Option to consume or not the stamina</param>
        public virtual void Jump(bool consumeStamina = false)
        {
            // trigger jump behaviour
            jumpCounter = jumpTimer;
            OnJump.Invoke();

            // trigger jump animations
            if (input.sqrMagnitude < 0.1f)
            {
                StartCoroutine(DelayToJump());
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            }
            else
            {
                isJumping = true;
                animator.CrossFadeInFixedTime("JumpMove", .2f);
            }

            // reduce stamina
            if (consumeStamina)
            {
                ReduceStamina(jumpStamina, false);
                currentStaminaRecoveryDelay = 1f;
            }
        }

        protected IEnumerator DelayToJump()
        {
            yield return new WaitForSeconds(jumpStandingDelay);
            isJumping = true;
        }

        /// <summary>
        /// Triggers the Roll Animation and set the stamina cost for this action
        /// </summary>
        public virtual void Roll()
        {
            OnRoll.Invoke();
            isRolling = true;
            animator.CrossFadeInFixedTime("Roll", rollTransition, baseLayer);
            ReduceStamina(rollStamina, false);
            currentStaminaRecoveryDelay = 2f;
        }


        #region Check Action Triggers

        /// <summary>
        /// Call this in OnTriggerEnter or OnTriggerStay to check if enter in triggerActions     
        /// </summary>
        /// <param name="other">collider trigger</param>                         
        protected override void OnTriggerStay(Collider other)
        {
            try
            {
                CheckForAutoCrouch(other);
            }
            catch (UnityException e)
            {
                Debug.LogWarning(e.Message);
            }

            base.OnTriggerStay(other);
        }

        /// <summary>
        /// Call this in OnTriggerExit to check if exit of triggerActions 
        /// </summary>
        /// <param name="other"></param>
        protected override void OnTriggerExit(Collider other)
        {
            AutoCrouchExit(other);
            base.OnTriggerExit(other);
        }

        #endregion
    }
}