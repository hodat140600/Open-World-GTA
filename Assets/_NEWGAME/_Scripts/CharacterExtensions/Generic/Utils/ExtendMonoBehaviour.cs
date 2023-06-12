using UnityEngine;

namespace _GAME._Scripts
{
    public class ExtendMonoBehaviour : MonoBehaviour
    {
        [SerializeField, @HideInInspector]
        private bool openCloseEvents;
        [SerializeField, @HideInInspector]
        private bool openCloseWindow;
        [SerializeField, @HideInInspector]
        private int selectedToolbar;
    }
}
