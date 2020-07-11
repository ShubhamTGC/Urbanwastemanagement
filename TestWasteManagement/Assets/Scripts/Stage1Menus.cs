using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Menus : MonoBehaviour
{
    public List<GameObject> Zones;
    public List<GameObject> ZoneButtons;
    public Sprite Pressed, Relased;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Initialsetup();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialsetup()
    {
        Zones[0].SetActive(true);
        ZoneButtons[0].GetComponent<Image>().sprite = Pressed;
        for(int a=1;a< ZoneButtons.Count; a++)
        {
            ZoneButtons[a].GetComponent<Image>().sprite = Relased;
            Zones[a].SetActive(false);
        }

    }

    public void Zoneselection(GameObject currentZone)
    {
        bool enable;
        Zones.ForEach(z =>
        {
            enable = z.name == currentZone.name;
            z.gameObject.SetActive(enable);
        });

        ZoneButtons.ForEach(b =>
        {
            b.GetComponent<Image>().sprite = b.name == currentZone.name ? Pressed : Relased;
        });


    }
}
