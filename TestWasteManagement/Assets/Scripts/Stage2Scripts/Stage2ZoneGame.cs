using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Stage2ZoneGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainZonePage,ZonePage,LevelPage;
    public GameObject Homepage,TriviaPage;
    public GameObject ChildZoneA, ChildZoneB;
    public GameObject ZoneA, ZoneB;
    [SerializeField]
    private Color RelasedEffect;
    public Sprite levelpagesprite,CityZoneSprite;
    public Stage2Controller stage2controller;
    private GameObject selectedobj;

    //====Stage 3 unlocking status =====
    public string MainUrl, levelClearnessApi, GetGameContentIDs;
    [SerializeField]
    private int Stage2UnlockScore = 630;
    public int ZoneNo;
    private int totalscoreOfUser;
    private bool Stage3unlocked = false;
    public GameObject GameClearedPopup;
    [SerializeField]
    private int Gamelevel;
    public List<int> id_game_content = new List<int>();

    //============= BONUS GAME POP UPS========================
    public GameObject MatchTheTile;
    void Start()
    {
        Debug.Log("checking");
        StartCoroutine(CheckForStage3());
        StartCoroutine(GetGameID());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

     void OnEnable()
    {
       
    }


    public void buttonmsg(GameObject Zone)
    {
        this.gameObject.GetComponent<Image>().color = RelasedEffect;
        ChildZoneA.SetActive(false);
        ChildZoneB.SetActive(false);
        Zone.SetActive(true);
        MainZonePage.SetActive(false);
    }

    public void Selectzone(GameObject SelectZonepage)
    {
        StartCoroutine(Zoneselected(SelectZonepage));
    }


    IEnumerator Zoneselected(GameObject SelectZonepage)
    {
        selectedobj = SelectZonepage;
        ZonePage.SetActive(false);
        this.gameObject.GetComponent<Image>().color = RelasedEffect;
        ChildZoneA.SetActive(false);
        ChildZoneB.SetActive(false);
        StartCoroutine(stage2controller.scenechanges(Homepage, levelpagesprite));
        yield return new WaitForSeconds(1.2f);
        LevelPage.SetActive(true);
        SelectZonepage.SetActive(true);
    }

    public void BackfromLevel()
    {
        StartCoroutine(Backtask());
     
    }

    IEnumerator Backtask()
    {
        selectedobj.SetActive(false);
        StartCoroutine(stage2controller.scenechanges(Homepage, CityZoneSprite));
        yield return new WaitForSeconds(1.2f);
        LevelPage.SetActive(false);
        ZonePage.SetActive(true);

    }

    public void ZoneALevelSelection(GameObject SelectdLevel)
    {
        StartCoroutine(ZoneSelectionTask(SelectdLevel));
     
    }

    IEnumerator ZoneSelectionTask(GameObject SelectdLevel)
    {
        StartCoroutine(fadeEffect());
        LevelPage.SetActive(false);
        selectedobj.SetActive(false);
        ChildZoneA.SetActive(false);
        ChildZoneB.SetActive(false);
        yield return new WaitForSeconds(1f);
        Homepage.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Homepage.GetComponent<Image>().sprite = CityZoneSprite;
        SelectdLevel.SetActive(true);
       
    }


    IEnumerator fadeEffect()
    {
        float shadevalue = Homepage.GetComponent<Image>().color.a;
        while (shadevalue >= 0)
        {
            Homepage.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator CheckForStage3()
    {
        TriviaPage.SetActive(true);
        string Response_url = MainUrl + levelClearnessApi + "?id_org_game=" + ZoneNo;
        WWW dashboard_res = new WWW(Response_url);
        yield return dashboard_res;
        if (dashboard_res.text != null)
        {
            List<LevelMovement> response_data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LevelMovement>>(dashboard_res.text);
            totalscoreOfUser = response_data.FirstOrDefault(x => x.id_level == ZoneNo)?.completion_score ?? 0;
        }

        Stage3unlocked = totalscoreOfUser <= Stage2UnlockScore;
        yield return new WaitForSeconds(0.5f);
        TriviaPage.SetActive(false);
        ZoneA.GetComponent<PolygonCollider2D>().enabled = false;
        ZoneB.GetComponent<PolygonCollider2D>().enabled = false;
        GameClearedPopup.SetActive(Stage3unlocked);
    }

    IEnumerator GetGameID()
    {
        string HittingUrl = MainUrl + GetGameContentIDs + "?UID=" +PlayerPrefs.GetInt("UID") + "&OID=" +PlayerPrefs.GetInt("OID") +
            "&id_org_game=" + 1 + "&id_level=" + Gamelevel;

        WWW GameIDs_www = new WWW(HittingUrl);
        yield return GameIDs_www;
        if(GameIDs_www.text != null)
        {
            GetLevelIDs Gameid = Newtonsoft.Json.JsonConvert.DeserializeObject<GetLevelIDs>(GameIDs_www.text);
            Gameid.content.ForEach(x =>
            {
                id_game_content.Add(x.id_game_content);
            });
        }
    }

    public void PLayBonusGame()
    {
        StartCoroutine(GameActiveTask());
    }

    public void closeBonusGAme()
    {
        StartCoroutine(CloseGame());
    }

    IEnumerator GameActiveTask()
    {
        ZoneA.GetComponent<PolygonCollider2D>().enabled = false;
        ZoneB.GetComponent<PolygonCollider2D>().enabled = false;
        iTween.ScaleTo(GameClearedPopup, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameClearedPopup.SetActive(false);
        MatchTheTile.SetActive(true);
    }

    IEnumerator CloseGame()
    {
        iTween.ScaleTo(GameClearedPopup, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameClearedPopup.SetActive(false);
        ZoneA.GetComponent<PolygonCollider2D>().enabled = true;
        ZoneB.GetComponent<PolygonCollider2D>().enabled = true;
    }

}
