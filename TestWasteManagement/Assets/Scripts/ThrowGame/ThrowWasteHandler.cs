using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowWasteHandler : MonoBehaviour
{
    public int Objectcount;
    public GameObject WastePrefeb;
    public Transform WastePerent;
    public LineRenderer ForntLine, BackLine;
    public Rigidbody2D catapultback;
    private List<GameObject> GeneratedObj = new List<GameObject>();
    public Image Previewimage1, Previewimage2, Previewimage3;
    private int Objectlive = 0;
    private void Awake()
    {
        initialsetup();
    }

    void initialsetup()
    {
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
    }
    void Start()
    {

    }
    
    void Update()
    {
        if(Objectlive < GeneratedObj.Count-1)
        {
            if (Objectlive == GeneratedObj.Count-1)
            {
                Previewimage3.gameObject.SetActive(false);
                Previewimage2.gameObject.SetActive(false);
                Previewimage1.gameObject.SetActive(false);
            }
            if (!GeneratedObj[Objectlive].activeInHierarchy)
            {
                Objectlive++;
                GeneratedObj[Objectlive].SetActive(true);

                if(Objectlive < GeneratedObj.Count)
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

                   
                   
                    
                }
               
              
               
            }
        }
       
        
    }
}
