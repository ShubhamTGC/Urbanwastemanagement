using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ThrowWasteHandler : MonoBehaviour
{
    public int Objectcount;
    private  Sprite[] WasteObejctSprites;
    public string wasteImagePath;
    public GameObject WastePrefeb;
    public Transform WastePerent;
    public LineRenderer ForntLine, BackLine;
    public Rigidbody2D catapultback;
    private List<GameObject> GeneratedObj = new List<GameObject>();
    public Image Previewimage1, Previewimage2, Previewimage3, Previewimage4;
    private int Objectlive = 0;
    private Vector3 timerPanelPos;
    public GameObject TimerPanel;
    public GameObject Gamecanvas, ZonePag,MainZone, Canvasobject;

    //============== COMPLETE GAME SCORE VARIABLE AND HELPING VARIABLE======================
    [Header("Time portion and score ")]
    [Space(10)]
    public float minut;
    public float second;
    private float Totaltimer, RunningTimer;
    private float sec;
    private bool helpingbool = true;
    public Image Timerbar;
    public Text Timer;
    public Text CorrectGuesscount,Totalwaste;
    public Text TotalscoreText;
    private int SCore;
    private int correctGoal=0;
    private bool gameend = false;
    private bool gameclose = false;
    [SerializeField]
    private int totalwasteCount;
    public Text Gamestaus;
    public List<Sprite> paper, glass, biohazard, ewaste, plastic, organic, metal;
    private List<KeyValuePair<string, List<Sprite>>> WasteCollectionList;
    public GameObject ScoreKnob;
    [SerializeField]
    private int TotalGameScore;
    private bool checkScore;
    private float Knobangle;
    private bool TimePaused = false;
    [SerializeField]
    private Color WrongEffect;
    private void Awake()
    {
      
     
    }

    private void OnEnable()
    {
        Gamecanvas.SetActive(true);
        initialsetup();
    }
    void initialsetup()
    {

        gameend = gameclose  = false;
        timerPanelPos = TimerPanel.GetComponent<RectTransform>().localPosition;
        WasteObejctSprites = Resources.LoadAll<Sprite>(wasteImagePath);
        getRandomeSprite();
        WasteCollectionList = new List<KeyValuePair<string, List<Sprite>>>()
        {
            new KeyValuePair<string, List<Sprite>>("Paper", paper),
            new KeyValuePair<string, List<Sprite>>("glass", glass),
            new KeyValuePair<string, List<Sprite>>("biohazard", biohazard),
            new KeyValuePair<string, List<Sprite>>("ewaste", ewaste),
            new KeyValuePair<string, List<Sprite>>("plastic", plastic),
            new KeyValuePair<string, List<Sprite>>("organic", organic),
            new KeyValuePair<string, List<Sprite>>("metal", metal)
        };
        sec = second;
        Objectlive = 0;
        Totaltimer = (minut * 60) + second;
        RunningTimer = Totaltimer;
        Totalwaste.text = totalwasteCount.ToString();
        CorrectGuesscount.text = "0";
        
        for (int a = 0; a < WasteObejctSprites.Length; a++)
        {
            GameObject gb = Instantiate(WastePrefeb, WastePerent, false);
            gb.SetActive(false);
            gb.GetComponent<ProjectileDragging>().catapultLineBack = BackLine;
            gb.GetComponent<SpringJoint2D>().connectedBody = catapultback;
            gb.GetComponent<ProjectileDragging>().catapultLineFront = ForntLine;
            gb.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = WasteObejctSprites[a];
            GeneratedObj.Add(gb);
        }

         SpriteUpdator(0);
         GeneratedObj[Objectlive].SetActive(true);

    }

    void getRandomeSprite()
    {
        for(int a=0;a< WasteObejctSprites.Length; a++)
        {
            Sprite temp = WasteObejctSprites[a];
            int randomindex = Random.Range(1, WasteObejctSprites.Length);
            WasteObejctSprites[a] = WasteObejctSprites[randomindex];
            WasteObejctSprites[randomindex] = temp;
        }
    }

    void SpriteUpdator(int a)
    {
        Previewimage1.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedObj[a].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Previewimage2.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedObj[a + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Previewimage3.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedObj[a + 2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Previewimage4.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedObj[a + 3].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

    }
    void Start()
    {

    }
    
    void Update()
    {
        if (!TimePaused)
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
                StartCoroutine(GameEndProcess());

            }
        }
       
        if (!gameclose)
        {
            ThrowWasteObjectactiveStatus();
        }

        if (checkScore)
        {
            checkScore = false;
            Knobangle = ((float)SCore / (float)TotalGameScore) * 200f;

        }
        var Rotationangle = Quaternion.Euler(0f, 0f, -Knobangle);
        ScoreKnob.GetComponent<RectTransform>().rotation = Quaternion.Lerp(ScoreKnob.GetComponent<RectTransform>().rotation, Rotationangle, 10 * 1 * Time.deltaTime);


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
                    Previewimage1.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedObj[Objectlive].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                    if (Objectlive < GeneratedObj.Count - 2)
                    {
                        Previewimage2.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedObj[Objectlive + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        Previewimage3.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedObj[Objectlive + 2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        Previewimage3.gameObject.SetActive(false);
                        Previewimage2.gameObject.SetActive(false);
                    }
                    if (Objectlive < GeneratedObj.Count - 3)
                    {
                        Previewimage3.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedObj[Objectlive + 2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

                    }
                    else
                    {
                        Previewimage3.gameObject.SetActive(false);
                    }
                    if (Objectlive < GeneratedObj.Count - 4)
                    {
                        Previewimage4.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedObj[Objectlive + 3].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

                    }
                    else
                    {
                        Previewimage4.gameObject.SetActive(false);
                    }
                }



            }
        }
        if (Objectlive == GeneratedObj.Count - 1 && !gameend)
        {
            Previewimage3.gameObject.SetActive(false);
            Previewimage2.gameObject.SetActive(false);
            Previewimage1.gameObject.SetActive(false);
            Previewimage4.gameObject.SetActive(false);
            gameend = true;
            StartCoroutine(GameEndProcess());
        }
    }

    IEnumerator GameEndProcess()
    {
        TimePaused = true;
        yield return new WaitForSeconds(0.1f);
        Gamestaus.text = "You have scored " + SCore;
        foreach(GameObject e in GeneratedObj)
        {
            e.SetActive(false);
        }
    }

    public void checkCollidedAns(string dustbin,string collidername,GameObject dustbinobj)
    {
        
        var CollidedDustbin = WasteCollectionList.FirstOrDefault(x => x.Key.Equals(dustbin, System.StringComparison.OrdinalIgnoreCase));
        var iscorrectWaste = CollidedDustbin.Value.Any(x => x.name.Equals(collidername,System.StringComparison.OrdinalIgnoreCase));
        
        if (iscorrectWaste)
        {
            dustbinobj.GetComponent<ShakeEffect>().enabled = true;
            correctGoal++;
            CorrectGuesscount.text = correctGoal.ToString();
            SCore += 10;
            TotalscoreText.text = SCore.ToString();
            checkScore = true;
        }
        else
        {
            dustbinobj.GetComponent<ShakeEffect>().enabled = true;
            dustbinobj.GetComponent<SpriteRenderer>().color = WrongEffect;
            Debug.Log(" nitb correct waste");
        }
        StartCoroutine(resetEffect(dustbinobj));
        
    }
    IEnumerator resetEffect(GameObject dustbinobj)
    {
        yield return new WaitForSeconds(1f);
        dustbinobj.GetComponent<SpriteRenderer>().color = Color.white; ;
        dustbinobj.GetComponent<ShakeEffect>().enabled = false;
    }

    public void CloseGame()
    {
        gameclose = true;
        StartCoroutine(CloserGame());
    }
    IEnumerator CloserGame()
    {
        iTween.MoveTo(TimerPanel, timerPanelPos, 0.5f);
        yield return new WaitForSeconds(0.5f);
        for(int a = 0; a < GeneratedObj.Count; a++)
        {
            DestroyImmediate(GeneratedObj[a].gameObject);
        }
        yield return new WaitForSeconds(0.3f);
        GeneratedObj.Clear();
        Gamecanvas.SetActive(false);
        ZonePag.SetActive(true);
        MainZone.SetActive(false);




    }
}
