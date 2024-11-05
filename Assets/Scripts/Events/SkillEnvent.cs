using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEnvent
{
    public delegate void SkillE(float currentCooldownE, float cooldownE);

    public static SkillE skillE;

    private delegate void SkillSpace(float currentCooldownE, float cooldownE);

    public static SkillE skillSpace;

}
