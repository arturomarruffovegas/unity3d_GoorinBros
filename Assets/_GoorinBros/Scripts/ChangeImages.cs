using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeImages : MonoBehaviour
{
    public List<Texture2D> images;
    public RawImage UIimg;
    public int index;

    private void Start()
    {
        StartCoroutine(OnChangeImage());
    }

    private IEnumerator OnChangeImage()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (index < images.Count-1)
            {
                index++;
                UIimg.DOFade(0, 0.2f).OnComplete(() => 
                {
                    UIimg.texture = images[index];
                    UIimg.DOFade(1, 0.2f);
                });
               
            }
            else
            {
                index = 0;
                UIimg.DOFade(0, 0.2f).OnComplete(() =>
                {
                    UIimg.texture = images[index];
                    UIimg.DOFade(1, 0.2f);
                });
            }
        }
    }
}
