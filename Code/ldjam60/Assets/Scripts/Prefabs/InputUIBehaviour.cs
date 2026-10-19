using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Prefabs
{
	public class InputUIBehaviour : MonoBehaviour
	{
		[SerializeField]
		private List<Button> interactables = new();

        private void Awake()
        {
            DisableUI();
        }

        public void EnableUI()
		{
			foreach (var button in interactables)
			{
				button.interactable = true;
			}
		}

        public void DisableUI()
        {
            foreach (var button in interactables)
            {
                button.interactable = false;
            }
        }
    }
}
