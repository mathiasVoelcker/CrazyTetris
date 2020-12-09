using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{

    [SerializeField]
    private GameObject SettingsPanel;

    [SerializeField]
    private GameObject MenuPanel;

    [SerializeField]
    private GameObject CheckSoundEffect;

    public bool isSoundActive {
        get
        {
            return CheckSoundEffect.GetComponent<Image>().color.a == 1;
        }
    }

    public void ChangeSound()
    {
        if (isSoundActive)
        {
            CheckSoundEffect.GetComponent<Image>().color = new Color(255,255,255,0);
            GameController.IsSoundActive = false;
        }
        else
        {
            CheckSoundEffect.GetComponent<Image>().color = new Color(255,255,255,1);
            GameController.IsSoundActive = true;
        }
    }

    public void BackToMenu()
    {
        SettingsPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }
    
}
