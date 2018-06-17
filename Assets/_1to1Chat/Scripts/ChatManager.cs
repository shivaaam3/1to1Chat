using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Chat;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour,IChatClientListener
{
    #region variables
   
    public Transform parent;
    public GameObject chatField;
    public Text status;
    public InputField userName;
    public InputField msg;

	ChatClient chatClient;
    GameObject newChat;
    internal Queue<GameObject> msgQueue;
    public string worldChat;
    #endregion
    // Use this for initialization
    void Awake ()
    {
        worldChat = new string("world".ToCharArray());
        msgQueue = new Queue<GameObject>();

        Application.runInBackground = true;
        if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.ChatAppID))
        {
            Debug.Log("Invalid aapID");
        }
        Debug.Log("CONNECTING!!!!");
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(this.chatClient!=null)
            this.chatClient.Service();
		
	}

    public void getConnected()
    {
        this.chatClient = new ChatClient(this);
        status.text = "Connecting";
        print("Trying To connect");
        this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.ChatAppID, "v1",new ExitGames.Client.Photon.Chat.AuthenticationValues(userName.text));
 
    }
    public void getSubscried()
    {
        this.chatClient.Subscribe(new string[] { worldChat }, 10);
    }
    public void sendMessage()
    {
		if (msg != null && !string.IsNullOrEmpty(msg.text))
        {
            chatClient.PublishMessage(worldChat, msg.text);
            msg.text = "";
        }
        else
            return;
    }
    #region IChatClientListener implementation

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
		return;
    }

    public void OnDisconnected()
    {
		this.chatClient.PublishMessage(worldChat, userName.text + " disconnected");
    }

    public void OnConnected()
    {
        Debug.Log("CONNECTED");
        status.text = "Connected";
        getSubscried();
        this.chatClient.PublishMessage(worldChat, userName.text + " connected");
    }

    public void OnChatStateChange(ChatState state)
    {
		return;
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
       // print("In GetMessage");
        if (channelName == worldChat)
        {
           // print("Inside if");
            for(int i=0;i<senders.Length;i++)
            {
               // print("inside For");
                newChat = Instantiate(chatField, parent);
                newChat.SetActive(true);
                msgQueue.Enqueue(newChat);
                newChat.GetComponent<Text>().text = senders[i] + " : " + messages[i];
                if (msgQueue.Count >= 30)
                    Destroy(msgQueue.Dequeue());
            }
        }
       
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
		return;
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
		return;
    }

    public void OnUnsubscribed(string[] channels)
    {
		return;  
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
		return;
    }

    #endregion
}
