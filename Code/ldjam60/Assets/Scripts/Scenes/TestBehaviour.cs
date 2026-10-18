using Assets.Scripts.Map;
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

        [SerializeField]
        private LandBehaviour generationBehaviour;


        public void SpawnRock()
        {
            var newRock = Instantiate(rockTemplate, objectContainer.transform);
            var objectBehaviour = newRock.GetComponent<ObjectsBehaviour>();
            objectBehaviour.Init(100, true, 2);
            newRock.SetActive(true);
        }


        public void SpawnCata()
        {

            Renderer cataRenderer = cataTemplate.GetComponent<Renderer>();
            var sizeX = cataRenderer.bounds.size.x;
            for (int pos = -470; pos < 500; pos = pos + 60)
            {
                var height = generationBehaviour.CreateFlatSurface(pos, sizeX + 5) + cataRenderer.bounds.extents.y + 2;
                Vector3 spawnPosition = new Vector3(pos, height, 0);

                Debug.Log($"spawnPosition: {spawnPosition}");
                var newRock = Instantiate(cataTemplate, spawnPosition, Quaternion.identity, objectContainer.transform);

                Debug.Log($"spawnedPosition: {newRock.transform.position}");
                var objectBehaviour = newRock.GetComponent<ObjectsBehaviour>();
                objectBehaviour.Init(100, false, 2);
                newRock.SetActive(true);
            }
        }

    }
}
