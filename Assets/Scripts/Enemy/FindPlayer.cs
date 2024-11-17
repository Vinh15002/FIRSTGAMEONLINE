
using Unity.Netcode;
using UnityEngine;

public class FindPlayer : NetworkBehaviour
{

    private void OnTriggerStay2D(Collider2D other) {
       
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<EnemyMovement>().findTarget(other.transform.position);
        }
        
        
    }

    //private void OnTriggerExit2D(Collider2D other) {
    //    if (other.CompareTag("Player")){
    //        if(IsServer)
    //            gameObject.GetComponentInParent<EnemyMovement>().hasTarget = false;
    //    }
    //}
}
