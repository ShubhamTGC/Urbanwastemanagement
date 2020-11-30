using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickEvent : MonoBehaviour
{
    private int tapcount = 0;
    public Generationlevel mainLevel;
    public GameObject Zonesrelated;

    [SerializeField]private float double_timer = 0.2f;
    private float lastclicktimer;



    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    tapcount = eventData.clickCount;
    //    if (tapcount == 2)
    //    {
    //        mainLevel.sublevelmethod(Zonesrelated);
    //    }
           
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            float timeclick = Time.time - lastclicktimer;
            if(timeclick <= double_timer)
            {
                Debug.Log("double click");
            }
            else
            {
                Debug.Log("single click");
            }
        }
    }
}
