﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DustbinCenterHandler : MonoBehaviour
{
    public GameBoard Gamemanager;
    public GameObject CorrectAns, WrongAns;
    private AudioSource SoundEffect;
    public AudioClip CenterSound, wrongcenter;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        SoundEffect = this.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Truck")
        {
            if (this.gameObject.name == other.gameObject.name)
            {
                other.gameObject.SetActive(false);
                string truckname = other.gameObject.name;
                Gamemanager.Blasteffect.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Wow";
                Gamemanager.Blasteffect.SetActive(true);
                CorrectAns.transform.position = this.transform.position;
                StartCoroutine(AnsStatus(CorrectAns, 50, truckname,this.gameObject.name, CenterSound));
            }
            else
            {
                string truckname = other.gameObject.name;
                other.gameObject.SetActive(false);
                Gamemanager.Blasteffect.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Oops";
                Gamemanager.Blasteffect.SetActive(true);
                string centername = this.gameObject.name;
                WrongAns.transform.position = this.transform.position;
                StartCoroutine(AnsStatus(WrongAns, 0, truckname, this.gameObject.name, wrongcenter));
            }
        }
       
    }


    IEnumerator AnsStatus(GameObject AnsEffect,int Score,string TruckName,string CenterName,AudioClip sound)
    {
        SoundEffect.clip = sound;
        SoundEffect.Play();
        AnsEffect.SetActive(true);
        Gamemanager.VibrateDevice();
        Gamemanager.TruckCenterResult(Score, TruckName,CenterName);
        yield return new WaitForSeconds(2f);
        iTween.ScaleTo(Gamemanager.Blasteffect, Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.3f);
        Gamemanager.Blasteffect.SetActive(false);



    }
}
