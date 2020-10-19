using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using LitJson;
using SimpleSQL;

public class GameBoard : MonoBehaviour
{

    private static int Boardwidth = 5000;
    private static int BoardHeight = 5000;
    public GameObject[,] board = new GameObject[Boardwidth, BoardHeight];

    public GameObject[] Biohazard;
    public GameObject[] Organic, Ewaste, Recycle;
    private List<int> randomindex = new List<int>();
    private List<GameObject> CurrentActive = new List<GameObject>();
    private List<GameObject> PlasticActive = new List<GameObject>();
    private List<GameObject> GlassActive = new List<GameObject>();
    private List<GameObject> MetalActive = new List<GameObject>();
    private int ObjCounter = 0;
    private int num = 0;
    public List<GameObject> TrucksPriority = new List<GameObject>();
    public GameObject TruckGamePlatform;
    private int selectionCounter = 1;
    private List<KeyValuePair<string, GameObject[]>> AllBins;
    public GameObject Monster, Monster2;
    private int ActiveTruckCount = 0;
    public List<GameObject> TruckPoits;
    public List<GameObject> StationaryTrucks = new List<GameObject>();
    private int Taskcounter = 0;
    private bool CheckedCollision;
    private List<GameObject> FirstBin, SecondBin, Thirdbin, ForthBin;



    [Header("Game Ui elements")]
    [Space(15)]
    public Image TruckImage;
    public List<Sprite> TruckFrontFaces;
    public Text DustbinCollection;
    private int DustbinCollectCount;
    public Text ScorePoints;
    public int ScorePointCounter;
    public List<MonsterMovement> monsters;
    public Text ScoreText;
    public GameObject Correcteffect, WrongEffect;
    public Canvas Maincanavs;



    [Header("Dashboard Fileds")]
    public GameObject DashboradPage;
    public Text GameScore, TotalGameScore;
    public Image GamescoreFiller, TotalGamescoreFiller;
    public Transform PriorityTable, AlignTable;
    public GameObject PriorityPrefeb, AlignPrefeb;
    public Sprite Correctans, WrongAns, CorrectOption;
    public Text OverAllPriority, OverallAlign;
    private List<int> ItemCollectionCount = new List<int>();
    private List<int> SCoreCollected = new List<int>();
    private List<string> PriorityUserdata;
    public List<GameObject> CorrectSequence;
    public List<string> TruckSequence = new List<string>();
    public List<int> TruckID = new List<int>();
    public List<int> UserSelectedId = new List<int>();
    public int AttemptNumber;
    public int CorrectPoint, WrongPoint;
    public List<GameObject> tableSequence;
    public List<GameObject> ALignTableseq;
    public List<GameObject> PriorityObj, AlignObj;
    private List<int> CorrectAlignStatus = new List<int>();
    [SerializeField]
    private List<string> CenterNames = new List<string>();
    private int DustbinCounter;
    private int DustinCollectScore;
    public float AllGameScore;
    public Button SkipButton;
    public GameObject TruckGamePage, startPage, LandingPage;
    public GameObject TimerPanel, timerPanelPos;
    [SerializeField]
    private List<Vector3> TrucksPos = new List<Vector3>();
    public List<GameObject> MainTrucks;
    public GameObject Montster1, monster2, monster3;


    public GameObject GameDonepanel;
    public Text Gamedonemsg;


    [Header("Timer Fields ==")]
    private bool TimePaused;
    public float minut;
    public float second;
    private float Totaltimer, RunningTimer;
    private float sec;
    private float Mintvalue;
    private bool helpingbool = true;
    public Image Timerbar;
    public Text Timer;
    public AudioClip GameSoundTrack,Rightsound,WrongSound;

    [Header("All Api Section")]
    public string MainUrl;
    public string MasterTabelPostApi, GetlevelWisedataApi, GetGamesIDApi, RoomData_api, PostPriorityLogApi, PostdrivingLogApi, GetAssessmentQuesApi, GetBadgeConfigApi,
        StageUnlockApi, PostBadgeUserApi;

    private int TotaltruckScore, GameAttemptNumber, level2GameBadgeId;
    public int Gamelevel;
    [SerializeField] private List<int> game_content = new List<int>();
    [SerializeField] private string ZoneName;
    [SerializeField] private List<int> RoomIds = new List<int>();
    [SerializeField] private string FirstGame, SecondGame;
    public SimpleSQLManager dbmanager;
    [SerializeField] private List<int> is_correct_PR;
    [SerializeField] private List<int> Truckscorevalue;
    [SerializeField] private List<string> UserselectedTruck;
    private List<string> CorrectSeqOfgame;


    //=========================== Truck Game list ==================
    [SerializeField] private List<string> TruckNamePlayed  = new List<string>();
    [SerializeField] private List<int>    dustbinCounts    = new List<int>();
    [SerializeField] private List<int>    TruckDustbinScore = new List<int>();
    [SerializeField] private List<string> Reachedcentername = new List<string>();
    [SerializeField] private List<int>    CenterScoreOfUser = new List<int>();
    [SerializeField] private List<int>    Is_correctreached = new List<int>();
    [SerializeField] private int finalgameScore;
    private int OverallScore;

