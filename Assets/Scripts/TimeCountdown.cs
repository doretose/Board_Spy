using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCountdown : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float timeCost;
    private bool timerBoolean = false;
    public static bool staticTimerBool = false;

    public void startTimer()
    {
        if (timerBoolean == false)
        {
            timeText.gameObject.SetActive(true);
            timeCost = 20;
            staticTimerBool = true;
        }
        else return;
    }

    private void Update()
    {
        timerBoolean = staticTimerBool;
        if (timerBoolean)
        {
            timeCost -= Time.deltaTime;
            timeText.text = string.Format("{0:0}", timeCost);

            if (timeCost == 0.0f)
            {
                this.GetComponent<NetworkRoundManager>().EndTrun();
                timeText.gameObject.SetActive(false);
<<<<<<< HEAD
                timerBoolean = false;
                this.GetComponent<NetworkRoundManager>().EndTrun();
=======
>>>>>>> b5bea4f971613b3000d6e515134d7712303b80fb
            }
        }
        else
        {
            timeText.gameObject.SetActive(false);
            timerBoolean = false;
        }
        
    }
}
