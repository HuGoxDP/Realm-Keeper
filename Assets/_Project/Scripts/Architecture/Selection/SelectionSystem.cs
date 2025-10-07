using System;
using System.Collections.Generic;
using Architecture.InputSystems;
using Architecture.Units.Core;
using R3;
using Reflex.Attributes;
using UnityEngine;

namespace Architecture.Selection
{
    public class SelectionSystem : MonoBehaviour, ISelectionSystem
    {
        public event Action<List<ISelectable>> OnSelectionChanged;
        
        [SerializeField] private LayerMask _selectedLayerMask;
        [SerializeField] private RectTransform _boxVisual;

        private UnityEngine.Camera _camera;
        private IPlayerInputService _playerInputService;
        private ISelectableRegistry _selectableRegistry;
        
        private List<ISelectable> _selectedObjects;
        private List<ISelectable> _selectableObjects;        
        private RaycastHit _hitInfo;
        
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Rect _selectionBox;
        private bool _isDragging;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        private void Setup(UnityEngine.Camera mainCamera, IPlayerInputService playerInputService, ISelectableRegistry selectableRegistry)
        {
            _camera = mainCamera;
            _playerInputService = playerInputService;
            _selectableRegistry = selectableRegistry;
        }
        private void Awake()
        {
            _selectedObjects = new List<ISelectable>();
            _selectableObjects = new List<ISelectable>();
            
            _endPosition = Vector3.zero;
            _startPosition = Vector3.zero;
        }
        private void Start()
        {
            if (_camera == null)
                throw new ArgumentNullException(nameof(_camera), "Main Camera cannot be null");
            if (_playerInputService == null)
                throw new ArgumentNullException(nameof(_playerInputService), "Player Input Service cannot be null");
            if (_selectableRegistry == null)
                throw new ArgumentNullException(nameof(_selectableRegistry), "Selectable Registry cannot be null");
            
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
            var selectableToAdd = FindSelectableUnderMouse(position);
            if (selectableToAdd == null)
                return;
            
            if (!_selectedObjects.Contains(selectableToAdd))
            {
                _selectedObjects.Add(selectableToAdd);
                selectableToAdd.Select();
            }
            else
            {
                selectableToAdd.Deselect();
                _selectedObjects.Remove(selectableToAdd);
            }
            
            OnSelectionChanged?.Invoke(_selectedObjects);
        }
        private void HandleDragSelection()
        {
            FindSelectableObjectsInBox();
            if (_selectableObjects.Count > 0)
            {
                foreach (var selectable in _selectableObjects)
                {
                    if (!_selectedObjects.Contains(selectable))
                    {
                        _selectedObjects.Add(selectable);
                        selectable.Select();
                    }
                    else
                    {
                        selectable.Deselect();
                        _selectedObjects.Remove(selectable);
                    }
                }
                
                OnSelectionChanged?.Invoke(_selectedObjects);
            }
        }
        private void FindSelectableObjectsInBox()
        {
            _selectableObjects.Clear();
            foreach (var selectable in _selectableRegistry.AllSelectables)
            {
                if (_selectionBox.Contains(_camera.WorldToScreenPoint(selectable.Transform.position)))
                {
                    _selectableObjects.Add(selectable);
                }
            }
        }
        
        private ISelectable FindSelectableUnderMouse(Vector3 position)
        {
            var ray = _camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray.origin, ray.direction, out _hitInfo, 100, _selectedLayerMask))
            {
                var selectable = _hitInfo.collider.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    return selectable;
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
        private void DeselectAll()
        {
            foreach (var unit in _selectedObjects)
            {
                unit.Deselect();
            }
            _selectedObjects.Clear();
        }
    }
}