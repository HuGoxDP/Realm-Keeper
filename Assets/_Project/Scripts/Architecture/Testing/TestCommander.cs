using System;
using System.Collections.Generic;
using Architecture.InputSystems;
using Architecture.Selection;
using Architecture.Units.Core;
using R3;
using Reflex.Attributes;
using UnityEngine;

namespace Architecture.Testing
{
    public class TestCommander : MonoBehaviour
    {
        [SerializeField] private LayerMask _clickableLayerMask;
        
        private RaycastHit _hitInfo;
        private List<BaseUnit> _unit;
        
        [Inject] private IPlayerInputService _playerInputService;
        [Inject] private UnityEngine.Camera _camera;
        [Inject] private ISelectionSystem _selectionSystem;
        
        private CompositeDisposable _disposables = new CompositeDisposable();

        private void Awake()
        {
            _unit = new List<BaseUnit>();
            _selectionSystem.OnSelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(List<ISelectable> selectables)
        {
            var units = new List<BaseUnit>();
            foreach (var selectable in selectables)
            {
                if (selectable.GameObject.TryGetComponent(out BaseUnit unit))
                {
                    units.Add(unit);
                }
            }
            _unit = units;
        }

        private void Start()
        {
            if (_playerInputService == null) throw new ArgumentNullException(nameof(_playerInputService), "PlayerInputService cannot be null");
            
            _playerInputService.OnMouseRightClickDown.Subscribe(position =>
            {
                if (_unit == null) return;
                
                var ray = _camera.ScreenPointToRay(position);
                if (!Physics.Raycast(ray.origin, ray.direction, out _hitInfo, 100, _clickableLayerMask)) return;
                foreach (var baseUnit in _unit)
                {
                    baseUnit.MoveTo(_hitInfo.point);
                }
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
