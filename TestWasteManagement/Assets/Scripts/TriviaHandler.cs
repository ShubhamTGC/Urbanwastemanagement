using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriviaHandler : MonoBehaviour
{
    public List<string> TriviaMsg;
    public Text ShowMSg;
    public Image LoadingBar;
    [HideInInspector]
    public bool Laodingstart;
    [SerializeField]
    private float totaltime;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        System.Random ran = new System.Random();
        int randomnum = ran.Next(1, TriviaMsg.Count);
            //int randomindex = UnityEngine.Random.Range(1, TriviaMsg.Count + 1);
        ShowMSg.text = TriviaMsg[randomnum];
        Laodingstart = true;
    }

     void OnEnable()
    {
     
    }

     void OnDisable()
    {
        Laodingstart = true;
        currentTime = 0f;
        LoadingBar.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Laodingstart)
        {
            currentTime += Time.deltaTime;
            LoadingBar.fillAmount = (currentTime / totaltime);
            Laodingstart = LoadingBar.fillAmount == 1 ? false : true;
        }
    }

   
}
