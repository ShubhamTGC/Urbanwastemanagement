using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2hoverEffect : MonoBehaviour
{

    public GameObject panel;
    public GameObject level1, level2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if (this.gameObject.name.StartsWith("S1"))
        {
            //panel.SetActive(true);
            panel.GetComponent<Image>().enabled = true;
            level2.transform.SetSiblingIndex(0);
            panel.transform.SetSiblingIndex(1);
            level1.transform.SetSiblingIndex(2);
        }
        if (this.gameObject.name.StartsWith("S2"))
        {
            //panel.SetActive(true);
            panel.GetComponent<Image>().enabled = true;
            level1.transform.SetSiblingIndex(0);
            panel.transform.SetSiblingIndex(1);
            level2.transform.SetSiblingIndex(2);
        }
       
    }

    public void OnMouseExit()
    {
        //panel.SetActive(false);
        panel.GetComponent<Image>().enabled = false;
        level1.transform.SetSiblingIndex(1);
        panel.transform.SetSiblingIndex(0);
        level2.transform.SetSiblingIndex(2);
    }
}
