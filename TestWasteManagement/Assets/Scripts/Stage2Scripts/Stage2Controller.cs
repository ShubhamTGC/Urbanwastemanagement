using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2Controller : MonoBehaviour
{
    public GameObject HomepageObject;
    public GameObject AvatarUpdatePage;
    public Sprite Stage2DirtyCity,LandingPage,CityPage;
    public GameObject OnboradingVideo, TriviaPage,SkipButton;
    public GameObject Zones;
    void Start()
    {
        StartCoroutine(sceneAppear());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator sceneAppear()
    {
        float shadevalue = HomepageObject.GetComponent<Image>().color.a;
        while (shadevalue < 1)
        {
            HomepageObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        StartCoroutine(scenechanges(HomepageObject, Stage2DirtyCity));
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(scenechanges(HomepageObject, LandingPage));
        yield return new WaitForSeconds(1.3f);
        SkipButton.SetActive(true);
        OnboradingVideo.SetActive(true);
        TriviaPage.SetActive(true);


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
        AvatarUpdatePage.SetActive(true);
    }

    public void CloseAvatarUpdate()
    {
        StartCoroutine(AavtarClose());
    }
  
    IEnumerator AavtarClose()
    {
        iTween.ScaleTo(AvatarUpdatePage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(scenechanges(HomepageObject, CityPage));
        yield return new WaitForSeconds(1.3f);
        Zones.SetActive(true);
        //HomepageObject.SetActive(false);

    }
}
