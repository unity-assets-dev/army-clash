using UnityEngine;
using UnityEngine.UI;

public class ActorHealthBar : MonoBehaviour {
    
    [SerializeField] private Image _healthBar;
    
    public void OnHealthChange(float value) {
        _healthBar.fillAmount = value;
    }
}
