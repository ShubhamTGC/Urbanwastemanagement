using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MatchTheTile : MonoBehaviour
{
    public GameObject ButtonPrefeb;
    public Transform TileParent;
    public int TileCount, wasteCount;
    public Sprite DefaultSprite;
    private Sprite[] DustbinSprite, WasteSprite;
    [SerializeField]
    private List<Sprite> GeneratedSprite = new List<Sprite>();
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
    private int totalTimercount, RunningTimeCount;
    public Text TimerText;
    private Coloreffect colorglow;
    private bool helpingbool = true;
    void Start()
    {
        colorglow = FindObjectOfType<Coloreffect>();
        totalTimercount = Timercount * 60;
        second = 60;
        Timercount = Timercount - 1;
        RunningTimeCount = totalTimercount;
        colorglow.timertask();
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

    // Update is called once per frame
    void Update()
    {
        //if(second == 0 && Timercount == 0 && helpingbool) {
        //    helpingbool = false;
        //    colorglow.isdone = true; 
        //}
        if(second >= 0 && Timercount >= 0 && helpingbool)
        {
            second = second - Time.deltaTime;
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
            colorglow.isdone = true;

        }
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
            gb.name = a.ToString();
            gb.SetActive(true);
        }

    }

    void SpriteMixer()
    {
        for (int a = 0; a < GeneratedSprite.Count; a++)
        {
            Sprite temp = GeneratedSprite[a];
            int randomindex = Random.Range(1, GeneratedSprite.Count);
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
            FirstSelectedobj.GetComponent<Image>().sprite = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)];
            FirstSelectedobj.GetComponent<Button>().enabled = false;
            FirstGuessName = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)].name;
           
            FirstSelectedobj.tag = DustbinNames.Contains(FirstGuessName) ? "Dustbin" : "Waste";
        }
        else if (!SecondGuess)
        {
            SecondGuess = true;
            SecondSelectedobj = EventSystem.current.currentSelectedGameObject;
            SecondObjSprite = SecondSelectedobj.GetComponent<Image>().sprite;
            SecondSelectedobj.GetComponent<Image>().sprite = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)];
            SecondSelectedobj.GetComponent<Button>().enabled = false;
            SecondGuessName = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)].name;
            
            SecondSelectedobj.tag = DustbinNames.Contains(SecondGuessName) ? "Dustbin" : "Waste";

            StartCoroutine(CheckAns());

        }
    }

    private string FirstSelectedName => FirstSelectedobj.GetComponent<Image>().sprite.name;
    private string SecondSelectedName => SecondSelectedobj.GetComponent<Image>().sprite.name;

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
                FirstSelectedobj.GetComponent<Image>().enabled = false;
                SecondSelectedobj.GetComponent<Image>().enabled = false;
                FirstSelectedobj.GetComponent<Button>().enabled = false;
                SecondSelectedobj.GetComponent<Button>().enabled = false;
                FirstGuess = SecondGuess = false;
                CorrectGuess++;
                CorrectTiles.text = CorrectGuess.ToString();


            }
            else
            {
                Debug.Log(" Found object");
                FirstSelectedobj.GetComponent<Image>().sprite = FirstObjSprite;
                SecondSelectedobj.GetComponent<Image>().sprite = SecondObjSprite;
                FirstSelectedobj.GetComponent<Button>().enabled = true;
                SecondSelectedobj.GetComponent<Button>().enabled = true;
                FirstGuess = SecondGuess = false;
            }
        }
        else if(SecondSelectedobj.tag == "Waste" && FirstSelectedobj.tag == "Dustbin")
        {
            var wasteType = WasteCollection.FirstOrDefault(x => x.Key.Equals(FirstSelectedName, System.StringComparison.OrdinalIgnoreCase));
            var isBelongToWasteType = wasteType.Value.
                Any(x => x.name.Equals(SecondSelectedName, System.StringComparison.OrdinalIgnoreCase));
            if (isBelongToWasteType)
            {
                FirstSelectedobj.GetComponent<Image>().enabled = false;
                SecondSelectedobj.GetComponent<Image>().enabled = false;
                FirstGuess = SecondGuess = false;
                FirstSelectedobj.GetComponent<Button>().enabled = false;
                SecondSelectedobj.GetComponent<Button>().enabled = false;
                CorrectGuess++;
                CorrectTiles.text = CorrectGuess.ToString();

            }
            else
            {
                Debug.Log(" Found object");
                FirstSelectedobj.GetComponent<Image>().sprite = FirstObjSprite;
                SecondSelectedobj.GetComponent<Image>().sprite = SecondObjSprite;
                FirstSelectedobj.GetComponent<Button>().enabled = true;
                SecondSelectedobj.GetComponent<Button>().enabled = true;
                FirstGuess = SecondGuess = false;
            }
        }
        else
        {
            Debug.Log("Found object");
            FirstSelectedobj.GetComponent<Image>().sprite = FirstObjSprite;
            SecondSelectedobj.GetComponent<Image>().sprite = SecondObjSprite;
            FirstSelectedobj.GetComponent<Button>().enabled = true;
            SecondSelectedobj.GetComponent<Button>().enabled = true;
            FirstGuess = SecondGuess = false;
        }
        //{
        //   
        //}

        if (CorrectGuess == TileCount)
        {
            Debug.Log("wind");
        }
    }
}
