using UnityEngine;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class TutorialTextTrigger : MonoBehaviour
    {
        [TextArea(5, 3000), Multiline]
        public string text;
        public Text _textUI;
        public GameObject painel;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EnableTutorialPanel();
            }
        }

        public void EnableTutorialPanel()
        {
            painel.SetActive(true);
            _textUI.gameObject.SetActive(true);
            _textUI.text = text;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                DisableTutorialPanel();
            }
        }

        public void DisableTutorialPanel()
        {
            painel.SetActive(false);
            _textUI.gameObject.SetActive(false);
            _textUI.text = " ";
        }
    }
}