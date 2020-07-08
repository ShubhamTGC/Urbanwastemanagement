using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2ZoneGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainZonePage;
    public GameObject ChildZoneA, ChildZoneB;
    [SerializeField]
    private Color RelasedEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonmsg(GameObject Zone)
    {
        this.gameObject.GetComponent<Image>().color = RelasedEffect;
        ChildZoneA.SetActive(false);
        ChildZoneB.SetActive(false);
        Zone.SetActive(true);
        MainZonePage.SetActive(false);
    }
}
