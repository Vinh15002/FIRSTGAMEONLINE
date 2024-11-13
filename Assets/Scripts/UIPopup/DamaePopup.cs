using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DamaePopup : NetworkBehaviour
{

    private float timeToExist = 2f;

    private float speed = 5f;

    [SerializeField]
    private TMP_Text textUI;

   

    private void Awake() {
       
        
    }

    

    public void setColor(Color color){
        textUI.color = color;
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
    }



    
    private void FixedUpdate() {
        
        transform.Translate(Vector2.up*speed*Time.deltaTime);
        textUI.alpha -= Time.deltaTime;
    }


    public void SetActive(Vector3 position, string text, Color color){
        gameObject.SetActive(true);
        transform.position=position;
        textUI.text = text;
        textUI.color = color;
        StartCoroutine(DestroyGameObject());
    }



    public IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(timeToExist);
        Reset();
       

    }


    public void Reset(){
        transform.position = Vector3.zero;
        textUI.text = "";
        textUI.color = Color.red;
        gameObject.SetActive(false);
    }

}
