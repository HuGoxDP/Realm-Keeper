using UnityEngine;

namespace Architecture
{
    public class UnitSelectionVisual : MonoBehaviour
    {
        public void Select()
        {
            gameObject.SetActive(true); 
        }
        
        public void Deselect()
        {
            gameObject.SetActive(false);       
        }
    }
}