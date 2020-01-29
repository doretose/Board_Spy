using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questionScript : MonoBehaviour
{
    public GameObject explanation_pannel;
    private bool is_active = false;

    public void ClickExplanation()
    {
        SoundManager.instance.ClickBtnSound();
        if (!is_active)
        {
            explanation_pannel.SetActive(true);
            is_active = true;
        }
        else
        {
            explanation_pannel.SetActive(false);
            is_active = false;
        }

    }
}
