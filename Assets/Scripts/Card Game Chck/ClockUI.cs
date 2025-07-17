using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    private Text _timeText;
    private float REAL_SECONDS_PER_GAME_DAY = 30f; //1 min
    private Transform clockHourHandTransform;
    private Transform clockMinHandTransform;

    private float day;
    // Start is called before the first frame update
    void Start()
    {
        clockHourHandTransform = transform.Find("hourHand");
        clockMinHandTransform = transform.Find("minHand");
        _timeText = transform.Find("timeText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        day += Time.deltaTime / REAL_SECONDS_PER_GAME_DAY;
        float dayNormalized = day % 1f;
        float rotateDegPerDay = 360f;
        clockHourHandTransform.eulerAngles = new Vector3(0,0,-dayNormalized*rotateDegPerDay);  //Only need to modify z and for clock wise it will be neg.

        float hoursPerday = 24f;
        clockMinHandTransform.eulerAngles = new Vector3(0, 0, -dayNormalized * rotateDegPerDay*hoursPerday);  //Only need to modify z and for clock wise it will be neg.

        string hoursString = Mathf.Floor(dayNormalized * hoursPerday).ToString("00");

        float minPerHour = 60f;
        string minString = Mathf.Floor(((dayNormalized * hoursPerday)%1f)*minPerHour).ToString("00");

        _timeText.text = hoursString + ":" + minString;
    }
}
