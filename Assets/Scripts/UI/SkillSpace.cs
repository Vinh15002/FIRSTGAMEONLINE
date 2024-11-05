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


    private void OnEnable() {
        SkillEnvent.skillSpace += ChangeUI;
    }

    private void OnDisable() {
        SkillEnvent.skillSpace -= ChangeUI;
    }

    private void ChangeUI(float currentCooldownSpace, float cooldownSpace)
    {
        image.fillAmount = currentCooldownSpace/cooldownSpace;
        text.text = Mathf.Ceil(currentCooldownSpace)+"";
    }
}
