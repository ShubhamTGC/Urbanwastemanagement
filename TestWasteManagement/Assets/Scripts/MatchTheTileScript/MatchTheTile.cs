using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MatchTheTile : MonoBehaviour
{
    public GameObject ButtonPrefeb;
    public Transform TileParent;
    public int TileCount, wasteCount;
    public Sprite DefaultSprite,WrongSprite,RightSprite, BlankTile;
    private Sprite[] DustbinSprite, WasteSprite;
    [SerializeField]
    private List<Sprite> GeneratedSprite = new List<Sprite>();
    private List<GameObject> GeneratedTiles = new List<GameObject>();
    public string DustbinPath, WastePath;

    //========== TILE GAME MAIN LOGIC VARIABLES================
    private bool FirstGuess, SecondGuess;
    private GameObject FirstSelectedobj, SecondSelectedobj;
    private string FirstGuessName, SecondGuessName;
    private Sprite FirstObjSprite, SecondObjSprite;
    private int CorrectGuess;
    public List<string> DustbinNames;

    public List<Sprite> paper, glass, biohazard, ewaste, plastic, organic, metal;
    private List<KeyValuePair<string, List<Sprite>>> WasteCollection;


    //==========TIMER AND SCORE PANEL DATRA ==========================
    public Text TotalTils, CorrectTiles;
    public int Timercount;
    private float second;
    private float totalTimercount, RunningTimeCount;
    public Text TimerText;
    public Image TimerImage;
    private Coloreffect colorglow;
    private bool helpingbool = true;
    public GameObject TimesUp;
    //======================== SCORE CAPTURING ELEMENTS ==================
    public GameObject ScoreKnob;
    private float knobAngle;
    private bool ScoreCheckNow = false;
    [SerializeField]
    private float BonusGameScore;
    public Text ScoreText;
    private int ScoreConuter;
    public GameObject ZoneA, ZoneB;


    void Start()
    {
        // colorglow = FindObjectOfType<Coloreffect>();
      

    }

     void OnEnable()
    {
        helpingbool = true;
        totalTimercount = Timercount * 60;
        second = 60;
        Timercount = Timercount - 1;
        RunningTimeCount = totalTimercount;
        //colorglow.timertask();
        TotalTils.text = TileCount.ToString();
        DustbinSprite = Resources.LoadAll<Sprite>(DustbinPath);
        WasteSprite = Resources.LoadAll<Sprite>(WastePath);
        GamespriteGenerator();
        WasteCollection = new List<KeyValuePair<string, List<Sprite>>>()
        {
            new KeyValuePair<string, List<Sprite>>("Paper", paper),
            new KeyValuePair<string, List<Sprite>>("Glass", glass),
            new KeyValuePair<string, List<Sprite>>("Biohazard", biohazard),
            new KeyValuePair<string, List<Sprite>>("Ewaste", ewaste),
            new KeyValuePair<string, List<Sprite>>("Plastic", plastic),
            new KeyValuePair<string, List<Sprite>>("Organic", organic),
            new KeyValuePair<string, List<Sprite>>("Metal", metal)
        };
    }
    private void OnDisable()
    {
        TimerImage.fillAmount = 1f;
        //second = 0.0f;
        //Timercount = 0;
        RunningTimeCount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //if (second.ToString("0") == "0" && Timercount == 0 && helpingbool)
        //{
        //    helpingbool = false;
        //    TimesUp.SetActive(true);
        //    //colorglow.isdone = true;
        //}
        if (second >= 0.0f && Timercount >= 0 && helpingbool)
        {
            second = second - Time.deltaTime;
            RunningTimeCount = RunningTimeCount - Time.deltaTime;
            TimerImage.fillAmount = RunningTimeCount / totalTimercount;
            if (second.ToString("0").Length > 1)
            {
                TimerText.text = "0" + Timercount.ToString("0") + ":" + second.ToString("0");
            }
            else
            {
                TimerText.text = "0" + Timercount.ToString("0") + ":" + "0" + second.ToString("0");
            }
           
            if(second.ToString("0") ==  "0" && Timercount >=0)
            {
                second = 60;
                Timercount = Timercount - 1;
            }     
        }else if (helpingbool)
        {
            helpingbool = false;
            StartCoroutine(TimeUpTask());

        }

        if (ScoreCheckNow)
        {
            ScoreCheckNow = false;
            knobAngle =  ((float)ScoreConuter / (float)BonusGameScore) * 200;

        }
        var rotationAngle = Quaternion.Euler(0f, 0f, -knobAngle);
        ScoreKnob.GetComponent<RectTransform>().rotation = Quaternion.Lerp(ScoreKnob.GetComponent<RectTransform>().rotation, rotationAngle, 10 * 1 * Time.deltaTime);
    }

    IEnumerator TimeUpTask()
    {
        GeneratedTiles.ForEach(DestroyImmediate);
        yield return new WaitForSeconds(0.1f);
        TimesUp.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        iTween.ScaleTo(TimesUp, Vector3.zero, 0.5f);
        GeneratedSprite.Clear();
        Array.Clear(DustbinSprite, 0, DustbinSprite.Length);
        Array.Clear(WasteSprite, 0, WasteSprite.Length);
        ScoreConuter = 0;
        totalTimercount = 0f;
        CorrectGuess = 0;
        CorrectTiles.text = "0";
        yield return new WaitForSeconds(0.5f);
        TimesUp.SetActive(false);
        ZoneA.GetComponent<PolygonCollider2D>().enabled = true;
        ZoneB.GetComponent<PolygonCollider2D>().enabled = true;
        this.gameObject.SetActive(false);

    }


    void GamespriteGenerator()
    {
        int index = 0;
        for (int a = 0; a < TileCount; a++)
        {
            if (index == TileCount / 3)
            {
                index = 0;
            }
            GeneratedSprite.Add(DustbinSprite[index]);
            index++;
        }

        for (int b = 0; b < wasteCount; b++)
        {
            GeneratedSprite.Add(WasteSprite[b]);
        }
        SpriteMixer();
    }

    void TileSetup()
    {
        for (int a = 0; a < GeneratedSprite.Count; a++)
        {
            GameObject gb = Instantiate(ButtonPrefeb, TileParent, false);
            gb.GetComponent<Image>().sprite = DefaultSprite;
            gb.GetComponent<Button>().onClick.AddListener(delegate { TilesAction(); });
            GeneratedTiles.Add(gb);
            gb.name = a.ToString();
            gb.SetActive(true);
        }

    }

    void SpriteMixer()
    {
        for (int a = 0; a < GeneratedSprite.Count; a++)
        {
            Sprite temp = GeneratedSprite[a];
            int randomindex = UnityEngine.Random.Range(1, GeneratedSprite.Count);
            GeneratedSprite[a] = GeneratedSprite[randomindex];
            GeneratedSprite[randomindex] = temp;

        }
        TileSetup();
    }

    void TilesAction()
    {
        if (!FirstGuess)
        {
            //string buttonName = EventSystem.current.currentSelectedGameObject.name;
            FirstGuess = true;
            FirstSelectedobj = EventSystem.current.currentSelectedGameObject;
            FirstObjSprite = FirstSelectedobj.GetComponent<Image>().sprite;
            FirstSelectedobj.GetComponent<Image>().sprite = BlankTile;
            FirstSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)];
            FirstSelectedobj.transform.GetChild(0).gameObject.SetActive(true);
            FirstSelectedobj.GetComponent<Button>().enabled = false;
            FirstGuessName = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)].name;
           
            FirstSelectedobj.tag = DustbinNames.Contains(FirstGuessName) ? "Dustbin" : "Waste";
        }
        else if (!SecondGuess)
        {
            SecondGuess = true;
            SecondSelectedobj = EventSystem.current.currentSelectedGameObject;
            SecondObjSprite = SecondSelectedobj.GetComponent<Image>().sprite;
            SecondSelectedobj.GetComponent<Image>().sprite = BlankTile;
            SecondSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)];
            SecondSelectedobj.transform.GetChild(0).gameObject.SetActive(true);
            SecondSelectedobj.GetComponent<Button>().enabled = false;
            SecondGuessName = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)].name;
            
            SecondSelectedobj.tag = DustbinNames.Contains(SecondGuessName) ? "Dustbin" : "Waste";

            StartCoroutine(CheckAns());

        }
    }

    private string FirstSelectedName => FirstSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite.name;
    private string SecondSelectedName => SecondSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite.name;

    IEnumerator CheckAns()
    {
        yield return new WaitForSeconds(0.5f);
        if (FirstSelectedobj.tag == "Waste" && SecondSelectedobj.tag == "Dustbin")
        {
            var wasteType = WasteCollection.FirstOrDefault(x => x.Key.Equals(SecondSelectedName, System.StringComparison.OrdinalIgnoreCase));
            var isBelongToWasteType = wasteType.Value.
                Any(x => x.name.Equals(FirstSelectedName, System.StringComparison.OrdinalIgnoreCase));
            if (isBelongToWasteType)
            {
                FirstSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = RightSprite;
                SecondSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = RightSprite;
                FirstSelectedobj.GetComponent<Button>().enabled = false;
                SecondSelectedobj.GetComponent<Button>().enabled = false;
                FirstGuess = SecondGuess = false;
                ScoreCheckNow = true;
                ScoreConuter += 10;
                ScoreText.text = ScoreConuter.ToString();
                CorrectGuess++;
                CorrectTiles.text = CorrectGuess.ToString();


            }
            else
            {
                Debug.Log(" Found object");
                FirstSelectedobj.transform.GetChild(0).gameObject.SetActive(false);
                SecondSelectedobj.transform.GetChild(0).gameObject.SetActive(false);
                FirstSelectedobj.GetComponent<Image>().sprite = WrongSprite;
                SecondSelectedobj.GetComponent<Image>().sprite = WrongSprite;
                StartCoroutine(ResetTiles(FirstSelectedobj, SecondSelectedobj));
            }
        }
        else if(SecondSelectedobj.tag == "Waste" && FirstSelectedobj.tag == "Dustbin")
        {
            var wasteType = WasteCollection.FirstOrDefault(x => x.Key.Equals(FirstSelectedName, System.StringComparison.OrdinalIgnoreCase));
            var isBelongToWasteType = wasteType.Value.
                Any(x => x.name.Equals(SecondSelectedName, System.StringComparison.OrdinalIgnoreCase));
            if (isBelongToWasteType)
            {
                FirstSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = RightSprite;
                SecondSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = RightSprite;
                FirstGuess = SecondGuess = false;
                FirstSelectedobj.GetComponent<Button>().enabled = false;
                SecondSelectedobj.GetComponent<Button>().enabled = false;
                ScoreCheckNow = true;
                ScoreConuter += 10;
                ScoreText.text = ScoreConuter.ToString();
                CorrectGuess++;
                CorrectTiles.text = CorrectGuess.ToString();

            }
            else
            {
                Debug.Log(" Found object");
                FirstSelectedobj.transform.GetChild(0).gameObject.SetActive(false);
                SecondSelectedobj.transform.GetChild(0).gameObject.SetActive(false);
                FirstSelectedobj.GetComponent<Image>().sprite = WrongSprite;
                SecondSelectedobj.GetComponent<Image>().sprite = WrongSprite;
                StartCoroutine(ResetTiles(FirstSelectedobj, SecondSelectedobj));
            }
        }
        else
        {
            Debug.Log("Found object");
            FirstSelectedobj.transform.GetChild(0).gameObject.SetActive(false);
            SecondSelectedobj.transform.GetChild(0).gameObject.SetActive(false);
            FirstSelectedobj.GetComponent<Image>().sprite = WrongSprite;
            SecondSelectedobj.GetComponent<Image>().sprite = WrongSprite;
            StartCoroutine(ResetTiles(FirstSelectedobj, SecondSelectedobj));
          
        }


        if (CorrectGuess == TileCount)
        {
            Debug.Log("wind");
        }
    }

    IEnumerator ResetTiles(GameObject FrstObj,GameObject secondObj)
    {
        yield return new WaitForSeconds(0.4f);
        FirstSelectedobj.GetComponent<Image>().sprite = DefaultSprite;
        SecondSelectedobj.GetComponent<Image>().sprite = DefaultSprite;
        FirstSelectedobj.GetComponent<Button>().enabled = true;
        SecondSelectedobj.GetComponent<Button>().enabled = true;
        FirstGuess = SecondGuess = false;
    }
}
