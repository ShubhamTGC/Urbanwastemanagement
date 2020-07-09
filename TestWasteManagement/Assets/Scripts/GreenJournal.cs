using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GreenJournal : MonoBehaviour
{
    public Sprite PressedSprite, Relasedsprite;
    public List<GameObject> TabsButtons;
    public List<GameObject> MainTabs;
    public GameObject ActionPlanPage, GameFeedPage, DiyPage;
    public GameObject GalleryPage,JournalPage;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        TabsButtons[0].gameObject.GetComponent<Image>().sprite = PressedSprite;
        TabsButtons[1].gameObject.GetComponent<Image>().sprite = Relasedsprite;
        TabsButtons[2].gameObject.GetComponent<Image>().sprite = Relasedsprite;
        ActionPlanPage.SetActive(true);
        GameFeedPage.SetActive(false);
        DiyPage.SetActive(false);
    }


    public void MainButtonsActivity(GameObject currentGameobject)
    {
        TabsButtons.ForEach(x =>
        {
            x.GetComponent<Image>().sprite = x.name == currentGameobject.name ? PressedSprite : Relasedsprite;
        
        });

        PageSelection(currentGameobject.name);

    }

    void PageSelection(string selectedBtn)
    {
        switch (selectedBtn.ToLower())
        {
            case "actionplan":
                ActionPlanActivity();
                break;
            case "gamefeed":
                GameFeedActivity();
                break;
            case "diy":
                DiyActivity();
                break;
            default:
                Debug.Log("unique");
                break;
        }
    }

    void ActionPlanActivity()
    {
        ActionPlanPage.SetActive(true);
        GameFeedPage.SetActive(false);
        DiyPage.SetActive(false);
    }

    void GameFeedActivity()
    {
        ActionPlanPage.SetActive(false);
        GameFeedPage.SetActive(true);
        DiyPage.SetActive(false);
    }
    void DiyActivity()
    {
        ActionPlanPage.SetActive(false);
        GameFeedPage.SetActive(false);
        DiyPage.SetActive(true);
    }

    public void ShowAllActionPlan()
    {
        GalleryPage.SetActive(true);
    }
}
