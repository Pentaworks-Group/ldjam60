using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.Game
{
    public class DialBehaviour : MonoBehaviour
    {
        [SerializeField]
        private DialScriptableObjectScript variable;

        [SerializeField]
        private Transform handleTransform;

        [SerializeField]
        private Transform additionalTransform;

        [SerializeField]
        private Image additionalColoring;

        [Tooltip("Click Sensitivity, changes percentage")]
        [SerializeField]
        private float sensitivty;



        private void OnEnable()
        {
            if (variable == null)
            {
                return;
            }
            variable.OnValueChanged += UpdateUI;

            // Initial refresh, since the value may already be set.
            UpdateUI(variable.Value);
        }

        private void OnDisable()
        {
            if (variable != null)
                variable.OnValueChanged -= UpdateUI;
        }

        public void ChangeDial(float percentage)
        {
            if (variable == null)
                return;

            variable.SetPercentage(variable.Percentage + percentage);
        }

        public void Increase()
        {
            variable.SetPercentage(variable.Percentage + sensitivty);
        }

        public void Decrease()
        {
            variable.SetPercentage(variable.Percentage - sensitivty);
        }

        private void UpdateUI(float value)
        {
            handleTransform.rotation = Quaternion.Euler(0, 0, 90 - variable.Percentage * 180);
            additionalTransform.rotation = Quaternion.Euler(0, 0, 90 - (variable.Percentage / 8) * 180);
            additionalColoring.color = GetColorByPercentage(variable.Percentage);
        }
        private Color GetColorByPercentage(float percentage)
        {

            Color color = Color.Lerp(Color.green, Color.red, percentage);
            color.a = 0.39215686f; //100f / 255f
            return color;
        }

        public float GetPercentage()
        {
            return variable != null
                ? variable.Percentage
                : 0f;
        }

    }
}
