using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SimplePlayerControl : MonoBehaviour
{
    public Text mText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void M_A() {
        transform.Rotate(new Vector3(0, -14, 0) * Time.deltaTime * 4.5f, Space.Self);
        mText.text = "A";
    }

    public void M_D()
    {
        transform.Rotate(new Vector3(0, 14, 0) * Time.deltaTime * 4.5f, Space.Self);
        mText.text = "D";
    }

    public void M_W()
    {
        transform.Translate(transform.forward * Time.deltaTime * 2.45f, Space.World);
        mText.text = "W";
    }

    public void M_S()
    {
        transform.Translate(-transform.forward * Time.deltaTime * 2.45f, Space.World);
        mText.text = "S";
    }
}
