using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace Architecture.InputSystems
{
    public interface IPlayerInputService
    {
        public Observable<Vector3> OnMouseClick { get; }
        public Observable<Vector3> OnMouseRightClick { get; }
    }
    
    public class PlayerInputService: IPlayerActions, IPlayerInputService, IDisposable
    {
        public Observable<Vector3> OnMouseClick => _onMouseClick;
        public Observable<Vector3> OnMouseRightClick => _onMouseRightClick;
        
        private Subject<Vector3> _onMouseClick = new Subject<Vector3>();
        private Subject<Vector3> _onMouseRightClick = new Subject<Vector3>();

        private readonly InputSystem_Actions _actions;
        
        private Vector3 MousePosition => Input.mousePosition;
        
        public PlayerInputService(InputSystem_Actions actions)
        {
            _actions = actions;
            _actions.Player.Enable();
            
            _actions.Player.Click.performed += OnClick;
            _actions.Player.RightClick.performed += OnRightClick;
        }
        
        public void OnClick(InputAction.CallbackContext context)
        {
            _onMouseClick?.OnNext(MousePosition);
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            _onMouseRightClick?.OnNext(MousePosition);
        }

        public void Dispose()
        {
            _actions.Player.Click.performed -= OnClick;
            _actions.Player.RightClick.performed -= OnRightClick;
            
            _onMouseClick?.Dispose();
            _onMouseRightClick?.Dispose();
            _actions.Player.Disable();
        }
    }
}