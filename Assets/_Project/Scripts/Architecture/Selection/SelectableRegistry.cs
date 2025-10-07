using System.Collections.Generic;
using UnityEngine;

namespace Architecture.Selection
{
    public class SelectableRegistry : ISelectableRegistry
    {
        
        private readonly List<ISelectable> _selectables = new List<ISelectable>();
        
        public IReadOnlyList<ISelectable> AllSelectables => _selectables;
        
        public void Register(ISelectable selectable)
        {
            if (!_selectables.Contains(selectable))
                _selectables.Add(selectable);
        }

        public void Unregister(ISelectable selectable)
        {
            _selectables.Remove(selectable);
        }
    }
}