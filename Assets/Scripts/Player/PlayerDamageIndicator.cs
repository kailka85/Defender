using UnityEngine;

[System.Serializable]
public class PlayerDamageIndicator
{
    public string IndicatorName;
    public GameObject Effect;

    [Tooltip("Effect activates when health drops below this level.")]
    public float HealthRatioThreshold;

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
