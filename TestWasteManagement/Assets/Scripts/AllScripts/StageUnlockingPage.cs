using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUnlockingPage : MonoBehaviour
{
    // Start is called before the first frame update
    public string MainUrl, StageUnlockApi;
    public int Stage1Score, Stage2Score;
    public Button Stage2Btn, Stage3Btn;
    void Start()
    {
        StartCoroutine(CheckStage2Unlock());
    }

    private void OnEnable()
    {
        
    }

    IEnumerator CheckStage2Unlock()
    {
        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={1}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if(StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            Stage2Btn.interactable =int.Parse(StageModel.ConsolidatedScore) > Stage1Score;
        }
        StartCoroutine(checkstage3Unlock());
    }

    IEnumerator checkstage3Unlock()
    {
        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={2}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            Stage3Btn.interactable = int.Parse(StageModel.ConsolidatedScore) > Stage2Score;
        }
    }
}
