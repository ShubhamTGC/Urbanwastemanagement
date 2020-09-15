using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiyLeaderBoardHandler : MonoBehaviour
{
    public Text UserName, ClassValue;
    private List<GameObject> Rows = new List<GameObject>();
    void Start()
    {
        
    }

    private void OnEnable()
    {
        UserName.text = PlayerPrefs.GetString("username");
        ClassValue.text = PlayerPrefs.GetString("User_grade");
        if(Rows.Count == 0)
        {
            StartCoroutine(GenerateLeaderBoard());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator GenerateLeaderBoard()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
