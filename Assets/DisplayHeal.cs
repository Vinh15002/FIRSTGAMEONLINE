using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayHeal : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txt;

    [SerializeField]
    private Slider slider;

    

    

    private void OnEnable() {
        HealthController.getHeal += OnChangeHeal;
    }
    private void OnDisable() {
        HealthController.getHeal -= OnChangeHeal;
    }

    public void OnChangeHeal(int _currentHealth, int _maxHealth){
        
        txt.SetText($"{_currentHealth}/{_maxHealth}");
        slider.value = (float)_currentHealth/_maxHealth;
        Debug.Log("IS CALLBACK");
    }
}
