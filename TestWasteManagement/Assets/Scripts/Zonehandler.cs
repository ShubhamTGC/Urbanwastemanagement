
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Linq;

public class Zonehandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject zonepage;
    public Sprite mainzone;
    public List<GameObject> subzones;
    public List<GameObject> room1, room2, room3;
    public List<GameObject> reduce, reuse, recycle, partially_reduce, partially_reuse, partially_recycle;
    public List<Sprite> subzonesprite;
    public Generationlevel startpage;
    public GameObject startpageobj;
    public GameObject dustbin, dustbintarget;
    private Vector3 initailpos_dusbin,initial_pos_timer;
    public int level1score = 0;
    public GameObject timerpanel,scoreknob;
    public Text scoretext;
    public Button backbtn,nextroombtn,yesbtn;
    public List< List<GameObject>> test;
    public bool room1_check = false,room2_check = false,room3_check= false;
    public bool room1_clear = false, room2_clear = false, room3_clear = false;
    public int waste_count = 0;
    public GameObject Done_msg_panel,exit_panel;                               
    private GameObject gb;
   // private List<Vector2> objectpos;
    [Header("====extra elements====")]
    public GameObject fridge;
    public GameObject tubelight,selectionpage;
    public List<GameObject> rooms;
    //public GameFrameHandler gameframe;
    private float totalobjs, totalscore, knobangle;
    public bool score_check = false;
    public GameObject leftdashboardbtn;
    private bool timerstart,timerwarining = true;
    public GameObject timesuppage,timer;
    public Button timesupbtn;
    public int countDown_mint;
    public bool zone_completed,final_completed;

    [Header("======For count down time====")]
    public float mint;
    public Text timertext;
    public float sec;
    private float totalsecond, TotalSec;
    public Image timerimage;
    public GameObject scorepanel;
    private bool timerstop =false;

    
    public Dictionary<string, string> room1_data = new Dictionary<string, string>();
    public Dictionary<string, string> room2_data = new Dictionary<string, string>();
    public Dictionary<string, string> room3_data = new Dictionary<string, string>();
    [Header("-====for dashbaord====")]
    public Sprite right_ans;
    public Sprite wrong_ans;
    public Text overall_score_r1,overall_score_r2,overall_score_r3;
    public Image player_image;
    public Sprite boy_image, girl_image;
    public Image scored_image, action_plan_image, total_score_img;
    public Text score_text, actionplan_text, total_score_text;
    private int total_score_game = 280;
    public int actionplan_score;
    public Text username;
    public GameObject dashboard_parent,dashboard_panel;
    public List<string> room1_data_collected = new List<string>(8);
    public List<string> room2_data_collected = new List<string>(8);
    public List<string> room3_data_collected = new List<string>(8);
    public List<GameObject> tabs;
    public GameObject tab_prefeb, data_row_prefeb;
    private List<GameObject> tab1_object,tab2_object,tab3_object;
    private string room1name, room2name, room3name;
    public List<GameObject> tab_obj;
    public Button next_Zone,kitch_btn,bedrom_btn,livingbtn;
    private bool is_win = false;
    private GameObject tabs_obj;
    public GameObject collected_text;
    public int collected_count;
    public Coloreffect timer_color_object;
    public GameObject action_plan_page;
    private bool action_check =  true;
    public List<GameObject> rowhandler_parent;
    private int room1_score =0, room2_score = 0, room3_score=0;
    public Text User_grade_field;
    [HideInInspector]
    public int active_room_end;
    public GameObject bonus_page;


    [Header("home_page")]
    public GameObject Home_page_dashboard;
    public string Zone_name;
    public int id_content;
    public bool is_zome_completed;
    public string MainUrl, dashboard_api;
    public Button home_btn;



    [Header("For leaderboard Data")]
    [Space(10)]
    public string RoomData_api;
    private List<int> RoomIds = new List<int>();
    public int Bonus_Score;
    private float Bonusscore_room1, Bonusscore_room2, Bonusscore_room3;
    public Image BonusScoreFiller;
    public float TotalBonusGameScore;
    public Text CollectedBonusScore;
   // private int Room_oneId,Room_twoId,Room_thirdId;

    //================================================start scripting=============================================//



    void Start()
    {
        tabs = new List<GameObject>(new GameObject[subzones.Count]);
        timerstop = false;
        room1_clear = room2_clear = room3_clear = false;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        backbtn.onClick.RemoveAllListeners();
        backbtn.onClick.AddListener(delegate { initialback(); });
        leftdashboardbtn.SetActive(true);
        score_check = false;
        float totalobjs = room1.Count + room2.Count + room3.Count;
        totalscore = totalobjs * 10;
        selectionpage.SetActive(true);
        //Initialtask(0);
    }
     void OnEnable()
    {
        tabs = new List<GameObject>(new GameObject[subzones.Count]);
        timerstop = false;
        room1_score = room2_score = room3_score = 0;
        room1_clear = room2_clear = room3_clear = false;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        backbtn.onClick.RemoveAllListeners();
        backbtn.onClick.AddListener(delegate { initialback(); });
        leftdashboardbtn.SetActive(true);
        score_check = false;
        float totalscore = room1.Count + room2.Count + room3.Count;
        totalscore = totalobjs * 10;
        timerpanel.SetActive(false);
        selectionpage.SetActive(true);
        //Initialtask(0);
        StartCoroutine(CollectRoomdata());
    }
    IEnumerator CollectRoomdata()
    {

        string Hittingurl = MainUrl + RoomData_api + "?id_user=" + PlayerPrefs.GetInt("UID") + "&id_org_content=" + id_content;   
        Debug.Log("main Url " + Hittingurl);
        WWW roominfo = new WWW(Hittingurl);
        yield return roominfo;
        if (roominfo.text != null)
        {
            
            JsonData RoomrRes = JsonMapper.ToObject(roominfo.text);
            for(int a = 0; a < RoomrRes.Count; a++)
            {
                RoomIds.Add(int.Parse(RoomrRes[a]["id_room"].ToString()));
                Debug.Log("Room " + a + " " + RoomIds[a]);
            }

        }
    }

    public void Initialtask(int roomno)
    {
        
        StartCoroutine(Initialactivity(roomno));
        
    }

    IEnumerator Initialactivity(int roomno)
    {
        room1_check = room2_check = room3_check = false;
        selectionpage.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(startpage.scenechanges(startpageobj, subzonesprite[roomno]));
        yield return new WaitForSeconds(1.5f);
        waste_count = 0;
        knobangle = 0;
        collected_count = 0;
        room1_score = room2_score = room3_score = 0;
        level1score = 0;
        initial_pos_timer = timerpanel.GetComponent<RectTransform>().localPosition;
        timerpanel.SetActive(true);
        timer.SetActive(true);
        mint = countDown_mint;
        Timeraction();
        timer.GetComponent<AudioSource>().enabled = true;
        timerstart = true;
        initailpos_dusbin = dustbin.GetComponent<RectTransform>().localPosition;
        Vector3 dustbinpos = dustbintarget.GetComponent<RectTransform>().localPosition;
        iTween.MoveTo(dustbin, iTween.Hash("position", dustbinpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));
        subzones[roomno].SetActive(true);
        home_btn.onClick.AddListener(delegate { yesclose(roomno); });
        yesbtn.onClick.AddListener(delegate { yesclose(roomno); });
        if (roomno == 0)
        {
            room1_check = true;
            
            rooms = room1;
        }
        else if (roomno == 1)
        {
            rooms = room2;
            
            room2_check = true;
        }
        else if(roomno == 2)
        {
            rooms = room3;
           
            room3_check = true;
        }
        backbtn.onClick.RemoveAllListeners();
        backbtn.onClick.AddListener(delegate { backtozonepage(rooms); });
    }

    void Timeraction()
    {
        totalsecond = mint * 60f;
        mint = mint - 1;
        sec = 60;
        TotalSec = totalsecond;
        StartCoroutine(Countdowntimer());
    }
    public void OnDisable()
    {
        RoomIds.Clear();
        level1score = 0;
        room1_clear = room2_clear = room3_clear = false;
        mint = 0;
        scoretext.text = "";
        room1_data.Clear();
        room2_data.Clear();
        room3_data.Clear();
        is_win = false;
        timerwarining = true;
        scoreknob.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 0f);
        level1score = 0;
        leftdashboardbtn.SetActive(false);
    }


    void Update()
    {
        //---------------------------------------timer section with 30 sec warning condition--------------//
        if(mint == 0 && sec == 59 && timerwarining)
        {
            timerwarining = false;
            timer_color_object.timertask();
        }
        
        if(mint == 0 && sec == 0 && timerstart)
        {
            timer.GetComponent<Coloreffect>().isdone = true;
            if (!is_win)
            {
                if (room1_check)
                {
                    room1_check = false;
                    timerstart = false;
                    room1_clear = true;
                    room1_score = level1score - (room2_score +room3_score);
                    //Bonusscore_room1 = 0;
                    timer.GetComponent<AudioSource>().enabled = false;
                    if (!room2_clear)
                    {
                        nextroombtn.onClick.RemoveAllListeners();
                        yesbtn.onClick.RemoveAllListeners();
                        string msg = "Times up for this room, you can proceed with next room.";
                        StartCoroutine(showstatus(msg));
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        yesbtn.onClick.RemoveAllListeners();
                        home_btn.onClick.RemoveAllListeners();
                        home_btn.onClick.AddListener(delegate { yesclose(1); });
                        yesbtn.onClick.AddListener(delegate { yesclose(1); });
                        nextroombtn.onClick.AddListener(delegate { movetonext(1, 0, 1); });
                    }
                    else if (!room3_clear)
                    {
                        nextroombtn.onClick.RemoveAllListeners();
                        yesbtn.onClick.RemoveAllListeners();
                        string msg = "Times up for this room, you can proceed with next room.";
                        StartCoroutine(showstatus(msg));
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        yesbtn.onClick.RemoveAllListeners();
                        home_btn.onClick.RemoveAllListeners();
                        home_btn.onClick.AddListener(delegate { yesclose(2); });
                        yesbtn.onClick.AddListener(delegate { yesclose(2); });
                        nextroombtn.onClick.AddListener(delegate { movetonext(2, 1, 2); });
                    }
                    else
                    {
                        timerstart = false;
                        timertext.text = "Times Up!";
                        iTween.ScaleTo(timesuppage, Vector3.one, 1f);
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        timer.GetComponent<Coloreffect>().isdone = true;
                        timesupbtn.onClick.AddListener(delegate { timesup_action(); });
                   }

                }
                 else if (room2_check)
                {
                    room2_check = false;
                    timerstart = false;
                    room2_clear = true;
                    timer.GetComponent<AudioSource>().enabled = false;
                    room2_score = level1score - (room1_score + room3_score);
                   // Bonusscore_room2 = 0;
                    if (!room3_clear)
                    {
                        nextroombtn.onClick.RemoveAllListeners();
                        yesbtn.onClick.RemoveAllListeners();
                        string msg = "Times up for this room, you can proceed with next room.";
                        StartCoroutine(showstatus(msg));
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        yesbtn.onClick.RemoveAllListeners();
                        home_btn.onClick.RemoveAllListeners();
                        home_btn.onClick.AddListener(delegate { yesclose(2); });
                        yesbtn.onClick.AddListener(delegate { yesclose(2); });
                        nextroombtn.onClick.AddListener(delegate { movetonext(2, 1, 2); });
                    }
                    else
                    {
                        timerstart = false;
                        timertext.text = "Times Up!";
                        iTween.ScaleTo(timesuppage, Vector3.one, 1f);
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        timer.GetComponent<Coloreffect>().isdone = true;
                        timesupbtn.onClick.AddListener(delegate { timesup_action(); });
                    }

                }
                else if (room3_check)
                {
                    room3_check = false;
                    timerstart = false;
                    room3_clear = true;
                    timer.GetComponent<AudioSource>().enabled = false;
                    //Bonusscore_room3 = 0;
                    room3_score = level1score - (room1_score + room2_score);
                    if (!room1_clear)
                    {
                        nextroombtn.onClick.RemoveAllListeners();
                        yesbtn.onClick.RemoveAllListeners();
                        string msg = "Times up for this room, you can proceed with next room.";
                        StartCoroutine(showstatus(msg));
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        yesbtn.onClick.RemoveAllListeners();
                        home_btn.onClick.RemoveAllListeners();
                        home_btn.onClick.AddListener(delegate { yesclose(0); });
                        yesbtn.onClick.AddListener(delegate { yesclose(0); });
                        nextroombtn.onClick.AddListener(delegate { movetonext(0, 1, 0); });
                    }
                    else
                    {
                        timerstart = false;
                        timertext.text = "Times Up!";
                        iTween.ScaleTo(timesuppage, Vector3.one, 1f);
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        timer.GetComponent<Coloreffect>().isdone = true;
                        timesupbtn.onClick.AddListener(delegate { timesup_action(); });
                    }

                }

                
  
            }
           
         

        }
     //-==========================================================================================//
        if(waste_count == room1.Count && room1_check)
        {
            room1_check = false;
            room1name = "Kitchen";
            room1_clear = true;
            room1_score = level1score - (room2_score + room3_score);
            if (room1_clear && room2_clear && room3_clear)
            {
               // Bonusscore_room1 = Bonus_Score;
                StartCoroutine(zone_completiontask(0));
            }
            else
            {
                timerstop = true;
                nextroombtn.onClick.RemoveAllListeners();
                yesbtn.onClick.RemoveAllListeners();
                string msg = "You have collected all the waste materials, you can move to next room.";
                StartCoroutine(showstatus(msg));
                if (!room2_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    home_btn.onClick.RemoveAllListeners();
                    home_btn.onClick.AddListener(delegate { yesclose(1); });
                    yesbtn.onClick.AddListener(delegate { yesclose(1); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(1,0, 1); });
                }
                else if (!room3_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    home_btn.onClick.RemoveAllListeners();
                    home_btn.onClick.AddListener(delegate { yesclose(2); });
                    yesbtn.onClick.AddListener(delegate { yesclose(2); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(2,0, 2); });
                }
               
            }
           
        }
        if(waste_count == room2.Count && room2_check)
        {
            room2_check = false;
            room2_clear = true;
            room2name = "Bedroom";
            room2_score = level1score - (room1_score + room3_score);
            if (room1_clear && room2_clear && room3_clear)
            {
               // Bonusscore_room2 = Bonus_Score;
                StartCoroutine(zone_completiontask(1));
            }
            else
            {
                timerstop = true;
                nextroombtn.onClick.RemoveAllListeners();
                yesbtn.onClick.RemoveAllListeners();
                string msg = "You have collected all the waste materials, you can move to next room.";
                StartCoroutine(showstatus(msg));

                if (!room1_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    home_btn.onClick.RemoveAllListeners();
                    home_btn.onClick.AddListener(delegate { yesclose(0); });
                    yesbtn.onClick.AddListener(delegate { yesclose(0); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(0,1, 0); });
                }
                else if (!room3_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    home_btn.onClick.RemoveAllListeners();
                    home_btn.onClick.AddListener(delegate { yesclose(2); });
                    yesbtn.onClick.AddListener(delegate { yesclose(2); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(2,1, 2); });
                }
            }
         
        }
        if (waste_count == room3.Count && room3_check)
        {
            room3_check = false;
            room3_clear = true;
            room3name = "Livingroom";
            room3_score = level1score - (room2_score + room1_score);
            if (room1_clear && room2_clear && room3_clear)
            {
               // Bonusscore_room3 = Bonus_Score; 
               StartCoroutine(zone_completiontask(2));
            }
            else
            {
                timerstop = true;
                nextroombtn.onClick.RemoveAllListeners();
                yesbtn.onClick.RemoveAllListeners();
                string msg = "You have collected all the waste materials, you can move to next room.";
                StartCoroutine(showstatus(msg));
                if (!room1_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    home_btn.onClick.RemoveAllListeners();
                    home_btn.onClick.AddListener(delegate { yesclose(0); });
                    yesbtn.onClick.AddListener(delegate { yesclose(0); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(0,2, 0); });
                }
                else if (!room2_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    home_btn.onClick.RemoveAllListeners();
                    home_btn.onClick.AddListener(delegate { yesclose(1); });
                    yesbtn.onClick.AddListener(delegate { yesclose(1); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(1,2, 1); });
                }
            }
       
        }

        if (score_check)
        {
            score_check = false;
            collected_text.GetComponent<Text>().text = collected_count.ToString();
            scorepanel.GetComponent<shakeeffect>().enabled = true;
            Invoke("stopshake", 1.5f);
            scoretext.text = level1score.ToString();
            knobangle = (level1score / totalscore) * 200;
            //scoreknob.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -knobangle);
            
        }
        var rotationangle = Quaternion.Euler(0f, 0f, -knobangle);
        scoreknob.GetComponent<RectTransform>().rotation = Quaternion.Lerp(scoreknob.GetComponent<RectTransform>().rotation, rotationangle, 10 * 1 * Time.deltaTime);
        //scoreknob.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -knobangle);
        if (action_plan_page.activeInHierarchy && action_check)
        {
            if (actionplan_score == 0)
            {

            }
            else
            {
                action_check = false;
                float total_scored;
                float level_score;
                score_text.text = level1score.ToString();
                total_score_text.text = (level1score + actionplan_score).ToString();
                total_scored = level1score + actionplan_score;
                total_score_img.fillAmount = total_scored / total_score_game;
                level_score = level1score;
                scored_image.fillAmount = level_score / total_score_game;
                float plan_score = (float)actionplan_score;
                action_plan_image.fillAmount = plan_score / 100.00f;
                actionplan_text.text = actionplan_score.ToString();
            }
        }
    }

    void Timer_functionlity(bool nextroom1,bool nextroom2)
    {
     
    }

    void stopshake()
    {
        scorepanel.GetComponent<shakeeffect>().enabled = false;
    }
    public void scoreupdated()
    {
        scoretext.text = level1score.ToString();
        float knobangle = (level1score / totalscore) * 200;
        scoreknob.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -knobangle);
    }

    IEnumerator showstatus(string msg)
    {
        yield return new WaitForSeconds(1.5f);
        waste_count = 0;
        Done_msg_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        iTween.ScaleTo(Done_msg_panel, Vector3.one, 1f);
    }

     void movetonext(int roomsprite,int lastroom, int zoneactive)
    {
        
        timer.GetComponent<Coloreffect>().isdone = true;
        if (zoneactive == 0)
        {
            backbtn.onClick.RemoveAllListeners();
            collected_count = 0;
            backbtn.onClick.AddListener(delegate { backtozonepage(room1); });
            room1_check = true;
        }
        else if(zoneactive == 1)
        {
            collected_count = 0;
            backbtn.onClick.RemoveAllListeners();
            backbtn.onClick.AddListener(delegate { backtozonepage(room2); });
            room2_check = true;
        }
        else if(zoneactive == 2)
        {
            collected_count = 0;
            room3_check = true;
            backbtn.onClick.RemoveAllListeners();
            backbtn.onClick.AddListener(delegate { backtozonepage(room3); });
        }
        
        iTween.ScaleTo(Done_msg_panel, Vector3.zero, 1f);
        Done_msg_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        StartCoroutine(nextroom( roomsprite, lastroom, zoneactive));
    }

    IEnumerator nextroom(int roomsprite,int lastroom,int zoneactive)
    {
        subzones[lastroom].SetActive(false);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(startpage.scenechanges(startpageobj, subzonesprite[roomsprite]));
        yield return new WaitForSeconds(1.5f);
        subzones[zoneactive].SetActive(true);
        mint = countDown_mint;
        if (timerstop)
        {
            timerstop = false;
            Timeraction();
        }
        else
        {
            totalsecond = mint * 60f;
            mint = mint - 1;
            sec = 60;
            TotalSec = totalsecond;
        }
        
        timerwarining = true;
        timer.GetComponent<AudioSource>().enabled = true;
        timerstart = true;
        collected_text.GetComponent<Text>().text ="0";
        Vector3 dustbinpos = dustbintarget.GetComponent<RectTransform>().localPosition;
        iTween.MoveTo(dustbin, iTween.Hash("position", dustbinpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));

    }
    public void backtozonepage(List<GameObject> roomobject)
    {
        if (waste_count == roomobject.Count)
        {
           
        }
        else
        {
            exit_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You have not found all the waste, Do you really want to exit!";
            iTween.ScaleTo(exit_panel, Vector3.one, 1f);
        
        }
    }


    void yesclose(int subzonevalue)
    {
        zone_completed = true;
        StartCoroutine(backtozone(subzonevalue));
        mint = 0;
        collected_text.GetComponent<Text>().text = "0";
        iTween.MoveTo(timerpanel, iTween.Hash("position", initial_pos_timer, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));
        iTween.MoveTo(dustbin, iTween.Hash("position", initailpos_dusbin, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));
    }

    IEnumerator backtozone(int subzone)
    {
        yield return new WaitForSeconds(0.1f);
        iTween.ScaleTo(exit_panel, Vector3.zero, 0.6f);
        exit_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(1f);
        timerpanel.SetActive(false);
        subzones[subzone].SetActive(false);
        StartCoroutine(startpage.scenechanges(startpageobj, mainzone));
        yield return new WaitForSeconds(1.5f);
        Camera.main.GetComponent<AudioSource>().enabled = true;
        zonepage.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void noclose()
    {
        iTween.ScaleTo(exit_panel, Vector3.zero, 1f);
        exit_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = ""; 
        
    }
    
    public void zonedone(int lastroom)
    {
        StartCoroutine(zonecomplete(lastroom));
        iTween.MoveTo(dustbin, iTween.Hash("position", initailpos_dusbin, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));
    }

    IEnumerator zonecomplete(int lastroom)
    {
        zone_completed = true;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < tab1_object.Count; i++)
        {
            Destroy(tab1_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < tab2_object.Count; i++)
        {
            Destroy(tab2_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < tab3_object.Count; i++)
        {
            Destroy(tab3_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int a = 0; a < tabs.Count; a++)
        {
            Destroy(tabs[a].gameObject);
            yield return new WaitForSeconds(0.2f);
        }
        for (int a = 0; a < tabs.Count; a++)
        {
           
            Destroy(tabs[a].gameObject);
            yield return new WaitForSeconds(0.2f);
        }
        collected_text.GetComponent<Text>().text = "0";
        room1_data_collected.Clear();
        room2_data_collected.Clear();
        room3_data_collected.Clear();
        livingbtn.transform.localScale = Vector3.one;
        kitch_btn.transform.localScale = Vector3.one;
        bedrom_btn.transform.localScale = Vector3.one;
        dashboard_panel.SetActive(false);
        bonus_page.SetActive(false);
        level1score = 0;
        iTween.ScaleTo(Done_msg_panel, Vector3.zero, 0.6f);
        Done_msg_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(0.6f);
        subzones[lastroom].SetActive(false);
        StartCoroutine(startpage.scenechanges(startpageobj, mainzone));
        yield return new WaitForSeconds(1.5f);
        Camera.main.GetComponent<AudioSource>().enabled = true;
        zonepage.SetActive(true);

        this.gameObject.SetActive(false);
    }

    void timesup_action()
    {
        mint = 0;
        timesupbtn.onClick.RemoveAllListeners();
        iTween.ScaleTo(timesuppage, Vector3.zero, 1f);
        next_Zone.onClick.RemoveAllListeners();
        next_Zone.onClick.AddListener(delegate { after_timeup_action(); });
        knobangle = 0;
        iTween.MoveTo(timerpanel, iTween.Hash("position", initial_pos_timer, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.6f));
        iTween.MoveTo(dustbin, iTween.Hash("position", initailpos_dusbin, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.6f));
        Final_dashboard();
    }

    void after_timeup_action()
    {
        StartCoroutine(after_timeup());
    }
    IEnumerator after_timeup()
    {
        zone_completed = true;
        yield return new WaitForSeconds(0.8f);
        timerpanel.SetActive(false);
        timer.GetComponent<Coloreffect>().enabled = false;
        for(int i = 0;i < tab1_object.Count; i++)
        {
            Destroy(tab1_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < tab2_object.Count; i++)
        {
            Destroy(tab2_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < tab3_object.Count; i++)
        {
            Destroy(tab3_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }   
        collected_text.GetComponent<Text>().text = "0";
        room1_data_collected.Clear();
        room2_data_collected.Clear();
        room3_data_collected.Clear();
        for (int a = 0; a < tabs.Count; a++)
        {
            Destroy(tabs[a].gameObject);
            yield return new WaitForSeconds(0.2f);
        }
        dashboard_panel.SetActive(false);
        timer.SetActive(false);
        for (int a = 0; a < subzones.Count; a++)
        {
            if (subzones[a].gameObject.activeInHierarchy)
            {
                subzones[a].SetActive(false);
            }
        }
        level1score = 0;
        StartCoroutine(startpage.scenechanges(startpageobj, mainzone));
        yield return new WaitForSeconds(1.5f);
        Camera.main.GetComponent<AudioSource>().enabled = true;
        zonepage.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //------------------stopwatchtimer---------------------------------//
    public IEnumerator Countdowntimer()
    {

        yield return new WaitForSecondsRealtime(1f);
        if (sec > 0)
        {
            sec--;
        }

        if (sec == 0 && mint != 0)
        {
            mint--;
            sec = 60;
        }
        timertext.text = mint + " : " + sec;
        Timefilling();
        if (!timerstop)
        {
            StartCoroutine(Countdowntimer());
        }
       
    }

    void Timefilling()
    {
        totalsecond--;
        float fill = (float)totalsecond / TotalSec;
        timerimage.fillAmount = fill;
    }

    void initialback()
    {
        StartCoroutine(initialback_task());
       
    }
    IEnumerator initialback_task()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(startpage.scenechanges(startpageobj, mainzone));
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
        Camera.main.GetComponent<AudioSource>().enabled = true;
        zonepage.SetActive(true);
    }



    IEnumerator zone_completiontask(int zone)
    {
        is_win = true;
        yield return new WaitForSeconds(0f);
        timer.GetComponent<AudioSource>().enabled = false;
        string donemsg = "OK, you have completed the zone, let’s see how have you scored!" +
"\nBy the way, you have to get at least 75 % " +
"score to get the Officer Badge!";
        nextroombtn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Go Ahead!";
        nextroombtn.onClick.RemoveAllListeners();
        nextroombtn.onClick.AddListener(delegate { Final_dashboard(); });
        timerstop = true;
        mint = 0;
        final_completed = true;
        timer.GetComponent<Coloreffect>().isdone = false;
        yield return new WaitForSeconds(1.5f);
        iTween.MoveTo(dustbin, iTween.Hash("position", initailpos_dusbin, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        iTween.MoveTo(timerpanel, iTween.Hash("position", initial_pos_timer, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        yield return new WaitForSeconds(0.6f);
        timerpanel.SetActive(false);
        StartCoroutine(showstatus(donemsg));
        next_Zone.onClick.AddListener(delegate { zonedone(zone); });
        active_room_end = zone;


    }

 

    //--------------------------------latest dashboard word ============================//

    void Final_dashboard()
    {
        dashboard_panel.SetActive(true);
        List<string> room1_elements = new List<string>(new string[room1.Count]);
        List<string> room2_elements = new List<string>(new string[room2.Count]);
        List<string> room3_elements = new List<string>(new string[room3.Count]);
        PlayerPrefs.SetString("zonename", this.gameObject.name);
        string[] room1_item_name = new string[3];
        string[] room2_item_name = new string[3];
        string[] room3_item_name = new string[3];
        List<string> item_name_list1 = new List<string>();// = new string[];
        List<string> item_name_list2 = new List<string>();// new string[room2_data_collected.Count];
        List<string> item_name_list3 = new List<string>();// new string[room3_data_collected.Count];
        List<string> dropped_bin1 = new List<string>();// new string[room1_data_collected.Count];
        List<string> dropped_bin2 = new List<string>(); 
        List<string> dropped_bin3 = new List<string>(); 
        List<string> score_room1 = new List<string>();// new string[room1.Count];
        List<string> score_room2 = new List<string>();// new string[room2.Count];
        List<string> score_room3 = new List<string>();
        string[] correct_option1 = new string[room1.Count];
        string[] correct_option2 = new string[room2.Count];
        string[] correct_option3 = new string[room3.Count];
        List<int> room1_is_right = new List<int>(); //new int[room1.Count];
        List<int> room2_is_right = new List<int>();
        List<int> room3_is_right = new List<int>();
        int charater_type = PlayerPrefs.GetInt("characterType");
        //List<string> room1_misssed = new List<string>();
        int r1 = 0, r2=0, r3=0;
        int correctansroom1 = 0;
        int correctansroom2 = 0;
        int correctansroom3 = 0;
        if(charater_type == 1)
        {
            player_image.sprite = boy_image;
        }
        else if(charater_type == 2)
        {
            player_image.sprite = girl_image;
        }
        
        for (int a = 0; a < room1_data_collected.Count; a++)
        {
            string get_data = room1_data_collected[a];
            room1_item_name = get_data.Split(","[0]);
            item_name_list1.Add(room1_item_name[0]);
            dropped_bin1.Add(room1_item_name[1]);
            score_room1.Add(room1_item_name[2]);

           // Debug.Log("here is the data " + item_name_list1[a] + " " + dropped_bin1[a] + " " + score_room1[a]);
            Array.Clear(room1_item_name, 0, room1_item_name.Length);
        }

        for (int a = 0; a < room2_data_collected.Count; a++)
        {
            string get_data = room2_data_collected[a];
            room2_item_name = get_data.Split(',');
            item_name_list2.Add(room2_item_name[0]);
            dropped_bin2.Add(room2_item_name[1]);
            score_room2.Add(room2_item_name[2]);
          //  Debug.Log("here is the data 2 " + item_name_list2[a] + " " + dropped_bin2[a] + " " + score_room2[a]);
            Array.Clear(room2_item_name, 0, room1_item_name.Length);
        }
        for (int a = 0; a < room3_data_collected.Count; a++)
        {
            string get_data = room3_data_collected[a];
            room3_item_name = get_data.Split(',');
            item_name_list3.Add(room3_item_name[0]);
            dropped_bin3.Add(room3_item_name[1]);
            score_room3.Add(room3_item_name[2]);
           // Debug.Log("here is the data " + item_name_list3[a] + " " + dropped_bin3[a] + " " + score_room3[a]);
            Array.Clear(room3_item_name, 0, room1_item_name.Length);
        }


        for (int a = 0; a < score_room1.Count; a++)
        {
            if (score_room1[a] == "10")
            {
                correctansroom1++;
            }
        }
        if (correctansroom1 == room1.Count)
        {
            Bonusscore_room1 = Bonus_Score;
        }
        for (int a = 0; a < score_room2.Count; a++)
        {
            if (score_room2[a] == "10")
            {
                correctansroom2++;
            }
           
        }
        if (correctansroom2 == room2.Count)
        {
            Bonusscore_room2 = Bonus_Score;
        }
        for (int a = 0; a < score_room3.Count; a++)
        {
            if (score_room3[a] == "10")
            {
                correctansroom3++;
            }
           
        }
        if (correctansroom3 == room3.Count)
        {
            Bonusscore_room3 = Bonus_Score;
        }

        for (int i = 0; i < subzones.Count; i++)
        {
            GameObject gb = Instantiate(tab_prefeb, rowhandler_parent[i].transform, false);
            gb.name = "Tab" + i;
            tabs[i] = gb;
            gb.SetActive(false);
        }
        tab1_object = new List<GameObject>(new GameObject[room1.Count]);
        tab2_object = new List<GameObject>(new GameObject[room2.Count]);
        tab3_object = new List<GameObject>(new GameObject[room3.Count]);
        User_grade_field.text = PlayerPrefs.GetString("User_grade");
        string user_name = PlayerPrefs.GetString("username");
        username.text = user_name;
        overall_score_r1.text = room1_score.ToString();
        overall_score_r2.text = room2_score.ToString();
        overall_score_r3.text = room3_score.ToString();
        float total_scored;
        float level_score;
        score_text.text = level1score.ToString();
        total_score_text.text = (level1score + actionplan_score).ToString();
        total_scored = level1score + actionplan_score;
        total_score_img.fillAmount = total_scored / total_score_game;
        level_score = level1score;
        scored_image.fillAmount = level_score / total_score_game;

        BonusScoreFiller.fillAmount = (Bonusscore_room1 + Bonusscore_room2 + Bonusscore_room3) / TotalBonusGameScore;
        CollectedBonusScore.text = (Bonusscore_room1 + Bonusscore_room2 + Bonusscore_room3).ToString();
        //====================================tab 1 data filling====================================================//
        // room1_misssed = new List<string>(new string[room1.Count - item_name_list1.Count]);
        // Debug.Log("lenght of missing items:  "+room1_misssed.Count);
        int counter1 = 0;

        var distinctElements_room1 = item_name_list1 != null && item_name_list1.Count > 0 ? room1.Where(x => !item_name_list1.Contains(x.name)).Select(x => x.name).ToList() : new List<string>();
        var distinctElements_room2 = item_name_list2 != null && item_name_list2.Count > 0 ? room2.Where(x => !item_name_list2.Contains(x.name)).Select(x => x.name).ToList() : new List<string>();
        var distinctElements_room3 = item_name_list3 != null && item_name_list3.Count > 0 ? room3.Where(x => !item_name_list3.Contains(x.name)).Select(x => x.name).ToList() : new List<string>();
       // Debug.Log("got object name: " + string.Join(", ", distinctElements));

       
        for (int i = 0; i < room1.Count; i++)
        {
            GameObject row = Instantiate(data_row_prefeb, tabs[0].gameObject.transform, false);
            row.gameObject.name = "data_row" + i;
            tab1_object[i] = row;
            row.SetActive(true);
            correct_option1[i] = "null";

        }

        for (int b = 0; b < tab1_object.Count; b++)
        {
            if(b < item_name_list1.Count)
            {
                tab1_object[b].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = item_name_list1[b];
                if (score_room1[b] != "0")
                {
                    tab1_object[b].gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "+" + score_room1[b];
                }
                else
                {
                    tab1_object[b].gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = score_room1[b];
                }

                if (dropped_bin1[b].ToLower() == "reduce")
                {
                    if (score_room1[b] == "10" || score_room1[b] == "5")    
                    {
                        room1_is_right.Add(1);
                        tab1_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab1_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right_ans;

                    }
                    else
                    {
                        for (int a = 0; a < reduce.Count; a++)
                        {

                            if (reduce[a].gameObject.name == item_name_list1[b])
                            {
                                correct_option1[b] = "Reduce" ;
                                tab1_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                           
                        }
                        for (int c = 0; c < reuse.Count; c++)
                        {
                            if (reuse[c].gameObject.name == item_name_list1[b])
                            {
                                correct_option1[b] = "Reuse";
                                tab1_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int d = 0; d < recycle.Count; d++)
                        {
                            if (recycle[d].gameObject.name == item_name_list1[b])
                            {
                                correct_option1[b] = "Recycle";
                                tab1_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        room1_is_right.Add(0);
                        tab1_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab1_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong_ans;
                    }

                }
                else if (dropped_bin1[b].ToLower() == "reuse")
                {
                    if (score_room1[b] == "10" || score_room1[b] == "5")
                    {
                        room1_is_right.Add(1);
                        tab1_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab1_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right_ans;
                    }
                    else
                    {
                        for (int a = 0; a < reduce.Count; a++)
                        {

                            if (reduce[a].gameObject.name == item_name_list1[b])
                            {
                                correct_option1[b] = "Reduce";
                                tab1_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < reuse.Count; a++)
                        {
                            if (reuse[a].gameObject.name == item_name_list1[b])
                            {
                                correct_option1[b] = "Reuse";
                                tab1_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < recycle.Count; a++)
                        {
                            if (recycle[a].gameObject.name == item_name_list1[b])
                            {
                                correct_option1[b] = "Recycle";
                                tab1_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        room1_is_right.Add(0);
                        tab1_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab1_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong_ans;
                    }
                }
                else if (dropped_bin1[b].ToLower() == "recycle")
                {
                    if (score_room1[b] == "10" || score_room1[b] == "5")
                    {
                        room1_is_right.Add(1);
                        tab1_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab1_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right_ans;
                    }
                    else
                    {
                        for (int a = 0; a < reduce.Count; a++)
                        {

                            if (reduce[a].gameObject.name == item_name_list1[b])
                            {
                                correct_option1[b] = "Reduce";
                                tab1_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < reuse.Count; a++)
                        {
                            if (reuse[a].gameObject.name == item_name_list1[b])
                            {
                                correct_option1[b] = "Reuse";
                                tab1_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < recycle.Count; a++)
                        {
                            if (recycle[a].gameObject.name == item_name_list1[b])
                            {
                                correct_option1[b] = "Recycle";
                                tab1_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        room1_is_right.Add(0);
                        tab1_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab1_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong_ans;
                    }
                }
            }
            else
            {
                score_room1.Add("0");
                room1_is_right.Add(2);
                dropped_bin1.Add("null");
                if (distinctElements_room1.Count > 0)
                {
                    item_name_list1.Add(distinctElements_room1[r1]);
                    tab1_object[b].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = distinctElements_room1[r1];
                    r1++;
                }
                else
                {
                    tab1_object[b].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = room1[b].name;
                }
               
               
                tab1_object[b].gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0";
                tab1_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "--";
                tab1_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "--";
                tab1_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "--";
               
                correct_option1[b] = "null";
            }
   

        }


        //=================================================tab 2 data filling==========================================================//
        for (int i = 0; i < room2.Count; i++)
        {
            GameObject row = Instantiate(data_row_prefeb, tabs[1].gameObject.transform, false);
            row.gameObject.name = "data_row" + i;
            tab2_object[i] = row;
            row.SetActive(true);
            correct_option2[i] = "null";

        }
        for (int b = 0; b < tab2_object.Count; b++)
        {
            if (b < item_name_list2.Count)
            {
                
                tab2_object[b].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = item_name_list2[b];
                if (score_room2[b] != "0")
                {
                    tab2_object[b].gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "+" + score_room2[b];
                }
                else
                {
                    tab2_object[b].gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = score_room2[b];
                }

                if (dropped_bin2[b].ToLower() == "reduce")
                {
                    if (score_room2[b] == "10" || score_room2[b] == "5")
                    {
                        room2_is_right.Add(1);
                        tab2_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab2_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right_ans;

                    }
                    else
                    {
                        for (int a = 0; a < reduce.Count; a++)
                        {

                            if (reduce[a].gameObject.name == item_name_list2[b])
                            {
                                correct_option2[b] = "Reduce";
                                tab2_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < reuse.Count; a++)
                        {
                            if (reuse[a].gameObject.name == item_name_list2[b])
                            {
                                correct_option2[b] = "Reuse";
                                tab2_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < recycle.Count; a++)
                        {
                            if (recycle[a].gameObject.name == item_name_list2[b])
                            {
                                correct_option2[b] = "Recycle";
                                tab2_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        room2_is_right.Add(0);
                        tab2_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab2_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong_ans;
                    }

                }
                else if (dropped_bin2[b].ToLower() == "reuse")
                {
                    if (score_room2[b] == "10" || score_room2[b] == "5")
                    {
                        room2_is_right.Add(1);
                        tab2_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab2_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right_ans;
                    }
                    else
                    {
                        for (int a = 0; a < reduce.Count; a++)
                        {

                            if (reduce[a].gameObject.name == item_name_list2[b])
                            {
                                correct_option2[b] = "Reduce";
                                tab2_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < reuse.Count; a++)
                        {
                            if (reuse[a].gameObject.name == item_name_list2[b])
                            {
                                correct_option2[b] = "Reuse";
                                tab2_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < recycle.Count; a++)
                        {
                            if (recycle[a].gameObject.name == item_name_list2[b])
                            {
                                correct_option2[b] = "Recycle";
                                tab2_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        room2_is_right.Add(0);
                        tab2_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab2_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong_ans;
                    }
                }
                else if (dropped_bin2[b].ToLower() == "recycle")
                {
                    if (score_room2[b] == "10" || score_room2[b] == "5")
                    {
                        room2_is_right.Add(1);
                        tab2_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab2_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right_ans;
                    }
                    else
                    {
                        for (int a = 0; a < reduce.Count; a++)
                        {

                            if (reduce[a].gameObject.name == item_name_list2[b])
                            {
                                correct_option2[b] = "Reduce";
                                tab2_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < reuse.Count; a++)
                        {
                            if (reuse[a].gameObject.name == item_name_list2[b])
                            {
                                correct_option2[b] = "Reuse";
                                tab2_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < recycle.Count; a++)
                        {
                            if (recycle[a].gameObject.name == item_name_list2[b])
                            {
                                correct_option2[b] = "Recycle";
                                tab2_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        room2_is_right.Add(0);
                        tab2_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab2_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong_ans;
                    }
                }
            }
            else
            {
                score_room2.Add("0");
                room2_is_right.Add(2);
                correct_option2[b] = "null";
                dropped_bin2.Add("null");
                if(distinctElements_room2.Count > 0)
                {
                    item_name_list2.Add(distinctElements_room2[r2]);
                    tab2_object[b].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = distinctElements_room2[r2];
                    r2++;
                }
                else
                {
                    tab2_object[b].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = room2[b].name;
                }
                
               
                tab2_object[b].gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0";
                tab2_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "--";
                tab2_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "--";
                tab2_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "--";
               
            }

        }

        //=================================================tab 3 data filling===========================================//
        for (int i = 0; i < room3.Count; i++)
        {
            GameObject row = Instantiate(data_row_prefeb, tabs[2].gameObject.transform, false);
            row.gameObject.name = "data_row" + i;
            tab3_object[i] = row;
            row.SetActive(true);
            correct_option3[i] = "null";
        }

        for (int b = 0; b < tab3_object.Count; b++)
        {
            if (b < item_name_list3.Count)
            {


                tab3_object[b].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = item_name_list3[b];
                if (score_room3[b] != "0")
                {
                    tab3_object[b].gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "+" + score_room3[b];
                }
                else
                {
                    tab3_object[b].gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = score_room3[b];
                }

                if (dropped_bin3[b].ToLower() == "reduce")
                {
                    if (score_room3[b] == "10" || score_room3[b] == "5")
                    {
                        room3_is_right.Add(1);
                        tab3_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab3_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right_ans;

                    }
                    else
                    {
                        for (int a = 0; a < reduce.Count; a++)
                        {

                            if (reduce[a].gameObject.name == item_name_list3[b])
                            {
                                correct_option3[b] = "Reduce";
                                tab3_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < reuse.Count; a++)
                        {
                            if (reuse[a].gameObject.name == item_name_list3[b])
                            {
                                correct_option3[b] = "Reuse";
                                tab3_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < recycle.Count; a++)
                        {
                            if (recycle[a].gameObject.name == item_name_list3[b])
                            {
                                correct_option3[b] = "Recycle";
                                tab3_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        room3_is_right.Add(0);
                        tab3_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab3_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong_ans;
                    }

                }
                else if (dropped_bin3[b].ToLower() == "reuse")
                {
                    if (score_room3[b] == "10" || score_room3[b] == "5")
                    {
                        room3_is_right.Add(1);
                        tab3_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab3_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right_ans;
                    }
                    else
                    {
                        for (int a = 0; a < reduce.Count; a++)
                        {

                            if (reduce[a].gameObject.name == item_name_list3[b])
                            {
                                correct_option3[b] = "Reduce";
                                tab3_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < reuse.Count; a++)
                        {
                            if (reuse[a].gameObject.name == item_name_list3[b])
                            {
                                correct_option3[b] = "Reuse";
                                tab3_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < recycle.Count; a++)
                        {
                            if (recycle[a].gameObject.name == item_name_list3[b])
                            {
                                correct_option3[b] = "Recycle";
                                tab3_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        room3_is_right.Add(0);
                        tab3_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab3_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong_ans;
                    }
                }
                else if (dropped_bin3[b].ToLower() == "recycle")
                {
                    if (score_room3[b] == "10" || score_room3[b] == "5")
                    {
                        room3_is_right.Add(1);
                        tab3_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab3_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right_ans;
                    }
                    else
                    {
                        for (int a = 0; a < reduce.Count; a++)
                        {

                            if (reduce[a].gameObject.name == item_name_list3[b])
                            {
                                correct_option3[b] = "Reduce";
                                tab3_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < reuse.Count; a++)
                        {
                            if (reuse[a].gameObject.name == item_name_list3[b])
                            {
                                correct_option3[b] = "Reuse";
                                tab3_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        for (int a = 0; a < recycle.Count; a++)
                        {
                            if (recycle[a].gameObject.name == item_name_list3[b])
                            {
                                correct_option3[b] = "Recycle";
                                tab3_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct";
                            }
                        }
                        room3_is_right.Add(0);
                        tab3_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        tab3_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong_ans;
                    }
                }
            }
            else
            {
                score_room3.Add("0");
                room3_is_right.Add(2);
                correct_option3[b] = "null";
                dropped_bin3.Add("null");
                if(distinctElements_room3.Count > 0)
                {
                    item_name_list3.Add(distinctElements_room3[r3]);
                    tab3_object[b].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = distinctElements_room3[r3];
                    r3++;
                }
                else
                {
                    tab3_object[b].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = room3[b].name;
                }
                
            
                tab3_object[b].gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0";
                tab3_object[b].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "--";
                tab3_object[b].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "--";
                tab3_object[b].gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "--";
              
            }

        }
        foreach(GameObject a in tabs)
        {
            a.SetActive(true);
        }

        var logs = new List<ZoneDataPost>();
        int l = 0;
        if(item_name_list1.Count > 0)
        {
            item_name_list1.ForEach(x =>
            {
                var log = new ZoneDataPost()
                {
                    item_collected = item_name_list1[l],
                    score = int.Parse(score_room1[l]),
                    is_right = room1_is_right[l],
                    correct_option = correct_option1[l],
                    id_content = id_content,
                    id_level = 1,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = dropped_bin1[l],
                    id_room = RoomIds[0]
                };
                l = l + 1;
                logs.Add(log);

            });
        }
        else
        {
            room1.ForEach(x =>
            {
                var log = new ZoneDataPost()
                {
                    item_collected = room1[l].name,
                    score = int.Parse(score_room1[l]),
                    is_right = room1_is_right[l],
                    correct_option = correct_option1[l],
                    id_content = id_content,
                    id_level = 1,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = dropped_bin1[l],
                    id_room = RoomIds[0]
                };
                l = l + 1;
                logs.Add(log);

            });
        }
     

        int k = 0;
        if(item_name_list2.Count > 0)
        {
            item_name_list2.ForEach(x =>
            {
                var log1 = new ZoneDataPost()
                {
                    item_collected = item_name_list2[k],
                    score = int.Parse(score_room2[k]),
                    is_right = room2_is_right[k],
                    correct_option = correct_option2[k],
                    id_content = id_content,
                    id_level = 1,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    dustbin = dropped_bin2[k],
                    id_user = PlayerPrefs.GetInt("UID"),
                    id_room = RoomIds[1]
                };
                k = k + 1;
                logs.Add(log1);
            });
        }
        else
        {
            room2.ForEach(x =>
            {
                var log1 = new ZoneDataPost()
                {
                    item_collected = room2[k].name,
                    score = int.Parse(score_room2[k]),
                    is_right = room2_is_right[k],
                    correct_option = correct_option2[k],
                    id_content = id_content,
                    id_level = 1,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    dustbin = dropped_bin2[k],
                    id_user = PlayerPrefs.GetInt("UID"),
                    id_room = RoomIds[1]
                };
                k = k + 1;
                logs.Add(log1);
            });
        }
      
        int j = 0;
        if(item_name_list3.Count > 0)
        {
            item_name_list3.ForEach(x =>
            {
                var log2 = new ZoneDataPost()
                {
                    item_collected = item_name_list3[j],
                    score = int.Parse(score_room3[j]),
                    is_right = room3_is_right[j],
                    correct_option = correct_option3[j],
                    id_content = id_content,
                    id_level = 1,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    dustbin = dropped_bin3[j],
                    id_user = PlayerPrefs.GetInt("UID"),
                    id_room = RoomIds[2]
                };
                j = j + 1;
                logs.Add(log2);
            });
        }
        else
        {
            room3.ForEach(x =>
            {
                var log2 = new ZoneDataPost()
                {
                    item_collected = room3[j].name,
                    score = int.Parse(score_room3[j]),
                    is_right = room3_is_right[j],
                    correct_option = correct_option3[j],
                    id_content = id_content,
                    id_level = 1,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    dustbin = dropped_bin3[j],
                    id_user = PlayerPrefs.GetInt("UID"),
                    id_room = RoomIds[2]
                };
                j = j + 1;
                logs.Add(log2);
            });
        }
       

        string data = JsonMapper.ToJson(logs);
        Debug.Log(data);
        StartCoroutine(Post_data(data));

        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(CaptureIt());
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string image_name = Zone_name + PlayerPrefs.GetInt("UID").ToString();
            UnityEngine.ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/" + image_name);
        }
        

    }

    IEnumerator Post_data(string log_data)
    {
        string post_url = MainUrl + dashboard_api;
        //=====post method ===================//
        WWWForm post_form = new WWWForm();
        post_form.AddField("log_string", log_data);
        WWW zone_www = new WWW(post_url, post_form);
        yield return zone_www;
        if (zone_www.text != null)
        {
            Debug.Log(zone_www.text);
            is_zome_completed = true;
        }

    }
    public void dash_tab1()
    {
        kitch_btn.gameObject.GetComponent<Image>().enabled = true;
        kitch_btn.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        bedrom_btn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        livingbtn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        bedrom_btn.gameObject.GetComponent<Image>().enabled = false;
        livingbtn.gameObject.GetComponent<Image>().enabled = false;
        tabs[0].gameObject.SetActive(true);
        tabs[1].gameObject.SetActive(false);
        tabs[2].gameObject.SetActive(false);
    }
    public void dash_tab2()
    {
        kitch_btn.gameObject.GetComponent<Image>().enabled = false;
        bedrom_btn.gameObject.GetComponent<Image>().enabled = true;
        kitch_btn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        bedrom_btn.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        livingbtn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        livingbtn.gameObject.GetComponent<Image>().enabled = false;
        tabs[0].gameObject.SetActive(false);
        tabs[1].gameObject.SetActive(true);
        tabs[2].gameObject.SetActive(false);
    }

    public void dash_tab3()
    {
        kitch_btn.gameObject.GetComponent<Image>().enabled = false;
        bedrom_btn.gameObject.GetComponent<Image>().enabled = false;
        kitch_btn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        bedrom_btn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        livingbtn.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        livingbtn.gameObject.GetComponent<Image>().enabled = true;
        tabs[0].gameObject.SetActive(false);
        tabs[1].gameObject.SetActive(false);
        tabs[2].gameObject.SetActive(true);
    }

    public void action_plan_activite()
    {
        //PlayerPrefs.SetString("zonename", this.gameObject.name);
        action_plan_page.SetActive(true);
    }

    IEnumerator CaptureIt()
    {
        //string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = Zone_name  + ".png";
        string pathToSave = fileName;
        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();
        //Instantiate(blink, new Vector2(0f, 0f), Quaternion.identity);
    }



}
