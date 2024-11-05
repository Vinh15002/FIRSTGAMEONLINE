using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthBar : NetworkBehaviour
{

    
    private Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    [ClientRpc]
    public void onChangeHealthBarClientRpc(float presentOfHealth){
        slider.value = presentOfHealth;
    }
}
