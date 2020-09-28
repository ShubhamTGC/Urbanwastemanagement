using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationPageHandler : MonoBehaviour
{
    
    public List<GameObject> tabs;
    public List<GameObject> Forms;
    public Sprite Pressed, notPressed;
    private int UserRole;
    public GameObject Formpage;


    void Start()
    {
        
    }

 
    void Update()
    {
      
    }

     void OnEnable()
    {
        Formpage.SetActive(true);
        for (int a = 0; a < tabs.Count; a++)
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

    public void closeRegistertion()
    {
        StartCoroutine(Closepage());

    }

    IEnumerator Closepage()
    {
        iTween.ScaleTo(Formpage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Formpage.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        this.gameObject.SetActive(false);

    }







}
