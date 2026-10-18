using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Prefabs
{
	public class HealthBarBehaviour : MonoBehaviour
	{
		[SerializeField] private Image healthImage;

		[SerializeField] private Single minValue = 0;
		[SerializeField] private Single maxValue = 100;
		[SerializeField] private Single value = 0;
		
        public float Percentage => Mathf.InverseLerp(minValue, maxValue, value);

        public void Init(Single minValue, Single maxValue, Single value)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;

            SetValue(value);
        }

        public void SetValue(Single newValue)
		{
            var clampedValue = Mathf.Clamp(newValue, minValue, maxValue);

            if (!Mathf.Approximately(this.value, clampedValue))
            {
                this.value = newValue;

                var normailzedValue = Mathf.InverseLerp(minValue, maxValue, value);

                this.healthImage.rectTransform.anchorMax = new Vector2(normailzedValue, 1);
            }
        }

        public void SetPercentage(Single percentage)
        {
            SetValue(Mathf.Lerp(minValue, maxValue, Mathf.Clamp01(percentage)));
        }
    }
}
