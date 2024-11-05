using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPlayer : NetworkBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TMP_Text text;

    private HealthController heal;


    public void OnChangeHealBar(int _currentHealth, int _maxHealth){
        //if(!IsOwner) return;
        slider.value = (float)_currentHealth/_maxHealth;
        // slider.value = heal.PercentOfHealth;
        // text.text = $"{heal._currentHealth.Value} / {heal._maxHealth}";
    }
}
