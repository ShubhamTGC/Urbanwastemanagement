using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Networking;
using LitJson;
using SimpleSQL;

public class AssessmentGameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Stage3handler Stage3Variables;
    public Text QuestionBar;
    public List<GameObject> Buttons;
    private int QuestionCounter = 0;
    public Sprite Correctans, WrongAns;
    public int Timercount;
    public float second;
    private float totalTimercount, RunningTimeCount;
    private bool TimeBool, helpingbool;
    [SerializeField]
    private float TimerToChnageQue;
    private string correctans = "";
    [SerializeField] private Color Normalcolor;
    [SerializeField] private Color correctcolor;
    public GameObject Coundown, Finaltestmsg;
    private List<string> UserAnswer = new List<string>();
    private List<string> UserQues  = new List<string>();
    private List<int> UserScore= new List<int>();
    private List<int> is_correct = new List<int>();
    private List<int> stageQues  = new List<int>();
    private List<int> randomindex = new List<int>();
    public Text QuestionCounterText;
    private bool changeQuest;


    //==DASHBOARD FILED DATA 
    private List<string> StagewiseQues = new List<string>();
    private List<string> StagewiseAns = new List<string>();
    
    private List<string> Stage1Ques = new List<string>();
    
    private List<string> Stage2Ques = new List<string>();
    private List<string> Stage3Ques  = new List<string>();
    private List<string> Stage1ans   = new List<string>();
    private List<string> Stage2ans   = new List<string>();
    private List<string> Stage3ans   = new List<string>();
    private List<int> Stage1Score = new List<int>();
    private List<int> Stage2Score = new List<int>();
    private List<int> Stage3Score = new List<int>();



    [Header("API INTEGRATION PART")]
    public string MainUrl;
    public string GetAssessmentQues,AssessmentPostApi, MasterTabelPostApi,GetAssessmentUserLOgApi;
    public GameObject DataBarPrefeb,Dashboard;
    public GameObject Stage1DataParent, Stage2DataParent, Stage3DataParent;
    private List<string> TotalQuestionSet = new List<string>();
    private List<int>  QuesIds = new List<int>();
    private List<string> Options = new List<string>();
    private List<string> CorrectAns  = new List<string>();
    private List<string> QuestionForUser = new List<string>();
    private List<int>  QuesIdsForUser = new List<int>();
    private List<string> OptionsForUser = new List<string>();
    private List<string> CorrectAnsForUser = new List<string>();
    private List<GameObject> Stage1Rows = new List<GameObject>();
    private List<GameObject> Stage2Rows = new List<GameObject>();
    private List<GameObject> Stage3Rows = new List<GameObject>();
    private int id = 1;
    private int quescounter = 1;
    [HideInInspector] public int Stage1UserScore, Stage2UserScore, Stage3UserScore;
    private List<int>  QuestionId = new List<int>();
    private List<string> AnswersID = new List<string>();
    private List<int>  QuestionIdUser = new List<int>();
    private List<string> AnswersIDUser= new List<string>();
    private List<int> UserGivenAnsId = new List<int>();
    private List<int> TotallistScore = new List<int>();
    private int UserSelectedId;
    private int GameAttemptNumber;
    public GameObject FinalPage;
    public AudioSource GameSound;
    public AudioClip CorrectAnsmusic, WrongAnsmusic;
    public SimpleSQLManager dbmanager;
    void Start()
    {
      
    }
    

    

    IEnumerator getAssessmentLog()
    {
        string HittingUrl = $"{MainUrl}{GetAssessmentUserLOgApi}?UID={PlayerPrefs.GetInt("UID")}";
        WWW UserLog = new WWW(HittingUrl);
        yield return UserLog;
        if(UserLog.text != null)
        {
            if(UserLog.text != "[]")
            {
                AssessmentUserLog Log = Newtonsoft.Json.JsonConvert.DeserializeObject<AssessmentUserLog>(UserLog.text);
                GameAttemptNumber = Log.LastAttemptNo;
            }
        }
    }


    IEnumerator GameStarting()
    {
        Finaltestmsg.SetActive(false);
        Coundown.SetActive(true);
        Coundown.GetComponent<Text>().text = "3";
        iTween.ScaleTo(Coundown, Vector3.one, 0.3f);
        yield return new WaitForSeconds(1);
        iTween.ScaleTo(Coundown, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        Coundown.GetComponent<Text>().text = "2";
        iTween.ScaleTo(Coundown, Vector3.one, 0.3f);
        yield return new WaitForSeconds(1);
        iTween.ScaleTo(Coundown, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        Coundown.GetComponent<Text>().text = "1";
        iTween.ScaleTo(Coundown, Vector3.one, 0.3f);
        yield return new WaitForSeconds(1);
        iTween.ScaleTo(Coundown, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        Coundown.GetComponent<Text>().text = "Lets Go!";
        iTween.ScaleTo(Coundown, Vector3.one, 0.3f);
        yield return new WaitForSeconds(1);
        iTween.ScaleTo(Coundown, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        Coundown.SetActive(false);
        Finaltestmsg.SetActive(true);
        TimerToChnageQue = 0f;
        QuestionCounter = 0;
        QuestionCounterText.text = QuestionCounter.ToString();
        TimeBool = true;
        helpingbool = true;
        changeQuest = true;
        totalTimercount = Timercount * 60 + second;
        RunningTimeCount = totalTimercount;
        ChangeQuestion();

    }

    private void OnEnable()
    {
        StartCoroutine(GetSounddata());
        QuestionBar.text = "";
        Buttons.ForEach(x =>
        {
            x.GetComponent<Button>().enabled = false;
            x.GetComponent<Image>().color = Normalcolor;
            x.GetComponent<Image>().sprite = null;
            x.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        });
        StartCoroutine(GetQuestionTask());
        StartCoroutine(getAssessmentLog());
    }

    IEnumerator GetSounddata()
    {

        var SettingLog = dbmanager.Table<GameSetting>().FirstOrDefault();
        if (SettingLog != null)
        {
            GameSound.volume = SettingLog.Sound;
            PlayerPrefs.SetString("VibrationEnable", SettingLog.Vibration == 1 ? "true" : "false");
        }
        else
        {
            GameSound.volume = 1;
        }
        yield return new WaitForSeconds(0.2f);
    }


    void Update()
    {
        if (TimeBool)
        {
            if (second >= 0.0f && Timercount >= 0 && helpingbool)
            {
                second = second - Time.deltaTime;
                RunningTimeCount = RunningTimeCount - Time.deltaTime;
                TimerToChnageQue += Time.deltaTime;
                if (TimerToChnageQue.ToString("0") == "30" && changeQuest)
                {
                    TimerToChnageQue = 0f;
                    ChangesQuestionAfterTimeout();
                }
                if (second.ToString("0") == "0" && Timercount >= 0)
                {
                    second = 60;
                    Timercount = Timercount - 1;
                }
            }
            else if (helpingbool)
            {
                Debug.Log(" done timer");
                helpingbool = false;
            }
        }
    }

    void ChangesQuestionAfterTimeout()
    {
        UserGivenAnsId.Add(0);
        UserAnswer.Add("---");
        UserQues.Add(QuestionForUser[QuestionCounter - 1]);
        stageQues.Add(QuesIdsForUser[QuestionCounter - 1]);
        UserScore.Add(0);
        is_correct.Add(0);
        ChangeQuestion();
    }


    void ChangeQuestion()
    {
        if (QuestionCounter < QuestionForUser.Count)
        {
            QuestionBar.text = QuestionForUser[QuestionCounter];
            string[] Ansoption = OptionsForUser[QuestionCounter].Split("@"[0]);
            string[] AnswerId = AnswersIDUser[QuestionCounter].Split("@"[0]); 
            for (int a = 0; a < Ansoption.Length; a++)
            {
                if (Ansoption[a] == CorrectAnsForUser[QuestionCounter])
                {
                    correctans = Ansoption[a];
                    UserSelectedId = int.Parse(AnswerId[a]);
                }
                Buttons[a].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = Ansoption[a];
            }
            Buttons.ForEach(x =>
            {
                x.GetComponent<Button>().enabled = true;
                x.GetComponent<Image>().color = Normalcolor;
                x.GetComponent<Image>().sprite = null;
                x.GetComponent<Button>().onClick.RemoveAllListeners();
                x.GetComponent<Button>().onClick.AddListener(delegate { UserbuttonClick(correctans, x.gameObject, UserSelectedId); });
            });
            QuestionCounter++;
            QuestionCounterText.text = QuestionCounter.ToString();
        }
        else
        {
            TimeBool = false;
            UserGivenAnsRecord();
        }

    }


    public void UserbuttonClick(string Answer, GameObject gb ,int AnsId)
    {
        GameObject SelecteObj = EventSystem.current.currentSelectedGameObject;

        UserScore.Add(Answer == SelecteObj.transform.GetChild(0).gameObject.GetComponent<Text>().text ? 10 : 0);
        is_correct.Add(Answer == SelecteObj.transform.GetChild(0).gameObject.GetComponent<Text>().text ? 1 : 0);
        for (int a = 0; a < Buttons.Count; a++)
        {
            if (Buttons[a].transform.GetChild(0).gameObject.GetComponent<Text>().text == Answer)
            {
                Buttons[a].gameObject.GetComponent<Image>().sprite = Correctans;
                Buttons[a].gameObject.GetComponent<Image>().color = correctcolor;
                GameSound.clip = CorrectAnsmusic;
                GameSound.Play();

                if (Buttons[a].name != SelecteObj.name)
                {
                    GameSound.clip = WrongAnsmusic;
                    GameSound.Play();
                    SelecteObj.GetComponent<Image>().sprite = WrongAns;
                    SelecteObj.GetComponent<Image>().color = correctcolor;
                }
            }
            Buttons[a].GetComponent<Button>().enabled = false;
        }

        string Userans = SelecteObj.transform.GetChild(0).gameObject.GetComponent<Text>().text;
        UserGivenAnsId.Add(AnsId);
        UserAnswer.Add(Userans);
        UserQues.Add(QuestionForUser[QuestionCounter - 1]);
        stageQues.Add(QuesIdsForUser[QuestionCounter - 1]);
        StartCoroutine(USerInputDone());
    }
    IEnumerator USerInputDone()
    {
        yield return new WaitForSeconds(1f);
        TimerToChnageQue = 0;
        ChangeQuestion();
    }

    void UserGivenAnsRecord()
    {
        for (int a = 0; a < stageQues.Count; a++)
        {
            if (stageQues[a] == 1)
            {
                Stage1Ques.Add(UserQues[a]);
                Stage1ans.Add(UserAnswer[a]);
                Stage1Score.Add(UserScore[a]);
            }
            else if (stageQues[a] == 2)
            {
                Stage2Ques.Add(UserQues[a]);
                Stage2ans.Add(UserAnswer[a]);
                Stage2Score.Add(UserScore[a]);
            }
            else if (stageQues[a] == 3)
            {
                Stage3Ques.Add(UserQues[a]);
                Stage3ans.Add(UserAnswer[a]);
                Stage3Score.Add(UserScore[a]);
            }
        }
        GenerateDashBoard();
        Stage3Variables.BonusGameBool = false;
        // StartCoroutine(PostAssessmentLog());
        //StartCoroutine(PostinginMastertable());

    }


    void GenerateDashBoard()
    {
        for(int a=0;a< Stage1Ques.Count; a++)
        {
            GameObject gb = Instantiate(DataBarPrefeb, Stage1DataParent.transform, false);
            Stage1Rows.Add(gb);
            gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = (a + 1).ToString();
            gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = Stage1Ques[a];
            gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = Stage1ans[a];
            gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = Stage1Score[a].ToString();
            Stage1UserScore += Stage1Score[a];
        }
        for (int a = 0; a < Stage2Ques.Count; a++)
        {
            GameObject gb = Instantiate(DataBarPrefeb, Stage2DataParent.transform, false);
            Stage2Rows.Add(gb);
            gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = (a + 1).ToString();
            gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = Stage2Ques[a];
            gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = Stage2ans[a];
            gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = Stage2Score[a].ToString();
            Stage2UserScore += Stage2Score[a];
        }
        for (int a = 0; a < Stage3Ques.Count; a++)
        {
            GameObject gb = Instantiate(DataBarPrefeb, Stage3DataParent.transform, false);
            Stage3Rows.Add(gb);
            gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = (a + 1).ToString();
            gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = Stage3Ques[a];
            gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = Stage3ans[a];
            gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = Stage3Score[a].ToString();
            Stage3UserScore += Stage3Score[a];
        }
     
        Dashboard.SetActive(true);
        //his.gameObject.SetActive(false);
        TotallistScore.Add(Stage1UserScore);
        TotallistScore.Add(Stage2UserScore);
        TotallistScore.Add(Stage3UserScore);
    }


    IEnumerator GetQuestionTask()
    {
        int q1 = 0, q2 = 0, q3 = 0;
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrl}{GetAssessmentQues}?UID={PlayerPrefs.GetInt("UID")}&OID={PlayerPrefs.GetInt("OID")}";
        WWW GetQuestion = new WWW(HittingUrl);
        yield return GetQuestion;
        if (GetQuestion.text != null)
        {
            if (GetQuestion.text != "[]")
            {
                List<FinalAssessmentLog> AssessmentLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FinalAssessmentLog>>(GetQuestion.text);
                //Extract all question, answer, Correct ans , Options
                AssessmentLog.ForEach(x =>
                {
                    TotalQuestionSet.Add(x.brief_question);
                    QuesIds.Add(x.id_level);
                    QuestionId.Add(x.id_brief_question);
                    var OptionList = x.answer.ToList();
                    Options.Add(string.Join("@", x.answer.Select(y => y.brief_answer).ToArray()));
                    AnswersID.Add(string.Join("@", x.answer.Select(c => c.id_brief_answer).ToArray()));
                    var correctAnswer = x.answer.FirstOrDefault(c => c.is_correct_answer == 1);
                    if (correctAnswer != null)
                    {
                        CorrectAns.Add(correctAnswer.brief_answer);
                    }
                });


                //Get correct random order without duplicate
                int maxLength = 0;
                while (maxLength < TotalQuestionSet.Count)
                {
                    int num = UnityEngine.Random.Range(0, TotalQuestionSet.Count);
                    if (!randomindex.Contains(num))
                    {
                        randomindex.Add(num);
                        maxLength++;
                    }
                }

                //For randomized all the question ,ID,correct answer and options
                for (int a = 0; a < randomindex.Count; a++)
                {
                    string tempque = TotalQuestionSet[randomindex[a]];
                    int tempid = QuesIds[randomindex[a]];
                    string tempoption = Options[randomindex[a]];
                    string tempcorrectans = CorrectAns[randomindex[a]];
                    int tempqueID = QuestionId[randomindex[a]];
                    string Anstempid = AnswersID[randomindex[a]];
                    TotalQuestionSet[randomindex[a]] = TotalQuestionSet[a];
                    TotalQuestionSet[a] = tempque;
                    QuesIds[randomindex[a]] = QuesIds[a];
                    QuesIds[a] = tempid;
                    Options[randomindex[a]] = Options[a];
                    Options[a] = tempoption;
                    CorrectAns[randomindex[a]] = CorrectAns[a];
                    CorrectAns[a] = tempcorrectans;
                    QuestionId[randomindex[a]] = QuestionId[a];
                    QuestionId[a] = tempqueID;
                    AnswersID[randomindex[a]] = AnswersID[a];
                    AnswersID[a] = Anstempid;
                }


                //Select 15 out of all question based on the randomized manner
                for (int a = 0; a < QuesIds.Count; a++)
                {
                    if (QuesIds[a] == 1 && q1 < 5)
                    {
                        QuestionForUser.Add(TotalQuestionSet[a]);
                        QuesIdsForUser.Add(QuesIds[a]);
                        OptionsForUser.Add(Options[a]);
                        CorrectAnsForUser.Add(CorrectAns[a]);
                        QuestionIdUser.Add(QuestionId[a]);
                        AnswersIDUser.Add(AnswersID[a]);
                        q1++;
                    }
                    if (QuesIds[a] == 2 && q2 < 5)
                    {
                        QuestionForUser.Add(TotalQuestionSet[a]);
                        QuesIdsForUser.Add(QuesIds[a]);
                        OptionsForUser.Add(Options[a]);
                        CorrectAnsForUser.Add(CorrectAns[a]);
                        QuestionIdUser.Add(QuestionId[a]);
                        AnswersIDUser.Add(AnswersID[a]);
                        q2++;
                    }
                    if (QuesIds[a] == 3 && q3 < 5)
                    {
                        QuestionForUser.Add(TotalQuestionSet[a]);
                        QuesIdsForUser.Add(QuesIds[a]);
                        OptionsForUser.Add(Options[a]);
                        CorrectAnsForUser.Add(CorrectAns[a]);
                        QuestionIdUser.Add(QuestionId[a]);
                        AnswersIDUser.Add(AnswersID[a]);
                        q3++;
                    }
                }
                StartCoroutine(GameStarting());
            }
        }
    }


    IEnumerator PostAssessmentLog()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = MainUrl + AssessmentPostApi;
        var Logs = new List<AssessmentPostLog>();
        int l = 0;
        UserQues.ForEach(x =>
        {
            var log = new AssessmentPostLog()
            {
                id_log = 1,
                id_org_game = PlayerPrefs.GetInt("OID"),
                id_user = PlayerPrefs.GetInt("UID"),
                id_org_game_content = 0,
                attempt_no = 1,
                id_org_game_level = 1,
                id_question = QuestionIdUser[l],
                id_answer_selected = UserGivenAnsId[l],
                is_correct = is_correct[l],
                status = "A",
                updated_date_time = DateTime.Now
            };
            l = l + 1;
            Logs.Add(log);
        });


        string Datalog = Newtonsoft.Json.JsonConvert.SerializeObject(Logs);
        Debug.Log("data log for final assessment " + Datalog);
        using (UnityWebRequest request = UnityWebRequest.Put(HittingUrl, Datalog))
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

   IEnumerator PostinginMastertable()
    {
        for (int a = 0; a < TotallistScore.Count; a++) 
        {
            yield return StartCoroutine(PostScoreDriveTask(a+1, TotallistScore[a]));
        }
    }

    IEnumerator PostScoreDriveTask(int Level_id,int score)
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = MainUrl + MasterTabelPostApi;
        ScorePostModel scorePost = new ScorePostModel();
        scorePost.UID = PlayerPrefs.GetInt("UID");
        scorePost.OID = PlayerPrefs.GetInt("OID");
        scorePost.id_user = PlayerPrefs.GetInt("UID");
        scorePost.id_game_content = 0;
        scorePost.score = score;
        scorePost.id_score_unit = 1;
        scorePost.score_type = 1;
        scorePost.score_unit = "points";
        scorePost.status = "A";
        scorePost.updated_date_time = DateTime.Now.ToString();
        scorePost.id_level = Level_id;
        scorePost.id_org_game = 1;
        scorePost.attempt_no = GameAttemptNumber + 1;
        scorePost.timetaken_to_complete = "00:00";
        scorePost.is_completed = 1;
        scorePost.game_type = 3;

        string Data_log = Newtonsoft.Json.JsonConvert.SerializeObject(scorePost);
        Debug.Log("data log for Drive Game" + Data_log);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, Data_log))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log(Request.downloadHandler.text);
                MasterTabelResponse masterRes = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterTabelResponse>(Request.downloadHandler.text);
                if (masterRes.STATUS.ToLower() == "success")
                {

                }
                else
                {
                    Debug.Log(" TSTATUS  ====  FAILED stage 1 zonehandler script ");
                }
            }

        }
    }


    public void CloseAssessment()
    {
        StartCoroutine(ClearAssessment());
    }

    IEnumerator ClearAssessment()
    {
        UserAnswer.Clear();
        UserQues.Clear();
        UserScore.Clear();
        is_correct.Clear();
        stageQues.Clear();
        randomindex.Clear();
        Stage2Ques.Clear();
        Stage3Ques.Clear();
        Stage1ans.Clear();
        Stage2ans.Clear();
        Stage3ans.Clear();
        Stage1Score.Clear();
        Stage2Score.Clear();
        Stage3Score.Clear();
        TotalQuestionSet.Clear();
        QuesIds.Clear();
        Options.Clear();
        CorrectAns.Clear();
        QuestionForUser.Clear();
        QuesIdsForUser.Clear();
        OptionsForUser.Clear();
        CorrectAnsForUser.Clear();
        Stage1Rows.Clear();
        Stage2Rows.Clear();
        Stage3Rows.Clear();
        QuestionId.Clear();
        AnswersID.Clear();
        QuestionIdUser.Clear();
        AnswersIDUser.Clear();
        UserGivenAnsId.Clear();
        TotallistScore.Clear();
        int stage1child = Stage1DataParent.transform.childCount;
        for(int a=0;a< stage1child; a++)
        {
            Destroy(Stage1DataParent.transform.GetChild(a).gameObject);
        }
        int stage2child = Stage2DataParent.transform.childCount;
        for (int a = 0; a < stage2child; a++)
        {
            Destroy(Stage2DataParent.transform.GetChild(a).gameObject);
        }
        int stage3child = Stage3DataParent.transform.childCount;
        for (int a = 0; a < stage3child; a++)
        {
            Destroy(Stage3DataParent.transform.GetChild(a).gameObject);
        }
       // Stage3Variables.BonusGameBool = false;
        yield return new WaitForSeconds(1f);
        Dashboard.SetActive(false);
        FinalPage.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
