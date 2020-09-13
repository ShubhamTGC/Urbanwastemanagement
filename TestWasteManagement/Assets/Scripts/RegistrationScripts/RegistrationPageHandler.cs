using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationPageHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> tabs;
    public List<GameObject> Forms;
    public Sprite Pressed, notPressed;
    private int UserRole;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnEnable()
    {
        for(int a = 0; a < tabs.Count; a++)
        {
            if (a == 0)
            {
                Forms[a].SetActive(true);
                UserRole = 1;
                tabs[a].GetComponent<Image>().sprite = Pressed;
            }
            else
            {
                Forms[a].SetActive(false);
                tabs[a].GetComponent<Image>().sprite = notPressed;
            }
        }

    }

    public void SelectRoleTab(GameObject tab)
    {
        
        tabs.ForEach(x =>
        {
            x.GetComponent<Image>().sprite = x.name == tab.name ? Pressed : notPressed;
            
        });
        Forms.ForEach(a =>
        {
            a.SetActive(a.name == tab.name ? true : false);
        });
        int index = tabs.FindIndex(y => y.name == tab.name);
        UserRole = index + 1;

    }


}
