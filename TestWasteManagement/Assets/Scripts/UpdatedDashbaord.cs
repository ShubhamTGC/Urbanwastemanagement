using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UpdatedDashbaord : MonoBehaviour
{
    public List<GameObject> Tabs;
    public Sprite PresssedSprite, RealesedSprite;
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
        Tabs[0].GetComponent<Image>().sprite = PresssedSprite;
        for(int a = 1; a < Tabs.Count; a++)
        {
            Tabs[a].GetComponent<Image>().sprite = RealesedSprite;
        }
    

    }

    public void Tabspressed(GameObject PressedTab)
    {
        Tabs.ForEach(t =>
        {
            t.GetComponent<Image>().sprite = t.name == PressedTab.name ? PresssedSprite : RealesedSprite;
        });
       
    }



}
