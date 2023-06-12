using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts
{
    public class SimpleItemSpawner : MonoBehaviour
    {
        public GameObject spawnItem;
        public Transform spawnPoint;
        public float spawnTime = 1;

        GameObject item;

        private void Start()
        {
            Respawn();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
                Respawn(spawnTime);
        }


        public void Respawn()
        {
            if (item != null)
                Destroy(item.gameObject);

            var go = Instantiate(spawnItem);
            go.transform.SetParent(transform);
            go.transform.position = spawnPoint.position;
            go.transform.rotation = spawnPoint.rotation;
            item = go;
        }

        public void Respawn(float time)
        {
            Invoke("Respawn", time);
        }
    }
}

