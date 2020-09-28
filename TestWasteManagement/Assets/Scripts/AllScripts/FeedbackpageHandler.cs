using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using SFB;
using System.IO;
using System;

public class FeedbackpageHandler : MonoBehaviour
{
    public string MainUrl, FeedBackPostApi;
    public InputField UserText, UserEmailId;
    public Button Send;
    public List<Toggle> FeedbackOptions;
    private string FeedBackType;
    private string selectedImgPath;
    private Texture2D test, tex;
    public GameObject ImagePreview;
    public Transform PreviewHandler;
    public GameObject TextForImage;
    public RawImage LargePreview;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Send.interactable = UserText.text != "";
        Send.interactable = UserEmailId.text != "";
    }


    public void GetUserdata()
    {
        for(int a = 0; a < FeedbackOptions.Count; a++)
        {
            if(FeedbackOptions[a].isOn == true)
            {
                FeedBackType = FeedbackOptions[a].name;
            }
        }
        var EmailAddress = UserEmailId.text;
        var userInfo = UserText.text;
        Debug.Log(" User inserted data " + FeedBackType +" " + EmailAddress.ToString() + " " + userInfo.ToString());

    }


    public void ImageSelection()
    {
        int count = PreviewHandler.transform.childCount;
        if(count > 0)
        {
            for(int a = 0; a < count; a++)
            {
                Destroy(PreviewHandler.transform.GetChild(a).gameObject);
            }
        }
        StartCoroutine(GetImageTask());
       
    }
    IEnumerator GetImageTask()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {

                if (path != null)
                {
                    selectedImgPath = path;
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
                    if (texture != null)
                    {
                        texture.name = "test.png";
                        test = texture;
                        tex = texture;
                        Sprite ImageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        GameObject gb = Instantiate(ImagePreview);
                        gb.transform.SetParent(PreviewHandler);
                        gb.GetComponent<Image>().sprite = ImageSprite;
                        gb.name = "preview";
                        gb.SetActive(true);
                        gb.GetComponent<ShowimagePreview>().PreviewTexture = texture;
                        gb.GetComponent<ShowimagePreview>().canvas = this.gameObject.transform;
                        TextForImage.SetActive(false);
                    }
                }


            }, "Select Your Image", "image/jpeg");
        }
        else
        {
            string[] image_path = StandaloneFileBrowser.OpenFilePanel("Select Image to Upload", "", "jpg", false);
            if (image_path.Length != 0)
            {

                byte[] image_data = File.ReadAllBytes(image_path[0]);
                yield return new WaitForSeconds(0.1f);
                Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGB24, true);
               
                texture.LoadImage(image_data);
                texture.Apply();
                texture.name = "test.png";
                test = texture;
                Texture2D newScreenshot = ScaleTexture(texture, 256, 256);
                var bytes = newScreenshot.EncodeToPNG();
                string imagestr = Convert.ToBase64String(bytes);
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(Convert.FromBase64String(imagestr));
                Sprite btn_sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                GameObject gb = Instantiate(ImagePreview, PreviewHandler, false);
                gb.name = "preview";
                gb.GetComponent<BoxCollider2D>().enabled = false;
                gb.GetComponent<Image>().sprite = btn_sprite;
                gb.GetComponent<ShowimagePreview>().PreviewTexture = texture;
                gb.GetComponent<ShowimagePreview>().canvas = this.gameObject.transform;
                gb.GetComponent<BoxCollider2D>().enabled = true;
                gb.SetActive(true);
                TextForImage.SetActive(false);

            }
        }
    }


    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth),
                              incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
}
