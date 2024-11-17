using Assets.Scripts.Enemy;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthBar : NetworkBehaviour
{

    [SerializeField]
    private Slider slider;

    private void Awake() {
        
    }

    public void OnEnable()
    {
        slider = GetComponent<Slider>();

        slider.value = 1;
        //ChangeHealthBarEnemy.changeHealth += OnChangeHealthBar;
    }

  
   


    public void ChangeHealthBar(int currentHeal, int maxHealth)
    {



        //slider.value = (float)currentHeal / maxHealth;
        slider.value = (float)currentHeal / maxHealth;

        SendInforClientRpc(currentHeal, maxHealth);
    }

    [ClientRpc]
    private void SendInforClientRpc(int currentHeal, int maxHealth)
    {
        slider.value = (float)currentHeal / maxHealth;
    }
}
