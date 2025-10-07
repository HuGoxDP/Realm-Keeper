using System;
using System.Collections.Generic;
using Architecture.Units.Core;

namespace Architecture.Selection
{
    public interface ISelectionSystem
    {
        public event Action<List<ISelectable>> OnSelectionChanged;
    }
}