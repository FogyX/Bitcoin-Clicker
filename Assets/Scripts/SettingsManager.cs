using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [HideInInspector]
    public bool SfxEnabled = true;
    [HideInInspector]
    public bool VibrationEnabled = true;

    public string PrivacyLink;
    [Space]

    public GameObject SfxButton;

    private int SfxIndex = 0;
    private int VibartionIndex = 0;


    void Start()
    {
        if (PlayerPrefs.GetInt("sfxindex") == 1)
        {
            SfxButton.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("sfxindex") == 0)
        {
            SfxButton.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void SFX()
    {
        if (SfxEnabled)
        {
            SfxEnabled = false;
            SfxButton.transform.GetChild(1).gameObject.SetActive(true);
            SfxIndex = 1;
            PlayerPrefs.SetInt("sfxindex", SfxIndex);


        }

        else if (!SfxEnabled)
        {
            SfxEnabled = true;
            SfxButton.transform.GetChild(1).gameObject.SetActive(false);
            SfxIndex = 0;
            PlayerPrefs.SetInt("sfxindex", SfxIndex);

        }
    }

    public void privacy()
    {
        Application.OpenURL(PrivacyLink);
    }
}
