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

    private float speedOrigin;


    [SerializeField]
    
    private float cooldownSkillE = 5f;

    [SerializeField]
    private float cooldownSpace = 20f;

    private NetworkVariable<float> _cooldownSkillSpace = new NetworkVariable<float>(20f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<float> _cooldownSkillE = new NetworkVariable<float>(5f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    [SerializeField]
    private NetworkObject prefabBom;

    private void Awake() {
        speedOrigin = GetComponent<NetworkMovementComponent>().speed.Value;
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

        if(!IsOwner && !Application.isFocused) return;
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
                SkillSpaceServerRpc(position,rotation, direction);
                
            }
        }
    }

    [ServerRpc]
    private void SkillSpaceServerRpc(Vector3 position, Quaternion rotation, Vector2 direction, ServerRpcParams serverRpcParams = default)
    {
        NetworkObject game = Instantiate(prefabBom, position, rotation);
        game.transform.up = direction;
      
        game.SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
        _cooldownSkillSpace.Value = cooldownSpace;
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

        GetComponent<NetworkMovementComponent>().speed.Value = speedOrigin + speedAdd;
        yield return new WaitForSeconds(time_Dash);
        GetComponent<NetworkMovementComponent>().speed.Value = speedOrigin;
        _cooldownSkillE.Value = cooldownSkillE;
    }



}
