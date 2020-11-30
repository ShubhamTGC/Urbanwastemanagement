using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loadingtrivia : MonoBehaviour
{
    public List<string> TriviaMsg;
    public Text ShowMSg;
    public Image LoadingBar;
    [HideInInspector]
    public bool Laodingstart;
    [SerializeField]
    private float totaltime;
    [SerializeField]
    private float LimitValue;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        currentTime = 0f;
        Laodingstart = false;
        int index = UnityEngine.Random.Range(0, TriviaMsg.Count);
        //System.Random ran = new System.Random();
        //int randomnum = ran.Next(0, TriviaMsg.Count);
        ShowMSg.text = TriviaMsg[index];
        Laodingstart = true;
        StartCoroutine(CustomLoader());
    }

    void OnDisable()
    {
        Laodingstart = false;
        currentTime = 0f;
        LoadingBar.fillAmount = 0f;

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CustomLoader()
    {
        if (currentTime < LimitValue)
        {
            yield return new WaitForSeconds(0.5f);
            currentTime += 2f;
            LoadingBar.fillAmount = currentTime / totaltime;

        }
        else
        {
            
            yield return new WaitForSeconds(0.5f);
            currentTime += 2f;
            LoadingBar.fillAmount = currentTime / totaltime;
            if (LoadingBar.fillAmount == 1)
            {
                Laodingstart = false;
                this.gameObject.SetActive(false);
            }

        }

        if (Laodingstart)
        {
            StartCoroutine(CustomLoader());
        }
    }

}

