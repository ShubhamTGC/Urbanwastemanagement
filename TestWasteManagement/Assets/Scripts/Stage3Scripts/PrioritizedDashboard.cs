using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using System.Linq;
using UnityEngine.UI;

public class PrioritizedDashboard : MonoBehaviour
{
    public Transform TableParent;
    public GameObject dataPrefeb;
    public SimpleSQLManager dbmanager;
    public Sprite CorrectAns, WrongAns, CorrectOpt;
    private List<string> Truckname = new List<string>();
    private List<int> is_correct = new List<int>();
    private List<int> truckScore = new List<int>();
    private List<string> Correctseq = new List<string>();
    private List<GameObject> dataRows = new List<GameObject>();
    public List<string> tableSequence;
    public Text OverallScore;

    [Header("API INTERGRATION PART")]
    public string MainUrl;
    public string GetPriorityLogApi;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(dataRows.Count == 0 )
        {
           // StartCoroutine(gettabeldata());
            StartCoroutine(getPriorityData());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator gettabeldata()
    {
        yield return new WaitForSeconds(0.1f);
        var Gamedata = dbmanager.Table<Prioritization>().ToList();
        int a = 0;
        Gamedata.ForEach(x =>
        {
            Truckname.Add(x.Truckname);
            is_correct.Add(x.Is_correct);
            truckScore.Add(x.Truckscore);
            Correctseq.Add(x.CorrectTruck);
            a++;
        });

        for(int b = 0; b < Truckname.Count + 1; b++)
        {
            GameObject gb = Instantiate(dataPrefeb, TableParent, false);
            dataRows.Add(gb);
        }

        GeneratedashBoard();
    }

    void GeneratedashBoard()
    {
        int allScore = 0;
        for(int a = 0; a < dataRows.Count; a++)
        {
            if (a == 0)
            {
                for(int b = 0; b < dataRows[a].transform.childCount; b++)
                {
                    dataRows[a].transform.GetChild(b).GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
                }
            }
         
        }

        for (int b = 0; b < Correctseq.Count; b++)
        {
            allScore += truckScore[b];
            OverallScore.text = allScore.ToString();
            if (is_correct[b] == 1)
            {
               
                for (int c = 0; c < tableSequence.Count; c++)
                {
                    if (Truckname[b] == tableSequence[c])
                    {
                        dataRows[b + 1].transform.GetChild(c).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        dataRows[b + 1].transform.GetChild(c).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectAns;
                        dataRows[b + 1].transform.GetChild(4).gameObject.GetComponent<Text>().text = truckScore[b].ToString();
                      
                    }
                }
            }
            else
            {

                int indexWrong = tableSequence.FindIndex(x => x == Truckname[b]);
                dataRows[b + 1].transform.GetChild(indexWrong).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                dataRows[b + 1].transform.GetChild(indexWrong).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                dataRows[b + 1].transform.GetChild(indexWrong).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = Correctseq[b];
                dataRows[b + 1].transform.GetChild(indexWrong).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongAns;
                dataRows[b + 1].transform.GetChild(4).gameObject.GetComponent<Text>().text = truckScore[b].ToString();
                int correctIndex = tableSequence.FindIndex(x => x == Correctseq[b]);
                dataRows[b + 1].transform.GetChild(correctIndex).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                dataRows[b + 1].transform.GetChild(correctIndex).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
           
            }
      

        }
    }


    IEnumerator getPriorityData()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrl}{GetPriorityLogApi}?UID={PlayerPrefs.GetInt("UID")}";
        WWW GetGamedata = new WWW(HittingUrl);
        yield return GetGamedata;
        if (GetGamedata.text != null)
        {
            if (GetGamedata.text != "[]")
            {
                List<TruckLogModel> LogModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TruckLogModel>>(GetGamedata.text);
                var MaxAttemptNo = LogModel.Max(x => x.attempt_no);
                LogModel.ForEach(x =>
                {
                    if(x.attempt_no == MaxAttemptNo)
                    {
                        Truckname.Add(x.truck_selected);
                        is_correct.Add(x.is_correct);
                        truckScore.Add(x.score);
                        Correctseq.Add(x.correct_truck);
                    }
               
                });
                for (int b = 0; b < Truckname.Count + 1; b++)
                {
                    GameObject gb = Instantiate(dataPrefeb, TableParent, false);
                    dataRows.Add(gb);
                }

                GeneratedashBoard();
            }
        }
    }



    public void Resettask()
    {
        for (int a = 0; a < TableParent.transform.childCount; a++)
        {
            Destroy(TableParent.transform.GetChild(a).gameObject, 0.05f);
        }
        dataRows.Clear();
        Truckname.Clear();
        is_correct.Clear();
        truckScore.Clear();
        Correctseq.Clear();

    }

 
}
