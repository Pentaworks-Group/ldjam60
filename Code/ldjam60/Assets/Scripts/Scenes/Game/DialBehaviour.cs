using UnityEngine;

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
        }

        public float GetPercentage()
        {
            return variable != null
                ? variable.Percentage
                : 0f;
        }

    }
}
