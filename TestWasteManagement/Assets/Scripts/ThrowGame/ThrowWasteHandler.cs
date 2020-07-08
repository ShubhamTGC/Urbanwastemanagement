using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowWasteHandler : MonoBehaviour
{
    public int Objectcount;
    public List<Sprite> WasteObejctSprites;
    public GameObject WastePrefeb;
    public Transform WastePerent;
    public LineRenderer ForntLine, BackLine;
    public Rigidbody2D catapultback;
    private List<GameObject> GeneratedObj = new List<GameObject>();
    public Image Previewimage1, Previewimage2, Previewimage3, Previewimage4;
    private int Objectlive = 0;

    [Header("Time portion")]
    [Space(10)]
    public float minut;
    public float second;
    private float Totaltimer, RunningTimer;
    private float sec;
    private bool helpingbool = true;
    public Image Timerbar;
    public Text Timer;
    public Text CorrectGuesscount,Totalwaste;
    [SerializeField]
    private int totalwasteCount;
    private void Awake()
    {
        initialsetup();
    }

    void initialsetup()
    {
        sec = second;
        Totaltimer = (minut * 60) + second;
        RunningTimer = Totaltimer;
        Totalwaste.text = totalwasteCount.ToString();
        for (int a = 0; a < Objectcount; a++)
        {
            GameObject gb = Instantiate(WastePrefeb, WastePerent, false);
            gb.SetActive(false);
            gb.GetComponent<ProjectileDragging>().catapultLineBack = BackLine;
            gb.GetComponent<SpringJoint2D>().connectedBody = catapultback;
            gb.GetComponent<ProjectileDragging>().catapultLineFront = ForntLine;
            GeneratedObj.Add(gb);
        }

         SpriteUpdator(0);
         GeneratedObj[Objectlive].SetActive(true);

    }

    void SpriteUpdator(int a)
    {
        Previewimage1.sprite = GeneratedObj[a].GetComponent<SpriteRenderer>().sprite;
        Previewimage2.sprite = GeneratedObj[a + 1].GetComponent<SpriteRenderer>().sprite;
        Previewimage3.sprite = GeneratedObj[a + 2].GetComponent<SpriteRenderer>().sprite;
        Previewimage4.sprite = GeneratedObj[a + 3].GetComponent<SpriteRenderer>().sprite;

    }
    void Start()
    {

    }
    
    void Update()
    {

        if (sec >= 0 && minut >= 0 && helpingbool)
        {
            sec = sec - Time.deltaTime;
            RunningTimer = RunningTimer - Time.deltaTime;
            Timerbar.fillAmount = RunningTimer / Totaltimer;
            if (sec.ToString("0").Length > 1)
            {
                Timer.text = "0" + minut.ToString("0") + ":" + sec.ToString("0");
            }
            else
            {
                Timer.text = "0" + minut.ToString("0") + ":" + "0" + sec.ToString("0");
            }

            if (sec.ToString("0") == "0" && minut >= 0)
            {
                sec = 60;
                minut = minut - 1;
            }
        }
        else if (helpingbool)
        {
            helpingbool = false;

        }

        ThrowWasteObjectactiveStatus();
      


    }

    void ThrowWasteObjectactiveStatus()
    {
        if (Objectlive < GeneratedObj.Count - 1)
        {

            if (!GeneratedObj[Objectlive].activeInHierarchy)
            {
                Objectlive++;
                GeneratedObj[Objectlive].SetActive(true);

                if (Objectlive < GeneratedObj.Count)
                {
                    Previewimage1.sprite = GeneratedObj[Objectlive].GetComponent<SpriteRenderer>().sprite;
                    if (Objectlive < GeneratedObj.Count - 2)
                    {
                        Previewimage2.sprite = GeneratedObj[Objectlive + 1].GetComponent<SpriteRenderer>().sprite;
                        Previewimage3.sprite = GeneratedObj[Objectlive + 2].GetComponent<SpriteRenderer>().sprite;

                    }
                    else
                    {
                        Previewimage3.gameObject.SetActive(false);
                        Previewimage2.gameObject.SetActive(false);
                    }
                    if (Objectlive < GeneratedObj.Count - 3)
                    {
                        Previewimage3.sprite = GeneratedObj[Objectlive + 2].GetComponent<SpriteRenderer>().sprite;

                    }
                    else
                    {
                        Previewimage3.gameObject.SetActive(false);
                    }
                    if (Objectlive < GeneratedObj.Count - 4)
                    {
                        Previewimage4.sprite = GeneratedObj[Objectlive + 3].GetComponent<SpriteRenderer>().sprite;

                    }
                    else
                    {
                        Previewimage4.gameObject.SetActive(false);
                    }
                }



            }
        }
        if (Objectlive == GeneratedObj.Count - 1)
        {
            Previewimage3.gameObject.SetActive(false);
            Previewimage2.gameObject.SetActive(false);
            Previewimage1.gameObject.SetActive(false);
            Previewimage4.gameObject.SetActive(false);
        }
    }
}
