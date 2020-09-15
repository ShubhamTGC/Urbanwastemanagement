using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLargerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject imagePanel;
    public Transform canvas;
    public Image ActualImage;
    private bool Pointed,HelpingBool;
    private RaycastHit2D hit;
    private Vector2 mousepos;
    private GameObject gb;

    void Start()
    {
        HelpingBool = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenpt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos = new Vector2(screenpt.x, screenpt.y);
            hit = Physics2D.Raycast(mousepos, Vector2.zero);
            if(hit.collider.gameObject.name == this.gameObject.name && HelpingBool)
            {
                HelpingBool = false;
                StartCoroutine(LargetView());
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(hit.collider.gameObject.name == this.gameObject.name && !HelpingBool)
            {
                HelpingBool = true;
                StartCoroutine(SmallView());
            }
        }
          
    }


    IEnumerator LargetView()
    {
        gb = Instantiate(imagePanel, canvas, false);
        gb.SetActive(true);
        gb.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ActualImage.sprite;
        iTween.ScaleTo(gb, Vector3.one, 0.5f);
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator SmallView()
    {
        iTween.ScaleTo(gb, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        gb.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
        gb.SetActive(false);
        Destroy(gb);
    }

  


}
