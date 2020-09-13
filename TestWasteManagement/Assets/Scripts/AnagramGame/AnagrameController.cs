using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using LitJson;

public class AnagrameController : MonoBehaviour
{
    [SerializeField]
    private string[] Answordlist;
    private string[] Questionlist;
    public Sprite[] AnswerSprite;
    public Text Currentquestion;
    public Image HintImage;
    private int CurrentWordCount =0;
    private string AnswerWord;
    private string[] WordSplits;
    public GameObject WordPrefebs,DashPrefeb;
    public Transform WordPrenet, DashPrenet;
    private List<GameObject> Words = new List<GameObject>();
    private List<GameObject> Dashs = new List<GameObject>();
    private string[] CorrectAns;
    private string[] SelectedWord;
    private List<string> AnsStatus = new List<string>();
    private List<string> UserGivenWord = new List<string>();
    private int[] OriginalIds;
    private int[] id_word;
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
    public GameObject BlastEffect;
    //============ TEMP WORKING VARIABLES===========================//
    private List<int> randomindex = new List<int>();
    [SerializeField]
    private AudioSource AudioObject;
    public AudioClip CorrectSound, WrongSound;

    public string MainUrl, AnagramApi,PostDataApi,UserScorePosting, GetlevelWisedataApi;
    private AnagramPostData anagramePostdata;
    private ZoneShowHandler ZoneHandlers;
    public int Gamelevel;
    private int GameAttemptNumber;
    void Start()
    {
        ZoneHandlers = new ZoneShowHandler();
        score = 0;
        sec = second;
        Timepaused = true;
        Timer.text = "0" + minute + ":" + second;
        Totaltimer = (minute * 60) + second;
        RunningTimer = Totaltimer;
        StartCoroutine(GetAnagramedata());

    }

    private void OnEnable()
    {
        StartCoroutine(GetGameAttemptNoTask());
    }

    IEnumerator GetGameAttemptNoTask()
    {
        string HittingUrl = $"{MainUrl}{GetlevelWisedataApi}?UID={PlayerPrefs.GetInt("UID")}&OID={PlayerPrefs.GetInt("OID")}&id_level={Gamelevel}&game_type={2}";
        WWW Attempt_res = new WWW(HittingUrl);
        yield return Attempt_res;
        if (Attempt_res.text != null)
        {
            if (Attempt_res.text != "[]")
            {
                List<LevelUserdataModel> leveldata = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LevelUserdataModel>>(Attempt_res.text);
                GameAttemptNumber = leveldata.Count;
            }
            else
            {
                GameAttemptNumber = 0;
            }


        }
    }

    void Mainsetup()
    {
        
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
        id_word = new int[Answordlist.Length];
        for (int a = 0; a < randomindex.Count; a++)
        {

            int num = randomindex[a];
            Debug.Log(num);
            id_word[a] = OriginalIds[num-1];
            string temp = Answordlist[a];
            string temp2 = Questionlist[a];
            Sprite tempsprite = AnswerSprite[a];
            Questionlist[a] = Questionlist[num - 1];
            Answordlist[a] = Answordlist[num - 1];
            AnswerSprite[a] = AnswerSprite[num - 1];
            AnswerSprite[num - 1] = tempsprite;
            Answordlist[num - 1] = temp;
            Questionlist[num - 1] = temp2;
        }
        GameSetup(CurrentWordCount);
    }

