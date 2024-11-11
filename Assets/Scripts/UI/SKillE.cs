using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SKillE : MonoBehaviour
{
    [SerializeField]
    private Image image;
    
    [SerializeField]
    private TMP_Text text;


    [SerializeField]
    private Sprite SkillE02;




    private void OnEnable() {
        SkillEnvent.skillE += ChangeUI;
        SkillEnvent.changeBackGroundSkill += ChangebackGround;
    }

    private void ChangebackGround(int index)
    {
        if(index == 2){
            transform.GetChild(1).GetComponent<Image>().sprite = SkillE02;
            transform.GetChild(2).GetComponent<Image>().sprite = SkillE02;
        }
    }

    private void OnDisable() {
        SkillEnvent.skillE -= ChangeUI;
         SkillEnvent.changeBackGroundSkill -= ChangebackGround;
    }

    private void ChangeUI(float currentCooldownE, float cooldownE)
    {
        image.fillAmount = currentCooldownE/cooldownE;
        text.text = Mathf.Ceil(currentCooldownE)+"";
    }
}
