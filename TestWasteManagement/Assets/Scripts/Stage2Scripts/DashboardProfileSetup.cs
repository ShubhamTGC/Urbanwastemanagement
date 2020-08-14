using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardProfileSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public Image BoyPRFFace,BoyPRFBBody;
    public Image GirlPRFFace, GirlPRFBBody;
    public List<Sprite> BoyFace,BoyBody, GirlFace,GirlBody;
    public GameObject BoyProfile, GirlProfile;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (PlayerPrefs.GetString("gender").ToLower() == "m")
        {
            BoyProfile.SetActive(true);
            GirlProfile.SetActive(false);
            PlayerSetup(BoyFace,BoyBody, BoyPRFFace, BoyPRFBBody);
        }
        else
        {
            BoyProfile.SetActive(false);
            GirlProfile.SetActive(true);
            PlayerSetup(GirlFace,GirlBody, GirlPRFFace, GirlPRFBBody);
        }
    }

    void PlayerSetup(List<Sprite> Face,List<Sprite> Body,Image PlayerFace,Image PlayerBody)
    {
        int CharacterNum = PlayerPrefs.GetInt("characterType");
        int BodyNum = PlayerPrefs.GetInt("PlayerBody");
        for (int a = 0; a < Face.Count; a++)
        {
            if (a == CharacterNum)
            {
                PlayerFace.sprite = Face[a];
            }
        }

        for (int b = 0; b < Body.Count; b++)
        {
            if (b == BodyNum)
            {
                PlayerBody.sprite = Body[b];
            }
        }
    }

 
}