    IEnumerator GetAnagramedata()
    {
       
        string HittingUrl = MainUrl + AnagramApi + "?id_sheet=" + 1;
        WWW Anagrame_www = new WWW(HittingUrl);
        yield return Anagrame_www;
        if(Anagrame_www.text != null)
        {
            Debug.Log("anagrame question " + Anagrame_www.text);
            List<AnagramModel> AnagramResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AnagramModel>>(Anagrame_www.text);
            int arrayindex =0;
            Answordlist = new string[AnagramResponse.Count];
            Questionlist = new string[AnagramResponse.Count];
            OriginalIds = new int[AnagramResponse.Count];
            AnagramResponse.ForEach(x =>
            {

                Questionlist[arrayindex] = x.question;
                Answordlist[arrayindex] = x.answer;
                OriginalIds[arrayindex] = x.id_word;
                arrayindex++;

            });
            Mainsetup();

        }
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
                StartCoroutine(GameoverPage());

            }
        }
      
       
        if (WrongGuess)
        {
            wrongGuessTimer = wrongGuessTimer + Time.deltaTime;
            if(wrongGuessTimer.ToString("0") == "15")
            {
                WrongGuess = false;
                StartCoroutine(CheckForAns());
            }
        }
    }

    void GameSetup(int WordNumber)
    {
        //this will split the word into letters
        AnswerWord = Answordlist[WordNumber];
        Currentquestion.text = "";
        Currentquestion.text = Questionlist[WordNumber];
        HintImage.sprite = AnswerSprite[WordNumber];
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
        selectedobj.GetComponent<Button>().enabled = false;
        selectedobj.GetComponent<Image>().enabled = false;
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
            AnsStatus.Add("1");
            AudioObject.clip = CorrectSound;
            StartCoroutine(ResetAnagramFields());
        }
        else
        {
            Debug.Log("wrong ans");
            // WrongGuess = true;
            AnsStatus.Add("0");
            BlastEffect.SetActive(true);
            wrongGuessTimer = 0;
            score += 0;
            AudioObject.clip = WrongSound;
            StartCoroutine(ResetAnagramFields());
        }
     
    }
    IEnumerator ResetAnagramFields()
    {

        if(SelectedWord.Length > 0)
        {
            string UserWord = string.Join("", SelectedWord);
            UserGivenWord.Add(UserWord);
        }
        else
        {
            UserGivenWord.Add("null");
        }
       
        for (int a = 0; a < Words.Count; a++)
        {
            DestroyImmediate(Words[a].gameObject);
            DestroyImmediate(Dashs[a].gameObject);
        }
        AudioObject.Play();
        SelectionCounter = 0;
        Words.Clear();
        Dashs.Clear();
        Array.Clear(CorrectAns, 0, CorrectAns.Length);
        Array.Clear(WordSplits, 0, WordSplits.Length);
        Array.Clear(SelectedWord, 0, SelectedWord.Length);
        CorrectAnsCounter = 0;
        yield return new WaitForSeconds(0.6f);
        iTween.ScaleTo(BlastEffect, Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.4f);
        BlastEffect.SetActive(false);
        CurrentWordCount++;
        WrongGuess = true;
        if (CurrentWordCount < Answordlist.Length)
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
        StartCoroutine(GameScorePosting());
        yield return new WaitForSeconds(0.5f);
        for (int a = 0; a < Words.Count; a++)
        {
            DestroyImmediate(Words[a].gameObject);
            DestroyImmediate(Dashs[a].gameObject);
        }
        SelectionCounter = 0;
        CorrectAnsCounter = 0;
        Words.Clear();
        Dashs.Clear();
        WrongGuess = false;
        Timepaused = false;
        PlayerPrefs.SetInt("BonusScore", score);
        ScoreText.text = "You got total bonus score : " + score;
        GameoverObj.SetActive(true);
        
        

    }
    IEnumerator GameScorePosting()
    {
        yield return new WaitForSeconds(0.1f);
        string hittingUrl = MainUrl + UserScorePosting;
        ScorePostModel postField = new ScorePostModel();
        postField.UID = PlayerPrefs.GetInt("UID");
        postField.OID = PlayerPrefs.GetInt("OID");
        postField.id_log = 1;
        postField.id_user = PlayerPrefs.GetInt("UID");
        postField.id_game_content = 0;
        postField.score = score;
        postField.id_score_unit = 1;
        postField.score_type = 1;
        postField.score_unit = "points";
        postField.status = "a";
        postField.updated_date_time = DateTime.Now.ToString();
        postField.id_level = 1;
        postField.id_org_game = 1;
        postField.attempt_no = GameAttemptNumber +1;
        postField.timetaken_to_complete = "00:00";
        postField.is_completed = 1;
        postField.game_type = 2;


        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(postField);
        Debug.Log("Score posting " + PostLog);

        using (UnityWebRequest request = UnityWebRequest.Put(hittingUrl, PostLog))
        {
            Debug.Log(request);
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {
                MasterTabelResponse post_res = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterTabelResponse>(request.downloadHandler.text);
                if (post_res.STATUS.ToLower() == "success")
                {
                    Debug.Log("successfully posted ");
                    ZoneHandlers.AnagramPlayed = true;
                    StartCoroutine(PostGameData());
                }
            }
            else
            {
                Debug.Log("prob : " + request.error);
            }
        }


    }

    IEnumerator PostGameData()
    {
        var logs = new List<AnagramPostData>();
        int a = 0;
        if(UserGivenWord.Count > 0)
        {
            UserGivenWord.ForEach(x =>
            {
                var log = new AnagramPostData()
                {
                    id_user = PlayerPrefs.GetInt("UID").ToString(),
                    id_sheet = "1",
                    id_word = id_word[a].ToString(),
                    is_correct = AnsStatus[a],
                    user_input = UserGivenWord[a]

                };
                a = a + 1;
                logs.Add(log);
            });
        }
        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(logs);
        Debug.Log("Getting log" + Newtonsoft.Json.JsonConvert.SerializeObject(logs));
        string HIttingUrl = MainUrl + PostDataApi;
        yield return new WaitForSeconds(0.1f);
        using (UnityWebRequest request = UnityWebRequest.Put(HIttingUrl, PostLog))
        {
            Debug.Log(request);
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {
                Debug.Log(request.downloadHandler.text);

                JsonData post_res = JsonMapper.ToObject(request.downloadHandler.text);
                string authstatus = post_res["STATUS"].ToString();
                if (authstatus.ToLower() == "success")
                {
                    Debug.Log("successfully posted ");
                }
            }
            else
            {
                Debug.Log("prob : " + request.error);
            }
        }


    }


}
