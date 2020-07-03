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
        //string image_path = EditorUtility.OpenFilePanel("Select Image to Upload", "", "jpg");
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string[] image_path = StandaloneFileBrowser.OpenFilePanel("Select Image to Upload", "", "jpg", false);
            if (image_path.Length != 0)
            {
                SelectedBtn = preview_btn;
                byte[] image_data = File.ReadAllBytes(image_path[0]);
                StartCoroutine(SaveImageToServer.instance.show_image_mathod(WriteByte(image_data), SelectedBtn));
                //string imagestr = Convert.ToBase64String(image_data);
                //Texture2D texture = new Texture2D(1, 1);
                //texture.LoadImage(Convert.FromBase64String(imagestr));
                //Sprite btn_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                //this.gameObject.GetComponent<Image>().sprite = btn_sprite;
            }
        }
      

        //screen  capture need to work=============================================//
        //Texture2D texture = new Texture2D(1200,600, TextureFormat.RGB24, false);
        ////Read the pixels in the Rect starting at 0,0 and ending at the screen's width and height
        //texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        //texture.Apply();
        // Sprite btn_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        // getimage.sprite = btn_sprite;
        //string path = Application.persistentDataPath + "/image.png";
        //Debug.Log(Application.persistentDataPath);
        //UnityEngine.ScreenCapture.CaptureScreenshot(path);

    }

    public byte[] WriteByte(byte[] t)
    {

        return t;
    }


}
