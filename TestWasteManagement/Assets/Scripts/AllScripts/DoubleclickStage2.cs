using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleclickStage2 : MonoBehaviour,IPointerClickHandler
{


    private int tapcount = 0;
    public Stage2ZoneGame mainLevel;
    public GameObject Zonesrelated;
    public void OnPointerClick(PointerEventData eventData)
    {
        tapcount = eventData.clickCount;
        if (tapcount == 2)
        {
            mainLevel.Selectzone(Zonesrelated);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
