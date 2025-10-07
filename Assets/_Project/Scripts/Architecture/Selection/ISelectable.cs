using UnityEngine;

namespace Architecture.Selection
{
    public interface ISelectable
    {
        GameObject GameObject { get; }
        Transform Transform { get; }
        void Select();
        void Deselect();
    }
}