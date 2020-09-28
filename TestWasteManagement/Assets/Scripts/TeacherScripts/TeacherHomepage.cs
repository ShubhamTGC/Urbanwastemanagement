using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherHomepage : MonoBehaviour
{
    public GameObject Performancepage, GameFeed, GreenJournalpage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowMoreUserPerformance()
    {
        Performancepage.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ShowGameFeed()
    {
        GameFeed.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ShowGreenJournal()
    {
        GreenJournalpage.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
