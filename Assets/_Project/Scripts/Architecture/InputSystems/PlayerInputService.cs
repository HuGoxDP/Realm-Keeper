using System;
using System.Runtime.CompilerServices;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace Architecture.InputSystems
{
    public interface IPlayerInputService
    {
        public ReadOnlyReactiveProperty<Vector3> MousePosition { get; }
        public Observable<Vector3> OnMouseLeftClickDown { get; }
        public Observable<Vector3> OnMouseLeftClickUp { get; }
        public Observable<Vector3> OnMouseRightClickDown { get; }
        public Observable<Vector3> OnMouseRightClickUp { get; }
        public Observable<Unit> OnShiftDown { get; }
        public Observable<Unit> OnShiftUp { get; }
        public ReadOnlyReactiveProperty<bool> IsLeftMouseButtonHold { get; }
        public ReadOnlyReactiveProperty<bool> IsRightMouseButtonHold { get; }
        public ReadOnlyReactiveProperty<bool> IsShiftButtonHold { get; }
    }
    
    public class PlayerInputService: IPlayerInputService, IDisposable
    {
        public ReadOnlyReactiveProperty<Vector3> MousePosition => _mousePosition;
        public Observable<Vector3> OnMouseLeftClickDown => _onMouseClickDown;
        public Observable<Vector3> OnMouseLeftClickUp => _onMouseClickUp;
        public Observable<Vector3> OnMouseRightClickDown => _onMouseRightClickDown;
        public Observable<Vector3> OnMouseRightClickUp => _onMouseRightClickUp;
        public Observable<Unit> OnShiftDown => _onShiftDown;
        public Observable<Unit> OnShiftUp => _onShiftUp;
        public ReadOnlyReactiveProperty<bool> IsLeftMouseButtonHold => _isLeftMouseButtonHold;
        public ReadOnlyReactiveProperty<bool> IsRightMouseButtonHold => _isRightMouseButtonHold;
        public ReadOnlyReactiveProperty<bool> IsShiftButtonHold => _isShiftButtonHold;


        private readonly Subject<Vector3> _onMouseClickDown;
        private readonly Subject<Vector3> _onMouseClickUp;
        private readonly Subject<Vector3> _onMouseRightClickDown;
        private readonly Subject<Vector3> _onMouseRightClickUp;
        private readonly Subject<Unit> _onShiftDown;
        private readonly Subject<Unit> _onShiftUp;
        
        private readonly ReactiveProperty<Vector3> _mousePosition;
        private readonly ReactiveProperty<bool> _isLeftMouseButtonHold;
        private readonly ReactiveProperty<bool> _isRightMouseButtonHold;
        private readonly ReactiveProperty<bool> _isShiftButtonHold;
        
        private readonly InputSystem_Actions _actions;
        
        
        public PlayerInputService(InputSystem_Actions actions)
        {
            _actions = actions;
            _actions.Player.Enable();
            
            _onMouseClickDown = new Subject<Vector3>();
            _onMouseClickUp = new Subject<Vector3>();
            _onMouseRightClickDown = new Subject<Vector3>();
            _onMouseRightClickUp = new Subject<Vector3>();
            _onShiftDown = new Subject<Unit>();
            _onShiftUp = new Subject<Unit>();
            _mousePosition = new ReactiveProperty<Vector3>(Vector3.zero);

            _isLeftMouseButtonHold = new ReactiveProperty<bool>(false);
            _isRightMouseButtonHold = new ReactiveProperty<bool>(false);
            _isShiftButtonHold = new ReactiveProperty<bool>(false);

            _actions.Player.MousePosition.performed += OnMousePositionChanged;
            _actions.Player.Click.performed += OnLeftClickPerform;
            _actions.Player.RightClick.performed += OnRightClickPerform;
            _actions.Player.Shift.performed += OnShiftButtonPerform;
        }
        
        private void OnMousePositionChanged(InputAction.CallbackContext obj)
        {
            _mousePosition.Value = obj.ReadValue<Vector2>();
        }
        
        private void OnRightClickPerform(InputAction.CallbackContext obj)
        {
            if (obj.ReadValueAsButton())
            {
                _onMouseRightClickDown?.OnNext(_mousePosition.CurrentValue);
                _isRightMouseButtonHold.Value = true;
            }
            
            if (!obj.ReadValueAsButton())
            {
                _onMouseRightClickUp?.OnNext(_mousePosition.CurrentValue);
                _isRightMouseButtonHold.Value = false;
            }
        }
        
        private void OnLeftClickPerform(InputAction.CallbackContext obj)
        {
            if (obj.ReadValueAsButton())
            {
                _onMouseClickDown?.OnNext(_mousePosition.CurrentValue);
                _isLeftMouseButtonHold.Value = true;
            }

            if (!obj.ReadValueAsButton())
            {
                _onMouseClickUp?.OnNext(_mousePosition.CurrentValue);
                _isLeftMouseButtonHold.Value = false;
            }
        }

        private void OnShiftButtonPerform(InputAction.CallbackContext obj)
        {
            if (obj.ReadValueAsButton())
            {
                _onShiftDown?.OnNext(Unit.Default);
                _isShiftButtonHold.Value = true;
            }
            
            if (!obj.ReadValueAsButton())
            {  
                _onShiftUp?.OnNext(Unit.Default);
                _isShiftButtonHold.Value = false;
            }
            
            Debug.Log($"Is Shift Button Hold: {_isShiftButtonHold.Value}");
            Debug.Log($"Is Shift Button Hold: {IsShiftButtonHold.CurrentValue}");
        }
        
        public void Dispose()
        {
            _actions.Player.Click.performed -= OnLeftClickPerform;
            _actions.Player.RightClick.performed -= OnRightClickPerform;
            _actions.Player.Shift.performed -= OnShiftButtonPerform;
            _actions.Player.MousePosition.performed -= OnMousePositionChanged;
            
            _mousePosition?.Dispose();
            _onMouseClickDown?.Dispose();
            _onMouseRightClickDown?.Dispose();
            _actions.Player.Disable();
        }
    }
}