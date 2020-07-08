using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;


public class AnagrameController : MonoBehaviour
{
    public string[] Answordlist;
    public string[] Questionlist;
    public Text Currentquestion;
    private int CurrentWordCount =0;
    private string AnswerWord;
    private string[] WordSplits;
    public GameObject WordPrefebs,DashPrefeb;
    public Transform WordPrenet, DashPrenet;
    private List<GameObject> Words = new List<GameObject>();
    private List<GameObject> Dashs = new List<GameObject>();
    private string[] CorrectAns;
    private string[] SelectedWord;
    private int SelectionCounter = 0;
    private int CorrectAnsCounter;
    private int RunningWordCount;
    [Header("timer section")]
    [Space(10)]
    public Text Timer;
    public Image Timerbar;
    public float minute, second;
    private float sec, Totaltimer, RunningTimer;
    private bool helpingbool = true;
    private bool WrongGuess = true;
    private float wrongGuessTimer;
    private bool Timepaused = true;
    public GameObject GameoverObj;
    public Text ScoreText;
    private int score;

    //============ TEMP WORKING VARIABLES===========================//
    [SerializeField]
    private List<int> randomindex;

    void Start()
    {

        Timer.text = "0" + minute + ":" + second;
        Totaltimer = (minute * 60) + second;     
        sec = second;
        RunningTimer = Totaltimer;
        int maxLength = 0;
        while (maxLength < Answordlist.Length)
        {
            int num = UnityEngine.Random.Range(1, Answordlist.Length + 1);
            if (!randomindex.Contains(num))
            {
                randomindex.Add(num);
                maxLength++;
            }
        }
        Debug.Log("total length " + randomindex.Count);
        for(int a = 0; a < randomindex.Count; a++)
        {
           
            int num = randomindex[a];
            Debug.Log(num);
            string temp = Answordlist[a];
            string temp2 = Questionlist[a];
            Questionlist[a] = Questionlist[num-1];
            Answordlist[a] = Answordlist[num-1];
            Answordlist[num-1] = temp;
            Questionlist[num-1] = temp2;
        }
        GameSetup(CurrentWordCount);
    }


    // Update is called once per frame
    void Update()
    {
        if (Timepaused)
        {
            if (sec >= 0 && minute >= 0 && helpingbool)
            {
                sec = sec - Time.deltaTime;
                RunningTimer = RunningTimer - Time.deltaTime;
                Timerbar.fillAmount = RunningTimer / Totaltimer;
                if (sec.ToString("0").Length > 1)
                {
                    Timer.text = "0" + minute.ToString("0") + ":" + sec.ToString("0");
                }
                else
                {
                    Timer.text = "0" + minute.ToString("0") + ":" + "0" + sec.ToString("0");
                }

                if (sec.ToString("0") == "0" && minute >= 0)
                {
                    sec = 60;
                    minute = minute - 1;
                }
            }
            else if (helpingbool)
            {
                helpingbool = false;

            }
        }
      
       
        if (WrongGuess)
        {
            wrongGuessTimer = wrongGuessTimer + Time.deltaTime;
            if(wrongGuessTimer.ToString("0") == "15")
            {
                WrongGuess = false;
                wrongGuessTimer = 0f;
                score += 0;
                StartCoroutine(ResetAnagramFields());
            }
        }
    }

    void GameSetup(int WordNumber)
    {
        //this will split the word into letters
        AnswerWord = Answordlist[WordNumber];
        Currentquestion.text = "";
        Currentquestion.text = Questionlist[WordNumber];
        WordSplits = new string[AnswerWord.Length];
        CorrectAns = new string[AnswerWord.Length];
        SelectedWord = new string[AnswerWord.Length];
        for (int a = 0; a < AnswerWord.Length; a++)
        {
            WordSplits[a] = Convert.ToString(AnswerWord[a]);
            CorrectAns[a] = WordSplits[a];

        }

        //Randomize the letters
        for(int b =0;b< WordSplits.Length; b++)
        {
            string temp = WordSplits[b];
            int Randomindex = UnityEngine.Random.Range(1, WordSplits.Length);
            WordSplits[b] = WordSplits[Randomindex];
            WordSplits[Randomindex] = temp;
        }

        for(int c =0;c < WordSplits.Length; c++)
        {
            GameObject _wordgb = Instantiate(WordPrefebs, WordPrenet, false);
            GameObject _dashgb = Instantiate(DashPrefeb, DashPrenet, false);
            _wordgb.transform.GetChild(0).GetComponent<Text>().text = WordSplits[c];
            _wordgb.GetComponent<Button>().onClick.AddListener(delegate { WordButtonMethod(); });
            Words.Add(_wordgb);
            Dashs.Add(_dashgb);

        }

    }

    public void WordButtonMethod()
    {
        GameObject selectedobj = EventSystem.current.currentSelectedGameObject;
        string word = selectedobj.transform.GetChild(0).GetComponent<Text>().text;
        Dashs[SelectionCounter].transform.GetChild(0).GetComponent<Text>().text = word;
        selectedobj.transform.GetChild(0).GetComponent<Text>().text = "";
        SelectedWord[SelectionCounter] = word;
        SelectionCounter++;
        selectedobj.GetComponent<Button>().enabled = false;
        if(SelectionCounter == Dashs.Count)
        {
            StartCoroutine(CheckForAns());
        }
        

    }
    IEnumerator CheckForAns()
    {
        yield return new WaitForSeconds(1f);
        for (int a = 0; a < CorrectAns.Length; a++)
        {
            if(CorrectAns[a] == SelectedWord[a])
            {
                CorrectAnsCounter++;
            }
        }
        if(CorrectAnsCounter == CorrectAns.Length)
        {
            wrongGuessTimer = 0;
            score += 10;
            StartCoroutine(ResetAnagramFields());
        }
        else
        {
            // WrongGuess = true;
            wrongGuessTimer = 0;
            score += 0;
            StartCoroutine(ResetAnagramFields());
        }
     
    }
    IEnumerator ResetAnagramFields()
    {
        for (int a = 0; a < Words.Count; a++)
        {
            DestroyImmediate(Words[a].gameObject);
            DestroyImmediate(Dashs[a].gameObject);
        }
        SelectionCounter = 0;
        Words.Clear();
        Dashs.Clear();
        Array.Clear(CorrectAns, 0, CorrectAns.Length);
        Array.Clear(WordSplits, 0, WordSplits.Length);
        Array.Clear(SelectedWord, 0, SelectedWord.Length);
        CorrectAnsCounter = 0;
        yield return new WaitForSeconds(1f);
        CurrentWordCount++;
        WrongGuess = true;
        if(CurrentWordCount < Answordlist.Length)
        {
            GameSetup(CurrentWordCount);
        }
        else
        {
          StartCoroutine(GameoverPage());
        }
        
        
    }
    IEnumerator GameoverPage()
    {
        yield return new WaitForSeconds(0.5f);
        Timepaused = false;
        ScoreText.text = "You got total bonus score : " + score;
        GameoverObj.SetActive(true);

    }


}
