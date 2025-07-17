using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    Image spriteImage;
    CardModel model;

    public AnimationCurve scaleCurve;
    public float duration = 0.5f;

    void Awake()
    {
        spriteImage = GetComponent<Image>();
        model = GetComponent<CardModel>();


    }

    public void FlipCard(Sprite startImg, Sprite endImg, int cardIndex) {
        StopCoroutine(Flip(startImg, endImg, cardIndex));
        StartCoroutine(Flip(startImg, endImg, cardIndex));
    }

    IEnumerator Flip(Sprite startImg, Sprite endImg, int cardIndex) {

        spriteImage.sprite = startImg;
        float time = 0f;
        while (time<=1f) {
            float scale = scaleCurve.Evaluate(time);
            time = time + Time.deltaTime/duration;

            Vector3 localScale = transform.localScale;
            localScale.x = scale;
            transform.localScale = localScale;  //Scaling it x axis like  squishing image

            if (time >=0.5f) {
                spriteImage.sprite = endImg;
            }

            yield return new WaitForFixedUpdate();
        }

        if (cardIndex==-1) {
            model.ToggleFace(false);
        }
        else
        {
            model.cardIndex = cardIndex;
            model.ToggleFace(true);
        }
    }

    public void FlipCard2(Sprite startImg, Sprite endImg)
    {
        StopCoroutine(Flip2(startImg, endImg));
        StartCoroutine(Flip2(startImg, endImg));
    }

    IEnumerator Flip2(Sprite startImg, Sprite endImg)
    {

        spriteImage.sprite = startImg;
        float time = 0f;
        while (time <= 1f)
        {
            float scale = scaleCurve.Evaluate(time);
            time = time + Time.deltaTime / duration;

            Vector3 localScale = transform.localScale;
            localScale.x = scale;
            transform.localScale = localScale;  //Scaling it x axis like  squishing image

            if (time >= 0.5f)
            {
                spriteImage.sprite = endImg;
            }

            yield return new WaitForFixedUpdate();
        }

    }
}
