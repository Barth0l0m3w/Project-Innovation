using System.Collections;
using System.Collections.Generic;
using shared;
using UnityEngine;

public abstract class ClientState : MonoBehaviour
{
    protected Client Client { get; private set; }
    
    public virtual void InitializeState(Client client)
    {
        Client = client;
        gameObject.SetActive(false);
    }
    
    public virtual void EnterState()
    {
        gameObject.SetActive(true);
    }
    
    public virtual void ExitState()
    {
        gameObject.SetActive(false);
    }
    
    protected virtual void ReceiveAndProcessNetworkMessages()
    {
        if (!Client.Channel.Connected)
        {
            return;
        }
        
        while (Client.Channel.HasMessage() && gameObject.activeSelf)
        {
            ASerializable message = Client.Channel.ReceiveMessage();
            HandleNetworkMessage(message);
        }
    }
    abstract protected void HandleNetworkMessage(ASerializable pMessage);
}
