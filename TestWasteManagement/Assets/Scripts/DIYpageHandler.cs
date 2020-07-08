using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIYpageHandler : MonoBehaviour
{
    // Start is called before the first frame updateNext
    public GameObject NextButton, BackButton;
    public List<GameObject> Pagelist;
    private int pagecounter=0;
    private int lastpagecounter=0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pagecounter == 0)
        {
            BackButton.SetActive(false);
            NextButton.SetActive(true);
        }
        else if(pagecounter >0 && pagecounter < Pagelist.Count - 1)
        {
            BackButton.SetActive(true);
            NextButton.SetActive(true);
        }
        else if(pagecounter < Pagelist.Count)
        {
            NextButton.SetActive(false);
        }
    }

    public void NextpageEnable()
    {
        lastpagecounter = pagecounter;
        pagecounter++;
        Pagelist[pagecounter].SetActive(true);
        Pagelist[lastpagecounter].SetActive(false);
    }

    void Enablepages()
    {

    }
    public void BackPageEnable()
    {
        lastpagecounter = pagecounter;
        pagecounter--;
        Pagelist[pagecounter].SetActive(true);
        Pagelist[lastpagecounter].SetActive(false);
    }
}
