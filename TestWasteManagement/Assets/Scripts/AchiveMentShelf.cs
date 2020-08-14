using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchiveMentShelf : MonoBehaviour
{
    public Text Username;
    public List<Sprite> BoyFace,GirlFace;
    public Image PlayerFace;
    public string MainUrl, AchivementApi;
    public Sprite LockedFrame, UnlockFrame;
    public List<GameObject> BadgeFrames;
    [Header("Api response data")]
    [Space(15)]
    public Text Playername;
    public Text HighscoreBadgeCount, MostObsBadgeCount, MostActiveCount;
    void Start()
    {
        
    }

    private void OnEnable()
    {

        if (PlayerPrefs.GetString("gender").ToLower() == "m")
        {
            PlayerSetup(BoyFace);
        }
        else
        {
            PlayerSetup(GirlFace);
        }
        StartCoroutine(GetAchivementdata());
        
    }
   
    void Update()
    {
        
    }

    void PlayerSetup(List<Sprite> Face)
    {
        Username.text = PlayerPrefs.GetString("username");
        for(int a = 0; a < Face.Count; a++)
        {
            if(a == PlayerPrefs.GetInt("characterType"))
            {
                PlayerFace.sprite = Face[a];
            }
        }

    }

    IEnumerator GetAchivementdata()
    {
        
        string HittingUrl = MainUrl + AchivementApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID");
        WWW Achivement_www = new WWW(HittingUrl);
        yield return Achivement_www;
        if(Achivement_www.text != null)
        {
            Debug.Log(Achivement_www.text);
            AchivementModel AchivementLog = Newtonsoft.Json.JsonConvert.DeserializeObject<AchivementModel>(Achivement_www.text);
            Playername.text = AchivementLog.User_name;
            HighscoreBadgeCount.text = AchivementLog.HighscorerBadge_count;
            MostObsBadgeCount.text = AchivementLog.MostObserventBadge_count;
            MostActiveCount.text = AchivementLog.MostactiveplayerBadge_count;
            var CurrentBadgeName = AchivementLog.Current_badge != null ? AchivementLog.Current_badge : "rookie";

            SetCurrentBadge(CurrentBadgeName);

        }
    }

    void SetCurrentBadge(string currentBadge)
    {
        
        for(int a = 0; a < BadgeFrames.Count; a++)
        {
            if(BadgeFrames[a].gameObject.name == currentBadge)
            {
                for(int b = 0; b < a; b++)
                {
                    BadgeFrames[b].gameObject.GetComponent<Image>().sprite = UnlockFrame;
                    BadgeFrames[b].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                BadgeFrames[a].gameObject.GetComponent<Image>().sprite = LockedFrame;
            }
        }
    }
}
