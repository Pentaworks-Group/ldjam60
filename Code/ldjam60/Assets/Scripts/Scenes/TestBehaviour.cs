using UnityEngine;

namespace Assets.Scripts.Scenes
{
	public class TestBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject rockTemplate;
        [SerializeField]
        private GameObject cataTemplate;
        [SerializeField]
        private GameObject objectContainer;


        public void SpawnRock()
        {
            var newRock = Instantiate(rockTemplate, objectContainer.transform);
            newRock.SetActive(true);
        }


        public void SpawnCata()
        {
            var newRock = Instantiate(cataTemplate, objectContainer.transform);
            newRock.SetActive(true);
        }

    }
}
