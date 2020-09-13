using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Stage2Controller : MonoBehaviour
{
    public GameObject HomepageObject;
    public GameObject AvatarUpdatePage, stars;
    public Sprite Stage2DirtyCity,LandingPage,CityPage;
    public GameObject OnboradingVideo, TriviaPage,SkipButton;
    public GameObject Zones;
    public Text USername;
    private bool videoPlayed, checkforEnd;
    void Start()
    {
        StartCoroutine(sceneAppear());
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
        OnboradingVideo.SetActive(false);
        TriviaPage.SetActive(false);
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

    public void BacktoHome()
    {
        SceneManager.LoadScene(0);
    }
}
