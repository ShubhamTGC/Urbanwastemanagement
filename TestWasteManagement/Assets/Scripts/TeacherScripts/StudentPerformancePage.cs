using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StudentPerformancePage : MonoBehaviour
{
    public string MainUrl, PerformanceApi;
    private List<GameObject> rows = new List<GameObject> ();
    public GameObject RowPrefeb;
    public Transform RowHandler;

    void Start()
    {
        
    }


    private void OnEnable()
    {
        if(rows.Count == 0)
        {
            StartCoroutine(GetPerformanceLog());
        }
    }
    IEnumerator GetPerformanceLog()
    {
        string HittingUrl = $"{MainUrl}{PerformanceApi}?id_user={PlayerPrefs.GetInt("UID")}&Grade={1}";
        WWW request = new WWW(HittingUrl);
        yield return request;
        if(request.text != null)
        {
            if(request.text != "[]")
            {
                Debug.Log("log " + request.text);
                List<StudentPerformanceModel> studentlog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StudentPerformanceModel>>(request.text);
                studentlog.ForEach(x =>
                {
                    GameObject gb = Instantiate(RowPrefeb, RowHandler, false);
                    rows.Add(gb);
                    gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.RollNo != null ? x.RollNo.ToString() :"0";
                    gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = x.Name;
                    gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = x.Level.ToString();
                    gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = x.Rank.ToString();
                    gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = x.Points.ToString();
                });
            }
        }
    }

    public void BackToMainpage()
    {
        rows.Clear();
        int count = RowHandler.transform.childCount;
        for(int a = 0; a < count; a++)
        {
            Destroy(RowHandler.transform.GetChild(a).gameObject);
        }
    }
}
