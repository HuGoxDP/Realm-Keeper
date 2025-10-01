using System;
using Architecture.InputSystems;
using Architecture.Units;
using R3;
using Reflex.Attributes;
using UnityEngine;

namespace Architecture
{
    public class TestCommander : MonoBehaviour
    {
        [SerializeField] private BaseUnit _unit;
        [SerializeField] private LayerMask _clickableLayerMask;
        
        private RaycastHit _hitInfo;
        
        [Inject] private IPlayerInputService _playerInputService;
        [Inject] private UnityEngine.Camera _camera;
        
        private CompositeDisposable _disposables = new CompositeDisposable();
        private void Start()
        {
            if (_unit == null) throw new ArgumentNullException(nameof(_unit), "Unit cannot be null");
            if (_playerInputService == null) throw new ArgumentNullException(nameof(_playerInputService), "PlayerInputService cannot be null");
            
            _playerInputService.OnMouseClick.Subscribe(position =>
            {
                var ray = _camera.ScreenPointToRay(position);
                if (!Physics.Raycast(ray.origin, ray.direction, out _hitInfo, 100, _clickableLayerMask)) return;
                
                _unit.MoveTo(_hitInfo.point);
            }).AddTo(_disposables);
            
            _playerInputService.OnMouseRightClick.Subscribe(position =>
            {
                _unit.Stop();
                _hitInfo = new RaycastHit();
            }).AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
            _disposables = null;
        }

        private void OnDrawGizmos()
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Gizmos.color = _hitInfo.collider != null ? Color.red : Color.green;
                Gizmos.DrawLine(_hitInfo.point, _hitInfo.point + Vector3.up * 10);
            }
        }
    }
}
