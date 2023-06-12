using UnityEngine;
using System.Collections;

namespace _GAME._Scripts
{
    public class RotateObject : MonoBehaviour
    {
        public Vector3 rotationSpeed;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}