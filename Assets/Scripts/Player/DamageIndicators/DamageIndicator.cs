using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageIndicator
{
    public string Name;
    public GameObject Effect;
    public float HealthRatioThreshold = 0.5f;

    private bool _effectOn;


    public void CheckIfShouldEnable(float healthRatio)
    {
        if(!_effectOn && healthRatio < HealthRatioThreshold)
        {
            _effectOn = true;
            Effect.SetActive(true);
        }
    }
}
