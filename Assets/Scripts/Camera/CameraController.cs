using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;

    [Header("Mouse Settings")]
    [SerializeField] private float _mouseSensitivity = 0.1f;

    [Header("Camera Settings")]
    [SerializeField] private float _cameraDistance = 5.0f;
    [SerializeField] private float _cameraHeight = 1.5f;
    [SerializeField] private float _cameraCollisionBuffer = 0.2f;
    [SerializeField] private Camera _camera;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask _obstacleMask;

    private Vector2 _lookInput;
    private float _pitch;
    private float _yaw;

    private void Start()
    { 
        _yaw = _player.eulerAngles.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }
    
    private void LateUpdate()
    {
        _yaw += _lookInput.x * _mouseSensitivity;
        _pitch += _lookInput.y * _mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, -30f, 60f);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);

        Vector3 targetPosition = _player.position + Vector3.up * _cameraHeight;

        Vector3 desiredOffset = rotation * new Vector3(0f, 0f, _cameraDistance);
        Vector3 desiredCameraPosition = targetPosition + desiredOffset;

        Vector3 directionToCamera = desiredCameraPosition - targetPosition;
        float desiredDistance = directionToCamera.magnitude;

        Vector3 finalCameraPosition = desiredCameraPosition;

        if (Physics.Raycast(targetPosition, directionToCamera.normalized, out RaycastHit hit, desiredDistance, _obstacleMask))
        {
            finalCameraPosition = hit.point - directionToCamera.normalized * _cameraCollisionBuffer;
        }

        _camera.transform.position = finalCameraPosition;
        _camera.transform.LookAt(targetPosition);

        Debug.DrawLine(targetPosition, desiredCameraPosition, Color.red);
    }
}


