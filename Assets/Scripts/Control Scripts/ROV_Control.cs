using UnityEngine;
using System.ComponentModel;
using System;


public class ROV_Control : MonoBehaviour
{
    public string MotorPower;
    public string MotorTorque;  //"9e167bb5e3b9afa08550e3d9a0c3f9f39666ca23"; //47bf14a271aed9a959b0a587b4c2d7cfa7af340c
    public GameObject M1;
    public GameObject M2;

    public bool triggerResultEmail = false;
    public bool resultEmailSucess;

    string SMTPClient;
    string SMTPPort;
    string UserName;
    string UserPass;
    string To;
    string Subject;
    string Body;
    string AttachFile;

    string Subjectt;

    // Use this for initialization
    void Start()
    {

        SMTPClient = "mail.missim.com.br";
        SMTPPort = "587";
        UserName = "dongle@missim.com.br";
        UserPass = "dongle@2022@";
        To = "dongle@missim.com.br";
        Subject = "Alerta de Tentativa em PC Diferente";
        AttachFile = PlayerPrefs.GetString("AttachFile");
        Subjectt = (SystemInfo.deviceUniqueIdentifier);
        Body = "Tentativa em PC Diferente - Código MAC: " + Subjectt;

        MotorPower = (SystemInfo.deviceUniqueIdentifier);
    }

    // Update is called once per frame
    void Update()
    {

        if (MotorPower != MotorTorque)
        {
            SendEmail();
            M1.SetActive(false);
            M2.SetActive(false);
        }


    }

    public void SendEmail()
    {
        SimpleEmailSender.emailSettings.STMPClient = SMTPClient.Trim();
        SimpleEmailSender.emailSettings.SMTPPort = Int32.Parse(SMTPPort.Trim());
        SimpleEmailSender.emailSettings.UserName = UserName.Trim();
        SimpleEmailSender.emailSettings.UserPass = UserPass.Trim();

        SimpleEmailSender.Send(To, Subject, Body, AttachFile, SendCompletedCallback);

    }

    private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Cancelled || e.Error != null)
        {
            print("Email not sent: " + e.Error.ToString());

            resultEmailSucess = false;
            triggerResultEmail = true;
        }
        else
        {
            print("Email successfully sent.");

            resultEmailSucess = true;
            triggerResultEmail = true;
        }
    }
}
