using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UpdatedDashbaord : MonoBehaviour
{
    public Text Username, GradeValue;
    public List<GameObject> Tabs;
    public Sprite PresssedSprite, RealesedSprite;
    public List<GameObject> MainPages;


    public Image OverallScoreFiller, ZoneScorezfiller, BonusScoreFiller;
    public Text OverallScore, ZoneScore, BonusScore;
    [SerializeField]
    private int Totalscore,BonusTotalscore;
    public List<StageOneDashboard> ZoneDashbaords;
    public GameObject triviapage;
    public string MainUrl, OverallDataApi;
    [Space(10)]
    public Image GameScorefiller;
    public Image GreenJournalFiller, TotalScoreFiller;
    public Text Gamescoretext, GreenJournalText, TotalScoretext;
    public Text PlayedStages, PlayedZones, PlayedBonusGames, StageText, ZoneText, BonusText;
    
    [SerializeField]
    private float TotalGameScore, TotalGreenJScore, TotalFinalScore;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(Overalldata());
        initialSetup();
    }

    void initialSetup() 
    {

        Tabs[0].GetComponent<Image>().sprite = PresssedSprite;
        MainPages[0].SetActive(true);
        for(int a = 1; a < Tabs.Count; a++)
        {
            Tabs[a].GetComponent<Image>().sprite = RealesedSprite;
            MainPages[a].SetActive(false);
        }


      

    }

    public void Tabspressed(GameObject PressedTab)
    {
        bool enabled;
        Tabs.ForEach(t =>
        {
            t.GetComponent<Image>().sprite = t.name == PressedTab.name ? PresssedSprite : RealesedSprite;
            
        });

        MainPages.ForEach(p =>
        {
            enabled = p.name == PressedTab.name;
            p.gameObject.SetActive(enabled);
        });

    }


    public void closeDashboard()
    {
        triviapage.SetActive(true);
        StartCoroutine(closeingtask());
       
       
    }

    IEnumerator closeingtask()
    {
        for(int a = 0; a < ZoneDashbaords.Count; a++)
        {
            ZoneDashbaords[a].resetTask();
            yield return new WaitForSeconds(0.5f);
        }
        triviapage.SetActive(false);
        this.gameObject.SetActive(false);
    }

    IEnumerator Overalldata()
    {
        string HittingUrl = MainUrl + OverallDataApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&id_org_game=" + 1;
        WWW response = new WWW(HittingUrl);
        yield return response;
        if(response.text != null)
        {
            OverallDashboardModel OverallModel = Newtonsoft.Json.JsonConvert.DeserializeObject<OverallDashboardModel>(response.text);
            Username.text = OverallModel.Name;
            GradeValue.text = OverallModel.Grade;
            float GameScore = int.Parse(OverallModel.TotalGameScore);
            float GreenJournal = int.Parse(OverallModel.GreenJournelScore);
            Gamescoretext.text = GameScore.ToString();
            GreenJournalText.text = GreenJournal.ToString();
            TotalScoretext.text = (GameScore + GreenJournal).ToString();
            GameScorefiller.fillAmount = GameScore / TotalGameScore;
            GreenJournalFiller.fillAmount = GreenJournal / TotalGreenJScore;
            TotalScoreFiller.fillAmount = (GameScore + GreenJournal) / TotalFinalScore;
            OverallScore.text = OverallModel.TotalAllLevelScore.ToString();
            ZoneScore.text = OverallModel.TotalZonesScore.ToString();
            BonusScore.text = OverallModel.TotalBonusScore.ToString();
            OverallScoreFiller.fillAmount = float.Parse(OverallModel.TotalAllLevelScore) / (float)Totalscore;
            ZoneScorezfiller.fillAmount = float.Parse(OverallModel.TotalZonesScore) / (float)Totalscore;
            BonusScoreFiller.fillAmount = float.Parse(OverallModel.TotalBonusScore) / (float)BonusTotalscore;
            PlayedStages.text = OverallModel.TotalStagesPlayed;
            PlayedZones.text = OverallModel.TotalZonesPlayed;
            PlayedBonusGames.text = OverallModel.TotalBonusPlayed;
            StageText.text = OverallModel.OverAllScoreText;
            ZoneText.text = OverallModel.ZoneScoreText;
            BonusText.text = OverallModel.BonusScoreText;
        }
    }

}
