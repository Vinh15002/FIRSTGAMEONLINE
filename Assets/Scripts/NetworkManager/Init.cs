using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{


    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        if(UnityServices.State == ServicesInitializationState.Initialized){

            AuthenticationService.Instance.SignedIn += OnSignIn;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
           
            if(AuthenticationService.Instance.IsSignedIn){
                string username = PlayerPrefs.GetString("Username");
                if(username == ""){
                    PlayerPrefs.SetString("Username", username);
                }

                SceneManager.LoadSceneAsync("MenuMain");
            }
        }
    }


    public void OnSignIn(){
        Debug.Log($"TOKEN: {AuthenticationService.Instance.AccessToken}" );
        Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}" );
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
