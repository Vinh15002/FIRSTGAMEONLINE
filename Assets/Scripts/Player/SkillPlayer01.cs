using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SkillPlayer01 : NetworkBehaviour
{
    [SerializeField]
    private float speedAdd = 20f;


    [SerializeField]
    private float time_Dash = 2f;


    [SerializeField]
    
    private float cooldownSkillE = 5f;

    [SerializeField]
    private float cooldownSpace = 10f;

    private NetworkVariable<float> _cooldownSkillSpace = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<float> _cooldownSkillE = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    [SerializeField]
    private NetworkObject prefabBom;

    private void Awake() {
        _cooldownSkillE.OnValueChanged += ChangeValue;
        _cooldownSkillSpace.OnValueChanged += ChangeValueSpace;
    }

    private void ChangeValueSpace(float previousValue, float newValue)
    {
        if(!IsOwner) return;
        SkillEnvent.skillSpace?.Invoke(newValue,cooldownSpace );
    }

    private void ChangeValue(float previousValue, float newValue)
    {
        if(!IsOwner) return;
        SkillEnvent.skillE?.Invoke(newValue,cooldownSkillE);
       
    }

    private void Update() {
        if(IsServer){
            _cooldownSkillE.Value = _cooldownSkillE.Value >=0 ? _cooldownSkillE.Value  - Time.deltaTime : 0;
            _cooldownSkillSpace.Value = _cooldownSkillSpace.Value >=0 ? _cooldownSkillSpace.Value  - Time.deltaTime : 0;
        }

        if(!IsOwner || !Application.isFocused) return;
        Skill01();
        Skill02();
        
    }

    private void Skill02()
    {
         if(Input.GetKeyDown(KeyCode.Space) && _cooldownSkillSpace.Value <=0){
            if(IsOwner){
                Vector3 position = transform.GetChild(0).transform.GetChild(0).transform.position;
                Quaternion rotation = transform.GetChild(0).transform.rotation;
                Vector2 direction = transform.GetChild(0).transform.up;
                int damageOrigin = GetComponent<Fire>().DamageFire.Value;
                FireServerRpc(position, rotation, direction, damageOrigin);
               
            }
        }
    }


    [ClientRpc]
    private void FireClientRpc(Vector3 position, Quaternion rotation, Vector3 direction, int damageOrigin)
    {
        ObjectPooling.Singleton.SpawnBullet(position,rotation, direction,2, damageOrigin*5);
        _cooldownSkillSpace.Value = cooldownSpace;
    }

    [ServerRpc]
    private void FireServerRpc(Vector3 position, Quaternion rotation, Vector3 direction, int damageOrigin)
    {
        
        ObjectPooling.Singleton.SpawnBullet(position,rotation, direction,2, damageOrigin*5);
        FireClientRpc(position,rotation, direction,damageOrigin);
        
    }


    private void Skill01(){
        if(Input.GetKeyDown(KeyCode.E) && _cooldownSkillE.Value <=0){
            if(IsOwner){
                SkillEServerRpc();
            }
        }

    }
    [ServerRpc]
    private void SkillEServerRpc()
    {
        StartCoroutine(DashTime());
    }

    public IEnumerator DashTime(){

        GetComponent<PlayerController>().speed.Value +=   speedAdd;
        yield return new WaitForSeconds(time_Dash);
        GetComponent<PlayerController>().speed.Value -= speedAdd;
        _cooldownSkillE.Value = cooldownSkillE;
    }



}
