﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleclickGameguide : MonoBehaviour,IPointerClickHandler
{
    private int tapcount = 0;
    public Stage3PageHandler mainLevel;
    public void OnPointerClick(PointerEventData eventData)
    {
        tapcount = eventData.clickCount;
        if (tapcount == 2)
        {
            mainLevel.ShowGameGuide();
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
