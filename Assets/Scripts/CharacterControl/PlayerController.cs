using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController character;
    [SerializeField] private PlayerInput input;
    [SerializeField] private float walkingSpeed = 5.0f;
    [SerializeField] private int rotationSpeed = 720;

    private Vector3 movementAxis;
    private Vector3 moveDirection;
    private Quaternion targetRotation;
    private Transform cameraTransform;
    private InputAction aim;
    private bool isAiming;

    private void Awake()
    {
        aim = input.actions["Aim"];
        cameraTransform = Camera.main.transform;        
    }

    private void Start()
    {
        targetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }

    void Update()
    {
        Move();
        Rotate();
    }

    public void OnMove(InputValue value) => movementAxis = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);

    private void Move()
    {
        moveDirection = cameraTransform.TransformDirection(movementAxis);
        moveDirection.y = 0;
        character.Move(moveDirection * walkingSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (isAiming)
        {
            targetRotation = Quaternion.Euler(transform.eulerAngles.x, cameraTransform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = targetRotation;
        }
        
        else
        {
            if (moveDirection != Vector3.zero)
                targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Aim()
    {
        isAiming = true;
    }
    private void CancelAim()
    {
        isAiming = false;
    }

    private void OnEnable()
    {
        aim.performed += ctx => Aim();
        aim.canceled += ctx => CancelAim();
    }

    private void OnDisable()
    {
        aim.performed -= ctx => Aim();
        aim.canceled -= ctx => CancelAim();
    }
}
