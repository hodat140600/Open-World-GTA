using _GAME._Scripts._CharacterController;
using UnityEngine;
namespace _GAME._Scripts._Utils
{
    [ClassHeader("Load Level", openClose = false)]
    public class LoadLevel : ExtendMonoBehaviour
    {
        [Tooltip("Write the name of the level you want to load")]
        public string levelToLoad;
        [Tooltip("Assign here the spawnPoint name of the scene that you will load")]
        public string spawnPointName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var thirdPerson = other.transform.gameObject.GetComponent<ThirdPersonInput>();
                LoadLevelHelper.LoadScene(levelToLoad, spawnPointName, thirdPerson);
            }
        }
    }
}