    public GameObject InstructionPanel;
    private RectTransform textRect;
    private Vector2 uiOffset;
    public List<GameObject> MosterSound;
    private AudioSource DustbinSound;
    public List<GameObject> CenterSound;
    public string Level3BadgeName;
    public int Stage3UnlockScore;
    private bool Stage3unlocked;
    public GameObject deberifingpage;
    [HideInInspector] public List<string> centernames = new List<string>();
    [HideInInspector] public List<int> CenterCorrectPoint = new List<int>();
    [HideInInspector] public List<int> CenterWrongPoint = new List<int>();
    [HideInInspector]  public int monsterAttackScore;

    private void Awake()
    {
        Debug.Log(Application.persistentDataPath);
        DustbinCollectCount = 0;
        UnityEngine.Object[] objects = GameObject.FindObjectsOfType(typeof(Transform));

        foreach (Transform o in objects)
        {
            Vector2 pos = o.transform.position;
            if (o.name == "Biohazard" || o.name == "E-Waste" || o.name == "Recycle" || o.name == "Organic")
            {
                Debug.Log(" Gameobject " + o.name);
            }
            else
            {
                board[(int)pos.x, (int)pos.y] = o.gameObject;
            }
        }


        AllBins = new List<KeyValuePair<string, GameObject[]>>()
        {
            new KeyValuePair<string, GameObject[]>("Biohazard", Biohazard),
            new KeyValuePair<string, GameObject[]>("E-Waste", Ewaste),
            new KeyValuePair<string, GameObject[]>("Recycle", Recycle),
            new KeyValuePair<string, GameObject[]>("Organic", Organic)

        };

        int maxLength = 0;

        while (maxLength < Biohazard.Length)
        {
            int num = UnityEngine.Random.Range(0, Biohazard.Length);
            if (!randomindex.Contains(num))
            {
                randomindex.Add(num);
                maxLength++;
            }
        }

    }


    void OnEnable()
    {
        DustbinSound = this.gameObject.GetComponent<AudioSource>();
        textRect = ScoreText.GetComponent<RectTransform>();
        StartCoroutine(GetAssessmentQues());
        AllCommonTask();
    }

    void AllCommonTask()
    {
        TimePaused = true;
        Mintvalue = minut;
        sec = second;
        Totaltimer = (Mintvalue * 60) + second;
        RunningTimer = Totaltimer;
        DustbinCounter = 0;
        DustinCollectScore = 0;
        PriorityObj = new List<GameObject>();
        AlignObj = new List<GameObject>();
        ScorePointCounter = 0;
        ScorePoints.text = ScorePointCounter.ToString();
        DustbinCollectCount = 0;
        DustbinCollection.text = DustbinCollectCount.ToString();
        GameScore.text = ScorePointCounter.ToString();
        GamescoreFiller.fillAmount = 0f;
        TotalGamescoreFiller.fillAmount = 0f;
        TotalGameScore.text = ScorePointCounter.ToString();
        for (int a = 0; a < MainTrucks.Count; a++)
        {
            TrucksPos[a] = MainTrucks[a].transform.position;
        }
    }
    void Start()
    {

    }

    void TruckGameSetup()
    {
        DustbinCollectCount = 0;
        DustbinCollection.text = DustbinCollectCount.ToString();
        var wasteDustbinInc = AllBins.FirstOrDefault(x => x.Key.Equals(StationaryTrucks[ActiveTruckCount].name, System.StringComparison.OrdinalIgnoreCase));
        var wasteDustbin = AllBins.Where(x => !x.Key.Equals(StationaryTrucks[ActiveTruckCount].name, System.StringComparison.OrdinalIgnoreCase)).ToList();
        var dustbinListInc = wasteDustbinInc.Value.ToList();
        FirstBin = dustbinListInc;

        TruckFrontFaces.ForEach(x =>
        {
            if (x.name == StationaryTrucks[ActiveTruckCount].name)
            {
                TruckImage.sprite = x;
            }

        });

        for (int a = 0; a < wasteDustbin.Count; a++)
        {
            if (a == 0)
            {
                SecondBin = wasteDustbin[a].Value.ToList();
            }
            else if (a == 1)
            {
                Thirdbin = wasteDustbin[a].Value.ToList();
            }
            else if (a == 2)
            {
                ForthBin = wasteDustbin[a].Value.ToList();
            }
        }

        CheckedCollision = true;
        for (int a = 0; a < 3; a++)
        {
            CurrentActive.Add(FirstBin[randomindex[a]]);
            PlasticActive.Add(SecondBin[randomindex[a]]);
            GlassActive.Add(Thirdbin[randomindex[a]]);
            MetalActive.Add(ForthBin[randomindex[a]]);
           
        }
        for(int a = 0; a < 3; a++)
        {
            CurrentActive[a].SetActive(true);
            ObjCounter++;
        }
        PlasticActive[0].SetActive(true);
        PlasticActive[1].SetActive(true);
        GlassActive[0].SetActive(true);
        MetalActive[0].SetActive(true);


        ActiveTruckCount++;

    }

    public void getGamedata()
    {
        StartCoroutine(GetSounddata());
        game_content.Clear();
        TruckNamePlayed.Clear();
        dustbinCounts.Clear();
        TruckDustbinScore.Clear();
        Reachedcentername.Clear();
        CenterScoreOfUser.Clear();
        Is_correctreached.Clear();
        ItemCollectionCount.Clear();
        CorrectAlignStatus.Clear();
        CenterNames.Clear();
        Truckscorevalue.Clear();
        UserselectedTruck.Clear();
        // StartCoroutine(GetGameAttemptNoTask());
        StartCoroutine(GetGamesIDactivity());

    }

