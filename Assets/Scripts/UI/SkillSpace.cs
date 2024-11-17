using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSpace : MonoBehaviour
{
    [SerializeField]
    private Image image;
    
    [SerializeField]
    private TMP_Text text;


    [SerializeField]
    private Sprite SkillSpace02;


    private void OnEnable() {
        SkillEnvent.skillSpace += ChangeUI;
        SkillEnvent.changeBackGroundSkill += ChangebackGround;
    }

    private void ChangebackGround(int index)
    {
        if(index == 2){
            transform.GetChild(1).GetComponent<Image>().sprite = SkillSpace02;
            transform.GetChild(2).GetComponent<Image>().sprite = SkillSpace02;
        }
    }

    private void OnDisable() {
        SkillEnvent.skillSpace -= ChangeUI;
        SkillEnvent.changeBackGroundSkill -= ChangebackGround;
    }

    private void ChangeUI(float currentCooldownSpace, float cooldownSpace)
    {
        image.fillAmount = currentCooldownSpace/cooldownSpace;
        float value = Mathf.Ceil(currentCooldownSpace);
        text.text = value >= 0.1 ? value.ToString() : "";
    }
}
