using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace goorinAR
{

    public class Utils 
    {
        public static Sprite CutTexture(Texture2D tex)
        {
            Sprite text = Sprite.Create(tex, new Rect(tex.width / 2, tex.height / 2, tex.width * 0.1f, tex.height * 0.1f), Vector2.zero);
            return text;
        }
    }
}
