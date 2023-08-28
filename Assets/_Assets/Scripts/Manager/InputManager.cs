using System;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public event Action OnPlayerPrimaryAction;
    public event Action OnPlayerSecondaryActionPerformed;
    public event Action OnPlayerSecondaryActionCancelled;

    [SerializeField] private FloatingJoystick moveJoystick;
    [SerializeField] private DynamicJoystick lookJoystick;
    [SerializeField] private Button pickButton;
    [SerializeField] private ButtonHold placeButton;

    private Controls playerControls;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (playerControls == null)
        {
            playerControls = new Controls();
        }
    }

    private void Start()
    {
        playerControls.Player.PrimaryAction.performed += Player_PrimaryAction_performed;
        pickButton.onClick.AddListener(() => { OnPlayerPrimaryAction?.Invoke(); });

        playerControls.Player.SecondaryAction.performed += SecondaryAction_performed;
        placeButton.OnButtonPressed += () => { OnPlayerSecondaryActionPerformed?.Invoke(); };

        playerControls.Player.SecondaryAction.canceled += SecondaryAction_canceled;
        placeButton.OnButtonReleased += () => { OnPlayerSecondaryActionCancelled?.Invoke(); };
    }

    private void SecondaryAction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerSecondaryActionCancelled?.Invoke();
    }

    private void SecondaryAction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerSecondaryActionPerformed?.Invoke();
    }

    private void Player_PrimaryAction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerPrimaryAction?.Invoke();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    public Vector3 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerControls.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        return moveDirection.normalized;
    }

    public Vector3 GetLookVectorNormalized()
    {
        Vector2 inputVector = playerControls.Player.Look.ReadValue<Vector2>();

        return inputVector.normalized;
    }

    public Vector3 GetJoystickMovementVectorNormalized()
    {
        Vector3 moveDirection = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);

        return moveDirection.normalized;
    }

    public Vector3 GetJoystickLookVectorNormalized()
    {
        Vector2 inputVector = new Vector3(lookJoystick.Horizontal, lookJoystick.Vertical);

        return inputVector.normalized;
    }

    public float GetRotationInput()
    {
        return playerControls.Player.Rotate.ReadValue<float>();
    }
}