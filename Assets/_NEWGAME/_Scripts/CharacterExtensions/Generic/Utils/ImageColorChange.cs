using UnityEngine;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class ImageColorChange : MonoBehaviour
    {
        public Image image;
        public Color[] colors;

        public void ChangeColor(int colorIndex)
        {
            if (colors.Length > 0 && colorIndex < colors.Length)
            {
                image.color = colors[colorIndex];
            }
        }
    }
}