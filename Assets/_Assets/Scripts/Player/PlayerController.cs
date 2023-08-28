using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float SENSITIVITY_MULTIPLIER = 100f;

    [Header("References")]
    [SerializeField] private Transform camHolder;
    [SerializeField] private Rigidbody playerBody;

    [Header("Constants")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lookSensitivity;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Look();
    }

    private void Move()
    {
        Vector3 moveDirection = InputManager.Instance.GetJoystickMovementVectorNormalized();

        moveDirection = camHolder.forward * moveDirection.z + camHolder.right * moveDirection.x;

        playerBody.velocity = moveDirection * moveSpeed;
    }

    private void Look()
    {
        Vector2 lookVector = InputManager.Instance.GetJoystickLookVectorNormalized();

        float mouseLookX = lookVector.x * lookSensitivity * Time.deltaTime * SENSITIVITY_MULTIPLIER;
        float mouseLookY = -lookVector.y * lookSensitivity * Time.deltaTime * SENSITIVITY_MULTIPLIER;

        float currRotation = transform.localEulerAngles.y;
        currRotation += mouseLookX;

        float currCameraRotation = camHolder.localRotation.eulerAngles.x;
        currCameraRotation += mouseLookY;
        currCameraRotation = Mathf.Clamp((currCameraRotation <= 180) ? currCameraRotation : -(360 - currCameraRotation), -70, 70);

        transform.localEulerAngles = new Vector3(0, currRotation, 0);

        camHolder.localRotation = Quaternion.Euler(currCameraRotation, 0, 0);
    }
}
