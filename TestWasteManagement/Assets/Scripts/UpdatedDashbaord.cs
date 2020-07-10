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
    void Start()
    {
        initialSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
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





}
