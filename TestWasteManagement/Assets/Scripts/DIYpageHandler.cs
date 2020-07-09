using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DIYpageHandler : MonoBehaviour
{
    // Start is called before the first frame updateNext
    public GameObject NextButton, BackButton;
    public List<GameObject> Pagelist;
    private int pagecounter=0;
    private int lastpagecounter=0;

    [Header("PDF file download section")]
    [Space(10)]
    public string Mainurl;
    public List<string> DiyUrls;
    private string PdfUrl;
    public GameObject PopUpmsgPage;

    void Start()
    {
        
      
    }
    private void OnEnable()
    {
       
        pagecounter = 0;
        for (int a = 0; a < Pagelist.Count; a++)
        {
            if (a == pagecounter)
            {
                Pagelist[a].SetActive(true);
            }
            else
            {
                Pagelist[a].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pagecounter == 0)
        {
            BackButton.SetActive(false);
            NextButton.SetActive(true);
        }
        else if(pagecounter >0 && pagecounter < Pagelist.Count - 1)
        {
            BackButton.SetActive(true);
            NextButton.SetActive(true);
        }
        else if(pagecounter < Pagelist.Count)
        {
            NextButton.SetActive(false);
        }
    }

    public void NextpageEnable()
    {
        lastpagecounter = pagecounter;
        pagecounter++;
      
        Pagelist[pagecounter].SetActive(true);
        Pagelist[lastpagecounter].SetActive(false);
    }

    public void BackPageEnable()
    {
        lastpagecounter = pagecounter;
        pagecounter--;
        Pagelist[pagecounter].SetActive(true);
        Pagelist[lastpagecounter].SetActive(false);
    }

    public void DownloadPdfFIle()
    {

        FileDownloader fileDownloader = new FileDownloader();

        //// This callback is triggered for DownloadFileAsync only
        //fileDownloader.DownloadProgressChanged += (sender, e) => Console.WriteLine("Progress changed " + e.BytesReceived + " " + e.TotalBytesToReceive);
        //// This callback is triggered for both DownloadFile and DownloadFileAsync
        //fileDownloader.DownloadFileCompleted += (sender, e) => Console.WriteLine("Download completed");
        PdfUrl = DiyUrls[pagecounter];
        string Diypage = "DIY" + (pagecounter + 1).ToString();
        string savingPath = "";
        string pathStart = "";
        if (UnityEngine.Application.platform == RuntimePlatform.Android)
        {
            if (UnityEngine.Application.platform == RuntimePlatform.Android)
            {
                 pathStart = "/storage/emulated/0/UrbanWasteManagement";
                if (Directory.Exists(pathStart) == false)
                {
                    Directory.CreateDirectory(pathStart);
                    savingPath = pathStart + "/" + Diypage + ".pdf";
                }
                else
                {
                    savingPath = Application.persistentDataPath + "/" + Diypage + ".pdf";
                }
            }
           
        }
        else
        {
            savingPath = UnityEngine.Application.persistentDataPath + "/" + Diypage + ".pdf";
        }
        Debug.Log("download file name  " + Diypage);
        fileDownloader.DownloadFileAsync(PdfUrl,savingPath);
        string msg = "You have successfully downloaded DIY "+ (pagecounter + 1) +" book!!";
        StartCoroutine(ShowMsgPop(msg));
    }
    IEnumerator ShowMsgPop(string msg)
    {
        yield return new WaitForSeconds(0.1f);
        PopUpmsgPage.transform.GetChild(0).GetComponent<Text>().text = msg;
        PopUpmsgPage.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        PopUpmsgPage.transform.GetChild(0).GetComponent<Text>().text = "";
        iTween.ScaleTo(PopUpmsgPage, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PopUpmsgPage.SetActive(false);

    }


    void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        if (e.Error == null)
        {
            //AllDone();
            Debug.Log("file downloaded");
        }
    }
}
