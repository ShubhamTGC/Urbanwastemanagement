using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ThrowWasteHandler : MonoBehaviour
{
    public int Objectcount;
    private Sprite[] WasteObejctSprites;
    public string wasteImagePath;
    public GameObject WastePrefeb;
    public Transform WastePerent;
    public LineRenderer ForntLine, BackLine;
    public Rigidbody2D catapultback;
    [SerializeField]
    private LineRenderer BackLineRendere, ForntLinerendere;
    [SerializeField]
    private List<Sprite> LevelObjects;
   
    private List<GameObject> GeneratedObj;
    public Image Previewimage1, Previewimage2, Previewimage3, Previewimage4;
    private int Objectlive = 0;
    private Vector3 timerPanelPos;
    public GameObject TimerPanel;
    public GameObject Gamecanvas, ZonePag,ZoneselectionPage, MainZone, Startpage;
    public Button closeButton;
    public AudioSource gameSound;
    public AudioClip RightAns, WrongAns;
    public AudioClip Shoot, wrong,Stretching;
    public GameObject PreviewCard;
    //============== COMPLETE GAME SCORE VARIABLE AND HELPING VARIABLE======================
    [Header("Time portion and score ")]
    [Space(10)]
    public float minut;
    public float second;
    private float Totaltimer, RunningTimer;
    private float sec;
    private float Mintvalue;
    private bool helpingbool = true;
    public Image Timerbar;
    public Text Timer;
    public Text CorrectGuesscount, Totalwaste;
    public Text TotalscoreText;
    public int SCore;
    private int correctGoal = 0;
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

    public Image PowerBar;
    [SerializeField]
    private float Maxstrecthvalue = 9.30f;
    //========================================== PRIVATE OBJECTS FOR GAME==================== 
    private List<string> Dustbins = new List<string>();
    [SerializeField]private List<string> ItemColleted = new List<string>();
    private List<int> ObjectScore = new List<int>();
    private List<int> is_correct = new List<int>();
    private List<string> CorrectOption = new List<string>();
    private List<string> DustbinsL2 = new List<string>();
    private List<string> ItemColletedL2 = new List<string>();
    private List<int> ObjectScoreL2 = new List<int>();
    private List<int> is_correctL2 = new List<int>();
    private List<string> CorrectOptionL2 = new List<string>();


    public GameObject onstrike;
    private bool closeGame;
    [HideInInspector]
    public bool LevelCompleted;
    [SerializeField]
    private List<GameObject> RowsLevel1;
    [SerializeField]
    private List<GameObject> RowsLevel2;
 
    // DASHBOARD VARIABLES=================================//
    [Header("====DASHBAORD DATA=====")]
    [Space(10)]
    public Transform LevelParent;
    public GameObject DatarowPrefeb;
    public Sprite Correct, Wrong, PCorrect, CorrectOpt;
    private GameObject gb;
    public Text OverallScore;
    public bool level1, level2;
    public Stage2ZoneHandler stage2handler;
    public ThrowWasteHandler level1Reference;
    public GameObject level1obj, level2obj, level2Mainpage, Leveel2mainpage;
    private ZoneDataPost Loglevel1,LogLevel2;
    private int wrongans;
    public List<GameObject> wasteItems;
    public GameObject wrongeffect;
    public GameObject initialSmoke, moderateSmoke, HighSmoke;
    private List<GameObject> dashboardL1Rows = new List<GameObject>(), dashboardL2Rows = new List<GameObject>();
    public GameObject DotPrefeb;
    public Transform Dotparent;
    private void Awake()
    {


    }

    private void OnEnable()
    {
        wasteItems.ForEach(x =>
        {
            x.SetActive(false);
        });
        TimePaused = true;
        Mintvalue = minut;
        sec = second;
        Objectlive = 0;
        TotalscoreText.text = "0";
        Totaltimer = (Mintvalue * 60) + second;
        Debug.Log("time data time " + Mintvalue + " sec " + second + "total time " + Totaltimer);
        RunningTimer = Totaltimer;
        initialSmoke.SetActive(false);
        moderateSmoke.SetActive(false);
        HighSmoke.SetActive(false);
        wrongans = 0;
        GeneratedObj = new List<GameObject>();
        RowsLevel1 = new List<GameObject>();
        RowsLevel2 = new List<GameObject>();
        gameclose = true;
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(delegate { CloseGame(); });
        Gamecanvas.SetActive(true);
       
        StartCoroutine(initialdestory());
        initialsetup();
    }
    IEnumerator initialdestory()
    {
        for (int a = 0; a < LevelParent.childCount; a++)
        {
            DestroyImmediate(LevelParent.GetChild(a).gameObject);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnDisable()
    {
        sec = 0;
        Totaltimer = 0;
        RunningTimer = 0f;
        Mintvalue = 0;
    }
    void initialsetup()
    {

        ScoreKnob.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        helpingbool = true;
        closeGame = false;
        
        gameend  = false;
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
       
        
        Totalwaste.text = "0 / " + totalwasteCount.ToString();
        //CorrectGuesscount.text = "0";
        LevelObjects = new List<Sprite>(WasteObejctSprites.Length);
        for (int a = 0; a < WasteObejctSprites.Length; a++)
        {
            GameObject gb = Instantiate(WastePrefeb, WastePerent, false);
            gb.SetActive(false);
            gb.GetComponent<ProjectileDragging>().catapultLineBack = BackLine;
            gb.GetComponent<SpringJoint2D>().connectedBody = catapultback;
            gb.GetComponent<ProjectileDragging>().catapultLineFront = ForntLine;
            gb.GetComponent<ProjectileDragging>().dots = DotPrefeb;
            gb.GetComponent<ProjectileDragging>().ProjectionParent = Dotparent;
            gb.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = WasteObejctSprites[a];
            
            gb.GetComponent<ProjectileDragging>().ONstrike = onstrike;
            GeneratedObj.Add(gb);
            LevelObjects.Add(WasteObejctSprites[a]);
        }
        gameclose = false;
        correctGoal = 0;
        SCore = 0;
        Previewimage1.GetComponentInParent<BoxCollider2D>().enabled = true;
        Previewimage2.GetComponentInParent<BoxCollider2D>().enabled = true;
        Previewimage3.GetComponentInParent<BoxCollider2D>().enabled = true;
        Previewimage4.GetComponentInParent<BoxCollider2D>().enabled = true;
        Previewimage1.gameObject.SetActive(true);
        Previewimage2.gameObject.SetActive(true);
        Previewimage3.gameObject.SetActive(true);
        Previewimage4.gameObject.SetActive(true);
        SpriteUpdator(0);
        GeneratedObj[Objectlive].SetActive(true);
        PreviewCard.GetComponent<PreviewPageHandler>().CheckBar();
        TimePaused = false; 

    }

    void getRandomeSprite()
    {
        for (int a = 0; a < WasteObejctSprites.Length; a++)
        {
            Sprite temp = WasteObejctSprites[a];
            int randomindex = UnityEngine.Random.Range(1, WasteObejctSprites.Length);
            WasteObejctSprites[a] = WasteObejctSprites[randomindex];
            WasteObejctSprites[randomindex] = temp;
        }
    }

    void SpriteUpdator(int a)
    {
        Previewimage1.GetComponent<Image>().sprite = GeneratedObj[a].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Previewimage2.GetComponent<Image>().sprite = GeneratedObj[a + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Previewimage3.GetComponent<Image>().sprite = GeneratedObj[a + 2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Previewimage4.GetComponent<Image>().sprite = GeneratedObj[a + 3].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

    }
    void Start()
    {

    }

    void Update()
    {
        if (!TimePaused)
        {
            if (sec >= 0 && Mintvalue >= 0 && helpingbool)
            {
                sec = sec - Time.deltaTime;
                RunningTimer = RunningTimer - Time.deltaTime;
                Timerbar.fillAmount = RunningTimer / Totaltimer;
                if (sec.ToString("0").Length > 1)
                {
                    Timer.text = "0" + Mintvalue.ToString("0") + ":" + sec.ToString("0");
                }
                else
                {
                    Timer.text = "0" + Mintvalue.ToString("0") + ":" + "0" + sec.ToString("0");
                }

                if (sec.ToString("0") == "0" && Mintvalue >= 0)
                {
                    sec = 60;
                    Mintvalue = Mintvalue - 1;
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
        if (!closeGame)
        {
            PowerBar.fillAmount = GeneratedObj[Objectlive].GetComponent<ProjectileDragging>().catapultToMouse.sqrMagnitude / Maxstrecthvalue;
        }

    }

    void ThrowWasteObjectactiveStatus()
    {
       
        if (Objectlive <= GeneratedObj.Count-1)
        {
            if (!GeneratedObj[Objectlive].activeInHierarchy)
            {
                Objectlive++;
                if(Objectlive >= GeneratedObj.Count)
                {

                }
                else
                {
                    GeneratedObj[Objectlive].SetActive(true);
                    if (Objectlive < GeneratedObj.Count)
                    {
                        Previewimage1.GetComponent<Image>().sprite = GeneratedObj[Objectlive].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

                        if (Objectlive < GeneratedObj.Count - 1)
                        {
                            
                            Previewimage2.GetComponent<Image>().sprite = GeneratedObj[Objectlive + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                        {
                            Previewimage2.GetComponentInParent<BoxCollider2D>().enabled = false;
                            Previewimage2.gameObject.SetActive(false);
                        }
                        if (Objectlive < GeneratedObj.Count - 2)
                        {

                            Previewimage3.GetComponent<Image>().sprite = GeneratedObj[Objectlive + 2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

                        }
                        else
                        {
                            Previewimage3.GetComponentInParent<BoxCollider2D>().enabled = false;
                            Previewimage3.gameObject.SetActive(false);
                        }

                        if (Objectlive < GeneratedObj.Count - 3)
                        {
                            Previewimage4.GetComponent<Image>().sprite = GeneratedObj[Objectlive + 3].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

                        }
                        else
                        {
                            Previewimage4.GetComponentInParent<BoxCollider2D>().enabled = false;
                            Previewimage4.gameObject.SetActive(false);
                        }

                    }
                }
            }
        }
        if (Objectlive == GeneratedObj.Count && !gameend)
        {
            closeGame = true;
            Previewimage1.GetComponentInParent<BoxCollider2D>().enabled = false;
            Previewimage2.GetComponentInParent<BoxCollider2D>().enabled = false;
            Previewimage3.GetComponentInParent<BoxCollider2D>().enabled = false;
            Previewimage4.GetComponentInParent<BoxCollider2D>().enabled = false;
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
        gameclose = true;
        BackLineRendere.enabled = false;
        ForntLinerendere.enabled = false;
        yield return new WaitForSeconds(0.1f);

        GeneratedObj.ForEach(x =>
        {
            x.SetActive(false);
        });
        yield return new WaitForSeconds(0.1f);
        LevelCompleted = true;
        stage2handler.checkLevelStatus();

    }

    public void checkCollidedAns(string dustbin, string collidername, GameObject dustbinobj)
    {
        
        if (level1)
        {
            if (dustbin != "wall")
            {
                var CollidedDustbin = WasteCollectionList.FirstOrDefault(x => x.Key.Equals(dustbin, System.StringComparison.OrdinalIgnoreCase));
                var iscorrectWaste = CollidedDustbin.Value.Any(x => x.name.Equals(collidername, System.StringComparison.OrdinalIgnoreCase));

                if (iscorrectWaste)
                {
                    gameSound.clip = RightAns;
                    gameSound.Play();
                    SCore += 10;
                    TotalscoreText.text = SCore.ToString();
                    checkScore = true;
                    is_correct.Add(1);
                    ObjectScore.Add(10);
                    CorrectOption.Add(dustbin);
                }
                else
                {

                    var RelatedDustbin = (from k in WasteCollectionList
                                          where k.Value.Any(x => x.name.Equals(collidername, System.StringComparison.OrdinalIgnoreCase))
                                          select k.Key).FirstOrDefault();
                    CorrectOption.Add(RelatedDustbin);
                    gameSound.clip = WrongAns;
                    gameSound.Play();
                    is_correct.Add(0);
                    ObjectScore.Add(0);
                    wrongans++;
                    StartCoroutine(WrongObjectTask(collidername));

                }
                Dustbins.Add(dustbin);
            }
            else
            {
                string RelatedDustbin = "null";
                CorrectOption.Add(RelatedDustbin);
                gameSound.clip = WrongAns;
                gameSound.Play();
                is_correct.Add(0);
                ObjectScore.Add(0);
                string nullDustbin = "null";
                Dustbins.Add(nullDustbin);
                wrongans++;
                StartCoroutine(WrongObjectTask(collidername));
            }
            correctGoal++;
            Totalwaste.text = correctGoal.ToString() + " / " + totalwasteCount.ToString();
            ItemColleted.Add(collidername);
        }
        //
       
        else
        {
            
            if (dustbin != "wall")
            {
                var CollidedDustbin = WasteCollectionList.FirstOrDefault(x => x.Key.Equals(dustbin, System.StringComparison.OrdinalIgnoreCase));
                var iscorrectWaste = CollidedDustbin.Value.Any(x => x.name.Equals(collidername, System.StringComparison.OrdinalIgnoreCase));

                if (iscorrectWaste)
                {
                    gameSound.clip = RightAns;
                    gameSound.Play();
                    SCore += 10;
                    TotalscoreText.text = SCore.ToString();
                    checkScore = true;
                    is_correctL2.Add(1);
                    ObjectScoreL2.Add(10);
                    CorrectOptionL2.Add(dustbin);
                }
                else
                {

                    var RelatedDustbin = (from k in WasteCollectionList
                                          where k.Value.Any(x => x.name.Equals(collidername, System.StringComparison.OrdinalIgnoreCase))
                                          select k.Key).FirstOrDefault();
                    CorrectOptionL2.Add(RelatedDustbin);
                    gameSound.clip = WrongAns;
                    gameSound.Play();
                    is_correctL2.Add(0);
                    ObjectScoreL2.Add(0);
                    wrongans++;
                    StartCoroutine(WrongObjectTask(collidername));

                }
                DustbinsL2.Add(dustbin);
            }
            else
            {
                string RelatedDustbin = "null";
                CorrectOptionL2.Add(RelatedDustbin);
                gameSound.clip = WrongAns;
                gameSound.Play();
                is_correctL2.Add(0);
                ObjectScoreL2.Add(0);
                string nullDustbin = "null";
                wrongans++;
                DustbinsL2.Add(nullDustbin);
                StartCoroutine(WrongObjectTask(collidername));
            }
            correctGoal++;
            Totalwaste.text = correctGoal.ToString() + " / " + totalwasteCount.ToString();
            ItemColletedL2.Add(collidername);
        }
        
    }

    IEnumerator WrongObjectTask(string ObjectName)
    {
        yield return new WaitForSeconds(0.1f);
        wasteItems.ForEach(x =>
        {
            if (x.gameObject.name.Equals(ObjectName))
            {
               wrongeffect.transform.localPosition = x.gameObject.transform.localPosition;
               x.gameObject.SetActive(true);
               wrongeffect.SetActive(true);
            }
           
        });
        if(wrongans == 2)
        {
            initialSmoke.SetActive(true);
        }
        if(wrongans == 5)
        {
            moderateSmoke.SetActive(true);
        }
        if(wrongans == 7)
        {
            HighSmoke.SetActive(true);
        }

    }
    IEnumerator resetEffect(GameObject dustbinobj)
    {
        yield return new WaitForSeconds(1f);
        dustbinobj.GetComponent<SpriteRenderer>().color = Color.white;
        dustbinobj.GetComponent<ShakeEffect>().enabled = false;
    }

    public void CloseGame()
    {
        closeGame = true;
        gameclose = true;
        StartCoroutine(CloserGame());
    }
    
    public IEnumerator destoryObj()
    {
        GeneratedObj.ForEach(DestroyImmediate);
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator CloserGame()
    {
        Objectlive = 0;
        iTween.MoveTo(TimerPanel, timerPanelPos, 0.5f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(destoryObj());
        ResetTask();
        yield return new WaitForSeconds(0.5f);
        RowsLevel1.Clear();
        RowsLevel2.Clear();
        if (level2)
        {
            level1Reference.CloserGame();
        }
        yield return new WaitForSeconds(1f);
        GeneratedObj.Clear();
        Gamecanvas.SetActive(false);
        //ZonePag.SetActive(true);
        ZoneselectionPage.SetActive(true);
        Startpage.SetActive(true);
        Startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        MainZone.SetActive(false);
        level1obj.SetActive(false);
        level2obj.SetActive(false);
        level2Mainpage.SetActive(false);
        Leveel2mainpage.SetActive(false);
    }

    IEnumerator PostZoneData()
    {
        yield return new WaitForSeconds(0.1f);
        ZoneDataPost Zonedata = new ZoneDataPost();
        
    }

    public void generateDashboardL1()
    {
        StartCoroutine(MakeDashbaord());
        
    }
    public void generateDashboardL2()
    {
        StartCoroutine(MakeLevel2Dashbaord());
    }

    //******************* Zone dashboard method*******************
    IEnumerator MakeDashbaord()
    {
        int a1 = 0;
        yield return new WaitForSeconds(0.1f);
        OverallScore.text = SCore.ToString();
        var distinctElements = ItemColleted != null && ItemColleted.Count > 0 ? LevelObjects.Where(x => !ItemColleted.Contains(x.name)).Select(x => x.name).ToList() : new List<string>();

        for(int b =0;b < LevelObjects.Count; b++)
        {
            gb = Instantiate(DatarowPrefeb, LevelParent, false);
            RowsLevel1.Add(gb);
        }

        for (int a = 0; a < WasteObejctSprites.Length; a++)
        {
            if (a < ItemColleted.Count)
            {
                RowsLevel1[a].transform.GetChild(0).gameObject.GetComponent<Text>().text = ItemColleted[a];
                RowsLevel1[a].transform.GetChild(4).gameObject.GetComponent<Text>().text = ObjectScore[a].ToString();

                if (ObjectScore[a] == 10)
                {
                    if (Dustbins[a].ToLower() == "paper")
                    {
                        RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;

                    }
                    if (Dustbins[a].ToLower() == "plastic")
                    {
                        RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (Dustbins[a].ToLower() == "organic")
                    {
                        RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (Dustbins[a] == "null")
                    {
                        RowsLevel1[a].transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel1[a].transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel1[a].transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                    }
                }
                else
                {
                    if (Dustbins[a].ToLower() == "paper")
                    {
                        RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOption[a];
                        if (CorrectOption[a].ToLower() == "paper")
                        {
                            RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOption[a].ToLower() == "plastic")
                        {
                            RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOption[a].ToLower() == "organic")
                        {
                            RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                    }
                    if (Dustbins[a].ToLower() == "plastic")
                    {
                        RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOption[a];
                        if (CorrectOption[a].ToLower() == "paper")
                        {
                            RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;


                        }
                        if (CorrectOption[a].ToLower() == "plastic")
                        {
                            RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOption[a].ToLower() == "organic")
                        {
                            RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                    }
                    if (Dustbins[a].ToLower() == "organic")
                    {
                        RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOption[a];
                        if (CorrectOption[a].ToLower() == "paper")
                        {
                            RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel1[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOption[a].ToLower() == "plastic")
                        {
                            RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel1[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOption[a].ToLower() == "organic")
                        {
                            RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel1[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                    }


                    if (Dustbins[a] == "null")
                    {
                        RowsLevel1[a].transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel1[a].transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel1[a].transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                    }
                }

               
            }
            else
            {
                Dustbins.Add("null");
                ObjectScore.Add(0);
                is_correct.Add(0);
                CorrectOption.Add("null");
                ItemColleted.Add(distinctElements[a1]);
                RowsLevel1[a].transform.GetChild(0).gameObject.GetComponent<Text>().text = distinctElements[a1];
                RowsLevel1[a].transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                RowsLevel1[a].transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                RowsLevel1[a].transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                RowsLevel1[a].transform.GetChild(4).gameObject.GetComponent<Text>().text = "0";
                a1++;
            }
        }

        MakePostData();

        //LevelCompleted = true;
        //stage2handler.checkLevelStatus();

    }

    void MakePostData()
    {
       
        int l = 0;
        if (ItemColleted.Count > 0)
        {
            ItemColleted.ForEach(x =>
            {
                Loglevel1 = new ZoneDataPost()
                {
                    item_collected = ItemColleted[l],
                    score = ObjectScore[l],
                    is_right = is_correct[l],
                    correct_option = CorrectOption[l],
                    id_content = stage2handler.id_game_content,
                    id_level = 2,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = Dustbins[l],
                    id_room = stage2handler.RoomIds[0]
                };
                l = l + 1;
                stage2handler.logs.Add(Loglevel1);

            });
        }
        else
        {
            GeneratedObj.ForEach(x =>
            {
                Loglevel1 = new ZoneDataPost()
                {
                    item_collected = WasteObejctSprites[l].name,
                    score = ObjectScore[l],
                    is_right = is_correct[l],
                    correct_option = CorrectOption[l],
                    id_content = stage2handler.id_game_content,
                    id_level = 2,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = Dustbins[l],
                    id_room = stage2handler.RoomIds[0]
                };
                l = l + 1;
                stage2handler.logs.Add(Loglevel1);

            });
        }

        

    }

    void MakePostDataLevel2()
    {
        Debug.Log("list count " + ItemColletedL2.Count);
        int l = 0;
        if (ItemColletedL2.Count > 0)
        {
            ItemColletedL2.ForEach(x =>
            {
                LogLevel2 = new ZoneDataPost()
                {
                    item_collected = ItemColletedL2[l],
                    score = ObjectScoreL2[l],
                    is_right = is_correctL2[l],
                    correct_option = CorrectOptionL2[l],
                    id_content = stage2handler.id_game_content,
                    id_level = 2,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = DustbinsL2[l],
                    id_room = stage2handler.RoomIds[1]
                };
                l = l + 1;
                stage2handler.logs.Add(LogLevel2);

            });
        }
        else
        {
            GeneratedObj.ForEach(x =>
            {
                Debug.Log("waste name " + WasteObejctSprites[l].name);
                LogLevel2 = new ZoneDataPost()
                {
                    item_collected = WasteObejctSprites[l].name,
                    score = ObjectScoreL2[l],
                    is_right = is_correctL2[l],
                    correct_option = CorrectOptionL2[l],
                    id_content = stage2handler.id_game_content,
                    id_level = 2,
                    status = "a",
                    updated_date_time = DateTime.Now,
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = DustbinsL2[l],
                    id_room = stage2handler.RoomIds[1]
                };
                l = l + 1;
                stage2handler.logs.Add(LogLevel2);

            });
        }

       
        string data = Newtonsoft.Json.JsonConvert.SerializeObject(stage2handler.logs);
        stage2handler.PostZoneData(data);
        Debug.Log("final log " + data);

    }
    IEnumerator MakeLevel2Dashbaord()
    {
       
        yield return new WaitForSeconds(0.1f);
        int a1 = 0;
        var distinctElements = ItemColletedL2 != null && ItemColletedL2.Count > 0 ? LevelObjects.Where(x => !ItemColletedL2.Contains(x.name)).Select(x => x.name).ToList() : new List<string>();
        OverallScore.text = SCore.ToString();
        for (int b = 0; b < LevelObjects.Count; b++)
        {
            gb = Instantiate(DatarowPrefeb, LevelParent, false);
            RowsLevel2.Add(gb);
        }

        for (int a = 0; a < WasteObejctSprites.Length; a++)
        {
            if (a < ItemColletedL2.Count)
            {

                RowsLevel2[a].transform.GetChild(0).gameObject.GetComponent<Text>().text = ItemColletedL2[a];
                RowsLevel2[a].transform.GetChild(5).gameObject.GetComponent<Text>().text = ObjectScoreL2[a].ToString();

                if (ObjectScoreL2[a] == 10)
                {
                    if (DustbinsL2[a].ToLower() == "biohazard")
                    {
                        RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;

                    }
                    if (DustbinsL2[a].ToLower() == "ewaste")
                    {
                        RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (DustbinsL2[a].ToLower() == "metal")
                    {
                        RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (DustbinsL2[a].ToLower() == "glass")
                    {
                        RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (DustbinsL2[a] == "null")
                    {
                        RowsLevel2[a].transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel2[a].transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel2[a].transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel2[a].transform.GetChild(4).gameObject.GetComponent<Text>().text = "----";
                    }
                }
                else
                {
                    if (DustbinsL2[a].ToLower() == "biohazard")
                    {
                        RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptionL2[a];
                        if (CorrectOptionL2[a].ToLower() == "biohazard")
                        {
                            RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "ewaste")
                        {
                            RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "metal")
                        {
                            RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "glass")
                        {
                            RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                    }
                    if (DustbinsL2[a].ToLower() == "ewaste")
                    {
                        RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptionL2[a];
                        if (CorrectOptionL2[a].ToLower() == "biohazard")
                        {
                            RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "ewaste")
                        {
                            RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "metal")
                        {
                            RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "glass")
                        {
                            RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                    }
                    if (DustbinsL2[a].ToLower() == "metal")
                    {
                        RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptionL2[a];
                        if (CorrectOptionL2[a].ToLower() == "biohazard")
                        {
                            RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "ewaste")
                        {
                            RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "metal")
                        {
                            RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "glass")
                        {
                            RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                    }
                    if (DustbinsL2[a].ToLower() == "glass")
                    {
                        RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptionL2[a];
                        if (CorrectOptionL2[a].ToLower() == "biohazard")
                        {
                            RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "ewaste")
                        {
                            RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "metal")
                        {
                            RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                        if (CorrectOptionL2[a].ToLower() == "glass")
                        {
                            RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                            RowsLevel2[a].transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                        }
                    }


                    if (DustbinsL2[a] == "null")
                    {
                        RowsLevel2[a].transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel2[a].transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel2[a].transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                        RowsLevel2[a].transform.GetChild(4).gameObject.GetComponent<Text>().text = "----";
                    }
                }


            }
            else
            {
                DustbinsL2.Add("null");
                ObjectScoreL2.Add(0);
                is_correctL2.Add(0);
                CorrectOptionL2.Add("null");
                ItemColletedL2.Add(distinctElements[a1]);
                RowsLevel2[a].transform.GetChild(0).gameObject.GetComponent<Text>().text = distinctElements[a1];
                RowsLevel2[a].transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                RowsLevel2[a].transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                RowsLevel2[a].transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                RowsLevel2[a].transform.GetChild(4).gameObject.GetComponent<Text>().text = "----";
                RowsLevel2[a].transform.GetChild(5).gameObject.GetComponent<Text>().text = "0";
                a1++;

            }
        }
        MakePostDataLevel2();
    }

    public void ResetTask()
    {
        StartCoroutine(resetingtask());
    }
    IEnumerator resetingtask()
    {
        for (int a = 0; a < LevelParent.childCount; a++)
        {
            Destroy(LevelParent.GetChild(a).gameObject,0.05f);
        }
        yield return new WaitForSeconds(0.1f);
        Dustbins.Clear();
        ItemColleted.Clear();
        ObjectScore.Clear();
        is_correct.Clear();
        CorrectOption.Clear();
        DustbinsL2.Clear();
        ItemColletedL2.Clear();
        ObjectScoreL2.Clear();
        is_correctL2.Clear();
        CorrectOptionL2.Clear();
    }
}
