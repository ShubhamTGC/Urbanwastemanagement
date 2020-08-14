using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using LitJson;

public class Stage2ZoneHandler : MonoBehaviour
{
    [Header("API SECTION======")]
    public string MainUrl;
    public string GetGamesIDApi,PostMasterScoreApi, RoomData_api,PostZoneDataApi;
    public int id_game_content;
    [SerializeField] private int Gamelevel;
    public string ZoneName;
    public GameObject level1, level2,level1Mainpage,level2MainPage;
    public ThrowWasteHandler Level1Controller, Level2Controller;
    public GameObject PopUpstatus;
    public Text PopupMsg;
    public Button LevelButton;
    public GameObject Dashboard;
    public Button skip,Back;
    public List<int> RoomIds;
    public GameObject MainZone, Startpage, ZoneselectionPage, ZonePag, Gamecanvas;
    [HideInInspector]
    public List<ZoneDataPost> logs;
    public Text Zonetext;
    [SerializeField] private string ZoneHeading;
    public Text Username, Gradeno;
    public Text GameScore, totalScoreText;
    public Image GreenscoreFiller, totalScorefiller;
    [SerializeField] private float GameScoretotal;

    [Space(20)]

    //============= achivement shelf api task========================
    public string GetBadgeConfigApi;
    public string MostActivePlayerApi, PostBadgeUserApi, LeaderBoardApi, CheckHighscoreApi,MostObservantApi, levelClearnessApi;
    [SerializeField] private string Highscorename;
    [SerializeField] private string mostActiveName;
    [SerializeField] private string MostobservantName;
    private int HighScoreBadgeid, ActivebadgeId,MostObservantBadgeId, MyTotalScore;
    private int level2GameBadgeId;
    public string Level2BadgeName;
    [SerializeField] private int Leve2number;
    private int totalscoreOfUser;
    private bool Stage2unlocked;
    [SerializeField] private int Stage2UnlockScore;
    void Start()
    {
        
    }

