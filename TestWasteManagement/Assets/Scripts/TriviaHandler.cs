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
    
    }

     void OnEnable()
    {
        int randomindex = UnityEngine.Random.Range(1, TriviaMsg.Count + 1);
        ShowMSg.text = TriviaMsg[randomindex];
        Laodingstart = true;
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
