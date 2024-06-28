using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Unity.FPS.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Tooltip("Sensitivity multiplier for moving the camera around")]
        public float LookSensitivity = 1f;

        [Tooltip("Additional sensitivity multiplier for WebGL")]
        public float WebglLookSensitivityMultiplier = 0.25f;

        [Tooltip("Limit to consider an input when using a trigger on a controller")]
        public float TriggerAxisThreshold = 0.4f;

        [Tooltip("Used to flip the vertical input axis")]
        public bool InvertYAxis = false;

        [Tooltip("Used to flip the horizontal input axis")]
        public bool InvertXAxis = false;

        GameFlowManager m_GameFlowManager;
        PlayerCharacterController m_PlayerCharacterController;
        bool m_FireInputWasHeld;

        [SerializeField] PlayerInput playerInput;
        private Vector2 moveVector;
        private Vector2 lookVector;
        private bool jumpPressed;
        private bool jumpHeld;
        private bool fireButtonPressed;
        private bool fireAltButtonPressed;
        private bool reloadPressed;
        private int mouseScroll;
        private bool crouchPressed;
        private bool sprintPressed;
        private bool tabPressed;

        void Start()
        {

            playerInput = GetComponent<PlayerInput>();

            m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, PlayerInputHandler>(
                m_PlayerCharacterController, this, gameObject);
            m_GameFlowManager = FindObjectOfType<GameFlowManager>();
            DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, PlayerInputHandler>(m_GameFlowManager, this);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (playerInput)
            {
                playerInput.actions.FindAction("Move").performed += MovePerformed;
                playerInput.actions.FindAction("Move").canceled += MoveCancelled;
                playerInput.actions.FindAction("Look").performed += LookPerformed;
                playerInput.actions.FindAction("Look").canceled += LookCancelled;
                playerInput.actions.FindAction("Jump").performed += JumpPerformed;
                playerInput.actions.FindAction("Jump").canceled += JumpCancelled;
                playerInput.actions.FindAction("Fire").performed += FirePerformed;
                playerInput.actions.FindAction("Fire").canceled += FireCancelled;
                playerInput.actions.FindAction("FireAlt").performed += FireAltPerformed;
                playerInput.actions.FindAction("FireAlt").canceled += FireAltCancelled;
                playerInput.actions.FindAction("Reload").performed += ReloadPerformed;
                playerInput.actions.FindAction("Reload").canceled += ReloadCancelled;
                playerInput.actions.FindAction("SwitchWeapon").performed += MouseWheelPerformed;
                playerInput.actions.FindAction("SwitchWeapon").canceled += MouseWheelCancelled;
                playerInput.actions.FindAction("Crouch").performed += CrouchPerformed;
              //  playerInput.actions.FindAction("Crouch").canceled += CrouchCancelled;
                playerInput.actions.FindAction("Sprint").performed += SprintPerformed;
              //  playerInput.actions.FindAction("Sprint").canceled += SprintCancelled;
                playerInput.actions.FindAction("Pause").performed += PausePerformed;
                playerInput.actions.FindAction("Pause").canceled += PauseCancelled;
            }
        }


        private void OnDisable()
        {
            if (playerInput)
            {
                playerInput.actions.FindAction("Move").performed -= MovePerformed;
                playerInput.actions.FindAction("Move").canceled -= MoveCancelled;
                playerInput.actions.FindAction("Look").performed -= LookPerformed;
                playerInput.actions.FindAction("Look").canceled -= LookCancelled;
                playerInput.actions.FindAction("Jump").performed -= JumpPerformed;
                playerInput.actions.FindAction("Jump").canceled -= JumpCancelled;
                playerInput.actions.FindAction("Fire").performed -= FirePerformed;
                playerInput.actions.FindAction("Fire").canceled -= FireCancelled;
                playerInput.actions.FindAction("FireAlt").performed -= FireAltPerformed;
                playerInput.actions.FindAction("FireAlt").canceled -= FireAltCancelled;
                playerInput.actions.FindAction("Reload").performed -= ReloadPerformed;
                playerInput.actions.FindAction("Reload").canceled -= ReloadCancelled;
                playerInput.actions.FindAction("SwitchWeapon").performed -= MouseWheelPerformed;
                playerInput.actions.FindAction("SwitchWeapon").canceled -= MouseWheelCancelled;
                playerInput.actions.FindAction("Crouch").performed -= CrouchPerformed;
             //   playerInput.actions.FindAction("Crouch").canceled -= CrouchCancelled;
                playerInput.actions.FindAction("Sprint").performed -= SprintPerformed;
             //   playerInput.actions.FindAction("Sprint").canceled -= SprintCancelled;
                playerInput.actions.FindAction("Pause").performed -= PausePerformed;
                playerInput.actions.FindAction("Pause").canceled -= PauseCancelled;
            }
        }

        private void MovePerformed(InputAction.CallbackContext obj)
        {
            moveVector = obj.ReadValue<Vector2>();
        }

        private void MoveCancelled(InputAction.CallbackContext obj)
        {
            moveVector = Vector2.zero;
        }

        private void LookPerformed(InputAction.CallbackContext obj)
        {
            lookVector = obj.ReadValue<Vector2>();
        }

        private void LookCancelled(InputAction.CallbackContext obj)
        {
            lookVector = Vector2.zero;
        }

        private void JumpPerformed(InputAction.CallbackContext obj)
        {
            jumpPressed = true;
        }

        private void JumpCancelled(InputAction.CallbackContext obj)
        {
            jumpPressed = false;
        }

        private void FirePerformed(InputAction.CallbackContext obj)
        {
            fireButtonPressed = true;
        }

        private void FireCancelled(InputAction.CallbackContext obj)
        {
            fireButtonPressed = false;
        }

        private void FireAltPerformed(InputAction.CallbackContext obj)
        {
            fireAltButtonPressed = true;
        }

        private void FireAltCancelled(InputAction.CallbackContext obj)
        {
            fireAltButtonPressed = false;
        }

        private void ReloadPerformed(InputAction.CallbackContext obj)
        {
            reloadPressed = true;
        }

        private void ReloadCancelled(InputAction.CallbackContext obj)
        {
            reloadPressed = false;
        }

        private void MouseWheelPerformed(InputAction.CallbackContext obj)
        {
            float rawMouse = obj.ReadValue<float>();

            if(rawMouse > 0)
            {
                mouseScroll = 1;
            }
            else if(rawMouse < 0)
            {
                mouseScroll = -1;
            }
            else
            {
                mouseScroll = 0;
            }
        }

        private void MouseWheelCancelled(InputAction.CallbackContext obj)
        {
            mouseScroll = 0;
        }

        private void CrouchPerformed(InputAction.CallbackContext obj)
        {
            crouchPressed = !crouchPressed;
        }

        //private void CrouchCancelled(InputAction.CallbackContext obj)
        //{
        //    crouchPressed = false;
        //}

        private void SprintPerformed(InputAction.CallbackContext obj)
        {
            sprintPressed = !sprintPressed;
        }

       // private void SprintCancelled(InputAction.CallbackContext obj)
       // {
       ////     sprintPressed = false;
       // }

        private void PausePerformed(InputAction.CallbackContext obj)
        {
            tabPressed = true;
        }

        private void PauseCancelled(InputAction.CallbackContext obj)
        {
            tabPressed = false;
        }

        public bool TabPressed()
        {
            return tabPressed;
        }

        void LateUpdate()
        {
            m_FireInputWasHeld = GetFireInputHeld();
        }

        public bool CanProcessInput()
        {
            return Cursor.lockState == CursorLockMode.Locked && !m_GameFlowManager.GameIsEnding;
        }

        public Vector3 GetMoveInput()
        {
            if (CanProcessInput())
            {
                Vector3 move = new Vector3(moveVector.x, 0f,
                    moveVector.y);

                // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
                move = Vector3.ClampMagnitude(move, 1);

                return move;
            }

            return Vector3.zero;
        }

        public float GetLookInputsHorizontal()
        {
            float val = lookVector.x;
            if (CanProcessInput())
            {
                // apply sensitivity multiplier
                val *= LookSensitivity;
                // as we are on mouse the delta time is aleady applied,so we actually need to slow down the input further.
                val *= 0.001f;

#if UNITY_WEBGL
                    // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
                   val *= WebglLookSensitivityMultiplier;
#endif
            }
            else
            {
                val = 0f;
            }
            return val;
        }

        public float GetLookInputsVertical()
        {
            float val = lookVector.y;
            if (CanProcessInput())
            {             
                // handle inverting vertical input
                if (InvertYAxis)
                    val *= -1f;

                // apply sensitivity multiplier
                val *= LookSensitivity;
                // as we are on mouse the delta time is aleady applied,so we actually need to slow down the input further.
                val *= 0.001f;

#if UNITY_WEBGL
                    // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
                   val *= WebglLookSensitivityMultiplier;
#endif
            }
            else
            {
                val = 0;
            }
            return val;
        }

        public bool GetJumpInputDown()
        {
            if (CanProcessInput())
            {
                return jumpPressed;
            }

            return false;
        }

        public bool GetJumpInputHeld()
        {
            if (CanProcessInput())
            {
                return jumpPressed;
            }

            return false;
        }

        public bool GetFireInputDown()
        {
            return GetFireInputHeld() && !m_FireInputWasHeld;
        }

        public bool GetFireInputReleased()
        {
            return !GetFireInputHeld() && m_FireInputWasHeld;
        }

        public bool GetFireInputHeld()
        {
            if (CanProcessInput())
            {
                return fireButtonPressed;
            }

            return false;
        }

        public bool GetAimInputHeld()
        {
            if (CanProcessInput())
            {
                return fireAltButtonPressed;
            }

            return false;
        }

        public bool GetSprintInputHeld()
        {
            if (CanProcessInput())
            {
                return sprintPressed;
            }

            return false;
        }

        public bool GetCrouchInputDown()
        {
            if (CanProcessInput())
            {
                return crouchPressed;
            }

            return false;
        }

        public bool GetCrouchInputReleased()
        {
            if (CanProcessInput())
            {
                return crouchPressed;
            }

            return false;
        }

        public bool GetReloadButtonDown()
        {
            if (CanProcessInput())
            {
                return reloadPressed;
            }

            return false;
        }

        public int GetSwitchWeaponInput()
        {
            if (CanProcessInput())
            { 
                return mouseScroll;
            }

            return 0;
        }

        public int GetSelectWeaponInput()
        {
            if (CanProcessInput())
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    return 1;
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    return 2;
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    return 3;
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                    return 4;
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                    return 5;
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                    return 6;
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                    return 7;
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                    return 8;
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                    return 9;
                else
                    return 0;
            }

            return 0;
        }
    }
}