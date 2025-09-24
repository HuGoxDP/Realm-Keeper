using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

// ReSharper disable CheckNamespace

namespace Architecture
{
    public interface ICameraInputService
    {
        public Observable<Vector2> MoveDirection { get; }
        public Observable<Vector2> ScrollWheel { get; }   
        
    }
    
    public class CameraInputSystem : ICameraInputService, ICameraActions, System.IDisposable
    {
        public Observable<Vector2> MoveDirection => _moveDirection;
        public Observable<Vector2> ScrollWheel => _scrollWheel;

        private readonly Subject<Vector2> _moveDirection = new Subject<Vector2>();
        private readonly Subject<Vector2> _scrollWheel = new Subject<Vector2>();

        private readonly InputSystem_Actions _actions;

        public CameraInputSystem(InputSystem_Actions actions)
        {
            _actions = actions;
            _actions.Camera.Enable();
            
            _actions.Camera.Move.performed += OnMove;
            _actions.Camera.Move.canceled += OnMove;
            
            _actions.Camera.ScrollWheel.performed += OnScrollWheel;
            _actions.Camera.ScrollWheel.canceled += OnScrollWheel;
        }

        public void Dispose()
        {
            _actions.Camera.Move.performed -= OnMove;
            _actions.Camera.Move.canceled -= OnMove;
            
            _actions.Camera.ScrollWheel.performed -= OnScrollWheel;
            _actions.Camera.ScrollWheel.canceled -= OnScrollWheel;
            
            _moveDirection?.Dispose();
            _scrollWheel?.Dispose();
            _actions.Camera.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
           var direction = context.ReadValue<Vector2>();
           _moveDirection?.OnNext(direction);
        }
        
        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _scrollWheel?.OnNext(direction);
        }
    }
}
