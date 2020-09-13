using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Stage3handler : MonoBehaviour
{
    public GameObject TriviaPage, OnbordingVideoPage, SkipButton, AvatarPage, Stage3LandingPage, Gamemanager, GamePoints;
    private bool videoPlayed, checkforEnd;

    [Header("API INTEGRATION PART")]
    public string MainUrl;
    public string StageUnlockApi;
    public int Gamelevel;
    public bool BonusGameBool;
    public int GameScore;


    public GameObject FinalMsgPage, AssessmentCanvas;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        OnbordingVideoPage.SetActive(true);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = false;
        TriviaPage.SetActive(true);
        videoPlayed = true;
        StartCoroutine(GetStageScore());
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayed)
        {
            if (OnbordingVideoPage.GetComponent<VideoPlayer>().isPlaying)
            {
                SkipButton.SetActive(true);
                checkforEnd = true;
            }
            if (checkforEnd)
            {
                if (!OnbordingVideoPage.GetComponent<VideoPlayer>().isPlaying)
                {
                    videoPlayed = false;
                    checkforEnd = false;
                    SkipVideo();
                }
            }
        }
    }
    public void SkipVideo()
    {
        SkipButton.SetActive(false);
        TriviaPage.SetActive(false);
        OnbordingVideoPage.SetActive(false);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = true;
        videoPlayed = checkforEnd = false;
        int avatar_data = PlayerPrefs.GetInt("characterType");
        //AvatarPage.SetActive(true);
        if (avatar_data > 4)
        {
            StartCoroutine(ClosingTask());
        }
        else
        {
            AvatarPage.SetActive(true);
        }


    }


    public void BacktoHome()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseAvatarPage()
    {
        StartCoroutine(ClosingTask());
    }

    IEnumerator ClosingTask()
    {
        iTween.ScaleTo(AvatarPage, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Stage3LandingPage.SetActive(true);

    }


    IEnumerator GetStageScore()
    {
        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={Gamelevel}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            BonusGameBool = int.Parse(StageModel.ConsolidatedScore) > GameScore;
        }
    }


    public void FinalPAgeClose()
    {
        StartCoroutine(ClosePage());
    }
    IEnumerator ClosePage()
    {
        iTween.ScaleTo(FinalMsgPage, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        FinalMsgPage.SetActive(false);
        AssessmentCanvas.SetActive(false);
        Stage3LandingPage.SetActive(true);
    }

}
