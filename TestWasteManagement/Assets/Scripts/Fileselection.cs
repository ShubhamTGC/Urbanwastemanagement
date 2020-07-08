using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Fileselection : MonoBehaviour
{
    // Start is called before the first frame update
    public static ClickAndGetImage instance;
    public GameObject SelectedBtn;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    //[MenuItem("Example/Overwrite Texture")]
    public  void Apply(GameObject preview_btn)
    {
        //===================image selection working=========================================//
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string[] image_path = StandaloneFileBrowser.OpenFilePanel("Select Image to Upload", "", "jpg", false);
            if (image_path.Length != 0)
            {
                SelectedBtn = preview_btn;
                byte[] image_data = File.ReadAllBytes(image_path[0]);
                StartCoroutine(SaveImageToServer.instance.show_image_mathod(WriteByte(image_data), SelectedBtn));
            
            }
        }
      


    }

    public byte[] WriteByte(byte[] t)
    {

        return t;
    }


}
