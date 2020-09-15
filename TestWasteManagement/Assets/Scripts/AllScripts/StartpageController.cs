using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Linq;
using UnityEngine.Networking;
using System;

public class StartpageController : MonoBehaviour 
{
    public static StartpageController Home_instane;
    public GameObject HomepageObject;
    [Header("login section")]
    public InputField username_input;
    public InputField password_input;
    public GameObject loginpage, gameheading, profilesetup_page, statusMsgpanel, loginbutton, gameFrame;
    public AESalgorithm asealgorithm;
    public GameObject firstscreen;
    public Narrationmanagement narration_activity;
    public string Mainurl, login_API,UpdateApkApi;
    public JsonData login_json;
    public GameObject login_msg;
    public float login_msg_time;
    public Toggle remeberme;
    [Header("-----profile setup elements-----")]
    public GameObject boy;
    public GameObject girl, submitbutton, characterpage, userdetailspage;
    public Image characterimage;
    public InputField profile_name;
    public Dropdown grade;
    private string charactertype;
    public Sprite boysprite, girlsprite, boyface, girlface;
    public GameObject buttonpanel, profileimage;
    public GameObject headingframe;
    public Text username_leftdashboard;
    public GameObject SignUp_panel;


    //------------------------------------//
    private Vector3 homebuttonpos, loginpage_pos;
    public GameObject homebuttonpage,homebutton;


    [Header("==superher narration part==")]
    public GameObject superhero;
    public Text narrationtext;
    public GameObject generation, seperation, management,next_btn_one_time;
    // Start is called before the first frame update
    public List<GameObject> levels;
    public List<GameObject> sublevels;
    public GameObject toplayer, midlayer,zonelayer;

    [Header("=====generation level sprites=====")]
    public Sprite toplayer_sprite;
    public Sprite midlayer_sprite,home_sprite,industry_sprite,hospital_sprite,forest_sprite,school_sprite,park_sprite, Dirty_Sky,Moderate_sky,Clear_sky, Stage2Bg;
    public Button Back,dashboardbtn,settingpanelbtn;


    [Header("====Generation level===")]
    public GameObject Generation_level;
    public List<GameObject> G_levels;
    public Text popupNotification;
    public Text zoneinfo;
    [Header("=====for double click======")]
    private int tap_count;
    public GameObject showMsg;
    int TapCount;
    public float MaxDubbleTapTime;
    float NewTime;
    public GameObject loadinganim;

    [Header("button slider panels")]
    public GameObject stagesliderpanel;
    public GameObject homesliderpanel;
    public GameObject UpdatedHomePage, UpdatedLoginPage;

    //===================== ON BORDING VARIABLES================= //
    public GameObject TriviaPage;
    public GameObject YoutubeVideopage,skipVideo;

    private bool videoPlayed, checkforEnd;
    public string nextSceneAddress;
    public GameObject UpdatePage,LoginPage;
    private bool UpdateAvailable;
    private void Awake()
    {
        Debug.Log(Application.persistentDataPath);
        loginpage_pos = loginpage.GetComponent<RectTransform>().localPosition;
        homebuttonpos = homebuttonpage.GetComponent<RectTransform>().localPosition;
        //StartCoroutine(CheckUpdatedApk());
    }

    IEnumerator CheckUpdatedApk()
    {
       
        string HittingUrl = $"{Mainurl}{UpdateApkApi}?vid={Application.version}";
        WWW checkApkwww = new WWW(HittingUrl);
        yield return checkApkwww;
        if(checkApkwww.text != null)
        {
            UpdateAvailable = checkApkwww.text == "\"1|Success\"" ? false : true;
        }
    }

    void Start()
    {
        TapCount = 0;
        
        // dashboardbtn.gameObject.SetActive(true);

        // Initialtask();
    }

    private void OnEnable()
    {
        Debug.Log("diy path " + UnityEngine.Application.persistentDataPath);
        if(Home_instane != null)
        {
            
            Destroy(Home_instane);
            ComeFromStages();
            Home_instane = this;
        }
        else
        {
            Home_instane = this;
          
            StartCoroutine(sceneappear());
        }
        DontDestroyOnLoad(Home_instane);
     
    }

    