     void OnEnable()
    {
        Username.text = PlayerPrefs.GetString("username");
        Gradeno.text = PlayerPrefs.GetString("User_grade");
        Zonetext.text = "STAGE 2 - " + ZoneHeading;
        logs = new List<ZoneDataPost>();
        logs.Clear();
        RoomIds = new List<int>();
        StartCoroutine(getGameContentid());
        level1.SetActive(true);
        level1Mainpage.SetActive(true);
        Level1Controller.level1 = true;
        skip.onClick.RemoveAllListeners();
        skip.onClick.AddListener(delegate { Skiplevel(); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator getGameContentid()
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
            id_game_content = ContentList.FirstOrDefault(x => x.title == ZoneName).id_game_content;
            StartCoroutine(GetlevelsIDS());
        }
    }

    IEnumerator GetlevelsIDS()
    {
        string Hittingurl = MainUrl + RoomData_api + "?id_user=" + PlayerPrefs.GetInt("UID") + "&id_org_content=" + id_game_content;
        
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

    public void checkLevelStatus()
    {
        if (Level1Controller.LevelCompleted && !Level2Controller.LevelCompleted)
        {
            StartCoroutine(Level1clear());
        }
        if(Level1Controller.LevelCompleted && Level2Controller.LevelCompleted)
        {
            StartCoroutine(getBadgeConfiguration(0));
            StartCoroutine(MAsterTablePosting());
            Level1Controller.generateDashboardL1();
            Level2Controller.generateDashboardL2();
            StartCoroutine(clearLevel());
            Level1Controller.LevelCompleted = false;
            Level2Controller.LevelCompleted = false;
            int TotalScore = Level1Controller.SCore + Level2Controller.SCore;
            GameScore.text = TotalScore.ToString();
            totalScoreText.text = TotalScore.ToString();
            GreenscoreFiller.fillAmount = (float)TotalScore / GameScoretotal;
            totalScorefiller.fillAmount = (float)TotalScore / GameScoretotal;
        }
    }

    IEnumerator Level1clear()
    {
        PopupMsg.text = "Congratulations! You have successfully completed Level 1\nPlease click on Level 2 to play!";
        PopUpstatus.SetActive(true);
        LevelButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Level 2";
        LevelButton.onClick.RemoveAllListeners();
        LevelButton.onClick.AddListener(delegate { switchLevel(); });
        iTween.ScaleTo(PopUpstatus, Vector3.one, 0.4f);
        yield return new WaitForSeconds(0.4f);

    }
    void switchLevel()
    {
        StartCoroutine(switchLeveltask());
    }

    IEnumerator switchLeveltask()
    {
        iTween.ScaleTo(PopUpstatus, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.0f);
        PopupMsg.text = "";
        PopUpstatus.SetActive(false);
        level1Mainpage.SetActive(false);
        Level1Controller.level1 = false;
        level2.SetActive(true);
        level2MainPage.SetActive(true);
        Level2Controller.level2 = true;
        Debug.Log(Level2Controller.level2 + " name " + Level2Controller.gameObject.name);
    }

    IEnumerator clearLevel()
    {
        PopupMsg.text = "Congratulations! You have successfully completed Zone A";
        LevelButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Next";
        PopUpstatus.SetActive(true);
        LevelButton.onClick.RemoveAllListeners();
        LevelButton.onClick.AddListener(delegate { showDashboard(); });
        iTween.ScaleTo(PopUpstatus, Vector3.one, 0.4f);
        yield return new WaitForSeconds(0.4f);
    }
    void showDashboard()
    {
        StartCoroutine(dashboardTask());
    }
    IEnumerator dashboardTask()
    {
        iTween.ScaleTo(PopUpstatus, Vector3.zero, 0.4f);
        LevelButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        PopupMsg.text = "";
        yield return new WaitForSeconds(0.4f);
        PopUpstatus.SetActive(false);
        level2MainPage.SetActive(false);
        Dashboard.SetActive(true);
    }

    public void Skiplevel()
    {
        StartCoroutine(levelReseting());

    }
    IEnumerator levelReseting()
    {
        Level1Controller.CloseGame();
        Level2Controller.CloseGame();
        Level1Controller.ResetTask();
        Level2Controller.ResetTask();
        yield return new WaitForSeconds(1f);
        Dashboard.SetActive(false);
        Gamecanvas.SetActive(false);
        //ZonePag.SetActive(true);
        ////ZoneselectionPage.SetActive(true);
        ////Startpage.SetActive(true);
        //Startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        //MainZone.SetActive(false);

    }

    public void BackFromLevel()
    {
        StartCoroutine(Backtask());
    }
    IEnumerator Backtask()
    {
        Level1Controller.CloseGame();
        Level2Controller.CloseGame();
        yield return new WaitForSeconds(2f);
        Gamecanvas.SetActive(false);
        ZonePag.SetActive(true);
        ZoneselectionPage.SetActive(true);
        Startpage.SetActive(true);
        Startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        MainZone.SetActive(false);

    }

    IEnumerator MAsterTablePosting()
    {

        int totalscore = Level1Controller.SCore + Level2Controller.SCore;
        ScorePostModel PostModel = new ScorePostModel();
        string HittingUrl = MainUrl + PostMasterScoreApi;
        PostModel.UID = PlayerPrefs.GetInt("UID");
        PostModel.OID = PlayerPrefs.GetInt("OID");
        PostModel.id_log = 1;
        PostModel.id_user = PlayerPrefs.GetInt("UID");
        PostModel.id_game_content = id_game_content;
        PostModel.score = totalscore;
        PostModel.id_score_unit = 1;
        PostModel.score_type = 1;
        PostModel.score_unit = "points";
        PostModel.status = "A";
        PostModel.updated_date_time = DateTime.Now.ToString();
        PostModel.id_level = 0;
        PostModel.id_org_game = 1;
        PostModel.attempt_no = 0;
        PostModel.timetaken_to_complete = "00:00";
        PostModel.is_completed = 1;

        string Postdata = Newtonsoft.Json.JsonConvert.SerializeObject(PostModel);
        using(UnityWebRequest Master_request = UnityWebRequest.Put(HittingUrl, Postdata))
        {
            Master_request.method = UnityWebRequest.kHttpVerbPOST;
            Master_request.SetRequestHeader("Content-Type", "application/json");
            Master_request.SetRequestHeader("Accept", "application/json");
            yield return Master_request.SendWebRequest();
            if (!Master_request.isNetworkError && !Master_request.isHttpError)
            {
                Debug.Log(Master_request.downloadHandler.text);
                MasterTabelResponse masterRes = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterTabelResponse>(Master_request.downloadHandler.text);
                if (masterRes.STATUS.ToLower() == "success")
                {
                    StartCoroutine(getHighScore());
                   // StartCoroutine(CheckForGameBadge());
                }
                else
                {
                    Debug.Log(" TSTATUS  ====  FAILED stage 1 zonehandler script ");
                }
            }
        }

    }

    public void PostZoneData(string jsondata)
    {
        StartCoroutine(PostzoneTask(jsondata));
    }
    IEnumerator PostzoneTask(string JsonValue)
    {
        string HittingUrl = MainUrl + PostZoneDataApi;
        WWWForm post_form = new WWWForm();
        post_form.AddField("log_string", JsonValue);
        WWW zone_www = new WWW(HittingUrl, post_form);
        yield return zone_www;
        if (zone_www.text != null)
        {
            Debug.Log(zone_www.text);
            MasterTabelResponse masterRes = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterTabelResponse>(zone_www.text);
            if (masterRes.STATUS.ToLower() == "success")
            {
                StartCoroutine(checkMostActivePTask());
            }
            else
            {
                Debug.Log(" STATUS  ====  FAILED stage 1 zonehandler script ");
            }
        }

    }

    //====================== MOST HIGH SCORER API TASK =========================================
    IEnumerator getHighScore()
    {
        string Hitting_Url = MainUrl + LeaderBoardApi + "?id_user=" + PlayerPrefs.GetInt("UID") + "&org_id=" + PlayerPrefs.GetInt("OID");
        //string Hitting_Url = "https://www.skillmuni.in/wsmapi/api/WMSLeaderboad" + "?id_user=" + PlayerPrefs.GetInt("UID") + "&org_id=" + PlayerPrefs.GetInt("OID");
        WWW Userscore_www = new WWW(Hitting_Url);
        yield return Userscore_www;
        if (Userscore_www.text != null)
        {
            List<LeaderBoardmodel> LeaderBoardData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LeaderBoardmodel>>(Userscore_www.text);
            MyTotalScore = LeaderBoardData.FirstOrDefault(x => x.id_user == PlayerPrefs.GetInt("UID")).Score;
            StartCoroutine(CheckHighScoreTask());

        }
        else
        {
            Debug.Log(" interner Lost ====  stage 1 zonehandler script ");
        }

    }

    IEnumerator CheckHighScoreTask()
    {
        string HittingUrl = MainUrl + CheckHighscoreApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") + "&id_level=" + 2
            + "&is_bonus_game=" + 0 + "&score=" + MyTotalScore;
        WWW highscorecheck = new WWW(HittingUrl);
        yield return highscorecheck;
        if (highscorecheck.text != null)
        {
            //Debug.Log(" high score data " + highscorecheck.text);
            HighScoreBadgeModel HighscoreRes = Newtonsoft.Json.JsonConvert.DeserializeObject<HighScoreBadgeModel>(highscorecheck.text);
            if (HighscoreRes.is_high_scorer == "1")
            {
                Debug.Log("  USER IS A HIGH SCORER PLATYER ");
                StartCoroutine(PostHighscoreBadge());

            }
            else
            {
                Debug.Log(" USER IS  NOT A HIGH SCORER PLATYER ");
            }
        }
    }

    IEnumerator PostHighscoreBadge()
    {
        string HittingUrl = MainUrl + PostBadgeUserApi;
        var BadgeModel = new PostBadgeModel()
        {
            id_user = PlayerPrefs.GetInt("UID").ToString(),
            id_level = "2",
            id_zone = "0",
            id_room = "0",
            id_special_game = "0",
            id_badge = HighScoreBadgeid.ToString()
        };

        string Data_log = Newtonsoft.Json.JsonConvert.SerializeObject(BadgeModel);
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
                    Debug.Log("High scorer badge uploaded " + status);
                }
                else
                {
                    Debug.Log("High scorer badge Something went wrong " + status);
                }

            }

        }

    }
    //===========================================================================================//


    IEnumerator getBadgeConfiguration(int levelid)
    {
        string HittingUrl = MainUrl + GetBadgeConfigApi + "?id_level=" + levelid;
        WWW badge_www = new WWW(HittingUrl);
        yield return badge_www;
        if (badge_www.text != null)
        {
            //Debug.Log(" badge infp " + badge_www.text);
            List<BadgeConfigModels> badgemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BadgeConfigModels>>(badge_www.text);
            HighScoreBadgeid = badgemodel.FirstOrDefault(x => x.badge_name == Highscorename).id_badge;
            ActivebadgeId = badgemodel.FirstOrDefault(x => x.badge_name == mostActiveName).id_badge;
            MostObservantBadgeId = badgemodel.FirstOrDefault(x => x.badge_name == MostobservantName).id_badge;
            StartCoroutine(CheckforMoseObservant());

        }
    }


    //================================  MOST ACTIVE PLAYER API TASK=========================================
    IEnumerator checkMostActivePTask()
    {
        string HittingUrl = MainUrl + MostActivePlayerApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") + "&id_level=" + 2;
        WWW activeplayer_www = new WWW(HittingUrl);
        yield return activeplayer_www;
        if (activeplayer_www.text != null)
        {
            //Debug.Log(" mose active usrer data " + activeplayer_www.text);
            MostActiveModel activeplayer_data = Newtonsoft.Json.JsonConvert.DeserializeObject<MostActiveModel>(activeplayer_www.text);
            if (activeplayer_data.play_count == "1")
            {
                Debug.Log("  USER IS A MOST ACTIVE PLATYER ");
                StartCoroutine(PostActiveBadge());
            }
            else
            {
                Debug.Log(" USER IS NOT A MOST ACTIVE PLATYER");
            }
        }
    }

    IEnumerator PostActiveBadge()
    {

        string HittingUrl = MainUrl + PostBadgeUserApi;
        var BadgeModel = new PostBadgeModel()
        {
            id_user = PlayerPrefs.GetInt("UID").ToString(),
            id_level = "2",
            id_zone = "0",
            id_room = "0",
            id_special_game = "0",
            id_badge = ActivebadgeId.ToString()
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

    //=====================================================================================================

    IEnumerator CheckforMoseObservant()
    {
        string HittingUrl = MainUrl + MostObservantApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID");
        WWW observant_www = new WWW(HittingUrl);
        yield return observant_www;
        if (observant_www.text != null)
        {
            MostObservantModel observantdata = Newtonsoft.Json.JsonConvert.DeserializeObject<MostObservantModel>(observant_www.text);
            if (observantdata.is_eligible == 1)
            {
                StartCoroutine(PostMostObservantbadge());
            }
        }
    }

    IEnumerator PostMostObservantbadge()
    {

        string HittingUrl = MainUrl + PostBadgeUserApi;
        var BadgeModel = new PostBadgeModel()
        {
            id_user = PlayerPrefs.GetInt("UID").ToString(),
            id_level = "1",
            id_zone = "0",
            id_room = "0",
            id_special_game = "0",
            id_badge = MostObservantBadgeId.ToString()
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



    IEnumerator CheckForGameBadge()
    {
        string HittingUrl = MainUrl + GetBadgeConfigApi + "?id_level=" + 2;
        WWW badge_www = new WWW(HittingUrl);
        yield return badge_www;
        if (badge_www.text != null)
        {
            //Debug.Log(" badge infp " + badge_www.text);
            List<BadgeConfigModels> badgemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BadgeConfigModels>>(badge_www.text);
            level2GameBadgeId = badgemodel.FirstOrDefault(x => x.badge_name == Level2BadgeName).id_badge;
            StartCoroutine(CheckForStage2());
        }
    }
    IEnumerator CheckForStage2()
    {
        string Response_url = MainUrl + levelClearnessApi + "?id_org_game=" + Leve2number;
        WWW dashboard_res = new WWW(Response_url);
        yield return dashboard_res;
        if (dashboard_res.text != null)
        {
            Debug.Log(" log data  " + dashboard_res.text);
            List<LevelMovement> response_data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LevelMovement>>(dashboard_res.text);
            totalscoreOfUser = response_data.FirstOrDefault(x => x.id_level == Leve2number)?.completion_score ?? 0;
        }
        Debug.Log(" user score  " + totalscoreOfUser);
        Stage2unlocked = MyTotalScore >= Stage2UnlockScore;
        if (Stage2unlocked)
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
