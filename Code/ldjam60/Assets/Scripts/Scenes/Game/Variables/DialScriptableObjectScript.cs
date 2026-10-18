using System;

using UnityEngine;

[CreateAssetMenu(fileName = "DialScriptableObjectScript", menuName = "Scriptable Objects/DialScriptableObjectScript")]
public class DialScriptableObjectScript : ScriptableObject
{
    [SerializeField] private float min = 0f;
    [SerializeField] private float max = 100f;
    [SerializeField] private float value = 50f;

    public float Min => min;
    public float Max => max;
    public float Value => value;

    public float Percentage => Mathf.InverseLerp(min, max, value);

    public event Action<float> OnValueChanged;

    public void SetValue(float newValue)
    {
        newValue = Mathf.Clamp(newValue, min, max);

        if (Mathf.Approximately(value, newValue))
        {
            return;
        }

        value = newValue;
        OnValueChanged?.Invoke(value);
    }

    public void SetPercentage(float percentage)
    {
        SetValue(Mathf.Lerp(min, max, Mathf.Clamp01(percentage)));
    }
}

