using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage3PageHandler : MonoBehaviour
{
    public GameObject Mainpage;
    public Text ShowMsgpanel;
    public GameObject MsgPanel, GamemanagerObj, ValidPointsObj,landingPage,truckselectionPage,levelCardPage,SelectionPage;
    public GameBoard Gamemanagerdata;
    public GameObject PriorityBtn, alignBtn;
    public GameObject AllPoints, gameManagerPart;
    public GameObject gameGuidePage;
    public Stage3handler Stage3MainPage;
    public GameObject ClearStage3, FinalAssessmentpage;
    public GameObject Backbtn;
    private int counter = 0;
    [SerializeField] private float clicktime;
    void Start()
    {
       
    }
    private void OnEnable()
    {
    
        if (Stage3MainPage.BonusGameBool)
        {
            ClearStage3.SetActive(true);
        }
        else
        {
            levelCardPage.SetActive(true);
        }
        Backbtn.SetActive(true);
        GamemanagerObj.SetActive(true);
        ValidPointsObj.SetActive(true);
     
    }


    void Update()
    {
        
    }


    public void AlignTruck()
    {
        counter++;
        if (counter == 1)
        {
            StartCoroutine(AlignDoubleclick());
        }
      
    }
    IEnumerator AlignDoubleclick()
    {
        yield return new WaitForSeconds(clicktime);
        if (counter > 1)
        {
            PriorityBtn.GetComponent<BoxCollider2D>().enabled = alignBtn.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(AlignButtonTask());
        }
        yield return new WaitForSeconds(0.05f);
        counter = 0;
    }

    IEnumerator AlignButtonTask()
    {
       
        if(Gamemanagerdata.StationaryTrucks.Count == 0)
        {
            ShowMsgpanel.text = "Please select the priority of the trucks first !";
        }
        else
        {
            Debug.Log(" play game!!!");
        }
        MsgPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        iTween.ScaleTo(MsgPanel, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.6f);
        ShowMsgpanel.text = "";
        MsgPanel.SetActive(false);
        PriorityBtn.GetComponent<BoxCollider2D>().enabled = alignBtn.GetComponent<BoxCollider2D>().enabled = true;

    }

    public void ShowGameGuide()
    {
        counter++;
        if (counter == 1)
        {
            StartCoroutine(GetDoubleclick());
        }
    }

    IEnumerator GetDoubleclick()
    {
        yield return new WaitForSeconds(clicktime);
        if (counter > 1)
        {
            StartCoroutine(shwogaemguideTask());
        }
        yield return new WaitForSeconds(0.05f);
        counter = 0;
    }

    IEnumerator shwogaemguideTask()
    {
        iTween.ScaleTo(landingPage, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.6f);
       
        //SelectionPage.SetActive(false);
        levelCardPage.SetActive(false);
        yield return new WaitForSeconds(0f);
        Backbtn.SetActive(false);
        gameGuidePage.SetActive(true);

    }
    public void TruckPriorityTask()
    {
        StartCoroutine(PrioritizedTask());
    }

    IEnumerator PrioritizedTask()
    {
        iTween.ScaleTo(gameGuidePage, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.6f);
        gameGuidePage.SetActive(false);
        truckselectionPage.SetActive(true);
        landingPage.SetActive(false);
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

    public void PlayBonusGame()
    {
        StartCoroutine(PlaypopUp());
    }

    IEnumerator PlaypopUp()
    {
        iTween.ScaleTo(ClearStage3, Vector3.zero, 0.3f);
        iTween.ScaleTo(landingPage, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        ClearStage3.SetActive(false);
        FinalAssessmentpage.SetActive(true);
        landingPage.SetActive(false);
    }

    public void Setnormal()
    {
        Mainpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

}
