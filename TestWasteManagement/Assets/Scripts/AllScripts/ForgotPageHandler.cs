using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgotPageHandler : MonoBehaviour
{
    public GameObject PageFields,PopUpPage,EmailVarificationPage,OTPPagevalidation,NewPasswordPage;
    public InputField EmailInput;
    public Button Verifybtn;
    public Text Showmsg;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        PageFields.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        Verifybtn.interactable = EmailInput.text != "";
    }

    public void BackToLoginPage()
    {
        StartCoroutine(Backtask());
    }

    IEnumerator Backtask()
    {
        iTween.ScaleTo(PageFields, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        PageFields.SetActive(false);
        this.gameObject.SetActive(false);

    }

    public void CheckEmailID()
    {
        string email = EmailInput.text;
        StartCoroutine(Emailvarification(email));
    }

    IEnumerator Emailvarification(string email)
    {
        yield return new WaitForSeconds(0.1f);
        string msg = "OTP send sucessfully";
        StartCoroutine(ShowMsgPopup(msg));
        yield return new WaitForSeconds(4f);
        EmailVarificationPage.SetActive(false);
        OTPPagevalidation.SetActive(true);
    }

    IEnumerator ShowMsgPopup(string msg)
    {
        PopUpPage.SetActive(true);
        Showmsg.text = msg;
        iTween.ScaleTo(PopUpPage, Vector3.one, 0.4f);
        yield return new WaitForSeconds(3f);
        iTween.ScaleTo(PopUpPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Showmsg.text = "";
        PopUpPage.SetActive(false);
    }
}
