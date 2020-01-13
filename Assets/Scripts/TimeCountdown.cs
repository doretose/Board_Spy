using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCountdown : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float timeCost;
    private bool timerBoolean = false;

    public void startTimer()
    {
        timeText.gameObject.SetActive(true);
        timeCost = 20;
        timerBoolean = true;
    }

    private void Update()
    {
        if (timerBoolean)
        {
            timeCost -= Time.deltaTime;
            timeText.text = string.Format("{0:0}", timeCost);

            if (timeCost == 0)
            {
                timeText.gameObject.SetActive(false);
                timerBoolean = false;
                this.GetComponent<NetworkRoundManager>().();
            }
        }
    }


}
