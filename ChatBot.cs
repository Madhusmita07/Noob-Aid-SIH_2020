using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
public class ChatBot : MonoBehaviour
{

    public GameObject input;
    public GameObject[] obj = new GameObject[7];
    public GameObject pre ;
    public GameObject warn ;
    public GameObject info ;
    public InputField inputf ;
    public Text mytext;
    public Text bot;
    private string user_input;
    private string direct_line = "https://directline.botframework.com/v3/directline/";
    private string convo = "conversations";
    private string Actv = "activities";
    private bool initialize = false;
    private string Token = "";
    private string convoId = "";
    private string watermrk = "000000";
    private string streamurl = "";
    //private string stream = "stream?t=";
    private string secretkey = "mTeSduem68w.zGKYRg4OAloMnIhrh7NNH3sw21Yjf9la19UctbOEGCU";
    public Root reply = new Root();
   // public string message = "Hello";
   // public string userId = "user_1";
    public string keytype = "message";
    public string botResponse = "";
    public Root root = new Root();
    public ChatBot()
    {
        initialize = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("Inside of Start.................");
        IsInitialize(secretkey);
        StartCoroutine(ConvoId());

    }
    public void userInput(string ip)
    {
        user_input = ip;
        mytext.color = Color.blue;
        //mytext.color = new Color(0,255,188);
        mytext.text += "USER : " + "\n" + user_input + "\n\n";

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (input.GetComponent<InputField>().isFocused)
            {
                user_input = input.GetComponent<InputField>().text;
                mytext.text += "USER : " + "\n" + user_input + "\n\n";
                execute();
            }
        }
    }
    public void execute()
    {
        inputf.text = "";
        StartCoroutine("SendMsg");

    }

    public void IsInitialize(string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            initialize = true;
        }
        else
        {
            throw new ArgumentException("Secret key should not be empty");
        }
    }

    public IEnumerator ConvoId()
    {
        Debug.Log("Sending message..................");
        if (initialize)          
        {
            string uri = direct_line + convo;
            UnityWebRequest www = UnityWebRequest.Post(uri,"");
            www.SetRequestHeader("Authorization", "Bearer "+secretkey);
            //mytext.text = "Authorization :" + "Bearer " + secretkey;
            //www.SetRequestHeader("Content-Type", "application/json");
             yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log("Netwoek error :" + www.error);
            }
            else
            {
                string response= www.downloadHandler.text;
                Debug.Log("Message sent successfully.......................................");
                reply = JsonUtility.FromJson<Root>(response);
                convoId = reply.conversationId;
                Token = reply.token;
                streamurl = reply.streamUrl;
            }
            //if (user_input != "")
            //{
            //    StartCoroutine(SendMsg());
            //}

        }
        yield return null;
    }

   public IEnumerator SendMsg()
    {
        if (initialize)
        { 
                root.type = "message";
                root.from.id = "user1";
                root.text=user_input;
                if (!String.IsNullOrEmpty(root.text))
                {
                    string data = JsonConvert.SerializeObject(root);
                    byte[] body = System.Text.Encoding.UTF8.GetBytes(data);
                    string uri = direct_line + convo + "/" + convoId + "/" + Actv;
                    UnityWebRequest www = UnityWebRequest.Post(uri, "");
                    www.SetRequestHeader("Authorization", "Bearer " + Token);
                    www.SetRequestHeader("Content-Type", "application/json");
                    www.uploadHandler = new UploadHandlerRaw(body);


                    //yield return 
                    yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    Debug.Log("Network error: " + www.error);
                    mytext.text = "send :" + www.error;
                }
                else
                {
                    //mytext.text = "Message sent : ";
                    string response = www.downloadHandler.text;
                    reply = JsonUtility.FromJson<Root>(response);
                    watermrk = reply.id.Replace(convoId + "|", "");
                }
                    StartCoroutine(GetMsg());
                    
                }
        }


    }
    public IEnumerator GetMsg()
    {
        if (initialize)
        {
            string uri = direct_line + convo + "/" + convoId + "/" + Actv + "?watermark=" + watermrk;
            UnityWebRequest www = UnityWebRequest.Get(uri);
            www.SetRequestHeader("Authorization", "Bearer " + Token);

            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log("Network error: " + www.error);
                mytext.text = "send :" + www.error;
            }
            else
            {
                string response = www.downloadHandler.text;
                Root botreply = JsonConvert.DeserializeObject<Root>(response);
                botResponse = botreply.activities[0].text;
                watermrk = botreply.activities[0].id.Replace(convoId + "|", "");
                //mytext.color = Color.red;
                //mytext.color = new Color(0,255,4);
                mytext.text += "Noddie(BOT) : " + "\n" + botResponse.ToUpper().Replace("**", null) + "\n\n";
                //inputf.text = botResponse;
                //yield return null;

                if (mytext.text.Contains("PATIENT"))
                {

                    obj[0].gameObject.SetActive(false);
                    obj[1].gameObject.SetActive(false);
                    obj[2].gameObject.SetActive(false);
                    obj[3].gameObject.SetActive(true);
                    obj[4].gameObject.SetActive(true);
                    obj[5].gameObject.SetActive(false);
                    obj[6].gameObject.SetActive(false);
                    obj[7].gameObject.SetActive(true);
                    

                }
                if (user_input.ToLower().Contains("precaution"))
                {
                    pre.gameObject.SetActive(true);
                    info.gameObject.SetActive(false);

                }
                if (user_input.ToLower().Contains("warning") || user_input.ToLower().Contains("label"))
                {

                    warn.gameObject.SetActive(true);
                    info.gameObject.SetActive(false);
                }
            }
        }
                
       
    }
  public void scene_switch()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("AR");
    }
    public void scn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CT_UI");
    }

}
