using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private CinemachineVirtualCamera defaultCam;
    [SerializeField] private CinemachineVirtualCamera aimCam;

    private InputAction aim;

    private void Awake()
    {
        aim = input.actions["Aim"];
        defaultCam.gameObject.SetActive(true);
        aimCam.gameObject.SetActive(false);
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

    private void Aim()
    {
        aimCam.gameObject.SetActive(true);
    }
    private void CancelAim()
    {
        aimCam.gameObject.SetActive(false);
    }
}
