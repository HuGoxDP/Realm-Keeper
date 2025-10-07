using Reflex.Attributes;
using UnityEngine;

namespace Architecture.Selection
{
    public class SelectableRegistrar : MonoBehaviour
    {
        [Inject] private ISelectableRegistry _selectableRegistry;
        [SerializeField] private SelectableComponent  _selectables;
        
        private void Awake() => _selectableRegistry.Register(_selectables);
        private void OnDestroy() => _selectableRegistry.Unregister(_selectables);
    }
}