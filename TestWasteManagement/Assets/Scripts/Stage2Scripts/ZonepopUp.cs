using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZonepopUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startpage;
    public Color HoverEffect, RelasedEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        startpage.GetComponent<Image>().color = HoverEffect;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        startpage.GetComponent<Image>().color = RelasedEffect;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
