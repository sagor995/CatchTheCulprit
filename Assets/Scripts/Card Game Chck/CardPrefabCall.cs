using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrefabCall : MonoBehaviour
{
    public GameObject card1;

    // Start is called before the first frame update
    void Start()
    {

        Instantiate(card1,transform.position,transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
