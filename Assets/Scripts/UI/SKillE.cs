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


    private void OnEnable() {
        SkillEnvent.skillE += ChangeUI;
    }

    private void OnDisable() {
        SkillEnvent.skillE -= ChangeUI;
    }

    private void ChangeUI(float currentCooldownE, float cooldownE)
    {
        image.fillAmount = currentCooldownE/cooldownE;
        text.text = Mathf.Ceil(currentCooldownE)+"";
    }
}
