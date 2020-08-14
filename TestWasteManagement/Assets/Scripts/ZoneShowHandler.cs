﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Linq;

public class ZoneShowHandler : MonoBehaviour
{
    [SerializeField]
    private float InfoTime;
    [SerializeField]
    private float time = 0.4f;
    [SerializeField]
    private List<GameObject> Zones = new List<GameObject>();
    private int Zonenum = 0;
    public GameObject ShadowPanel;
    public GameObject ZoneMSg;
    [SerializeField]
    private float ZonemsgTime =3f;
    public Text ZoneInfo;
    public GameObject ZoneButtons;
    public GameObject Videoplaypanel,skipBtn;
    public GameObject TriviaPage;
    private bool VideoPlayed, CheckForEnd;
    public GameObject skipbutton, backbtn, YoutubePlayer;
    public GameObject Stage2popup;
    public GameObject Headingbar, ZoneselectionBar;


    //==================== API's======================
    public string MainUrl, levelClearnessApi,WMSBadgeLogUserApi,PostBadgeUserApi,GetBadgeConfigApi;
    public int ZoneNo;
    private int totalscoreOfUser;
    bool Stage2unlocked;
    public int Stage2UnlockScore;


    //=============== BONUS GAME PAGE ELEMENTS ===================
    [Header("BONUS GAME PAGE========")]
    [Space(10)]
    public Sprite GreenBackground;
    public Sprite CityPage;
    public Generationlevel Mainpage;
    public GameObject startpage, Bonusgamepage;
    public GameObject StageWiseLeaderBOard;

    public Zonehandler Home, School, Park, Hospital, Industry, Office;
    private bool is_skipped;
    public bool AnagramPlayed = false;
    void Start()
    {

        AnagramPlayed = false;
    }
    private void OnEnable()
    {
   
        StartCoroutine(CheckForStage2());
        Zonenum = 0;
        is_skipped = false;

    }

    void OffInitialPops()
    {
        if (Home.zone_completed || School.zone_completed || Hospital.zone_completed || Office.zone_completed
          || Industry.zone_completed || Park.zone_completed)
        {
            if (Home.final_completed && School.final_completed && Hospital.final_completed && Office.final_completed
                && Industry.final_completed && Park.final_completed)
            {
                
            }
            else
            {
                SkippedTask();
            }

        }
        else
        {
            StartCoroutine(Intromsgs());
        }
    }


    void Update()
    {
        if (VideoPlayed)
        {
            if (YoutubePlayer.GetComponent<VideoPlayer>().isPlaying)
            {
                CheckForEnd = true;
            }
            if (CheckForEnd)
            {
                if (!YoutubePlayer.GetComponent<VideoPlayer>().isPlaying)
                {
                    Debug.Log("done video");
                    CheckForEnd = false;
                    VideoPlayed = false;
                    close_video();
                }
            }
        }
    }

