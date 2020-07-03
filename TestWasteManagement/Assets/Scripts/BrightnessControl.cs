using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BrightnessControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Text valuetext;
    private float brightnessvalue;
    public Image brightnesspanel;

    public GameObject SoundBtn,VibrationBtn,MusicBtn;
    public Sprite OnSprite, OffSprite;
    private string SoundStatus, VibrationStatus = "true";
    public Slider MusicSlider;
    void Start()
    {
     
    }

    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            MusicSlider.value = 1f;
        }
        else
        {
            MusicSlider.value = PlayerPrefs.GetFloat("volume");
        }
    }

    // Update is called once per frame
    void Update()
    {
        brightnesspanel.color = new Color(0, 0, 0, brightnessvalue);
    }

    public void SetVolume(float val)
    {
        brightnessvalue = 1- val;
        float value = val * 100;
        valuetext.text = value.ToString("F0");
    }


    public void saveData()
    {
        //PlayerPrefs.SetFloat("brightness", brightnessvalue);
        PlayerPrefs.SetString("Sound", SoundStatus);
        PlayerPrefs.SetString("VibrationEnable", VibrationStatus);
        this.gameObject.SetActive(false);
    }

    public void SoundControl()
    {
        if(SoundBtn.GetComponent<Image>().sprite.name == "On")
        {
            SoundBtn.GetComponent<Image>().sprite = OffSprite;
            SoundStatus = "false";
        }
        else
        {
            SoundStatus = "true";
            SoundBtn.GetComponent<Image>().sprite = OnSprite;
        }
    }

    public void VibrationControl()
    {
        if (VibrationBtn.GetComponent<Image>().sprite.name == "On")
        {
            VibrationStatus = "false";
            VibrationBtn.GetComponent<Image>().sprite = OffSprite;
        }
        else
        {
            VibrationStatus = "true";
            VibrationBtn.GetComponent<Image>().sprite = OnSprite;
        }
    }

    public void MusicControl()
    {
        if (MusicBtn.GetComponent<Image>().sprite.name == "On")
        {
            //SoundStatus = "false";
            MusicBtn.GetComponent<Image>().sprite = OffSprite;
            //Camera.main.gameObject.GetComponent<AudioSource>().enabled = false;
            MusicSlider.value = 0;
        }
        else
        {
            //VibrationStatus = "true";
            MusicBtn.GetComponent<Image>().sprite = OnSprite;
            // Camera.main.gameObject.GetComponent<AudioSource>().enabled = true;
            MusicSlider.value = PlayerPrefs.GetFloat("volume");
        }
    }



}
