using System;
using System.Collections.Generic;
using Architecture.InputSystems;
using Architecture.Units.Core;
using R3;
using Reflex.Attributes;
using UnityEngine;

namespace Architecture
{
    public interface IUnitSelectionSystem
    {
        public event Action<List<BaseUnit>> OnSelectionChanged;
    }

    public class UnitSelectionSystem : MonoBehaviour, IUnitSelectionSystem
    {
        public event Action<List<BaseUnit>> OnSelectionChanged;
        
        [SerializeField] private LayerMask _selectedLayerMask;
        [SerializeField] private RectTransform _boxVisual;

        private UnityEngine.Camera _camera;
        private IPlayerInputService _playerInputService;
        private IUnitManager _unitManager;
        
        private List<BaseUnit> _selectedUnits;
        private List<BaseUnit> _units;        
        private RaycastHit _hitInfo;
        
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Rect _selectionBox;
        private bool _isDragging;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        private void Setup(UnityEngine.Camera mainCamera, IPlayerInputService playerInputService, IUnitManager unitManager)
        {
            _camera = mainCamera;
            _playerInputService = playerInputService;
            _unitManager = unitManager;
        }
        private void Awake()
        {
            _selectedUnits = new List<BaseUnit>();
            _units = new List<BaseUnit>();
        }
        private void Start()
        {
            if (_camera == null)
                throw new ArgumentNullException(nameof(_camera), "Main Camera cannot be null");
            if (_playerInputService == null)
                throw new ArgumentNullException(nameof(_playerInputService), "Player Input Service cannot be null");
            
            _units = new List<BaseUnit>();
            _endPosition = Vector3.zero;
            _startPosition = Vector3.zero;
            
            DrawVisual();
            
            if (_playerInputService != null)
            {
                _playerInputService.OnMouseLeftClickDown.Subscribe(StartSelect).AddTo(_disposables);
                
                _playerInputService.OnMouseLeftClickUp     
                    .Subscribe(pos => FinishSelection(pos, _playerInputService.IsShiftButtonHold.CurrentValue))
                    .AddTo(_disposables);

                Observable.EveryUpdate()
                    .ZipLatest(_playerInputService.MousePosition, (x, pos) => pos)
                    .Where(_ => _playerInputService.IsLeftMouseButtonHold.CurrentValue)
                    .Subscribe(UpdateSelection)
                    .AddTo(_disposables);
            }
        }
        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
        private void StartSelect(Vector3 position)
        {
            _startPosition = position;
            _selectionBox = new Rect();
            _isDragging = false;
        }
        private void UpdateSelection(Vector3 position)
        {
            _endPosition = position;
            DrawVisual();
            DrawSelectionBox(position);
            _isDragging = true;
        }
        private void FinishSelection(Vector3 position, bool isShift)
        {
            if (!isShift)
            {
                DeselectAll();
            }
            
            switch (_isDragging)
            {
                case true:
                    HandleDragSelection();
                    break;
                default:
                    HandleSingleSelection(position);
                    break;
            }
            
            _startPosition = Vector3.zero;
            _endPosition = Vector3.zero;
            DrawVisual();
        }
        private void HandleSingleSelection(Vector3 position)
        {
            var unitToAdd = SelectUnit(position);
            if (unitToAdd == null)
                return;
            
            if (!_selectedUnits.Contains(unitToAdd))
            {
                _selectedUnits.Add(unitToAdd);
                unitToAdd.Select();
            }
            else
            {
                Deselect(unitToAdd);
            }
            
            OnSelectionChanged?.Invoke(_selectedUnits);
        }
        private void HandleDragSelection()
        {
            SelectUnits();
            if (_units.Count > 0)
            {
                foreach (var unit in _units)
                {
                    if (!_selectedUnits.Contains(unit))
                    {
                        _selectedUnits.Add(unit);
                        unit.Select();
                    }
                    else
                    {
                        unit.Deselect();
                        _selectedUnits.Remove(unit);
                    }
                }
                
                OnSelectionChanged?.Invoke(_selectedUnits);
            }
        }
        private void SelectUnits()
        {
            _units.Clear();
            foreach (var unit in _unitManager.UnitList)
            {
                if (_selectionBox.Contains(_camera.WorldToScreenPoint(unit.transform.position)))
                {
                    _units.Add(unit);
                }
            }
        }
        
        private BaseUnit SelectUnit(Vector3 position)
        {
            var ray = _camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray.origin, ray.direction, out _hitInfo, 100, _selectedLayerMask))
            {
                var unit = _hitInfo.collider.GetComponent<BaseUnit>();
                if (unit != null)
                {
                    return unit;
                }
            }
            return null;
        }
        private void DrawSelectionBox(Vector3 mousePosition)
        {
            if (mousePosition.x < _startPosition.x)
            {
             _selectionBox.xMin = mousePosition.x;
             _selectionBox.xMax = _startPosition.x; 
            }
            else
            {
                _selectionBox.xMin = _startPosition.x;
                _selectionBox.xMax = mousePosition.x;
            }

            if (mousePosition.y < _startPosition.y)
            {
                _selectionBox.yMin = mousePosition.y;
                _selectionBox.yMax = _startPosition.y;
            }
            else
            {
                _selectionBox.yMin = _startPosition.y;
                _selectionBox.yMax = mousePosition.y;
            }
        }
        private void DrawVisual()
        {
            Vector2 boxStart = _startPosition;
            Vector2 boxEnd = _endPosition;

            Vector2 boxCenter = boxStart.Center(boxEnd);
            _boxVisual.position = boxCenter;
            
            Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
            _boxVisual.sizeDelta = boxSize;
        }
        private void Deselect(BaseUnit unitToDeselect)
        {
            if (_selectedUnits.Contains(unitToDeselect))
            {
                unitToDeselect.Deselect();
                _selectedUnits.Remove(unitToDeselect);
            }
        }
        private void DeselectAll()
        {
            foreach (var unit in _selectedUnits)
            {
                unit.Deselect();
            }
            _selectedUnits.Clear();
        }
    }
}