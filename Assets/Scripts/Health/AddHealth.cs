using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AddHealth : NetworkBehaviour
{
    [SerializeField]
    private int heal = 2;
    
    [SerializeField]
    private float timeToDestroy = 5f;

    private float timeHeal = 0.5f;
    private float _timeHeal = 0.5f;

    [SerializeField]

    private NetworkObject UIHeal;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        StartCoroutine(DestroyGameObject());

    }

    private IEnumerator DestroyGameObject()
    {
       
        yield return new WaitForSeconds(timeToDestroy);
        
        if(NetworkManager.Singleton.IsServer){
            GetComponent<NetworkObject>().Despawn();
        }
        
        
        
    }
    private void FixedUpdate() {
        _timeHeal += Time.deltaTime;
        
    }


    private void OnTriggerStay2D(Collider2D other) {

        if(other.CompareTag("Player")){
           if(NetworkManager.Singleton.IsServer){
                if(_timeHeal >= timeHeal){
                    NetworkObject game =  Instantiate(UIHeal, other.transform.position, Quaternion.identity);
                    game.Spawn();
                    game.GetComponent<DamaePopup>().textValue.Value = $"+{heal}";
                    game.GetComponent<DamaePopup>().textColor.Value = "Green";
                    other.GetComponent<HealthController>().GetHeal(heal);
                    _timeHeal = 0;
                }
               
           }
        }
        
            
    }
}
