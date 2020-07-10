using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class Zonenarrations : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> zones;
    public List<string> zone_info;
    public GameObject superhero,msg1,msg2,msg3,popupobject,skipbtn,touchinfo,backbtn;
    private string onetime_narration;
    public GameObject videplayer;

    [Header("===zone narrarion======")]
    public GameObject textbox;
    public GameObject zone_text,video_msg_panel;
    private bool isskipped = false;
    public GameObject startpage;
    public Zonehandler hoomzone, schoolzone,Hospitalzone,officezone,industryzone,parkzone;
    public GameObject last_msg;
    public GameObject YoutubePlayer,skipbutton,videomsg;


    [Header("Stage 2 unlock Portion")]
    [Space(10)]
    public string MainUrl;
    public string DashbaordApi;
    [SerializeField]
    private int Stage2UnlockScore;
    private int totalscoreOfUser;
    public GameObject Stage2popup;
    private bool Stage2unlocked = false;

    public GameObject startpageobj;
    private Generationlevel Mainpage;
    public Sprite greenBackground,CityPage;
    public GameObject Bonusgamepage;
    void Start()
    {
        Mainpage = FindObjectOfType<Generationlevel>();
        //onetime_narration = PlayerPrefs.GetString("zone_guide");
        //if (onetime_narration != "done")
        //{
        //    StartCoroutine(startguide());
        //}
        //else
        //{
        //    skipbtn.SetActive(false);
        //    superhero.SetActive(false);
        //    for (int a = 0; a < zones.Count; a++)
        //    {
        //        zones[a].gameObject.SetActive(true);
        //    }
        //}
        if (hoomzone.zone_completed || schoolzone.zone_completed || Hospitalzone.zone_completed || officezone.zone_completed
            || industryzone.zone_completed || parkzone.zone_completed)
        {
            if(hoomzone.final_completed && schoolzone.final_completed && Hospitalzone.final_completed && officezone.final_completed 
                && industryzone.final_completed && parkzone.final_completed)
            {
                for (int a = 0; a < zones.Count; a++)
                {
                    zones[a].gameObject.SetActive(false);
                }
                StartCoroutine(last_msg_task());
            }
            else
            {
                for (int a = 0; a < zones.Count; a++)
                {
                    zones[a].gameObject.SetActive(true);
                }
            }
           
        }
        else
        {
            StartCoroutine(startNarration());
        }
       
    
        
    }



     void OnEnable()
    {
      

        StartCoroutine(CheckForStage2());
        if (hoomzone.zone_completed || schoolzone.zone_completed || Hospitalzone.zone_completed || officezone.zone_completed 
            || industryzone.zone_completed || parkzone.zone_completed)
        {
            if (hoomzone.final_completed && schoolzone.final_completed && Hospitalzone.final_completed && officezone.final_completed
                && industryzone.final_completed && parkzone.final_completed)
            {
                for (int a = 0; a < zones.Count; a++)
                {
                    zones[a].gameObject.SetActive(false);
                }
                StartCoroutine(last_msg_task());
               
            }
            else
            {
                for (int a = 0; a < zones.Count; a++)
                {
                    zones[a].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            StartCoroutine(startNarration());
        }
    }


    IEnumerator last_msg_task()
    {
        last_msg.SetActive(true);
        yield return new WaitForSeconds(5f);
        last_msg.SetActive(false);
    }
    private void OnDisable()
    {
        StopCoroutine("startguide");
        touchinfo.SetActive(false);
        skipbtn.SetActive(false);
        msg1.SetActive(false);
        video_msg_panel.SetActive(false);
        msg2.SetActive(false);
        superhero.SetActive(false);
        popupobject.SetActive(false);
        zone_text.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        zone_text.SetActive(false);
        msg1.SetActive(false);
        msg2.SetActive(false);
        msg3.SetActive(false);
        popupobject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator startguide()
    {
        yield return new WaitForSeconds(0.1f);
        touchinfo.SetActive(true);
        PlayerPrefs.SetString("zone_guide", "done");
        superhero.SetActive(true);
        yield return new WaitForSeconds(1f);
        popupobject.SetActive(true);
        msg1.SetActive(true);
        yield return new WaitForSeconds(10f);
        msg1.SetActive(false);
        msg2.SetActive(true);
       

    }
    public void for_msg2()
    {
        StartCoroutine(msg2_task());
    }

    IEnumerator msg2_task()
    {
        msg1.SetActive(false);
        msg2.SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }

    public void playVideo()
    {
        video_msg_panel.SetActive(false);
        popupobject.SetActive(false);
        Camera.main.GetComponent<AudioSource>().enabled = false;
        skipbtn.SetActive(true);
        //videplayer.SetActive(true);
        videomsg.SetActive(true);
        YoutubePlayer.SetActive(true);
        skipbutton.SetActive(true);
        backbtn.SetActive(false);
        msg2.SetActive(false);
    }

    public void close_video()
    {
        StartCoroutine(closevideo_action());
    }

    IEnumerator closevideo_action()
    {
        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponent<AudioSource>().enabled =true;
        backbtn.SetActive(true);
        // videplayer.GetComponent<RawImage>().enabled = false;
        // videplayer.SetActive(false);
        YoutubePlayer.SetActive(false);
        skipbutton.SetActive(false);
        popupobject.SetActive(true);
        msg3.SetActive(true);
        yield return new WaitForSeconds(10f);
        touchinfo.SetActive(false);
        skipbtn.SetActive(false);
        msg1.SetActive(false);
        msg2.SetActive(false);
        msg3.SetActive(false);
        superhero.SetActive(false);
        popupobject.SetActive(false);
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(true);
        }
    }
    public void skip()
    {
        StopCoroutine("startguide");
        touchinfo.SetActive(false);
        skipbtn.SetActive(false);
        msg1.SetActive(false);
        msg2.SetActive(false);
        superhero.SetActive(false);
        video_msg_panel.SetActive(false);
        popupobject.SetActive(false);
        StopAllCoroutines();
        startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        foreach (GameObject e in zones)
        {
           e.transform.GetChild(0).gameObject.SetActive(false);
           e.GetComponent<BoxCollider2D>().enabled = true;
           e.SetActive(false);
        }
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(true);
        }
        if (Stage2unlocked)
        {
            Stage2popup.SetActive(true);
        }
        
    }


    //=======================================================latest pop up ====================================================//


    IEnumerator startNarration()
    {
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        superhero.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        textbox.SetActive(true);
        yield return new WaitForSeconds(8f);
        StartCoroutine(zone_highlighter());
    }

    //------------------need to be change========//
    IEnumerator zone_highlighter()
    {
        textbox.SetActive(false);
        skipbtn.SetActive(true);
        zone_text.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        for(int a = 0; a < zones.Count; a++)
        {
            zone_text.transform.GetChild(0).gameObject.GetComponent<Text>().text = zone_info[a];
            startpage.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
            zones[a].SetActive(true);
            zones[a].GetComponent<BoxCollider2D>().enabled = false;
            zones[a].transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(6f);
            zone_text.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
            zones[a].transform.GetChild(0).gameObject.SetActive(false);
            zones[a].GetComponent<BoxCollider2D>().enabled = true;
            zones[a].SetActive(false);

        }
        zone_text.SetActive(false);
        video_msg_panel.SetActive(true);

    }

    IEnumerator CheckForStage2()
    {
        string Response_url = MainUrl + DashbaordApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
        "&id_org_game=" + 1;//PlayerPrefs.GetInt("game_id");//

        WWW dashboard_res = new WWW(Response_url);
        yield return dashboard_res;
        if (dashboard_res.text != null)
        {
            JsonData response_data = JsonMapper.ToObject(dashboard_res.text);
            int loopcount = int.Parse(response_data[0]["ContentList"].Count.ToString());
            for (int a = 0; a < loopcount; a++)
            {
                totalscoreOfUser += int.Parse(response_data[0]["ContentList"][a]["totalscore"].ToString());
            }
        }
        if (totalscoreOfUser >= Stage2UnlockScore)
        {
            Stage2unlocked = true;
            Debug.Log("Cleared level");
                
        }
        else
        {
            Debug.Log("User score is less");
        }
    }

    public void ClosePopup()
    {
        iTween.ScaleTo(Stage2popup, Vector3.zero, 0.5f);
        startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        foreach (GameObject e in zones)
        {
            e.transform.GetChild(0).gameObject.SetActive(false);
            e.GetComponent<BoxCollider2D>().enabled = true;
            e.SetActive(false);
        }
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(true);
        }
    
    }
    
    public void PlayBonusGame()
    {
        StartCoroutine(BonusgameTask());
    }

    IEnumerator BonusgameTask()
    {
        startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(false);
        }
        iTween.ScaleTo(Stage2popup, Vector3.zero, 0.5f);
        StartCoroutine(Mainpage.scenechanges(startpage, greenBackground));
        yield return new WaitForSeconds(1.2f);
        Bonusgamepage.SetActive(true);

    }

    public void CloseBonusGame()
    {
        StartCoroutine(ClosingTask());
    }
    IEnumerator ClosingTask()
    {
        Bonusgamepage.SetActive(false);
        iTween.ScaleTo(Stage2popup, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.6f);
        Stage2popup.SetActive(false);
        StartCoroutine(Mainpage.scenechanges(startpage, CityPage));
        yield return new WaitForSeconds(1.2f);
        startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(true);
        }
        
      
    }



}
