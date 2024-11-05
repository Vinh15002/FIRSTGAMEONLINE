using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : NetworkBehaviour
{
    [SerializeField]
    public NetworkVariable<int> _currentHealth = new NetworkVariable<int>(90, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);

    [SerializeField]
    public int _maxHealth = 100;
    
 
    public float PercentOfHealth {
        get {return (float)_currentHealth.Value/ _maxHealth;}
        
    }

    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // Muốn client có thể gửi tín hiệu 
        if(!IsServer) _currentHealth.OnValueChanged += ChangeHeal;
    }

    public static event System.Action<int, int> getHeal;
  
    //Hàm xử lý tín hiệu nhận được 
    private void ChangeHeal(int previousValue, int newValue)
    {
        if(!IsOwner) return;
        getHeal?.Invoke(_currentHealth.Value, _maxHealth);
       
        Debug.Log($"ID: {OwnerClientId} :  LENGTH: {_currentHealth.Value}");
       
    }

   // Cái này chỉ được gọi bởi server
    public void GetHeal(int amountHeal){

        _currentHealth.Value += amountHeal;
         _currentHealth.Value = _currentHealth.Value <=_maxHealth ? _currentHealth.Value : _maxHealth;

        if(!IsOwner) return;
      
        getHeal?.Invoke(_currentHealth.Value, _maxHealth);
        
    }


    public void TakeDame(int amount){
        _currentHealth.Value -= amount;
        _currentHealth.Value = _currentHealth.Value >=0 ? _currentHealth.Value : 0;
        
        if(!IsOwner) return;
        

        getHeal?.Invoke(_currentHealth.Value, _maxHealth);
        
    }





    











}
