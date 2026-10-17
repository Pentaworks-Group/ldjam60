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
            var objectBehaviour = newRock.GetComponent<ObjectsBehaviour>();
            objectBehaviour.Init(100, true, 2);
            newRock.SetActive(true);
        }


        public void SpawnCata()
        {
            var newRock = Instantiate(cataTemplate, objectContainer.transform);
            var objectBehaviour = newRock.GetComponent<ObjectsBehaviour>();
            objectBehaviour.Init(100, false, 2);
            newRock.SetActive(true);
        }

    }
}
