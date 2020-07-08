using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2ZoneGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainZonePage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonmsg(GameObject Zone)
    {
        Zone.SetActive(true);
        MainZonePage.SetActive(false);
    }
}
