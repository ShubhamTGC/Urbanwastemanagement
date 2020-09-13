using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Studentregistration : MonoBehaviour
{
    // Start is called before the first frame update
    public string MainUrl, SchoolListApi, CheckUserID;
    public Dropdown school_dropdown,gender,TeacherName;
    public InputField Username, Grade, UserId, Emailid;
    private Dictionary<string, int> SchoolIdList = new Dictionary<string, int>();
    public GameObject PopUppage;
    public Text ExtramsgBox;
    [SerializeField] private List<InputField> Allfields;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(SchoolIdList.Count == 0)
        {
            StartCoroutine(Get_schoolnames());
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator Get_schoolnames()
    {
        yield return new WaitForSeconds(0.1f);
        string hitting_url = MainUrl + SchoolListApi;
        WWW school_names = new WWW(hitting_url);
        yield return school_names;
        if (school_names.text != null)
        {
            if(school_names.text != "[]")
            {
                List<SchoolListModel> SchoolList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SchoolListModel>>(school_names.text);
                school_dropdown.options.Clear();
                school_dropdown.value = 0;
                school_dropdown.options.Add(new Dropdown.OptionData() { text = "School Name" });
                SchoolList.ForEach(x =>
                {
                    school_dropdown.options.Add(new Dropdown.OptionData() { text = x.school_name });
                  //  SchoolIdList.Add(x.school_name, x.id_school);
                });
            }
        }
    }


    public void RegisterUser()
    {
        if (Username.text == ""  || Grade.text == "" || UserId.text == "" || Emailid.text == "" || school_dropdown.value == 0 || TeacherName.value == 0)
        {

            string msg = "Please fill the required information.";
            StartCoroutine(showtext(msg));
        }
        else
        {
            int school_id = SchoolIdList[school_dropdown.options[school_dropdown.value].text];
            string user_id = "gs" + UserId.text + "_" + school_id;
            Debug.Log(user_id);
            StartCoroutine(Check_user(user_id));

        }
    }

    IEnumerator Check_user(string userid)
    {
        yield return new WaitForSeconds(0.1f);
        WWWForm user_check = new WWWForm();
        user_check.AddField("UserID", userid);
        string user_checkingUrl = MainUrl + CheckUserID;
        WWW User_status = new WWW(user_checkingUrl, user_check);
        yield return User_status;
        if (User_status != null)
        {
            JsonData user_response = JsonMapper.ToObject(User_status.text);
            string user = user_response["IsPresent"].ToString();
            if (user.ToLower() == "true")
            {
                Debug.Log("already regsitered");
                string msg = "You have already Registered";
                StartCoroutine(showtext(msg));
                for (int a = 0; a < Allfields.Count; a++)
                {
                    Allfields[a].text = "";
                }
                school_dropdown.value = 0;
            }
            else
            {

                //StartCoroutine(getUserdata());
            }

        }

    }

    IEnumerator showtext(string msg)
    {
       
        ExtramsgBox.text = msg;
        PopUppage.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        ExtramsgBox.text = "";
        iTween.ScaleTo(PopUppage, Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.3f);
        PopUppage.SetActive(false);
    }
}
