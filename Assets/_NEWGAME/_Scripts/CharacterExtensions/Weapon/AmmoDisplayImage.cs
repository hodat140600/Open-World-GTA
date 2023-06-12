using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace _GAME._Scripts._ItemManager
{
    public class AmmoDisplayImage : MonoBehaviour
    {
        [System.Serializable]
        public class DisplayImage
        {
            public Sprite ammoImage;
            public int ammoId;
        }

        public Image displayImage;
        public Sprite defaultAmmoImage;
        public List<DisplayImage> displayImages = new List<DisplayImage>();

        private int currentAmmoId;

        /// <summary>
        /// Change Ammo display image by id
        /// </summary>
        /// <param name="id"></param>
        public void ChangeAmmoDisplayImage(int id)
        {
            if (currentAmmoId != id && displayImages != null)
            {
                var display = displayImages.Find(d => d.ammoId.Equals(id));
                if (display != null)
                {
                    displayImage.sprite = display.ammoImage;
                }
                else
                {
                    displayImage.sprite = defaultAmmoImage;
                }
                currentAmmoId = id;
            }
        }
    }
}
