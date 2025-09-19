using System;
using Architecture._Project.Scripts.Architecture;
using R3;
using Reflex.Attributes;
using Unity.Cinemachine;
using UnityEngine;

namespace Architecture._Project.Scripts.Architecture.Camera
{
    public class CameraController : MonoBehaviour, IDisposable
    {
        [Header("Components")]
        [SerializeField] private CinemachineOrbitalFollow _orbitalFollow;
        [SerializeField] private Transform _cameraTarget;
        
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 20f;
        [SerializeField] private float _acceleration = 30f;
        [SerializeField] private float _deceleration = 40f;
      
        [Header("Zoom")]
        [SerializeField] private float _zoomSpeed = 0.5f;
        [SerializeField] private float zoomAcceleration = 5f;
        
        [Header("Bounds")]
        [SerializeField] private Vector2 _minBounds = Vector2.zero;
        [SerializeField] private Vector2 _maxBounds = Vector2.zero;
        [SerializeField, Space] private Vector2 _zoomBounds = Vector2.zero;
        
        private UnityEngine.Camera _camera;
        private ICameraInputService _cameraInputSystem;
        
        private Vector3 _moveVelocity = Vector3.zero;
        private float _zoomVelocity = 0;
        
        private Vector2 moveInput;
        private Vector2 zoomInput;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        private void Construct(ICameraInputService cameraInputService)
        {
            _cameraInputSystem = cameraInputService;
            
            if (_cameraInputSystem == null)
            {
                throw new NullReferenceException("Camera input service is null.");
            }
            
            _cameraInputSystem.MoveDirection.Subscribe(x => moveInput = x).AddTo(_disposable);
            _cameraInputSystem.ScrollWheel.Subscribe(x => zoomInput = x).AddTo(_disposable);
        }
        
        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
            if (_camera == null)
            {
                throw new NullReferenceException("Main camera is null.");
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        private void LateUpdate()
        {
            HandleMovement(Time.deltaTime);
            HandleZoom(Time.deltaTime);
        }

        private void HandleMovement(float deltaTime)
        {
            Vector3 forward = _camera.transform.forward;
            forward.y = 0;
            forward.Normalize();
            
            Vector3 right = _camera.transform.right;
            right.y = 0;
            right.Normalize();
            
            Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y) * _moveSpeed;

            _moveVelocity = targetVelocity.sqrMagnitude > 0.01f ?
                Vector3.MoveTowards(_moveVelocity, targetVelocity, _acceleration * deltaTime) :
                Vector3.MoveTowards(_moveVelocity, Vector3.zero, _deceleration * deltaTime);
            
            Vector3 motion = _moveVelocity * deltaTime;
            Vector3 newPosition = _cameraTarget.position + forward * motion.z + right * motion.x;
            
            newPosition.x = Mathf.Clamp(newPosition.x, _minBounds.x, _maxBounds.x);
            newPosition.z = Mathf.Clamp(newPosition.z, _minBounds.y, _maxBounds.y);
            
            _cameraTarget.position = newPosition;
        }
        
        private void HandleZoom(float deltaTime)
        {
            var currentZoom = _orbitalFollow.Radius;
            var targetVelocity = 0f;

            if (Mathf.Abs(zoomInput.y) > 0.01f)
            {
                targetVelocity = _zoomSpeed * zoomInput.y;
            }
            _zoomVelocity = Mathf.Lerp(_zoomVelocity, targetVelocity, zoomAcceleration * deltaTime);
            
            
            
            var newZoom = currentZoom - _zoomVelocity;
            newZoom = Mathf.Clamp(newZoom, _zoomBounds.x, _zoomBounds.y);
            _orbitalFollow.Radius = newZoom;
        }
    }
}