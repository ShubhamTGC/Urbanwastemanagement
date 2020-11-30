using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using SimpleSQL;
using System.Linq;

public class Stage2Controller : MonoBehaviour
{
    public GameObject HomepageObject;
    public GameObject AvatarUpdatePage, stars;
    public Sprite Stage2DirtyCity,LandingPage,CityPage;
    public GameObject OnboradingVideo, TriviaPage,SkipButton;
    public GameObject Zones;
    public Text USername;
    private bool videoPlayed, checkforEnd;
    public string MainUrl, GetCmsConfigApi;
    public SimpleSQLManager dbmanager;
    public Stage2ZoneGame Stage2Parent;
    public List<Stage2ZoneHandler> stage2zones;
    public YoutubePlayer.YoutubePlayer YoutubeVideoPage;
    public string StageUnlockApi;
    public bool Stage3unlocked;
    public int scoretest;

    void Start()
    {
        StartCoroutine(getCmsdata());
        StartCoroutine(GetYoutubeLink());
    }
    private void OnEnable()
    {
        StartCoroutine(sceneAppear());
    }

    IEnumerator GetYoutubeLink()
    {
        yield return new WaitForSeconds(0.1f);
        var LocalLog = dbmanager.Table<VideoUrls>().FirstOrDefault(x => x.LevelId == 2);
        if (LocalLog != null)
        {
            YoutubeVideoPage.youtubeUrl = LocalLog.VideoLink;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (videoPlayed)
        {
            if (OnboradingVideo.GetComponent<VideoPlayer>().isPlaying)
            {
                SkipButton.SetActive(true);
                checkforEnd = true;
            }
            if (checkforEnd)
            {
                if (!OnboradingVideo.GetComponent<VideoPlayer>().isPlaying)
                {
                    videoPlayed = false;
                    checkforEnd = false;
                    SkipVideo();
                }
            }
        }
    }
    
    IEnumerator sceneAppear()
    {
        float shadevalue = HomepageObject.GetComponent<Image>().color.a;
        while (shadevalue < 1)
        {
            HomepageObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        OnboradingVideo.SetActive(true);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = false;
        TriviaPage.SetActive(true);
        videoPlayed = true;


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

    public void SkipVideo()
    {
        SkipButton.SetActive(false);
        videoPlayed = false;
        OnboradingVideo.SetActive(false);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = true;
        if (PlayerPrefs.GetInt("PlayerBody") < 5)
        {
            USername.text = PlayerPrefs.GetString("username");
            AvatarUpdatePage.SetActive(true);
        }
        else
        {
            Zones.SetActive(true);
        }
        
    }

    public void CloseAvatarUpdate()
    {
        StartCoroutine(AavtarClose());
    }
  
    IEnumerator AavtarClose()
    {
        iTween.ScaleTo(AvatarUpdatePage, Vector3.zero, 0.4f);
        iTween.ScaleTo(stars, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.5f);
        Zones.SetActive(true);

    }

    IEnumerator CheckForStage3(int score)
    {

        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={2}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            Stage3unlocked = int.Parse(StageModel.ConsolidatedScore) >= score;
        }
        Stage2Parent.StageClearChecked = Stage3unlocked;
       

    }

    public void BacktoHome()
    {
        SceneManager.LoadScene(0);
    }


    IEnumerator getCmsdata()
    {
        string Hitting = $"{MainUrl}{GetCmsConfigApi}?id_level={2}";
        WWW Cmswww = new WWW(Hitting);
        yield return Cmswww;
        int Totalscore = 0;
        if(Cmswww.text != null)
        {
            if(Cmswww.text != "[]")
            {
                List<Stage1CMSModel> StageCmsLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Stage1CMSModel>>(Cmswww.text);
                StageCmsLog.ForEach(x =>
                {
                    var LocalCmsLog = dbmanager.Table<WasteSeperation>().FirstOrDefault(y => y.ItemId == x.item_Id);
                    if (LocalCmsLog == null)
                    {
                        WasteSeperation WasteLog = new WasteSeperation
                        {
                            ItemId = x.item_Id,
                            ItemName = x.item_Name,
                            PCscore = x.partialcorrect_point,
                            Cscore = x.correct_point,
                            RoomId = x.id_room
                        };
                        dbmanager.Insert(WasteLog);
                    }
                    else
                    {
                        LocalCmsLog.ItemId = x.item_Id;
                        LocalCmsLog.ItemName = x.item_Name;
                        LocalCmsLog.PCscore = x.partialcorrect_point;
                        LocalCmsLog.Cscore = x.correct_point;
                        LocalCmsLog.RoomId = x.id_room;
                        dbmanager.UpdateTable(LocalCmsLog);
                    }

                    Totalscore += x.correct_point;
                });

                var percentlog = dbmanager.Table<LevelPercentageTable>().FirstOrDefault(c => c.LevelId == 2).LevelPercentage;
                int FinalLevelScore = (Totalscore / 100) * percentlog;
                stage2zones.ForEach(x =>
                {
                    x.Stage2UnlockScore = FinalLevelScore;
                });
                Stage2Parent.Stage2UnlockScore = FinalLevelScore;
                StartCoroutine(CheckForStage3(FinalLevelScore));
                var scoreconfig = dbmanager.Table<ScoreConfiguration>().FirstOrDefault(a => a.levelId == 2);

                if (scoreconfig == null)
                {
                    ScoreConfiguration log = new ScoreConfiguration
                    {
                        levelId = 2,
                        TotalScore = Totalscore,
                        PercentScore = percentlog,
                        UnlockScore = FinalLevelScore
                    };
                    dbmanager.Insert(log);
                }
                else
                {
                    scoreconfig.PercentScore = percentlog;
                    scoreconfig.levelId = 2;
                    scoreconfig.TotalScore = Totalscore;
                    scoreconfig.UnlockScore = FinalLevelScore;
                    dbmanager.UpdateTable(scoreconfig);
                }

            }
        }
    }
}
