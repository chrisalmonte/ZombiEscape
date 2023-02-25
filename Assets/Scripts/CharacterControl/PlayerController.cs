using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private CharacterController character;
    [SerializeField] private PlayerInput input;

    [Header("Walking Parameters")]
    [SerializeField] private float speedWalk = 5.0f;
    [SerializeField] private float dodgeSpeed = 12.0f;
    [SerializeField] private float dodgeDistanceWalk = 3.5f;

    [Header("Runnning Parameters")]
    [SerializeField] private float speedRun = 10.0f;
    [SerializeField] private float dodgeDistanceRun = 6.0f;
    [SerializeField] private float dodgeSpeedRun = 15.0f;

    [Header("Aiming Parameters")]
    [SerializeField] private float speedAim = 3.5f;
    [SerializeField] private float dodgeDistanceAim = 2.0f;
    [SerializeField] private float dodgeSpeedAim = 16.0f;

    [Header("Internal Parameters")]
    [SerializeField] private int rotationAnimationSpeed = 720; //How fast character turns around. Does not directly affect gameplay

    private InputAction aim;
    private InputAction run;
    private Transform cameraTransform;
    private Vector3 movementAxis;
    private Vector3 moveDirection;
    private Quaternion targetRotation;
    private bool isAiming;
    private bool isRunning;
    private bool isDodging;
    private float speed;
    private float dodgeMaxDistance;

    public void OnMove(InputValue value) => movementAxis = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    public void OnDodge(InputValue value) => StartDodge();

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

    private void Move()
    {
        if(!isDodging)
        {
            moveDirection = cameraTransform.TransformDirection(movementAxis);
            moveDirection.y = 0;            
        }

        character.Move(moveDirection * speed * Time.deltaTime);
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

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationAnimationSpeed * Time.deltaTime);
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
                speed = speedAim;

            else speed = isRunning ? speedRun : speedWalk;
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

    private void StartDodge()
    {
        if (isDodging)  //  && !character.isGrounded when gravity is added.
            return;

        if (movementAxis != Vector3.zero)
            moveDirection = cameraTransform.TransformDirection(movementAxis);
        else
            moveDirection = isAiming ? -transform.forward : transform.forward;

        moveDirection.y = 0;

        if (isAiming)
            dodgeMaxDistance = dodgeDistanceAim;
        else
            dodgeMaxDistance = isRunning ? dodgeDistanceRun : dodgeDistanceWalk;

        StartCoroutine(Dodge());           
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        SetMovementSpeed();
        yield return new WaitForSeconds(dodgeMaxDistance / speed);
        ExitDodge();
    }

    public void ExitDodge()
    {
        if (!isDodging)
            return;

        isDodging = false;
        SetMovementSpeed();
    }    
}
