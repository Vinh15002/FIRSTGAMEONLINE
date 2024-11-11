using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EmemyBomDealDame : NetworkBehaviour
{
    [SerializeField]
    private int damage = 10;

    [SerializeField]
    private NetworkObject UIDamege;


    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            if(NetworkManager.Singleton.IsServer){
                other.GetComponent<HealthController>().TakeDame(damage);

                NetworkObject game =  Instantiate(UIDamege, other.transform.position, Quaternion.identity);
               
                game.Spawn();
                game.GetComponent<DamaePopup>().textValue.Value = $"-{damage}";

                StartCoroutine(destroy());
            }
        }
    }
    IEnumerator destroy(){
        yield return new WaitForSeconds(0.3f);
        GetComponent<NetworkObject>().Despawn();
    }
}
