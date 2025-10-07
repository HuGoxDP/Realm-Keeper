using UnityEngine;

namespace Architecture.Selection
{
    public class SelectableComponent : MonoBehaviour, ISelectable
    {
        [SerializeField] private GameObject _selectionVisual;
        
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        
        public void Select()
        {
            _selectionVisual.SetActive(true);
        }

        public void Deselect()
        {
            _selectionVisual.SetActive(false);       
        }
    }
}