    void ComeFromStages()
    {
        //UpdatedHomePage.SetActive(true);
        superhero.SetActive(true);
        settingpanelbtn.gameObject.SetActive(true);
        superhero.GetComponent<CompleteIntroPage>().after_badge();
    }

    IEnumerator sceneappear()
    {
        float shadevalue = HomepageObject.GetComponent<Image>().color.a;
        while(shadevalue < 1)
        {
            HomepageObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        UpdatedHomePage.SetActive(true);


    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (username_input.text != "" && password_input.text != "")
            {
                loginbutton.GetComponent<Button>().interactable = true;
            }
            else
            {
                loginbutton.GetComponent<Button>().interactable = false;
            }

            if (charactertype != "" && profile_name.text != "" && grade.value.ToString() != "")
            {
                submitbutton.GetComponent<Button>().interactable = true;
            }
            else
            {
                submitbutton.GetComponent<Button>().interactable = false;
            }


            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    TapCount += 1;
                }
                if (TapCount == 1)
                {
                    NewTime = Time.time + MaxDubbleTapTime;
                }
                else if (TapCount == 2 && Time.time <= NewTime)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
                    if (hit.collider.transform.gameObject.name == "home")
                    {
                        StartCoroutine(midlevel_task(home_sprite, 0));
                        TapCount = 0;
                    }
                    if (hit.collider.transform.gameObject.name == "school")
                    {
                        StartCoroutine(midlevel_task(school_sprite, 1));
                        TapCount = 0;
                    }
                }

            }
            if (Time.time > NewTime)
            {
                TapCount = 0;
            }

            if (videoPlayed)
            {
                if (YoutubeVideopage.GetComponent<VideoPlayer>().isPlaying)
                {
                    skipVideo.SetActive(true);
                    checkforEnd = true;
                }
                if (checkforEnd)
                {
                    if (!YoutubeVideopage.GetComponent<VideoPlayer>().isPlaying)
                    {
                        videoPlayed = false;
                        checkforEnd = false;
                        SkipVideoTask();
                    }
                }
            }
        }
    }



    public void Initialtask()
    {
        popupNotification.gameObject.SetActive(false);
        superhero.SetActive(true);
    }

    IEnumerator superheronarration()
    {
        yield return new WaitForSeconds(0.1f);
        narrationtext.text = "generation narration";
        generation.SetActive(true);
        yield return new WaitForSeconds(3f);
        narrationtext.text = "separation narration";
        seperation.SetActive(true);
        yield return new WaitForSeconds(3f);
        narrationtext.text = "management narration";
        management.SetActive(true);
        yield return new WaitForSeconds(3f);
        superhero.SetActive(false);
        toplayer.SetActive(true);
        PlayerPrefs.SetString("isfirst","done");
        for (int a = 0; a < levels.Count; a++)
        {
            levels[a].gameObject.GetComponent<Animator>().enabled = false;
        }
        StartCoroutine(popup());


    }

    public void afternarrationtask()
    {
        int count = superhero.transform.childCount;
        for(int i = 0; i < count; i++)
        {
            superhero.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        
        //next_btn_one_time.SetActive(false);
        //popupNotification.gameObject.SetActive(true);
        for (int a = 0; a < levels.Count; a++)
        {
            levels[a].gameObject.GetComponent<Animator>().enabled = false;
        }
       
        StartCoroutine(popup());
    }

    public IEnumerator scenechanges(GameObject parentobejct, Sprite new_sprite)
    {
        yield return new WaitForSeconds(0.1f);
        float bgvalue = parentobejct.GetComponent<Image>().color.a;
        while (bgvalue > 0)
        {
            bgvalue -= 0.1f;
            yield return new WaitForSeconds(0.05f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }
        parentobejct.GetComponent<Image>().sprite = new_sprite;
        bgvalue = parentobejct.GetComponent<Image>().color.a;

        while (bgvalue < 1)
        {
            bgvalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }

    }


    //------------singal method for top layer level like generation, seperation & management -------------------//
    public void Levelselection(GameObject selectedobj)
    {
        string buttonName = selectedobj.name;
        levelaction(buttonName, selectedobj);

       
    }
    IEnumerator levelanim(GameObject selectedobj)
    {
        yield return new WaitForSeconds(0.1f);
        for (int a= 0; a < levels.Count; a++)
        {
            levels[a].gameObject.GetComponent<Animator>().enabled = false;
            iTween.ScaleTo(levels[a].gameObject, Vector3.zero, 0.5f);
        }
        
    }

    void levelaction(string buttobiobj,GameObject selectedobj)
    {
        switch (buttobiobj)
        {
            case "Generation":
                StartCoroutine(ZoneGameActive(1, midlayer_sprite));
                StartCoroutine(scenechanges(HomepageObject, Dirty_Sky));
                break;
            case "seperation":
                StartCoroutine(ZoneGameActive(2, Stage2Bg));
                StartCoroutine(scenechanges(HomepageObject, Moderate_sky));
                break;
            case "Mangement":
                StartCoroutine(ZoneGameActive(3, Stage2Bg));
                StartCoroutine(scenechanges(HomepageObject, Clear_sky));
                // StartCoroutine(scenechanges(this.gameObject, midlayer_sprite));
                break;
            default:
                break;
        }
    }

    IEnumerator ZoneGameActive(int Sceneno,Sprite midlayer_sprite)
    {
        yield return new WaitForSeconds(0.2f);
        toplayer.SetActive(false);
        yield return new WaitForSeconds(1.2f);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(scenechanges(HomepageObject, midlayer_sprite));
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(Sceneno);

    }
    //-------------------end of method----------------------//

    //--------------------singal method for middle layer levels like, home,hospital,factory etc-------------------------//

    public void sublevelmethod(GameObject selectedbtn)
    {
           midlayer.SetActive(false);
           string buttonname = selectedbtn.name;
           selectedbtn.transform.GetChild(0).gameObject.SetActive(false);
           zoneinfo.text = "";
           sublevelactions(buttonname);
    }
    
    void sublevelactions(string name)
    {
        switch (name.ToLower())
        {
            case "home":
                 StartCoroutine(midlevel_task(home_sprite, 0));
                break;
            case "school":
                 StartCoroutine(midlevel_task(school_sprite, 1));
                break;
            case "hospital":
                StartCoroutine(midlevel_task(home_sprite, 2));
                break;
            case "office":
                StartCoroutine(midlevel_task(home_sprite, 3));
                break;
            case "industry":
                StartCoroutine(midlevel_task(home_sprite, 4));
                break;
            case "park":
                StartCoroutine(midlevel_task(home_sprite, 5));
                break;
            default:
                Debug.Log("unique zone");
                break;
        }

    }

    

    IEnumerator showindication(string msg,Text msgbox)
    {
        yield return new WaitForSeconds(0.1f);
        msgbox.text = msg;
        iTween.ScaleTo(msgbox.gameObject, Vector3.one, 0.5f);
        yield return new WaitForSeconds(2f);
        iTween.ScaleTo(msgbox.gameObject, Vector3.zero, 0.5f);
        msgbox.text = "";
    }

    IEnumerator midlevel_task(Sprite scene_sprite,int level_id)
    {
       
        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponent<AudioSource>().enabled = false;
        zonelayer.SetActive(false);
        StartCoroutine(scenechanges(HomepageObject, scene_sprite));
        yield return new WaitForSeconds(1f);
        Generation_level.SetActive(true);
        G_levels[level_id].SetActive(true);


    }

    IEnumerator popup()
    {
        StartCoroutine(scenechanges(HomepageObject, home_sprite));
        yield return new WaitForSeconds(1.2f);
        toplayer.SetActive(true);
        for (int i =0;i < levels.Count; i++)
        {
            iTween.ScaleTo(levels[i].gameObject, Vector3.one, 0.3f);
            yield return new WaitForSeconds(0.2f);
        }
        for (int a = 0; a < levels.Count; a++)
        {
            levels[a].gameObject.GetComponent<Animator>().enabled = true;
        }

    }

   public void midlayerenable()
    {
        homesliderpanel.SetActive(false);
        stagesliderpanel.SetActive(true);
        toplayer.SetActive(false);
        Back.gameObject.SetActive(true);
        Back.onClick.RemoveAllListeners();
        Back.onClick.AddListener(delegate { backfrommidlayer(); });
        midlayer.SetActive(true);
    }
    
    void fromLevelToMidlayer()
    {
        StartCoroutine(fromleveltimidTask());
        StartCoroutine(scenechanges(HomepageObject, midlayer_sprite));

    }

    IEnumerator fromleveltimidTask()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(scenechanges(HomepageObject, midlayer_sprite));
        yield return new WaitForSeconds(1f);
        for (int a = 0; a < sublevels.Count; a++)
        {
            sublevels[a].transform.localScale = Vector3.one;
            sublevels[a].GetComponent<Animator>().enabled = true;
        }
        midlayer.SetActive(true);
        Back.onClick.RemoveAllListeners();
        Back.onClick.AddListener(delegate { backfrommidlayer(); });

    }


    public void backfrommidlayer()
    {
        foreach(GameObject e in G_levels)
        {
            e.gameObject.GetComponent<Zonehandler>().zone_completed = false;
            e.gameObject.GetComponent<Zonehandler>().final_completed = false;
        }
        homesliderpanel.SetActive(true);
        stagesliderpanel.SetActive(false);
        StartCoroutine(back_from_stage());
       
    }
    IEnumerator back_from_stage()
    {
        midlayer.SetActive(false);
        StartCoroutine(scenechanges(HomepageObject, home_sprite));
        yield return new WaitForSeconds(1.2f);
        toplayer.SetActive(true);
    }
    public void play()
    {
        string remeber_data = PlayerPrefs.GetString("logged");
        if (UpdateAvailable)
        {
            UpdatedLoginPage.SetActive(true);
            LoginPage.SetActive(false);
            UpdatePage.SetActive(true);
        }
        else
        {
           
            if (remeber_data == "true")
            {
                settingpanelbtn.gameObject.SetActive(true);
                StartCoroutine(game_play());
            }
            else
            {
                StartCoroutine(play_action());
            }
        }
    
      
    }

    public void SignUp_page()
    {
        StartCoroutine(signup_task());
    }

    IEnumerator signup_task()
    {
        iTween.MoveTo(loginpage, iTween.Hash("position", loginpage_pos, "easeType", iTween.EaseType.linear, "isLocal", true,
      "time", 0.5f));
        yield return new WaitForSeconds(0.6f);
        homebutton.SetActive(false);
        loginpage.SetActive(false);
        SignUp_panel.SetActive(true);

    }

    IEnumerator game_play()
    {
        //iTween.MoveTo(homebuttonpage, iTween.Hash("position", homebuttonpos, "easeType", iTween.EaseType.linear, "isLocal", true,
        //  "time", 0.3f));
        yield return new WaitForSeconds(0.3f);
        UpdatedHomePage.SetActive(false);

        Debug.Log(PlayerPrefs.GetInt("UID") + " " + PlayerPrefs.GetInt("OID"));
        StartCoroutine(scenechanges(HomepageObject, toplayer_sprite));
        yield return new WaitForSeconds(1.2f);
        homebuttonpage.SetActive(false);
        string profile_data = PlayerPrefs.GetString("profile_done");
        superhero.SetActive(true);
        
      
    }
    IEnumerator play_action()
    {
        //iTween.MoveTo(homebuttonpage, iTween.Hash("position", homebuttonpos, "easeType", iTween.EaseType.linear, "isLocal", true,
        //   "time", 0.3f));
        yield return new WaitForSeconds(0.3f);
       
        string msg = PlayerPrefs.GetString("login_msg");
        if (msg != "done")
        {
            login_msg.SetActive(true);
        }
        else
        {
            login_msg.SetActive(false);
        }
        //loginpage.SetActive(true);
        // homebutton.SetActive(true);
        UpdatedHomePage.SetActive(false);
        LoginPage.SetActive(true);
        UpdatedLoginPage.SetActive(true);
        homebuttonpage.SetActive(false);
    }

    public void backtohome()
    {
        StartCoroutine(backhome_action());
    }

    IEnumerator backhome_action()
    {
        
        homebutton.SetActive(false);
        homebuttonpage.SetActive(true);
        iTween.MoveTo(loginpage, iTween.Hash("position", loginpage_pos, "easeType", iTween.EaseType.linear, "isLocal", true,
          "time", 0.5f));
        yield return new WaitForSeconds(0.6f);
        loginpage.SetActive(false);


    }

    public void login_task()
    {
        PlayerPrefs.SetString("login_msg", "done");
        StartCoroutine(login_action());
    }

    IEnumerator login_action()
    {
        iTween.ScaleTo(UpdatedLoginPage, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.6f);
        var username = username_input.text;
        var password = password_input.text;
        string encryptedpassword1 = asealgorithm.getEncryptedString(password);
        Debug.Log("encrypred password " + encryptedpassword1);

        UpdatedLoginPage.SetActive(false);
        if (username != "" && password != "")
        {
            loadinganim.SetActive(true);
            username_input.text = "";
            password_input.text = "";
           

            //----------------------api integration -------------//
            string loginlink = Mainurl + login_API;
            var Loginlog = new LoginModel
            {
                IMEI = "",
                USERID = username,
                PASSWORD = encryptedpassword1,
                OS = "",
                Network = "",
                OSVersion = "",
                Details = "",
                REURL = ""
            };

            string LoginLogdata = Newtonsoft.Json.JsonConvert.SerializeObject(Loginlog);
            Debug.Log("Login data " + LoginLogdata);


            using (UnityWebRequest Request = UnityWebRequest.Put(loginlink, LoginLogdata))
            {
                Request.method = UnityWebRequest.kHttpVerbPOST;
                Request.SetRequestHeader("Content-Type", "application/json");
                Request.SetRequestHeader("Accept", "application/json");
                yield return Request.SendWebRequest();
                if (!Request.isNetworkError && !Request.isHttpError)
                {
                    Debug.Log(Request.downloadHandler.text);
                    if (Request.downloadHandler.text != null)
                    {
                        UserAuthenticationModel UserdataLog = Newtonsoft.Json.JsonConvert.DeserializeObject<UserAuthenticationModel>(Request.downloadHandler.text);
                        if (UserdataLog.AuthStatus.Equals("success", System.StringComparison.OrdinalIgnoreCase))
                        {
                            PlayerPrefs.SetInt("UID", UserdataLog.IDUSER);
                            PlayerPrefs.SetInt("OID", UserdataLog.OID);
                            PlayerPrefs.SetInt("game_id", UserdataLog.id_org_game_unit);
                            PlayerPrefs.SetString("gender", Convert.ToString(UserdataLog.GENDER));
                            PlayerPrefs.SetString("username", Convert.ToString(UserdataLog.FIRST_NAME != null ? UserdataLog.FIRST_NAME : "--"));
                            PlayerPrefs.SetString("User_grade", UserdataLog.UserGrade != null ? UserdataLog.UserGrade.ToString() :"0");
                            PlayerPrefs.SetInt("id_school", UserdataLog.id_school != null ? UserdataLog.id_school : 0);
                            PlayerPrefs.SetInt("characterType", UserdataLog.avatar_type != null ? UserdataLog.avatar_type : 0);
                            PlayerPrefs.SetInt("PlayerBody", UserdataLog.body_type != null ? UserdataLog.body_type : 0); 
                            PlayerPrefs.SetString("profile_done", UserdataLog.avatar_type != null ? "done" :"false");
                            settingpanelbtn.gameObject.SetActive(true);
                            username_leftdashboard.text = PlayerPrefs.GetString("username");
                            username_input.GetComponent<InputField>().enabled = true;
                            password_input.GetComponent<InputField>().enabled = true;
                            loginbutton.GetComponent<Button>().enabled = true;
                            homebutton.SetActive(false);
                            loadinganim.SetActive(false);
                            string msg = "Logged In Successfully!";
                            StartCoroutine(Messagedisplay(msg));
                            yield return new WaitForSeconds(3.6f);
                            loginpage.SetActive(false);
                            videoPlayed = true;
                            YoutubeVideopage.SetActive(true);
                            TriviaPage.SetActive(true);
                            Camera.main.gameObject.GetComponent<AudioSource>().enabled = false;

                        }
                        else
                        {
                            username_input.GetComponent<InputField>().enabled = true;
                            password_input.GetComponent<InputField>().enabled = true;
                            loginbutton.GetComponent<Button>().enabled = true;
                            loadinganim.SetActive(false);
                            string msg = "Login In Failed!!";
                            StartCoroutine(Messagedisplay(msg));
                            yield return new WaitForSeconds(3.5f);
                            UpdatedLoginPage.SetActive(true);
                        }
                    }
                }
                else
                {
                    username_input.GetComponent<InputField>().enabled = true;
                    password_input.GetComponent<InputField>().enabled = true;
                    loginbutton.GetComponent<Button>().enabled = true;
                    loadinganim.SetActive(false);
                    string msg = "Check your internet Connection !!!";
                    StartCoroutine(Messagedisplay(msg));
                    yield return new WaitForSeconds(3.5f);
                    UpdatedLoginPage.SetActive(true);
                }

            }


            //Debug.Log(loginlink);
            //WWWForm loginForm = new WWWForm();
            //loginForm.AddField("IMEI", "");
            //loginForm.AddField("USERID", username);
            //loginForm.AddField("PASSWORD", encryptedpassword1);
            //loginForm.AddField("OS", "");
            //loginForm.AddField("Network", "");
            //loginForm.AddField("OSVersion", "");
            //loginForm.AddField("Details", "");
            //loginForm.AddField("REURL", "");

            //WWW loginurl = new WWW(loginlink, loginForm);
            //yield return loginurl;
            //if (loginurl.text != null)
            //{
            //    Debug.Log(loginurl.text);
            //    login_json = JsonMapper.ToObject(loginurl.text);
            //    string status = login_json["AuthStatus"].ToString();
            //    int UID = int.Parse(login_json["IDUSER"].ToString());
            //    int OID = int.Parse(login_json["OID"].ToString());
            //    int game_id = int.Parse(login_json["id_org_game_unit"].ToString());
            //    PlayerPrefs.SetInt("game_id", game_id);
              
            //    if(status.ToLower() == "success")
            //    {

            //        if (UID == PlayerPrefs.GetInt("UID"))
            //        {
            //            PlayerPrefs.SetInt("UID", UID);
            //            PlayerPrefs.SetInt("OID", OID);
            //            PlayerPrefs.SetInt("game_id", game_id);
            //            PlayerPrefs.SetString("gender", login_json["GENDER"].ToString());
            //            if (login_json["FIRST_NAME"].ToString() != null)
            //            {
            //                PlayerPrefs.SetString("username", login_json["FIRST_NAME"].ToString());
            //            }
            //            if (login_json["UserGrade"] != null)
            //            {
            //                PlayerPrefs.SetString("User_grade", login_json["UserGrade"].ToString());
            //            }
            //            if(login_json["id_school"] != null)
            //            {
            //                PlayerPrefs.SetInt("id_school", int.Parse(login_json["id_school"].ToString()));
            //            }
            //            PlayerPrefs.SetInt("characterType", int.Parse(login_json["avatar_type"].ToString()));
            //            PlayerPrefs.SetInt("PlayerBody", int.Parse(login_json["body_type"].ToString()));
            //            PlayerPrefs.SetString("Gender", login_json["GENDER"].ToString());
            //            if (login_json["avatar_type"] != null)
            //            {
            //                PlayerPrefs.SetString("profile_done", "done");
                           
            //            }
            //        }
            //        else
            //        {
            //            PlayerPrefs.SetInt("UID", UID);
            //            PlayerPrefs.SetInt("OID", OID);
            //            PlayerPrefs.SetInt("game_id", game_id);
            //            PlayerPrefs.SetString("gender", login_json["GENDER"].ToString());
            //            if (login_json["FIRST_NAME"] != null)
            //            {
            //                PlayerPrefs.SetString("username", login_json["FIRST_NAME"].ToString());
            //            }
            //            if (login_json["UserGrade"] != null)
            //            {
            //                PlayerPrefs.SetString("User_grade", login_json["UserGrade"].ToString());
            //            }
            //            if (login_json["id_school"] != null)
            //            {
            //                PlayerPrefs.SetInt("id_school", int.Parse(login_json["id_school"].ToString()));
            //            }
            //            PlayerPrefs.SetInt("characterType", int.Parse(login_json["avatar_type"].ToString()));
            //            PlayerPrefs.SetInt("PlayerBody", int.Parse(login_json["body_type"].ToString()));
            //            PlayerPrefs.SetString("Gender", login_json["GENDER"].ToString());
            //            if (login_json["avatar_type"] != null)
            //            {
            //                PlayerPrefs.SetString("profile_done", "done");
            //            }
            //            //else
            //            //{
            //            //    PlayerPrefs.DeleteKey("profile_done");
            //            //    PlayerPrefs.DeleteKey("username");
            //            //    PlayerPrefs.DeleteKey("User_grade");
            //            //    PlayerPrefs.DeleteKey("characterType");
            //            //    PlayerPrefs.DeleteKey("PlayerBody");

            //            //}
            //        }
            //        //===============================================================//
            //        settingpanelbtn.gameObject.SetActive(true);
            //        username_leftdashboard.text = PlayerPrefs.GetString("username");
            //        username_input.GetComponent<InputField>().enabled = true;
            //        password_input.GetComponent<InputField>().enabled = true;
            //        loginbutton.GetComponent<Button>().enabled = true;
            //        homebutton.SetActive(false);
            //        loadinganim.SetActive(false);
            //        string msg = "Logged In Successfully!";
            //        StartCoroutine(Messagedisplay(msg));
            //        yield return new WaitForSeconds(3.6f);
            //        loginpage.SetActive(false);
            //        videoPlayed = true;
            //        YoutubeVideopage.SetActive(true);
            //        TriviaPage.SetActive(true);
            //        Camera.main.gameObject.GetComponent<AudioSource>().enabled = false;
                  
            //    }
            //    else
            //    {
            //        username_input.GetComponent<InputField>().enabled = true;
            //        password_input.GetComponent<InputField>().enabled = true;
            //        loginbutton.GetComponent<Button>().enabled = true;
            //        loadinganim.SetActive(false);
            //        string msg = "Login In Failed!!";
            //        StartCoroutine(Messagedisplay(msg));
            //        yield return new WaitForSeconds(3.5f);
            //        UpdatedLoginPage.SetActive(true);
            //    }
            //}

        }
    }

    public void SkipVideoTask()
    {
        videoPlayed = false;
        StartCoroutine(CoroutineskipVideo());
    }
    IEnumerator CoroutineskipVideo()
    {
        skipVideo.SetActive(false);
        YoutubeVideopage.SetActive(false);
        TriviaPage.SetActive(false);
        if (remeberme.isOn == true)
        {
            PlayerPrefs.SetString("logged", "true");
        }
        remeberme.isOn = false;
        StartCoroutine(scenechanges(HomepageObject, toplayer_sprite));
        yield return new WaitForSeconds(1.2f);
        superhero.SetActive(true);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = true;
    }






    IEnumerator Enablepage(GameObject enableobject, GameObject disableobject, float time)
    {
        yield return new WaitForSeconds(time);
        enableobject.SetActive(true);
        disableobject.SetActive(false);

    }

    public IEnumerator Messagedisplay(string msg)
    {
        yield return new WaitForSeconds(0.1f);
        statusMsgpanel.SetActive(true);
        statusMsgpanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        iTween.ScaleTo(statusMsgpanel, Vector3.one, 0.8f);
        yield return new WaitForSeconds(2f);
        iTween.ScaleTo(statusMsgpanel, Vector3.zero, 0.8f);
        yield return new WaitForSeconds(1f);
        statusMsgpanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        statusMsgpanel.SetActive(false);
    }

    //------------------------avatar seletion--------------//

    public void boyselection()
    {
        charactertype = "boy";
        StartCoroutine(Enablepage(userdetailspage, characterpage, 1f));
        characterimage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 300);
        characterimage.sprite = boysprite;

    }
    public void girlselection()
    {
        charactertype = "girl";
        StartCoroutine(Enablepage(userdetailspage, characterpage, 1f));
        characterimage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 330);
        characterimage.sprite = girlsprite;
    }

    public void submit()
    {
        string profilename = profile_name.text;
        string grade_value = grade.options[grade.value].text;
        if (charactertype == "boy")
        {
            PlayerPrefs.SetInt("characterType", 1);
            profileimage.GetComponent<Image>().sprite = boyface;
        }
        else
        {
            PlayerPrefs.SetInt("characterType", 0);
            profileimage.GetComponent<Image>().sprite = girlface;
        }
        profilesetup_page.SetActive(false);
        narration_activity.AfterAvatar_task();


    }

    public void backpage()
    {
        StartCoroutine(Enablepage(characterpage, userdetailspage, 1f));
    }

   
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

 


}
