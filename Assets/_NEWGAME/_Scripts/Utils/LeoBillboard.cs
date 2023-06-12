using _GAME._Scripts._Camera;
using UnityEngine;

namespace _GAME._Scripts.Utils
{
    public class LeoBillboard : MonoBehaviour
    {
        public Transform FacedObject;

        private Transform ActiveFacedObject
        {
            get
            {
                if (FacedObject != null) return FacedObject;
                if (_camera != null) return _camera.transform;
                _camera = FindObjectOfType<ThirdPersonCamera>().targetCamera;
				
                return _camera == null ? null : _camera.transform;
            }
        }
        private static Camera _camera;

        private void Update()
        {
            if (ActiveFacedObject == null) return;
            transform.LookAt(ActiveFacedObject);
        }
    }
}