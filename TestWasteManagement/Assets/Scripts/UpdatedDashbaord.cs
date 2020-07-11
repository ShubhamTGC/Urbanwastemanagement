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
    public Text Gamescore,TotalGamescore;
    public Image GamescoreFiller, TotalGameFiller;
    public List<StageOneDashboard> ZoneDashbaords;
    public GameObject triviapage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        initialSetup();
    }

    void initialSetup() 
    {
        Username.text = PlayerPrefs.GetString("username");
        GradeValue.text = PlayerPrefs.GetString("User_grade");
        Tabs[0].GetComponent<Image>().sprite = PresssedSprite;
        MainPages[0].SetActive(true);
        for(int a = 1; a < Tabs.Count; a++)
        {
            Tabs[a].GetComponent<Image>().sprite = RealesedSprite;
            MainPages[a].SetActive(false);
        }


        OverallScore.text = PlayerPrefs.GetInt("ZoneScore").ToString();
        Gamescore.text = PlayerPrefs.GetInt("ZoneScore").ToString();
        TotalGamescore.text = PlayerPrefs.GetInt("ZoneScore").ToString();
        ZoneScore.text = PlayerPrefs.GetInt("ZoneScore").ToString();
        BonusScore.text = PlayerPrefs.GetInt("BonusScore").ToString();

        OverallScoreFiller.fillAmount =(float)PlayerPrefs.GetInt("ZoneScore") / (float)Totalscore;
        GamescoreFiller.fillAmount =(float)PlayerPrefs.GetInt("ZoneScore") / (float)Totalscore;
        TotalGameFiller.fillAmount =(float)PlayerPrefs.GetInt("ZoneScore") / (float)Totalscore;
        ZoneScorezfiller.fillAmount =(float)PlayerPrefs.GetInt("ZoneScore") / (float)Totalscore;
        BonusScoreFiller.fillAmount =(float)PlayerPrefs.GetInt("BonusScore") / (float)BonusTotalscore;

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

}
