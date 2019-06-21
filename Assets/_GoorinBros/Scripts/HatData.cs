using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace goorinAR
{

    public class HatData : MonoBehaviour
    {
        [SerializeField]
        private string hatName;
        [SerializeField]
        private Sprite hatImage;

        public void SethatName(string text)
        {
            hatName = text;
        }

        public string GethatName()
        {
            return hatName;
        }

        public void SethatImage(Sprite img)
        {
            hatImage = img;
        }

        public Sprite GethatImage()
        {
            return hatImage;
        }
       
       
    }
}
