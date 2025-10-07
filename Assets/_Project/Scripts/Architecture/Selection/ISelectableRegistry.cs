using System.Collections.Generic;

namespace Architecture.Selection
{
    public interface ISelectableRegistry
    {
        IReadOnlyList<ISelectable> AllSelectables { get; }
        void Register(ISelectable selectable);
        void Unregister(ISelectable selectable);
    }
}