    IEnumerator Intromsgs()
    {
        ZoneMSg.SetActive(true);
        yield return new WaitForSeconds(ZonemsgTime);
        iTween.ScaleTo(ZoneMSg, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ZoneMSg.SetActive(false);
        ShadowPanel.transform.SetSiblingIndex(Zonenum);
        StartCoroutine(zonePopups());
    }

    IEnumerator zonePopups()
    {

        skipBtn.SetActive(true);
        if (Zonenum < Zones.Count)
        {
          
            ShadowPanel.SetActive(true);
            Zones[Zonenum].SetActive(true);
            Zones[Zonenum].GetComponent<ShowCardEffect>().enabled = true;
            Zones[Zonenum].GetComponent<ShowCardEffect>().ShowEffect();
            yield return new WaitForSeconds(time);
            Zones[Zonenum].transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(InfoTime);
            Zones[Zonenum].transform.GetChild(0).gameObject.SetActive(false);
            Zonenum++;
            ShadowPanel.transform.SetSiblingIndex(Zonenum+1);
            if (!is_skipped)
            {
                StartCoroutine(zonePopups());
            }
            else
            {
                SkippedTask();
            }
            
        }
        else
        {
            ShadowPanel.SetActive(false);
            //SetupZones();
            Zonenum = 0;
            skipBtn.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(VideoplayPortion());
        }
    }

    IEnumerator VideoplayPortion()
    {
        ZoneButtons.transform.localScale = Vector3.zero;
        Videoplaypanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }

   

    public void PlayYoutubeVideo()
    {
        iTween.ScaleTo(Videoplaypanel, Vector3.zero, 0.2f);
        Camera.main.GetComponent<AudioSource>().enabled = false;
        skipBtn.SetActive(false);
        //videplayer.SetActive(true);
        TriviaPage.SetActive(true);
        VideoPlayed = true;
        YoutubePlayer.SetActive(true);
        //skipbutton.SetActive(true);
        backbtn.SetActive(false);
        Videoplaypanel.SetActive(false);
    }

    public void close_video()
    {
        VideoPlayed = false;
        StartCoroutine(closevideo_action());
    }

    IEnumerator closevideo_action()
    {
        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponent<AudioSource>().enabled = true;
        backbtn.SetActive(true);
        YoutubePlayer.SetActive(false);
        skipBtn.SetActive(false);
        TriviaPage.SetActive(false);
        skipbutton.SetActive(false);
        ZoneButtons.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(1f);
        ObjectCommonTask(true);
    }

    public void SkipVideo()
    {
        is_skipped = true;
        skipBtn.SetActive(false);
    }

    void SkippedTask()
    {
        StopCoroutine("zonePopups");
        StartCoroutine(VideoSkipTask());
        ObjectCommonTask(false);
        if (!AnagramPlayed && Stage2unlocked)
        {
            Stage2popup.SetActive(Stage2unlocked);
        }
      
       
    }
    IEnumerator VideoSkipTask()
    {
        Videoplaypanel.SetActive(false);
        ShadowPanel.SetActive(false);
        Videoplaypanel.transform.localScale = Vector3.zero;
        Videoplaypanel.SetActive(false);
        ZoneButtons.transform.localScale = Vector3.one;
        Zones.ForEach(x =>
        {
            x.GetComponent<ShowCardEffect>().StopCoroutine("ShowCard");
            x.GetComponent<ShowCardEffect>().enabled = false;
            x.SetActive(true);
            x.transform.GetChild(0).gameObject.SetActive(false);
            Vector3 pos = x.GetComponent<ShowCardEffect>().Targetpos;
            x.transform.localPosition = pos;
            x.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        });
        Videoplaypanel.transform.localScale = Vector3.zero;
        Videoplaypanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        StopCoroutine("zonePopups");
        ObjectCommonTask(true);
        StopAllCoroutines();

    }


    IEnumerator CheckForStage2()
    {
        TriviaPage.SetActive(true);
        string Response_url = MainUrl + levelClearnessApi + "?id_org_game=" + ZoneNo;
        WWW dashboard_res = new WWW(Response_url);
        yield return dashboard_res;
        if (dashboard_res.text != null)
        {
            Debug.Log(" log data  " + dashboard_res.text);
            List<LevelMovement> response_data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LevelMovement>>(dashboard_res.text);
            totalscoreOfUser = response_data.FirstOrDefault(x => x.id_level == ZoneNo)?.completion_score ?? 0;
        }
        Debug.Log(" user score  " + totalscoreOfUser);
        Stage2unlocked = totalscoreOfUser >= Stage2UnlockScore;
      
        TriviaPage.SetActive(false);
        StartCoroutine(PostBadgedata());
        StartCoroutine(getBadgeConfiguration());
        OffInitialPops();

    }


    public void PlayBonusGame()
    {
        StartCoroutine(BonusgameTask());
    }

    IEnumerator BonusgameTask()
    {
        ZoneInfo.text = "";
        ObjectCommonTask(false);
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(Stage2popup, Vector3.zero, 0.5f);
        ZoneselectionBar.SetActive(false);
        Headingbar.SetActive(false);
        ZoneButtons.transform.localScale = Vector3.zero;
        StartCoroutine(Mainpage.scenechanges(startpage, GreenBackground));
        yield return new WaitForSeconds(1.2f);
        Bonusgamepage.SetActive(true);

    }

    public void ClosePopup()
    {
        iTween.ScaleTo(Stage2popup, Vector3.zero, 0.4f);
        ObjectCommonTask(true);

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
        StageWiseLeaderBOard.SetActive(true);

    }

    public void CloseStageDashboard()
    {
        StageWiseLeaderBOard.SetActive(false);
        ZoneselectionBar.SetActive(true);
        Headingbar.SetActive(true);
        ZoneButtons.transform.localScale = Vector3.one;
        ObjectCommonTask(true);

    }

    void ObjectCommonTask(bool active)
    {
        Zones.ForEach(x =>
        {
            x.GetComponent<Button>().enabled = active;
            x.GetComponent<BoxCollider2D>().enabled = active;
            x.GetComponent<Animator>().enabled = active;
        });
    }

    IEnumerator PostBadgedata()
    {
        string HittinhUrl = MainUrl + WMSBadgeLogUserApi + "?id_user=" + PlayerPrefs.GetInt("UID");
        WWW BadgeLog = new WWW(HittinhUrl);
        yield return BadgeLog;
        if(BadgeLog.text != null)
        {
            Debug.Log("Badge log " + BadgeLog.text);
        }

    }

    IEnumerator getBadgeConfiguration()
    {
        string HittingUrl = MainUrl + GetBadgeConfigApi + "?id_level=" + 1;
        WWW badge_www = new WWW(HittingUrl);
        yield return badge_www;
        if(badge_www.text != null)
        {
            Debug.Log(" badge infp " + badge_www.text);
        }
    }

    IEnumerator PostBadgeOfStage()
    {
        string HittingUrl = MainUrl + PostBadgeUserApi;
        var BadgeModel = new PostBadgeModel();
        WWWForm PostBadgeLog = new WWWForm();
        PostBadgeLog.AddField(BadgeModel.id_user, PlayerPrefs.GetInt("UID").ToString());
        PostBadgeLog.AddField(BadgeModel.id_level,"1");
        PostBadgeLog.AddField(BadgeModel.id_zone,"0");
        PostBadgeLog.AddField(BadgeModel.id_room,"0");
        PostBadgeLog.AddField(BadgeModel.id_special_game,"0");
        PostBadgeLog.AddField(BadgeModel.id_badge,"1");

        WWW Post_www = new WWW(HittingUrl, PostBadgeLog);
        yield return Post_www;

    }

}
