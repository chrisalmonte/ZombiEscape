using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController character;
    [SerializeField] private PlayerInput input;
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 30.0f;

    private Vector3 movementAxis;
    private Vector3 moveDirection;
    private Quaternion targetRotation;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        targetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }

    void Update()
    {
        Move();
    }

    public void OnMove(InputValue value) => movementAxis = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);

    private void Move()
    {
        moveDirection = cameraTransform.TransformDirection(movementAxis);
        moveDirection.y = 0;

        if(moveDirection != Vector3.zero) 
            targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation , rotationSpeed * Time.deltaTime);
        character.Move( moveDirection * movementSpeed * Time.deltaTime);
    }
}
