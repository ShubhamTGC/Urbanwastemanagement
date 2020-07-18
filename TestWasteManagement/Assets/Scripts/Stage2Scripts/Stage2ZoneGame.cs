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
    public GameObject Homepage;
    public GameObject ChildZoneA, ChildZoneB;
    [SerializeField]
    private Color RelasedEffect;
    public Sprite levelpagesprite,CityZoneSprite;
    public Stage2Controller stage2controller;
    private GameObject selectedobj;
        
    //====Stage 3 unlocking status =====
    public string MainUrl, levelClearnessApi;
    [SerializeField]
    private int Stage2UnlockScore = 630;
    public int ZoneNo;
    private int totalscoreOfUser;
    private bool Stage3unlocked;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(CheckForStage3());
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
        yield return new WaitForSeconds(0.6f);
        Homepage.GetComponent<Image>().sprite = CityZoneSprite;
        SelectdLevel.SetActive(true);
       
    }


    IEnumerator fadeEffect()
    {
        float shadevalue = Homepage.GetComponent<Image>().color.a;
        while (shadevalue > 0)
        {
            Homepage.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator CheckForStage3()
    {

        string Response_url = MainUrl + levelClearnessApi + "?id_org_game=" + ZoneNo;

        WWW dashboard_res = new WWW(Response_url);
        yield return dashboard_res;
        if (dashboard_res.text != null)
        {
            List<LevelMovement> response_data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LevelMovement>>(dashboard_res.text);
            totalscoreOfUser = response_data.FirstOrDefault(x => x.id_level == ZoneNo)?.completion_score ?? 0;
        }

        Stage3unlocked = totalscoreOfUser >= Stage2UnlockScore;
        Debug.Log("stage 3 " + Stage3unlocked.ToString());
    }


}
