using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bom : NetworkBehaviour
{
    private float _timeToDestroy = 3f;
    
    


    public override void OnNetworkSpawn()
    {
        
        base.OnNetworkSpawn();

        //GetComponent<Rigidbody2D>().velocity = transform.up*speed;
        
        

    }


    public void SetActive(Vector3 position){
        gameObject.SetActive(true);
        transform.position=position;
        StartCoroutine(DestroyGameObject());
    }



    public IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        Reset();
       

    }


    public void Reset(){
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }





}