    IEnumerator GetSounddata()
    {
        var SettingLog = dbmanager.Table<GameSetting>().FirstOrDefault();
        if (SettingLog != null)
        {
            DustbinSound.volume = SettingLog.Sound;
            CenterSound.ForEach(x =>
            {
                x.GetComponent<AudioSource>().volume = SettingLog.Sound;
            });
            MosterSound.ForEach(y =>
            {
                y.GetComponent<AudioSource>().volume = SettingLog.Sound;
            });
           TruckGamePlatform.GetComponent<AudioSource>().volume = SettingLog.Music;
            PlayerPrefs.SetString("VibrationEnable", SettingLog.Vibration == 1 ? "true" : "false");
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator GetGameAttemptNoTask()
    {
        string HittingUrl = $"{MainUrl}{GetlevelWisedataApi}?UID={PlayerPrefs.GetInt("UID")}&OID={PlayerPrefs.GetInt("OID")}&id_level={Gamelevel}&game_type={1}";
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


    IEnumerator GetGamesIDactivity()
    {
        string HittingUrl = MainUrl + GetGamesIDApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
            "&id_org_game=" + 1 + "&id_level=" + Gamelevel;
        WWW GameResponse = new WWW(HittingUrl);
        yield return GameResponse;
        if (GameResponse.text != null)
        {
            Debug.Log("game id data " + GameResponse.text);
            GetLevelIDs gameIDs = Newtonsoft.Json.JsonConvert.DeserializeObject<GetLevelIDs>(GameResponse.text);
            var ContentList = gameIDs.content.ToList();
            game_content.Add(ContentList.FirstOrDefault(x => x.title == FirstGame).id_game_content);
            game_content.Add(ContentList.FirstOrDefault(x => x.title == SecondGame).id_game_content);
            //StartCoroutine(CollectRoomdata());
        }
    }



    IEnumerator CollectRoomdata()
    {

        string Hittingurl = MainUrl + RoomData_api + "?id_user=" + PlayerPrefs.GetInt("UID") + "&id_org_content=" + game_content;
        Debug.Log("main Url " + Hittingurl);
        WWW roominfo = new WWW(Hittingurl);
        yield return roominfo;
        if (roominfo.text != null)
        {
            Debug.Log("rooom id " + roominfo.text);
            JsonData RoomrRes = JsonMapper.ToObject(roominfo.text);
            for (int a = 0; a < RoomrRes.Count; a++)
            {
                RoomIds.Add(int.Parse(RoomrRes[a]["id_room"].ToString()));
            }

        }
    }


    void Update()
    {
        if (!TimePaused)
        {
            if (sec >= 0 && Mintvalue >= 0 && helpingbool)
            {
                sec = sec - Time.deltaTime;
                RunningTimer = RunningTimer - Time.deltaTime;
                Timerbar.fillAmount = RunningTimer / Totaltimer;
                if (sec.ToString("0").Length > 1)
                {
                    Timer.text = "0" + Mintvalue.ToString("0") + ":" + sec.ToString("0");
                }
                else
                {
                    Timer.text = "0" + Mintvalue.ToString("0") + ":" + "0" + sec.ToString("0");
                }

                if (sec.ToString("0") == "0" && Mintvalue >= 0)
                {
                    sec = 60;
                    Mintvalue = Mintvalue - 1;
                }
            }
            else if (helpingbool)
            {
                StartCoroutine(GameEndProcess());

            }
        }
    }


    public void CheckCollision(GameObject dustbinName, GameObject Truck)
    {

        num = CurrentActive.IndexOf(dustbinName);
        if (ObjCounter < randomindex.Count && CheckedCollision)
        {
            Taskcounter++;
            DustbinCounter++;
            CheckedCollision = false;
            CurrentActive[num].SetActive(false);
            DustbinCollectCount++;
            DustbinCollection.text = DustbinCollectCount.ToString();
            ScorePointCounter += CorrectPoint;
            DustinCollectScore += CorrectPoint;
            ScorePoints.text = ScorePointCounter.ToString();
            CurrentActive[num] = num != -1 ? FirstBin[randomindex[ObjCounter]] : null;
            CurrentActive[num].SetActive(true);
            StartCoroutine(ResetBool());
            ObjCounter++;

        }
        else
        {
            DustbinCollectCount++;
            DustbinCounter++;
            DustbinCollection.text = DustbinCollectCount.ToString();
            ScorePointCounter += CorrectPoint;
            DustinCollectScore += CorrectPoint;
            ScorePoints.text = ScorePointCounter.ToString();
            Taskcounter++;
            CurrentActive[num].SetActive(false);
        }

    }

    IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(1f);
        CheckedCollision = true;
    }

    //public void MoveToClickPoint(Vector3 objectTransformPosition)
    //{
    //    // Get the position on the canvas
    //    Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectTransformPosition);
    //    Vector2 proportionalPosition = new Vector2(ViewportPosition.x * Canvas.sizeDelta.x, ViewportPosition.y * Canvas.sizeDelta.y);

    //    // Set the position and remove the screen offset
    //    this.rectTransform.localPosition = proportionalPosition - uiOffset;
    //}


    public void CheckCorrectAns(GameObject dustbin, GameObject truck)
    {
        Debug.Log(" data " + dustbin.name + "   " + truck.name);
        var wasteDustbin = AllBins.FirstOrDefault(x => x.Key.Equals(truck.name, System.StringComparison.OrdinalIgnoreCase));
        var BinValid = wasteDustbin.Value.Any(x => x.name.Equals(dustbin.name, System.StringComparison.OrdinalIgnoreCase));
        if (BinValid)
        {
            CheckCollision(dustbin, truck);
            DustbinSound.clip = Rightsound;
            DustbinSound.Play();
            Correcteffect.transform.position = dustbin.transform.position;
            ScoreText.gameObject.SetActive(true);
            Vector2 pos = Camera.main.WorldToViewportPoint(dustbin.transform.position);
            ScoreText.text = "+" + CorrectPoint;
            var screen = Camera.main.WorldToScreenPoint(dustbin.transform.position);
            screen.z = (Maincanavs.transform.position - Camera.main.transform.position).magnitude;
            var position = Camera.main.ScreenToWorldPoint(screen);
            ScoreText.gameObject.transform.position = position;
            Correcteffect.SetActive(true);
            StartCoroutine(ResetEffectgame());
         
        }
        else
        {
            WrongCollision();
            dustbin.SetActive(false);
            ScoreText.gameObject.SetActive(true);
            DustbinSound.clip = WrongSound;
            DustbinSound.Play();
            Vector2 pos = Camera.main.WorldToViewportPoint(dustbin.transform.position);
            ScoreText.text = "+" + WrongPoint;
            var screen = Camera.main.WorldToScreenPoint(dustbin.transform.position);
            screen.z = (Maincanavs.transform.position - Camera.main.transform.position).magnitude;
            var position = Camera.main.ScreenToWorldPoint(screen);
            ScoreText.gameObject.transform.position = position;
            WrongEffect.transform.position = dustbin.transform.position;
            WrongEffect.SetActive(true);
            StartCoroutine(ResetEffectgame());
        }

    }

    IEnumerator ResetEffectgame()
    {
        yield return new WaitForSeconds(1f);
        ScoreText.text = "";
        ScoreText.gameObject.SetActive(false);
    }

    void WrongCollision()
    {
        ScorePointCounter += 0;
        DustinCollectScore += 0;
        ScorePoints.text = ScorePointCounter.ToString();
    }



    public void PlayGame()
    {
        Taskcounter = 0;
        ObjCounter = 0;
        if (ActiveTruckCount > 0)
        {
            CurrentActive.Clear();
            PlasticActive.Clear();
            MetalActive.Clear();
            GlassActive.Clear();
            TrucksPriority[ActiveTruckCount].gameObject.SetActive(true);
            TrucksPriority[ActiveTruckCount - 1].gameObject.SetActive(false);
            StationaryTrucks[ActiveTruckCount].SetActive(false);
        }
        else
        {
            TimePaused = false;
            Mintvalue = minut;
            sec = second;
            Totaltimer = (Mintvalue * 60) + second;
            RunningTimer = Totaltimer;
            is_correct_PR = new List<int>(new int[CorrectSequence.Count]);
            Truckscorevalue = new List<int>(new int[CorrectSequence.Count]);
            UserselectedTruck = new List<string>(new string[CorrectSequence.Count]);
            CorrectSeqOfgame = new List<string>(new string[CorrectSequence.Count]);
            TruckGamePlatform.SetActive(true);
            TrucksPriority[ActiveTruckCount].gameObject.SetActive(true);
            StationaryTrucks[ActiveTruckCount].SetActive(false);
            StartCoroutine(InstructionTask());

        }

        TruckGameSetup();

    }

    IEnumerator InstructionTask()
    {
        InstructionPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }

    public void CloseInstruction()
    {
        StartCoroutine(CloseInstructionTask());
    }

    IEnumerator CloseInstructionTask()
    {
        Time.timeScale = 1f;
        iTween.ScaleTo(InstructionPanel, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        InstructionPanel.SetActive(false);
    }

    public void TruckCenterResult(int score, string truckName, string CenterName)
    {
        int cmsScore = 0;
        if(CenterName != "null"){
            string center = CenterName + " Center";
            int index = centernames.FindIndex(x => x.Equals(center, System.StringComparison.OrdinalIgnoreCase));
            if (score == 50)
            {
                cmsScore = CenterCorrectPoint[index];
            }
            else
            {
                cmsScore = CenterWrongPoint[index];
            }
        }
        else
        {
            cmsScore = score;
        }
       
       
        CenterNames.Add(CenterName != "" ? CenterName : "null");
        CorrectAlignStatus.Add(score == 50 ? 1 : 0);
        if (ActiveTruckCount < StationaryTrucks.Count)
        {
            monsters.ForEach(x =>
            {
                x.gameObject.transform.position = x.MonsterInitialPos;
                x.StartMoving = false;
            });
            ScorePointCounter += cmsScore;
            ScorePoints.text = ScorePointCounter.ToString();
            CurrentActive.ForEach(x =>
            {
                x.gameObject.SetActive(false);
            });

            PlasticActive.ForEach(x =>
            {
                x.gameObject.SetActive(false);
            });
            GlassActive.ForEach(x =>
            {
                x.gameObject.SetActive(false);
            });
            MetalActive.ForEach(x =>
            {
                x.gameObject.SetActive(false);
            });
            ItemCollectionCount.Add(DustbinCounter);
            DustbinCounter = 0;

            StartCoroutine(ShowCenterResult(truckName));

        }
        else
        {
            ScorePointCounter += cmsScore;
            ScorePoints.text = ScorePointCounter.ToString();
            ItemCollectionCount.Add(DustbinCounter);
            DustbinCounter = 0;
            StartCoroutine(AllDoneTruck());

        }

    }

    IEnumerator ShowCenterResult(string Truckname)
    {
        PlayGame();
        yield return new WaitForSeconds(0.5f);
        monsters.ForEach(x =>
        {
            x.targetNode = null;
            x.currentnode = null;
            x.InitialSetup();
        });
    }


    IEnumerator AllDoneTruck()
    {
        TimePaused = true;
        yield return new WaitForSeconds(0.5f);
        iTween.MoveTo(TimerPanel, iTween.Hash("position", new Vector3(0f, -680f, 0f), "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.4));
        yield return new WaitForSeconds(0.5f);
        CorrectAlignStatus.ForEach(x =>
        {
            Gamedonemsg.text = x == 1 ? "Congratulations ! You have successfully completed this level" : "You can do better! try next Time!";
        });
        GameDonepanel.SetActive(true);

    }

    IEnumerator GameEndProcess()
    {

        TimePaused = true;
        yield return new WaitForSeconds(0.5f);
        int length = StationaryTrucks.Count - CenterNames.Count;
        MainTrucks.ForEach(x =>
        {
            x.GetComponent<TruckPlayer>().ResetNodes();
            x.GetComponent<TruckPlayer>().NextDirection = Vector2.zero;
            x.SetActive(false);
        });
        for (int a = 0; a < length; a++)
        {
            CenterNames.Add("null");
            CorrectAlignStatus.Add(0);
            ItemCollectionCount.Add(0);
        }
        iTween.MoveTo(TimerPanel, iTween.Hash("position", new Vector3(0f, -680f, 0f), "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.4));
        yield return new WaitForSeconds(0.5f);
        Gamedonemsg.text = "Your time is up!";
        GameDonepanel.SetActive(true);


    }
    public void ShowDashboard()
    {
        StartCoroutine(DashBoardTAsk());
    }

    IEnumerator DashBoardTAsk()
    {
        iTween.ScaleTo(GameDonepanel, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameDonepanel.SetActive(false);
        MakePriorityDashborad();
    }


    //======================================== DASHBOARD METHODS ===================================//


    void MakePriorityDashborad()
    {
        Montster1.SetActive(false);
        monster2.SetActive(false);
        monster3.SetActive(false);

        SkipButton.onClick.RemoveAllListeners();
        SkipButton.onClick.AddListener(delegate { ResetAllTask(); });
         OverallScore = 0;

        // Truck Priority Game data handler =============================
        for (int a = 0; a < CorrectSequence.Count ; a++)
        {
            GameObject gb = Instantiate(PriorityPrefeb, PriorityTable, false);
            PriorityObj.Add(gb);
        }

        for(int a = 0; a < CorrectSequence.Count + 1; a++)
        {
            GameObject Gb2 = Instantiate(AlignPrefeb, AlignTable, false);
            AlignObj.Add(Gb2);
        }

        for (int b = 0; b < StationaryTrucks.Count; b++)
        {
            if (StationaryTrucks[b].name == CorrectSequence[b].name)
            {
                OverallScore += CorrectPoint;
                OverAllPriority.text = OverallScore.ToString();
                for (int c = 0; c < tableSequence.Count; c++)
                {
                    if (StationaryTrucks[b].name == tableSequence[c].name)
                    {
                        PriorityObj[b].transform.GetChild(c).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        PriorityObj[b].transform.GetChild(c).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctans;
                        PriorityObj[b].transform.GetChild(4).gameObject.GetComponent<Text>().text = CorrectPoint.ToString();
                        is_correct_PR[b] = 1;
                        Truckscorevalue[b] = CorrectPoint;
                        UserselectedTruck[b] = StationaryTrucks[b].name;
                        CorrectSeqOfgame[b] = TruckSequence[b];
                        //UserSelectedId[b] = TruckID[b];
                    }
                }
            }
            else
            {

                int indexWrong = tableSequence.FindIndex(x => x.name == StationaryTrucks[b].name);
                PriorityObj[b].transform.GetChild(indexWrong).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                PriorityObj[b].transform.GetChild(indexWrong).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                PriorityObj[b].transform.GetChild(indexWrong).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectSequence[b].name;
                PriorityObj[b].transform.GetChild(indexWrong).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongAns;
                PriorityObj[b].transform.GetChild(4).gameObject.GetComponent<Text>().text = WrongPoint.ToString();
                int correctIndex = tableSequence.FindIndex(x => x.name == CorrectSequence[b].name);
                PriorityObj[b].transform.GetChild(correctIndex).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                PriorityObj[b].transform.GetChild(correctIndex).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOption;
                is_correct_PR[b] = 0;
                Truckscorevalue[b] = WrongPoint;
                UserselectedTruck[b] = StationaryTrucks[b].name;
                CorrectSeqOfgame[b] = TruckSequence[b];

            }

        }


        for (int a = 0; a < AlignObj.Count; a++)
        {
           
            if (a == 0)
            {
                for (int b = 0; b < StationaryTrucks.Count; b++)
                {
                    AlignObj[a].gameObject.transform.GetChild(b).gameObject.GetComponent<Text>().text = ItemCollectionCount[b].ToString() + "/7";
                    AlignObj[a].gameObject.transform.GetChild(4).gameObject.GetComponent<Text>().text = DustinCollectScore.ToString();

                }
            }
        }
        TotaltruckScore = OverallScore + ScorePointCounter;
        GameScore.text = (OverallScore + ScorePointCounter).ToString();
        TotalGameScore.text = (OverallScore + ScorePointCounter).ToString();
        GamescoreFiller.fillAmount = (float)(OverallScore + ScorePointCounter) / AllGameScore;
        TotalGamescoreFiller.fillAmount = (float)(OverallScore + ScorePointCounter) / AllGameScore;
        OverallAlign.text = ScorePointCounter.ToString();
        for (int a = 0; a < StationaryTrucks.Count; a++)
        {
            if (CorrectAlignStatus[a] == 1)
            {
                for (int b = 0; b < ALignTableseq.Count; b++)
                {
                    if (StationaryTrucks[a].name == ALignTableseq[b].name)
                    {
                        AlignObj[b + 1].gameObject.transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        AlignObj[b + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctans;
                        AlignObj[b + 1].transform.GetChild(4).gameObject.GetComponent<Text>().text = "50";
                        CenterScoreOfUser.Add(50);
                        Is_correctreached.Add(1);
                        Reachedcentername.Add(ALignTableseq[b].name);
                    }
                }
            }
            else if (CorrectAlignStatus[a] == 0)
            {
                if (CenterNames[a] != "null")
                {
                    int GotWrong = ALignTableseq.FindIndex(x => x.name == CenterNames[a]);
                    AlignObj[GotWrong + 1].gameObject.transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    AlignObj[GotWrong + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongAns;
                    AlignObj[GotWrong + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    AlignObj[GotWrong + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = StationaryTrucks[a].name;
                    int Gotoption = CorrectSequence.FindIndex(y => y.name == StationaryTrucks[a].name);
                    for (int b = 0; b < ALignTableseq.Count; b++)
                    {
                        if (StationaryTrucks[a].name == ALignTableseq[b].name)
                        {
                            AlignObj[b + 1].gameObject.transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            AlignObj[b + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOption;
                            AlignObj[b + 1].transform.GetChild(4).gameObject.GetComponent<Text>().text = "0";
                            CenterScoreOfUser.Add(0);
                            Is_correctreached.Add(0);
                        }
                    }
                    Reachedcentername.Add(CenterNames[a]);
                }
                else
                {
                    for (int b = 0; b < ALignTableseq.Count; b++)
                    {
                        if (StationaryTrucks[a].name == ALignTableseq[b].name)
                        {
                            AlignObj[b + 1].gameObject.transform.GetChild(a).gameObject.GetComponent<Text>().text = "---";
                            AlignObj[b + 1].transform.GetChild(4).gameObject.GetComponent<Text>().text = "0";
                            CenterScoreOfUser.Add(0);
                            Is_correctreached.Add(0);
                            Reachedcentername.Add("null");
                        }
                    }
                }

            }
            TruckNamePlayed.Add(StationaryTrucks[a].name);
            dustbinCounts.Add(ItemCollectionCount[a]);
            TruckDustbinScore.Add(ItemCollectionCount[a] * CorrectPoint);
            //CorrectCenterName.Add()
        }
        for (int a = 0; a < MainTrucks.Count; a++)
        {
            MainTrucks[a].transform.position = TrucksPos[a];
        }

        DashboradPage.SetActive(true);
        TruckPoits.ForEach(x =>
        {
            x.gameObject.SetActive(true);
        });


        // POstdashBoarddata();     //local database entry method
        //PosttruckGamedata();     //local database entry method

        StartCoroutine(CheckForGameBadge());
        StartCoroutine(PostPriorityGamedata());
        StartCoroutine(PostDrivingGamedata());
        StartCoroutine(PostScorePriorityTask());
        StartCoroutine(PostScoreDriveTask());



    }

    void POstdashBoarddata()
    {
        for (int a = 0; a < CorrectSequence.Count; a++)
        {
            var priority = dbmanager.Table<Prioritization>().FirstOrDefault(x => x.Truckname == UserselectedTruck[a]);
            if (priority == null)
            {
                Prioritization PriorityModel = new Prioritization
                {
                    Is_correct = is_correct_PR[a],
                    Truckname = UserselectedTruck[a],
                    CorrectTruck = CorrectSeqOfgame[a],
                    Truckscore = Truckscorevalue[a]
                };
                dbmanager.Insert(PriorityModel);
            }
            else
            {
                priority.Is_correct = is_correct_PR[a];
                priority.Truckname = UserselectedTruck[a];
                priority.CorrectTruck = CorrectSeqOfgame[a];
                priority.Truckscore = Truckscorevalue[a];
                dbmanager.UpdateTable(priority);
            }
        }


        is_correct_PR.Clear();
        UserselectedTruck.Clear();
        Truckscorevalue.Clear();
        CorrectSeqOfgame.Clear();
    }

    void PosttruckGamedata()
    {
        for (int a = 0; a < CorrectSequence.Count; a++)
        {
            var priority = dbmanager.Table<TruckGameModel>().FirstOrDefault(x => x.Truckname == TruckNamePlayed[a]);
            if (priority == null)
            {
                TruckGameModel PriorityModel = new TruckGameModel
                {
                    Truckname = TruckNamePlayed[a],
                    dustbinCollected = dustbinCounts[a],
                    CenterScore = CenterScoreOfUser[a],
                    TruckScore = TotaltruckScore,
                    Reachedcentername = Reachedcentername[a],
                    is_correctReached = Is_correctreached[a]
                };
                dbmanager.Insert(PriorityModel);
            }
            else
            {
                priority.Truckname = TruckNamePlayed[a];
                priority.dustbinCollected = dustbinCounts[a];
                priority.CenterScore = CenterScoreOfUser[a];
                priority.TruckScore = priority.TruckScore + TotaltruckScore;
                priority.Reachedcentername = Reachedcentername[a];
                priority.is_correctReached = Is_correctreached[a];
                dbmanager.UpdateTable(priority);
            }
        }

        TruckNamePlayed.Clear();
        dustbinCounts.Clear();
        CenterScoreOfUser.Clear();
        Reachedcentername.Clear();
        Is_correctreached.Clear();
    }


    IEnumerator PostPriorityGamedata()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = MainUrl + PostPriorityLogApi;
        var TruckModel = new List<TruckSeqPostModel>();
        int l = 0;
        Debug.Log("Count data " + TruckSequence.Count);
        TruckSequence.ForEach(x =>
        {
            var log = new TruckSeqPostModel()
            {
                id_user = PlayerPrefs.GetInt("UID").ToString(),
                truck_selected = UserselectedTruck[l],
                is_correct = is_correct_PR[l].ToString(),
                correct_truck = TruckSequence[l],
                score = Truckscorevalue[l].ToString(),
                attempt_no =( AttemptNumber + 1 ).ToString(),
                sequence = (l + 1).ToString()
            };
            TruckModel.Add(log);
            l = l+ 1;
        });
        string Log_data = Newtonsoft.Json.JsonConvert.SerializeObject(TruckModel);
        Debug.Log("Priority Game data " + Log_data);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, Log_data))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log("Response of Priority post " + Request.downloadHandler.text);
              
            }

        }

    }


    IEnumerator PostDrivingGamedata()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = MainUrl + PostdrivingLogApi;
        var TruckDriveLog = new List<TruckDrivePostModel>();
        int l = 0;
        TruckSequence.ForEach(x =>
        {
            var log = new TruckDrivePostModel()
            {
                id_truck = UserSelectedId[l].ToString(),
                id_user = PlayerPrefs.GetInt("UID").ToString(),
                dustbin_collected = ItemCollectionCount[l].ToString(),
                truck_score = TruckDustbinScore[l].ToString(),
                reached_center = Reachedcentername[l].ToString(),
                center_score = CenterScoreOfUser[l].ToString(),
                is_correct_reached = Is_correctreached[l].ToString(),
                attempt_no = (AttemptNumber + 1).ToString(),
                truck_name = UserselectedTruck[l]
            };
            l = l +1;
            TruckDriveLog.Add(log);
        });


        string data_log = Newtonsoft.Json.JsonConvert.SerializeObject(TruckDriveLog);
        Debug.Log(" drive game data " + data_log);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, data_log))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log("Reponse for Driving game " + Request.downloadHandler.text);
              
            }

        }
    }

    //RESET MAIN METHODS FOR NEW GAME
    public void ResetAllTask()
    {
        StartCoroutine(ResetActivity());

    }

    //RESET ALL THE VARIABLLS FOR NEW GAME
    IEnumerator ResetActivity()
    {
        AllCommonTask();
        ActiveTruckCount = 0;
        MainTrucks.ForEach(x =>
        {
            x.GetComponent<TruckPlayer>().ResetNodes();
            x.GetComponent<TruckPlayer>().NextDirection = Vector2.zero;
        });
        for (int b = 0; b < PriorityTable.childCount; b++)
        {
            Destroy(PriorityTable.GetChild(b).gameObject, 0.05f);
            Destroy(AlignTable.GetChild(b).gameObject, 0.05f);
        }
        yield return new WaitForSeconds(1f);
        AlignObj.ForEach(x =>
        {
            Destroy(x.gameObject);
        });
        PriorityObj.ForEach(x =>
        {
            Destroy(x.gameObject);
        });
        yield return new WaitForSeconds(1.5f);
        PriorityObj.Clear();
        AlignObj.Clear();
        TimerPanel.SetActive(false);
        timerPanelPos.SetActive(false);
        TrucksPriority.Clear();
        StationaryTrucks.Clear();
        CorrectAlignStatus.Clear();
        CenterNames.Clear();
        monsters.ForEach(x =>
        {
            x.gameObject.transform.position = x.MonsterInitialPos;
            x.StartMoving = false;
            x.currentnode = null;
            x.targetNode = null;
            x.gameObject.SetActive(false);
        });
        DashboradPage.SetActive(false);
        TruckGamePage.SetActive(false);
        startPage.SetActive(true);
        LandingPage.SetActive(true);
        deberifingpage.SetActive(true);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = true;
        TruckGamePage.GetComponent<AudioSource>().enabled = false;
        //this.gameObject.SetActive(false);

    }


    //FOR ANDROID DEVICE VIRBRATION EFFECT
    public void VibrateDevice()
    {
        if (!PlayerPrefs.HasKey("VibrationEnable"))
        {
            Vibration.Vibrate(400);
            Debug.Log("vibration");
        }
        else
        {
            string vibration = PlayerPrefs.GetString("VibrationEnable");

            if (vibration == "true")
            {
                Vibration.Vibrate(400);
                Debug.Log("vibration");
            }
        }
    }



    IEnumerator PostScorePriorityTask()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = MainUrl + MasterTabelPostApi;
        ScorePostModel scorePost = new ScorePostModel();
        scorePost.UID = PlayerPrefs.GetInt("UID");
        scorePost.OID = PlayerPrefs.GetInt("OID");
        scorePost.id_user = PlayerPrefs.GetInt("UID");
        scorePost.id_game_content = game_content[0];
        scorePost.score = OverallScore;
        scorePost.id_score_unit = 1;
        scorePost.score_type = 1;
        scorePost.score_unit = "points";
        scorePost.status = "A";
        scorePost.updated_date_time = DateTime.Now.ToString();
        scorePost.id_level = Gamelevel;
        scorePost.id_org_game = 1;
        scorePost.attempt_no = GameAttemptNumber + 1;
        scorePost.timetaken_to_complete = "00:00";
        scorePost.is_completed = 1;
        scorePost.game_type = 1;

        string Data_log = Newtonsoft.Json.JsonConvert.SerializeObject(scorePost);
        Debug.Log("data log Priority game " + Data_log);
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


    IEnumerator PostScoreDriveTask()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = MainUrl + MasterTabelPostApi;
        ScorePostModel scorePost = new ScorePostModel();
        scorePost.UID = PlayerPrefs.GetInt("UID");
        scorePost.OID = PlayerPrefs.GetInt("OID");
        scorePost.id_user = PlayerPrefs.GetInt("UID");
        scorePost.id_game_content = game_content[1];
        scorePost.score = ScorePointCounter;
        scorePost.id_score_unit = 1;
        scorePost.score_type = 1;
        scorePost.score_unit = "points";
        scorePost.status = "A";
        scorePost.updated_date_time = DateTime.Now.ToString();
        scorePost.id_level = Gamelevel;
        scorePost.id_org_game = 1;
        scorePost.attempt_no = GameAttemptNumber + 1;
        scorePost.timetaken_to_complete = "00:00";
        scorePost.is_completed = 1;
        scorePost.game_type = 1;

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


    IEnumerator GetAssessmentQues()
    {
        string HittingUrl = $"{MainUrl}{GetAssessmentQuesApi}?UID={PlayerPrefs.GetInt("UID")}&OID={PlayerPrefs.GetInt("OID")}";
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

                    var AssessmentLogDB = dbmanager.Table<FinalAssessment>().FirstOrDefault(y => y.Question == x.brief_question);
                    if(AssessmentLogDB == null)
                    {
                        FinalAssessment log = new FinalAssessment
                        {
                            QuesId = x.id_brief_question,
                            Question = x.brief_question,
                            Options = string.Join("@", x.answer.Select(y => y.brief_answer).ToArray()),
                            OptionsID = string.Join("@", x.answer.Select(c => c.id_brief_answer).ToArray()),
                            CorrectAns = x.answer.FirstOrDefault(t => t.is_correct_answer ==1).brief_answer,
                            Levelid = x.id_level
                        };
                        dbmanager.Insert(log);
                    }
                    else
                    {
                        AssessmentLogDB.QuesId = x.id_brief_question;
                        AssessmentLogDB.Question = x.brief_question;
                        AssessmentLogDB.Options = string.Join("@", x.answer.Select(y => y.brief_answer).ToArray());
                        AssessmentLogDB.OptionsID = string.Join("@", x.answer.Select(c => c.id_brief_answer).ToArray());
                        AssessmentLogDB.CorrectAns = x.answer.FirstOrDefault(t => t.is_correct_answer == 1).brief_answer;
                        AssessmentLogDB.Levelid = x.id_level;
                        dbmanager.UpdateTable(AssessmentLogDB);
                    }

                });
            }
        }
    }




    IEnumerator CheckForGameBadge()
    {
        string HittingUrl = MainUrl + GetBadgeConfigApi + "?id_level=" + 3;
        WWW badge_www = new WWW(HittingUrl);
        yield return badge_www;
        if (badge_www.text != null)
        {
            //Debug.Log(" badge infp " + badge_www.text);
            List<BadgeConfigModels> badgemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BadgeConfigModels>>(badge_www.text);
            level2GameBadgeId = badgemodel.FirstOrDefault(x => x.badge_name == Level3BadgeName).id_badge;
            StartCoroutine(CheckForStage2());
        }
    }
    IEnumerator CheckForStage2()
    {


        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={3}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            Stage3unlocked = int.Parse(StageModel.ConsolidatedScore) >= Stage3UnlockScore;
        }

        if (Stage3unlocked)
        {
            StartCoroutine(PostGameBadgeLevelWise());
        }
    }


    IEnumerator PostGameBadgeLevelWise()
    {

        string HittingUrl = MainUrl + PostBadgeUserApi;
        var BadgeModel = new PostBadgeModel()
        {
            id_user = PlayerPrefs.GetInt("UID").ToString(),
            id_level = "1",
            id_zone = "0",
            id_room = "0",
            id_special_game = "0",
            id_badge = level2GameBadgeId.ToString()
        };

        string Data_log = Newtonsoft.Json.JsonConvert.SerializeObject(BadgeModel);
        Debug.Log("data log " + Data_log);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, Data_log))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                string status = Request.downloadHandler.text;
                if (status.ToLower() == "success")
                {
                    Debug.Log("MOST ACTIVE badge uploaded " + status);
                }
                else
                {
                    Debug.Log("MOST ACTIVE badge Something went wrong " + status);
                }

            }

        }

    }
}
