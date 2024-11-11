using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DamaePopup : NetworkBehaviour
{
    [SerializeField]
    private float timeToExist;

    public NetworkVariable<FixedString32Bytes> textValue = new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<FixedString32Bytes> textColor = new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public TextMeshPro textUI;

   

    private void Awake() {
        textUI = GetComponent<TextMeshPro>();
        
    }

    

    public void setColor(Color color){
        textUI.color = color;
    }


    public override void OnNetworkSpawn()
    {
        
        
        textValue.OnValueChanged += OntextChange;
        textColor.OnValueChanged += OnColorChange;
        

        base.OnNetworkSpawn();
        
        StartCoroutine(ExistCoroutine());
        
        
    }

    private void OnColorChange(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
         if(newValue == "Green"){
            textUI.color = Color.green;
        }
    }

    private void OntextChange(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        textUI.SetText(newValue.ToString());
       
    }

    private IEnumerator ExistCoroutine()
    {
        while(timeToExist >0){
            timeToExist -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f,
            transform.position.z);
            textUI.alpha -= 0.005f;

            yield return null;
        }
        if(IsOwner)
            sendTheServerRpc();
        
    }

    [ServerRpc]
    private void sendTheServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
    }
}
