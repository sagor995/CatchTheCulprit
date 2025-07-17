using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardModel : MonoBehaviour
{

    Image spriteImage;
    public Sprite[] faces;
    public Sprite cardBack;
    public Sprite cardWho;
    public Sprite cardEmpty;

    public int cardIndex;

    public void ToggleFace(bool showFace) {
         var sprites = Resources.LoadAll<Sprite>("avatar");

        if (showFace) {
            spriteImage.sprite = faces[cardIndex];
        }
        else{
            spriteImage.sprite = cardBack;
        }
    }

    void Awake()
    {
        spriteImage = GetComponent<Image>();
    }
}
