using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChangeCard : MonoBehaviour
{
    CardFlipper flipper;
    CardModel cardModel;
    int cardIndex = 0;
    public GameObject card;

    bool isFlipped = false;
    [SerializeField]
    int ci = 2;

    void Awake()
    {
        cardModel = card.GetComponent<CardModel>();
        flipper = card.GetComponent<CardFlipper>();
    }

    void OnGUI()
    {

        /*
        if (GUI.Button(new Rect(10,10,100,28),"Hit me!")) {

            if (cardIndex>=cardModel.faces.Length) {
                cardIndex = 0;
                //cardModel.ToggleFace(false);
                flipper.FlipCard(cardModel.faces[cardModel.faces.Length-1],cardModel.cardBack,-1);
            }
            else
            {
                if (cardIndex>0) {
                    flipper.FlipCard(cardModel.faces[cardIndex-1], cardModel.faces[cardIndex], cardIndex);
                }
                else
                {
                    flipper.FlipCard(cardModel.cardBack,cardModel.faces[cardIndex],cardIndex);
                }
                //cardModel.cardIndex = cardIndex;
                //cardModel.ToggleFace(true);

                cardIndex++;
            }
            
        }

        */

        if (GUI.Button(new Rect(10, 100, 100, 28), "Hit me2!"))
        {
            if (isFlipped == false) {
                flipper.FlipCard2(cardModel.cardBack, cardModel.faces[ci]);
                isFlipped = true;
            }else if(isFlipped == true) {
                flipper.FlipCard2( cardModel.faces[ci], cardModel.cardBack);
                isFlipped = false;
            }   
        }
    }

    public void OnCard_Click() {
        
        if (isFlipped == false)
        {
            flipper.FlipCard2(cardModel.cardBack, cardModel.faces[ci]);
            isFlipped = true;
        }
        else if (isFlipped == true)
        {
            flipper.FlipCard2(cardModel.faces[ci], cardModel.cardBack);
            isFlipped = false;
        }

      
    }
}
