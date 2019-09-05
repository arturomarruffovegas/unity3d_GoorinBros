using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace goorinAR
{

    public class Tags
    {
        public static bool useTag = false;
        private static string basic = "tag:'3d_model:true''category_enabled:true'";
        public static string tag = "tag:'3d_model:true'";



        public static void SetTag(bool value, string tagName = null)
        {
            useTag = value;
            if (!string.IsNullOrEmpty(tagName))
            {
                tag = basic + "'shape:" + tagName + "'";
            }
        }
    }

   
}

