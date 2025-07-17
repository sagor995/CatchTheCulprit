using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testM : MonoBehaviour
{
    public Text uText;
    public GameObject time;
    public Image uImg;
    int mode_vlaue = 1;
    int mode_ovalue = 5;

    int picking_index = 0;

    public GameObject uPanel;
    //public GameObject button;
    public GameObject contentView;
    public GameObject contentData;
    public GameObject emoBox;

    Coroutine c1;
    void Start()
    {
        AssginEmo();
       // emoBox.SetActive(true);
    }

    

    void AssginEmo()
    {
        var sprites = Resources.LoadAll<Sprite>("emoj");
        String length = sprites.Length.ToString();
        if (Convert.ToInt32(length)>0) {
            RectTransform rct = (RectTransform)contentView.transform;
            // foreach (Sprite value in sprites) {
            for (int i = 0;i<sprites.Length;i++) {
                Sprite value = sprites[i];
                GameObject emoBtn = (GameObject)Instantiate(contentData);
                emoBtn.transform.SetParent(contentView.transform);
                emoBtn.transform.localScale = new Vector3(1,1,1);
                emoBtn.transform.localPosition = new Vector3(0,0,0);
                emoBtn.transform.GetComponent<Image>().sprite = value;
                int id = i;
                emoBtn.transform.GetComponent<Button>().onClick.AddListener(() => CallEmoNow(id));
            }
        }
    }

    void CallEmoNow(int index)
    {
        var sprites = Resources.LoadAll<Sprite>("emoj");
        uText.text = "" + index;
        uImg.sprite = sprites[index];
    }

    public void CloseEmoBox() {
        emoBox.SetActive(false);
    }

    void actionToMaterial(int idx)
    {
        Debug.Log("change material to HIT  on material :  " + idx);
    }

    void OpenPopUpMsgPanel() {


    }

    int selectImgIndex() {

        return 1;
    }

    void Update()
    {
        if (uPanel.activeSelf) {
            Debug.Log("Active");
        }
        else
        {
            Debug.Log("Inactive");
        }

        
        
    }

    public void ClosePanel() {
        c1 = StartCoroutine(TurnTimer(15));
    }

    public void CloseCoroutine()
    {
        StopCoroutine(c1);
    }

    IEnumerator TurnTimer(int v)
    {
        time.SetActive(true);

        float f = 1 ;

        for (int i=0;i<v;i++)
        {
            time.GetComponentInChildren<Text>().text = "" + (i+1);

            float fill = (float)1 / 15;
            f -= fill;
            time.GetComponent<Button>().image.fillAmount = f;

            yield return new WaitForSeconds(1);
        }
        time.GetComponent<Text>().text = "Done";
        yield return new WaitForSeconds(2);
        //time.GetComponent<Text>().text = "Boom!";
        time.SetActive(false);
    }
}


