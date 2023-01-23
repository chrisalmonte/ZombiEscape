using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController character;
    [SerializeField] private PlayerInput input;
    [SerializeField] private float walkingSpeed = 5.0f;
    [SerializeField] private float walkingSpeedAim = 3.5f;
    [SerializeField] private float runningSpeed = 10.0f;
    [SerializeField] private float dodgeDistanceWalk = 3.5f;
    [SerializeField] private float dodgeSpeed = 12.0f;
    [SerializeField] private float dodgeDistanceAim = 2.0f;
    [SerializeField] private float dodgeSpeedAim = 16.0f;
    [SerializeField] private float dodgeDistanceRun = 6.0f;
    [SerializeField] private float dodgeSpeedRun = 15.0f;
    [SerializeField] private int rotationSpeed = 720; //How fast character turns around.

    private Vector3 movementAxis;
    private Vector3 moveDirection;
    private Vector3 dodgeLandPoint;
    private Vector3 dodgeDistance;
    private Quaternion targetRotation;
    private Transform cameraTransform;
    private InputAction aim;
    private InputAction run;
    private bool isAiming;
    private bool isRunning;
    private bool isDodging;
    private float speed;

    private void Awake()
    {
        aim = input.actions["Aim"];
        run = input.actions["Run"];
        cameraTransform = Camera.main.transform;        
    }

    private void Start()
    {
        SetMovementSpeed();
        targetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }

    void Update()
    {
        Move();
        Rotate();
    }

    public void OnMove(InputValue value) => movementAxis = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    public void OnDodge(InputValue value) => Dodge();

    private void Move()
    { 
        if(!isDodging)
        {
            moveDirection = cameraTransform.TransformDirection(movementAxis);
            moveDirection.y = 0;
            character.Move(moveDirection * speed * Time.deltaTime);
        }
        
        else
        {
            dodgeDistance = Vector3.MoveTowards(transform.position, dodgeLandPoint, speed * Time.deltaTime) - transform.position;
            character.Move(dodgeDistance);

            if (Vector3.Distance(transform.position, dodgeLandPoint) < 0.8f)
                ExitDodge();
        }
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

    private void SetMovementSpeed()
    {
        if (isDodging) 
        {
            if (isAiming)
                speed = dodgeSpeedAim;

            else speed = isRunning ? dodgeSpeedRun : dodgeSpeed;
        }

        else
        {
            if (isAiming)
                speed = walkingSpeedAim;

            else speed = isRunning ? runningSpeed : walkingSpeed;
        }        
    }

    private void Aim()
    {
        if (isDodging)
            return;

        isAiming = true;
        SetMovementSpeed();
    }

    private void CancelAim()
    {
        if (!isAiming)
            return;

        isAiming = false;
        SetMovementSpeed();
    }

    private void Run()
    {
        if (isDodging)
            return;

        isRunning = true;
        SetMovementSpeed();
    }

    private void CancelRun()
    {
        if (!isRunning)
            return;

        isRunning = false;
        SetMovementSpeed();
    }    

    private void Dodge()
    {
        if (isDodging)
            return;

        //Implement if (character.isGrounded) when gravity is added.

        isDodging = true;
        SetMovementSpeed();

        if (movementAxis != Vector3.zero)
            moveDirection = cameraTransform.TransformDirection(movementAxis);

        else 
            moveDirection = isAiming ? -transform.forward : transform.forward;

        moveDirection.y = 0;

        //While animation and exit point are implemented, dodge will end when close to the point.
        if(isAiming)
            dodgeLandPoint = transform.position + (moveDirection * dodgeDistanceAim);
        else
        dodgeLandPoint = transform.position + (moveDirection * (isRunning ? dodgeDistanceRun : dodgeDistanceWalk));
    }

    public void ExitDodge()
    {
        if (!isDodging)
            return;

        isDodging = false;
        SetMovementSpeed();
    }

    private void OnEnable()
    {
        aim.performed += ctx => Aim();
        aim.canceled += ctx => CancelAim();
        run.performed += ctx => Run();
        run.canceled += ctx => CancelRun();
    }

    private void OnDisable()
    {
        aim.performed -= ctx => Aim();
        aim.canceled -= ctx => CancelAim();
        run.performed -= ctx => Run();
        run.canceled -= ctx => CancelRun();
    }
}
