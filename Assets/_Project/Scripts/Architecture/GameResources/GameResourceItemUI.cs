using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Architecture.GameResources
{
    public class GameResourceItemUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amount;

        public void Initialize(IGameResource resource, int amount)
        {
            _icon.sprite = resource.Icon;
            _amount.text = amount.ToString();
        }
        
        public void UpdateAmount(int amount)
        {
            _amount.text = amount.ToString();
        }
    }
}