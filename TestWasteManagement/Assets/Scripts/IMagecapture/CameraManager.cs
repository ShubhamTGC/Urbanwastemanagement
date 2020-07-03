using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    bool isCameraAvailale;
    public WebCamTexture backCamera;
    Texture defaultBg;
    public SaveImageToServer after_capture;
    public RawImage background;
    public GameObject click_button;

    // Start is called before the first frame update
    void Start()
    {
        OpenBackCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCameraAvailale == false)
            return;


        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1, scaleY, 1);

        int orient = -backCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void CloseCamera()
    {
        background.gameObject.SetActive(false);
        after_capture.reset_objects();
        backCamera.Stop();
    }

    public void PlayCamera(GameObject thisobject)
    {
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            click_button = thisobject;
            Debug.Log("hitting");
            background.gameObject.SetActive(true);
            backCamera.Play();
            background.texture = backCamera;
            isCameraAvailale = true;
        }
      
    }

    public void OpenBackCamera()
    {
        defaultBg = background.texture;
        WebCamDevice[] deveices = WebCamTexture.devices;

        if (deveices.Length == 0)
        {
            Debug.Log("No Camera Detected");
            isCameraAvailale = false;
            return;
        }

        for (int i = 0; i < deveices.Length; i++)
        {
            if (!deveices[i].isFrontFacing)
            {
                backCamera = new WebCamTexture(deveices[i].name, Screen.width, Screen.height);
            }

            if (backCamera == null)
            {
                Debug.Log("No Back Camera Found");
                return;
            }


        }
    }
}
