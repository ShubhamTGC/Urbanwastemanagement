using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SaveImageToServer : MonoBehaviour
{
    string URL;
    public string Mainurl, upload_image_API;
    public GameObject closebtn, capturebtn, camera_preview;
    public GameObject capturebtn1;
    public Sprite default_sprite;
    public CameraManager cameraController;
    public Sprite image_sprite;
    public InputField user_text, plan_title;
    public Text respose_text;
    private byte[] bytes;
    private byte[] imageBytes;
    private byte[] Without_image;
    private string image_path;
    public Button save;
    public Text statusmsg;
    Texture2D test;
    private GameObject zone_data;
    private Zonehandler zone_handle;
    public Sprite boy, girl;
    public Image characater;
    public Text player_name, zone_name;
    public GameObject posting_msg;
    private GameObject show_img_obj;
    public Button skipbtn, nextbutton;
    private string zone;
    private int level_zone =0;
    public int Bigger_count;
    public List<byte[]> post_image_byte = new List<byte[]>(3);
    private List<string> User_plan = new List<string>(3), User_text = new List<string>(3);
    public static SaveImageToServer instance;
    public GameObject loadingAnim;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        Initialtask();
        Without_image = new byte[1];
        test = new Texture2D(128, 128, TextureFormat.RGB24, true);
        test.name = "test.png";
    }



     void OnEnable()
    {
       Initialtask();
    }

    void Initialtask()
    {
        //player_name.text = PlayerPrefs.GetString("username");

        //zone_name.text = PlayerPrefs.GetString("zonename");
        //zone = PlayerPrefs.GetString("zonename");
        //zone_data = GameObject.Find(zone_name.text);
        //if (zone.ToLower() == "homezone")
        //{
        //    level_zone = 1;
        //}
        //else if (zone.ToLower() == "schoolzone")
        //{
        //    level_zone = 2;
        //}
        PlayerPrefs.SetInt("level_value", level_zone);
        zone_handle = zone_data.GetComponent<Zonehandler>();
        Debug.Log("zone obejct " + zone_data.name);
        int character_data = PlayerPrefs.GetInt("characterType");
        if (character_data == 1)
        {
            characater.sprite = boy;
        }
        else if (character_data == 2)
        {
            characater.sprite = girl;
        }
        skipbtn.onClick.AddListener(delegate { zone_handle.zonedone(zone_handle.active_room_end); });
        nextbutton.onClick.AddListener(delegate { zone_handle.zonedone(zone_handle.active_room_end); });
    }
    void OnDisable()
    {
        //posting_msg.SetActive(false);
        camera_preview.SetActive(false);
        capturebtn1.gameObject.GetComponent<Image>().sprite = default_sprite;
        user_text.text = "";
        plan_title.text = "";
        level_zone = 0;
        zone = "";
        zone_data = null;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void saveimage()
    {
        show_img_obj = cameraController.click_button;
        StartCoroutine(SaveToServer(show_img_obj));
    }


    public IEnumerator SaveToServer(GameObject captureimage)
    {
        post_image_byte.Clear();
        closebtn.SetActive(false);
        capturebtn.SetActive(false);
        yield return new WaitForSeconds(0.1f);      
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, true);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        Sprite image_taken = SpriteFromTexture2D(tex);
        yield return new WaitForSeconds(1f);
        captureimage.gameObject.GetComponent<Image>().sprite = image_taken;
        yield return new WaitForSeconds(0.5f);
        camera_preview.gameObject.SetActive(false);
        // Encode texture into PNG

        Texture2D newScreenshot = ScaleTexture(tex, 256, 256);

        var bytes = newScreenshot.EncodeToPNG();
        post_image_byte.Add(bytes);
        tex.name = "test.Png";
        imageBytes = bytes;
        test = tex;
        closebtn.SetActive(true);
        capturebtn.SetActive(true);


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



    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    public void reset_objects()
    {

        capturebtn1.gameObject.GetComponent<Image>().sprite = default_sprite;
    }

    /// <summary>
    /// //capture image and while sending just check that input filed should not be blank
    /// </summary>
    public void Capture_data_post()
    {

        if (user_text.text == "" && plan_title.text == "" && post_image_byte.Count == 0)
        {
            string msg = "Please make your plan!";
            StartCoroutine(statusmsgshow(msg));
        }
        else
        {
            StartCoroutine(Upload());
        }

    }

    public IEnumerator show_image_mathod(byte[] image_byte, GameObject selected_btn)
    {
        post_image_byte.Clear();
        yield return new WaitForSeconds(0.1f);
        Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGB24, true);
        texture.LoadImage(image_byte);
        texture.Apply();
        texture.name = "test.png";
        test = texture;
        Texture2D newScreenshot = ScaleTexture(texture, 256, 256);
        var bytes = newScreenshot.EncodeToPNG();
        string imagestr = Convert.ToBase64String(bytes);
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(Convert.FromBase64String(imagestr));



        if (selected_btn.name == "Pic")
        {
            post_image_byte.Add(bytes);
            Sprite btn_sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            selected_btn.GetComponent<Image>().sprite = btn_sprite;
        }
     
    }


    IEnumerator statusmsgshow(string msg)
    {
        statusmsg.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        statusmsg.text = msg;
        yield return new WaitForSeconds(2f);
        statusmsg.text = "";
        statusmsg.gameObject.SetActive(false);

    }

    public IEnumerator Upload()
    {
        //loadingAnim.SetActive(true);
        //posting_msg.SetActive(true);
        //string post_url = Mainurl + upload_image_API;
        string post_url = "https://www.skillmuni.in/wsmapi/api/PostPhotoUpload/TagPhotoUpload";
        //Debug.Log(BitConverter.ToString(imageBytes));
        string usertext = user_text.text;
        string plan = plan_title.text;
        string level = PlayerPrefs.GetInt("game_id").ToString();
        string lati = PlayerPrefs.GetFloat("LAT").ToString();
        string longi = PlayerPrefs.GetFloat("LONG").ToString();
        string uid = PlayerPrefs.GetInt("UID").ToString();
        string oid = PlayerPrefs.GetInt("OID").ToString();

 
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("UID", uid));
        formData.Add(new MultipartFormDataSection("OID", oid));
        formData.Add(new MultipartFormDataSection("EXTN", "png"));
        formData.Add(new MultipartFormFileSection("Media", post_image_byte[0], test.name, "image/png"));
        formData.Add(new MultipartFormDataSection("GCI", level));
        formData.Add(new MultipartFormDataSection("Level", level_zone.ToString()));
        formData.Add(new MultipartFormDataSection("LATI", lati));
        formData.Add(new MultipartFormDataSection("LONGI", longi));
        formData.Add(new MultipartFormDataSection("DETAIL", user_text.text));
        formData.Add(new MultipartFormDataSection("KEYINFO", plan_title.text));


        UnityWebRequest www = UnityWebRequest.Post(post_url, formData);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            respose_text.text = www.downloadHandler.text;
            //posting_msg.SetActive(false);
            statusmsg.text = "PLEASE TRY LATER!";
            yield return new WaitForSeconds(3f);
            statusmsg.text = "";
            imageBytes = null;
            statusmsg.gameObject.SetActive(false);
        }
        else
        {
           
            Debug.Log(www.downloadHandler.text);
            yield return new WaitForSeconds(0.5f);
            formData.Clear();
            ////zone_handle.actionplan_score += 25;
               
        }
        
        //posting_msg.SetActive(false);
        //loadingAnim.SetActive(false);
        statusmsg.gameObject.SetActive(true);
        statusmsg.text = "PLAN SUCCESSFULLY GENERATED!";
        yield return new WaitForSeconds(2.5f);
        statusmsg.text = "";
        statusmsg.gameObject.SetActive(false);
        user_text.text = "";
        plan_title.text = "";
        Bigger_count = 0;
        post_image_byte.Clear();
        capturebtn1.gameObject.GetComponent<Image>().sprite = default_sprite;
    }

}
