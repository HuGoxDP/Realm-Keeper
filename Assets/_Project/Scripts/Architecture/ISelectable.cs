using R3;
using UnityEngine;

namespace Architecture
{
    public interface ISelectable
    {
        GameObject GameObject { get; }
        ReactiveProperty<bool> IsSelected { get; }
        ReactiveProperty<bool> IsAlive { get; }
        void Select();
        void Deselect();
    }
    
    public class Selectable : MonoBehaviour, ISelectable
    {
        
       [SerializeField] private GameObject _selectionVisualObject;
       
        
        public GameObject GameObject => gameObject;
        public ReactiveProperty<bool> IsSelected { get; } = new();
        public ReactiveProperty<bool> IsAlive { get; } = new();
        
        public void Select()
        {
            IsSelected.Value = true;
            _selectionVisualObject.SetActive(true);
        }

        public void Deselect()
        {
            IsSelected.Value = false;
            _selectionVisualObject.SetActive(false);
        }
    }
}