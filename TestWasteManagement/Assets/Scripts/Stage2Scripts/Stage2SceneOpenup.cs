using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Stage2SceneOpenup : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartPage;
    private Stage2Controller Stage2controller; 
    public List<GameObject> Buttons;
    public GameObject SideBoardsZoneA,SideBoardsZoneB;
    public GameObject CanvasMainPage,LandingPage;
    private Vector3 rotateTarget1 = Vector3.zero, rotateTarget2 = Vector3.zero;
    [Space(40)]
    public string MainUrl;
    public string levelClearnessApi;
    public int ZoneNo, Stage2UnlockScore;
    public GameObject TriviaPage, GameClearedPopup;
    private int totalscoreOfUser;
    private bool Stage3unlocked;
    public bool BonusGamePLay = false;
    public GameObject MatchThetileGame;
    public GameObject GameGuidePage;
    private GameObject selectedZone;
    public List<GameObject> PopupinGameGuide;
    void Start()
    {

        BonusGamePLay = false;

    }
    private void OnEnable()
    {
        
        StartCoroutine(CheckForStage3());
    
   
    }


    IEnumerator ShowBaords()
    {
        iTween.RotateTo(SideBoardsZoneA, iTween.Hash("rotation", rotateTarget1, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.4f));
        iTween.RotateTo(SideBoardsZoneB, iTween.Hash("rotation", rotateTarget2, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.4f));
        yield return new WaitForSeconds(1);
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

        Debug.Log("user score " + totalscoreOfUser);
        Stage3unlocked = totalscoreOfUser >= Stage2UnlockScore;
        yield return new WaitForSeconds(0.5f);
        TriviaPage.SetActive(false);
        LandingPage.SetActive(true);
        if (!BonusGamePLay && Stage3unlocked)
        {
            GameClearedPopup.SetActive(Stage3unlocked);
        }
        else
        {
            CommonTask();
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ZoneSelected(GameObject Zone)
    {
        StartCoroutine(selectionTask(Zone));
    }

    IEnumerator selectionTask(GameObject Zone)
    {
        selectedZone = Zone;
        // Vector3 Target1 = new Vector3(0f, 0f, 90f);
        // Vector3 Target2 = new Vector3(0f, 0f, -90f);
        // Vector3 targetpos = new Vector3(0f, -1100f, 0f);
        //for(int a = 0; a < Buttons.Count; a++)
        // {
        //     iTween.ScaleTo(Buttons[a], Vector3.zero, 0.2f);
        //     iTween.MoveTo(Buttons[a], iTween.Hash("position", targetpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.2f));
        // }
        // yield return new WaitForSeconds(0.2f);
        // iTween.RotateTo(SideBoardsZoneA, iTween.Hash("rotation", Target1, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.2f));
        // iTween.RotateTo(SideBoardsZoneB, iTween.Hash("rotation", Target2, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.2f));
        // yield return new WaitForSeconds(0.2f);
        iTween.ScaleTo(LandingPage, Vector3.zero, 0.4f);
        CanvasMainPage.SetActive(false);
        float shadevalue = StartPage.GetComponent<Image>().color.a;
        while (shadevalue >= 0)
        {
            StartPage.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.2f);
        LandingPage.SetActive(false);
        StartPage.SetActive(false);
        GameGuidePage.SetActive(true);
     
    }


    public void CloseGameGuide()
    {
        StartCoroutine(CloseGameGuidetask());
    }

    IEnumerator CloseGameGuidetask()
    {
        PopupinGameGuide.ForEach(x =>
        {
            iTween.ScaleTo(x.gameObject, Vector3.zero, 0.2f);
            iTween.MoveTo(x.gameObject,iTween.Hash("position",Vector3.zero,"isLocal",true,"time",0.2f));
        });
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(GameGuidePage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameGuidePage.SetActive(false);
        selectedZone.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //========================== BONNUS GAME PLAY =======================
    public void PLayBonusGame()
    {
        BonusGamePLay = false;
        StartCoroutine(GameActiveTask());
    }

    IEnumerator GameActiveTask()
    {
        iTween.ScaleTo(GameClearedPopup, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameClearedPopup.SetActive(false);
        MatchThetileGame.SetActive(true);
    }

    public void CloseBonusGame()
    {
        StartCoroutine(CloseGame());
    }
    IEnumerator CloseGame()
    {
        iTween.ScaleTo(GameClearedPopup, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameClearedPopup.SetActive(false);
        CommonTask();
    }

    public void CommonTask()
    {
        CanvasMainPage.SetActive(true);
        StartCoroutine(ShowBaords());
    }


}
