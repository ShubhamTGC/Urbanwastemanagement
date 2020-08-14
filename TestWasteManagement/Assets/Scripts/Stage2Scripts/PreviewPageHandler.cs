using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PreviewPageHandler : MonoBehaviour
{
    public GameObject ShowButtonLeft, ShowButtonRight;
    public Sprite downArrow, UpArrow;
    public GameObject PreviewBar, BarTarget;
    [HideInInspector]public Vector3 initialpos, Targetpos;
    [SerializeField]
    private float moveTime;


    private void Awake()
    {
    
    }
    void Start()
    {
       
    }
    private void OnEnable()
    {
        initialpos = PreviewBar.GetComponent<RectTransform>().localPosition;
        Targetpos = BarTarget.GetComponent<RectTransform>().localPosition;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void PreviewBarTask()
    {
     
        if (PreviewBar.GetComponent<RectTransform>().localPosition.Equals(initialpos))
        {
            ShowButtonLeft.GetComponent<Image>().sprite = downArrow;
            ShowButtonRight.GetComponent<Image>().sprite = downArrow;
            StartCoroutine(MoveBar(Targetpos));
        }
        else
        {
            ShowButtonLeft.GetComponent<Image>().sprite = UpArrow;
            ShowButtonRight.GetComponent<Image>().sprite = UpArrow;
            StartCoroutine(MoveBar(initialpos));
        }
    }

    IEnumerator MoveBar(Vector3  Pos)
    {
        iTween.MoveTo(PreviewBar, iTween.Hash("position", Pos,"easeType", iTween.EaseType.linear,"isLocal",true, "time", moveTime));
        yield return new WaitForSeconds(moveTime + 0.2f);
    }


    public void CheckBar()
    {
        if (!PreviewBar.GetComponent<RectTransform>().localPosition.Equals(initialpos))
        {
            ShowButtonLeft.GetComponent<Image>().sprite = UpArrow;
            ShowButtonRight.GetComponent<Image>().sprite = UpArrow;
            StartCoroutine(MoveBar(initialpos));
        }
        //else
        //{
        //    ShowButtonLeft.GetComponent<Image>().sprite = UpArrow;
        //    ShowButtonRight.GetComponent<Image>().sprite = UpArrow;
        //    StartCoroutine(MoveBar(Targetpos));
        //}
    }
}
