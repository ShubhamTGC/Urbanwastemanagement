using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profilehandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject boyinput_page, girlinput_page;
    private int boybtn_click = 1, girlbtn_click = 1;
    public InputField boy_name_field, boy_grade_field, girl_name_field, girl_grade_field;
    public Button boy_btn,girl_btn;
    public Image profileimage;
    public Sprite girlface, boyface;
    public Narrationmanagement narration_activity;
    public InputField boy_username,girl_username;
    public string Mainurl, AvatarAPI, Userdata_API;
    private int character_data;
    public GameObject boy_btn_1, girlbtn_1;
    public StartpageController startpage;
    public GameObject startpageObj;
    public Sprite bagde_sprite,hero_intro;
    public GameObject intro_panel;
    private string grade_value;
    private string playername;

    [Header("User Updated Avatar SelectionPage")]
    [Space(10)]
    public List<GameObject> Faces;
    public List<GameObject> Body;
    public List<Sprite> FaceSprite,BodySprites;
    public Image PlayerFace, PlayerBody;
    void Start()
    {
        for(int a = 0; a < Faces.Count; a++)
        {
            if(PlayerFace.sprite.name == Faces[a].name)
            {
                Faces[a].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                Faces[a].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        for(int b =0;b < Body.Count; b++)
        {
            if(PlayerBody.sprite.name == Body[b].name)
            {
                Body[b].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                Body[b].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(boy_name_field.text !="" && boy_grade_field.text != "")
        {
            boy_btn.interactable = true;
            
        }
        else
        {
            boy_btn.interactable = false;
        }

        if (girl_name_field.text != "" && girl_grade_field.text != "")
        {
            girl_btn.interactable = true;
        }
        else
        {
            girl_btn.interactable = false;
        }

    }

    //---------------------------------boy button selection------------------//
    public void boyselection()
    {
        boyinput_page.SetActive(true);
        StartCoroutine(boy_action());
        
    }

    public void girlselection()
    {
        girlinput_page.SetActive(true);
        StartCoroutine(girl_action());
    }
    
    IEnumerator boy_action()
    {
        yield return new WaitForSeconds(0f);

        if (boybtn_click % 2 == 0)
        {
            int childs = boyinput_page.transform.childCount;
            for (int a = 0; a < childs; a++)
            {
                boyinput_page.transform.GetChild(a).gameObject.SetActive(false);
            }
            float image_amout = boyinput_page.GetComponent<Image>().fillAmount;
            girlbtn_1.GetComponent<Button>().interactable = true;
            while (image_amout > 0)
            {
                image_amout -= 0.2f;
                boyinput_page.GetComponent<Image>().fillAmount = image_amout;
                yield return new WaitForSeconds(0.05f);
            }
            boyinput_page.SetActive(false);


        }
        else
        {
            float image_amout = boyinput_page.GetComponent<Image>().fillAmount;
            while (image_amout < 1)
            {
                image_amout += 0.2f;
                boyinput_page.GetComponent<Image>().fillAmount = image_amout;
                yield return new WaitForSeconds(0.05f);
            }
            girlbtn_1.GetComponent<Button>().interactable = false;
            int childs = boyinput_page.transform.childCount;
            for (int a = 0; a < childs; a++)
            {
                boyinput_page.transform.GetChild(a).gameObject.SetActive(true);
            }
        }
        boybtn_click += 1;

    }

    IEnumerator girl_action()
    {
        yield return new WaitForSeconds(0f);

        if (girlbtn_click % 2 == 0)
        {
            int childs = girlinput_page.transform.childCount;
            for (int a = 0; a < childs; a++)
            {
                girlinput_page.transform.GetChild(a).gameObject.SetActive(false);
            }
            float image_amout = girlinput_page.GetComponent<Image>().fillAmount;
            boy_btn_1.GetComponent<Button>().interactable = true;
            while (image_amout > 0)
            {
                image_amout -= 0.2f;
                girlinput_page.GetComponent<Image>().fillAmount = image_amout;
                yield return new WaitForSeconds(0.05f);
            }
            girlinput_page.SetActive(false);

        }
        else
        {
            float image_amout = girlinput_page.GetComponent<Image>().fillAmount;
            while (image_amout < 1)
            {
                image_amout += 0.2f;
                girlinput_page.GetComponent<Image>().fillAmount = image_amout;
                yield return new WaitForSeconds(0.05f);
            }
            int childs = girlinput_page.transform.childCount;
            boy_btn_1.GetComponent<Button>().interactable = false;
            for (int a = 0; a < childs; a++)
            {
                girlinput_page.transform.GetChild(a).gameObject.SetActive(true);
            }
        }
        girlbtn_click += 1;
    }

    //--------------------------------------------------------------------------------------------//

    public void boy_ok()
    {
        StartCoroutine(boy_action());
        string boy_name = boy_username.text;
        grade_value = boy_grade_field.text;
        playername = boy_username.text;
        PlayerPrefs.SetString("username", boy_name);
        PlayerPrefs.SetString("profile_done", "done");
        character_data = 1;
        profileimage.GetComponent<Image>().sprite = boyface;
        StartCoroutine(ok_task());

    }

    public void girl_ok()
    {
        StartCoroutine(girl_action());
        string girl_name = girl_username.text;
        playername = girl_username.text;
        grade_value = girl_grade_field.text;
        PlayerPrefs.SetString("username", girl_name);
        PlayerPrefs.SetString("profile_done", "done");
        character_data = 2;
        profileimage.GetComponent<Image>().sprite = girlface;
        StartCoroutine(ok_task());
    }

    //IEnumerator ok_task()
    //{
       
    //    yield return new WaitForSeconds(0.5f);
    //    iTween.ScaleTo(this.gameObject, Vector3.zero, 0.5f);
    //    yield return new WaitForSeconds(0.5f);
    //    StartCoroutine(startpage.scenechanges(startpage.gameObject, bagde_sprite));
    //    yield return new WaitForSeconds(1.2f);
    //    this.gameObject.SetActive(false);
    //    intro_panel.GetComponent<CompleteIntroPage>().After_profile();
    //    intro_panel.SetActive(true);
    //    PlayerPrefs.SetString("User_grade", grade_value);
    //    PlayerPrefs.SetInt("characterType", character_data);
    //    //StartCoroutine(postAvatarData());

    //}
    //IEnumerator postAvatarData()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    string avatar_url = Mainurl + AvatarAPI;
    //    WWWForm avatar_form = new WWWForm();
    //    int uid = PlayerPrefs.GetInt("UID");
    //    int oid = PlayerPrefs.GetInt("OID");
    //    avatar_form.AddField("UID", uid);
    //    avatar_form.AddField("OID", oid);
    //    avatar_form.AddField("avatar_type", character_data);
    //    Debug.Log("UID" + uid + "oid" + oid + " " + character_data);
    //    PlayerPrefs.SetInt("characterType", character_data);
    //    WWW avatar_www = new WWW(avatar_url, avatar_form);
    //    yield return avatar_www;
    //    if (avatar_www.text != null)
    //    {
    //        Debug.Log(avatar_www.text);
    //    }
    //    this.gameObject.SetActive(false);
    //    narration_activity.AfterAvatar_task();
    //}

    public void back_from_profile()
    {
        StartCoroutine(back_task());
    }

    IEnumerator back_task()
    {
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(this.gameObject, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(startpage.scenechanges(startpageObj, hero_intro));
        yield return new WaitForSeconds(1.2f);
        intro_panel.SetActive(true);
        this.gameObject.SetActive(false);
        
    }

    IEnumerator ok_task()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(postAvatarData());

    }
    IEnumerator postAvatarData()
    {
        yield return new WaitForSeconds(0.1f);
        string avatar_url = Mainurl + AvatarAPI;
        WWWForm avatar_form = new WWWForm();
        int uid = PlayerPrefs.GetInt("UID");
        avatar_form.AddField("UID", uid);
        avatar_form.AddField("UserName", playername);
        avatar_form.AddField("UserGrade", grade_value);
        WWW avatar_www = new WWW(avatar_url, avatar_form);
        yield return avatar_www;
        if (avatar_www.text != null)
        {
            Debug.Log(avatar_www.text);
            StartCoroutine(PostUserData());
        }

    }

    IEnumerator PostUserData()
    {
        yield return new WaitForSeconds(1f);
        string avatar_url = Mainurl + Userdata_API;

        WWWForm userdata_form = new WWWForm();
        userdata_form.AddField("UID", PlayerPrefs.GetInt("UID"));
        userdata_form.AddField("OID", PlayerPrefs.GetInt("OID"));
        userdata_form.AddField("avatar_type", character_data);

        WWW avatar_www = new WWW(avatar_url, userdata_form);
        yield return avatar_www;
        if (avatar_www.text != null)
        {
            Debug.Log(avatar_www.text);
            yield return new WaitForSeconds(0.5f);
            iTween.ScaleTo(this.gameObject, Vector3.zero, 0.5f);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(startpage.scenechanges(startpage.gameObject, bagde_sprite));
            yield return new WaitForSeconds(1.2f);
            this.gameObject.SetActive(false);
            intro_panel.GetComponent<CompleteIntroPage>().After_profile();
            intro_panel.SetActive(true);
            PlayerPrefs.SetString("User_grade", grade_value);
            PlayerPrefs.SetInt("characterType", character_data);
            //this.gameObject.SetActive(false);
            //narration_activity.AfterAvatar_task();
        }

    }


    //========================== PLAYER UPDATE AVATAR SELECTION METHODS ===============================
    public void selectdFace(string facename)
    {
        for(int a = 0; a < Faces.Count; a++)
        {
            if(Faces[a].name == facename)
            {
                PlayerFace.sprite = FaceSprite[a];
                Faces[a].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                Faces[a].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void SelectBody(string bodyname)
    {
        for (int a = 0; a < Body.Count; a++)
        {
            if (Body[a].name == bodyname)
            {
                PlayerBody.sprite = BodySprites[a];
                Body[a].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                Body[a].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
