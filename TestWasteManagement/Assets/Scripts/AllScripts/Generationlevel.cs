﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Generationlevel : MonoBehaviour
{

 
   
    public GameObject Done_msg_panel,dusbin;
    //[Header("==kitchen objects list===")]
    //public List<GameObject> kitchen_reduce;
    //public List<GameObject> kitchen_reuse, kitchen_recycle,K_partially_reduce, K_partially_reuse, K_partially_recycle;
    public GameObject HomepageObject,Backbutton;


    [Header("stage selection")]
    [Space(10)]
    public Sprite home_sprite;
    public List<GameObject> G_levels;
    public Text zoneinfo;
    public GameObject zonelayer,ButtonSlider;


    [Header("GET LEVEL DATA")]
    [Space(10)]
    public List<int> GameIDS = new List<int>();
    public string MainUrl, GetGamesIDApi,GetBadgeIdApi;
    public int Gamelevel,BadgeType;
    public List<GameObject> ZonesButtons;
    public Text Zoneinfo;

    // Start is called before the first frame update
    void Start()
    {
        Backbutton.SetActive(true);
        StartCoroutine(GetGamesIDactivity());
        StartCoroutine(GetBadgeinfo());
    }
     void OnEnable()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {
       
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
        //waste_count = 0;
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(Done_msg_panel, Vector3.one, 1f);
    }

    IEnumerator GetGamesIDactivity()
    {
        string HittingUrl = MainUrl + GetGamesIDApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
            "&id_org_game=" + 1 + "&id_level=" + Gamelevel;
        WWW GameResponse = new WWW(HittingUrl);
        yield return GameResponse;
        if(GameResponse.text != null)
        {
            GetLevelIDs gameIDs = Newtonsoft.Json.JsonConvert.DeserializeObject<GetLevelIDs>(GameResponse.text);
            gameIDs.content.ForEach(x =>
            {
                GameIDS.Add(x.id_game_content);
            });
        }
    }
    IEnumerator GetBadgeinfo()
    {
        string HittingUrl = MainUrl + GetBadgeIdApi + "?id_level=" + Gamelevel;
        WWW Hitting_res = new WWW(HittingUrl);
        yield return Hitting_res;
        if(Hitting_res.text != null)
        {
            List<BadgeDetails> Badgeinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BadgeDetails>>(Hitting_res.text);
            var id_badge = Badgeinfo.FirstOrDefault(x => x.id_level == Gamelevel && x.badge_type == BadgeType)?.id_badge;
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
            case "homezone":
                StartCoroutine(midlevel_task(home_sprite, 0));
                break;
            case "schoolzone":
                StartCoroutine(midlevel_task(home_sprite, 1));
                break;
            case "hospitalzone":
                StartCoroutine(midlevel_task(home_sprite, 2));
                break;
            case "officezone":
                StartCoroutine(midlevel_task(home_sprite, 3));
                break;
            case "factoryzone":
                StartCoroutine(midlevel_task(home_sprite, 4));
                break;
            case "parkzone":
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
        Zoneinfo.text = "";
        ZonesButtons.ForEach(x =>
        {
            x.transform.localPosition = new Vector3(0f, -1160f, 0f);
            x.transform.localScale = Vector3.zero;
            x.SetActive(false);
            x.GetComponent<BoxCollider2D>().enabled = false;
            x.GetComponent<Button>().enabled = false;
            x.GetComponent<Animator>().enabled = false;
        });
        zonelayer.SetActive(false);
        StartCoroutine(scenechanges(HomepageObject, scene_sprite));
        yield return new WaitForSeconds(1.2f);
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