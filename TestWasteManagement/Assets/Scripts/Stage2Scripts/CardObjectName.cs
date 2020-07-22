using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObjectName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        string ObjectName = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name;
        this.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = ObjectName;
        this.gameObject.transform.GetChild(1).gameObject.SetActive(true);

    }
    public void OnMouseExit()
    {
        this.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
