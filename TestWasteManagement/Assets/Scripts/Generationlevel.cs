using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Generationlevel : MonoBehaviour
{

    //public List<GameObject> home_kitchen,home_bedroom,home_living, industry_waste,school_waste,forest_waste,park_waste,hospital_waste;
    public List<GameObject> levels;
    //[HideInInspector]
    public int waste_count = 0;
    private bool home_check, industry_check, school_check, park_check, hospital_check, forest_check;
    private bool is_check = true;
    public GameObject Done_msg_panel,dusbin,exit_panel;
    public Vector3 dusbinpos;
    public StartpageController startpage_activitys;
    public GameObject startpage;
    public Sprite startpage_sprite;
    private GameObject gb;

    [Header("==kitchen objects list===")]
    public List<GameObject> kitchen_reduce;
    public List<GameObject> kitchen_reuse, kitchen_recycle,K_partially_reduce, K_partially_reuse, K_partially_recycle;
    public Text score1;
    public int level1score;
    private bool kitchen, bedroom, livingroom;
    public GameObject HomepageObject,Backbutton;


    [Header("stage selection")]
    [Space(10)]
    public Sprite home_sprite;
    public List<GameObject> G_levels;
    public Text zoneinfo;
    public GameObject zonelayer,ButtonSlider;
    // Start is called before the first frame update
    void Start()
    {
        ButtonSlider.SetActive(true);
        // dusbin.SetActive(true);
        is_check = true;
    }
     void OnEnable()
    {
       // dusbin.SetActive(true);
        is_check = true;
        //StartCoroutine(sceneappear());
        Backbutton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //if(waste_count == home_kitchen.Count && is_check)
        //{
        //    is_check = false;
        //    StartCoroutine(showstatus());
        //}
        score1.text = level1score.ToString(); ;
    }

    public IEnumerator scenechanges(GameObject parentobejct, Sprite new_sprite)
    {
        yield return new WaitForSeconds(0.1f);
        float bgvalue = parentobejct.GetComponent<Image>().color.a;
        while (bgvalue > 0)
        {
            bgvalue -= 0.1f;
            yield return new WaitForSeconds(0.05f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }
        parentobejct.GetComponent<Image>().sprite = new_sprite;
        bgvalue = parentobejct.GetComponent<Image>().color.a;

        while (bgvalue < 1)
        {
            bgvalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }

    }

    IEnumerator sceneappear()
    {
        float shadevalue = HomepageObject.GetComponent<Image>().color.a;
        while (shadevalue < 1)
        {
            HomepageObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        Backbutton.SetActive(true);
    }

    IEnumerator showstatus()
    {
        waste_count = 0;
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(Done_msg_panel, Vector3.one, 1f);
    }

    public void movetonext()
    {
        StartCoroutine(nextroomactivity());
     
    }

    IEnumerator nextroomactivity()
    {
        yield return new WaitForSeconds(0.1f);
        iTween.ScaleTo(Done_msg_panel, Vector3.zero, 1f);
        iTween.MoveTo(dusbin, iTween.Hash("x", dusbinpos.x, "y", dusbinpos.y, "z", dusbinpos.z, "easeType", iTween.EaseType.linear, "isLocal", true,
           "time", 0.8f));
        yield return new WaitForSeconds(1f);
        dusbin.SetActive(false);
        StartCoroutine(startpage_activitys.scenechanges(startpage,startpage_sprite));
        yield return new WaitForSeconds(1f);
        startpage_activitys.midlayerenable();
    }

    public void backtoG_level()
    {
        
        for(int a = 0; a < levels.Count; a++)
        {
            if(levels[a].gameObject.activeInHierarchy == true)
            {
                gb = levels[a].gameObject;
               
            }
        }

        if(waste_count == gb.transform.childCount)
        {
            Debug.Log("level completed");
        }
        else
        {

        }
        {
            exit_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You have not found all the waste, Do you really want to exit!";
            iTween.ScaleTo(exit_panel, Vector3.one, 1f);
            Debug.Log(" not level completed");
        }
      
    }


    public void BackToMainPage()
    {
        SceneManager.LoadScene(0);
    }


    //======================== stage selection methos ==============================
    public void sublevelmethod(GameObject selectedbtn)
    {
        //midlayer.SetActive(false);
        string buttonname = selectedbtn.name;
        selectedbtn.transform.GetChild(0).gameObject.SetActive(false);
        zoneinfo.text = "";
        sublevelactions(buttonname);
    }

    void sublevelactions(string name)
    {
        switch (name.ToLower())
        {
            case "home":
                StartCoroutine(midlevel_task(home_sprite, 0));
                break;
            case "school":
                StartCoroutine(midlevel_task(home_sprite, 1));
                break;
            case "hospital":
                StartCoroutine(midlevel_task(home_sprite, 2));
                break;
            case "office":
                StartCoroutine(midlevel_task(home_sprite, 3));
                break;
            case "industry":
                StartCoroutine(midlevel_task(home_sprite, 4));
                break;
            case "park":
                StartCoroutine(midlevel_task(home_sprite, 5));
                break;
            default:
                Debug.Log("unique zone");
                break;
        }

    }

    IEnumerator midlevel_task(Sprite scene_sprite, int level_id)
    {

        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponent<AudioSource>().enabled = false;
        zonelayer.SetActive(false);
        StartCoroutine(scenechanges(HomepageObject, scene_sprite));
        yield return new WaitForSeconds(1f);
        //Generation_level.SetActive(true);
        G_levels[level_id].SetActive(true);


    }

    public void VibrateDevice()
    {
        if (!PlayerPrefs.HasKey("VibrationEnable"))
        {
            Vibration.Vibrate(400);
            Debug.Log("vibration");
        }
        else
        {
            string vibration = PlayerPrefs.GetString("VibrationEnable");

            if (vibration == "true")
            {
                Vibration.Vibrate(400);
                Debug.Log("vibration");
            }
        }


    }

}
