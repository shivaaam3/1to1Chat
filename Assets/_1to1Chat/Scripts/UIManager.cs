using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public ChatManager chatManager;
   	public void connect()
    {
        chatManager.getConnected();
    }
    public void send()
    {
        chatManager.sendMessage();
        
    }
}
