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
        Debug.Log("Entering application state " + this);
        gameObject.SetActive(true);
    }
    
    public virtual void ExitState()
    {
        Debug.Log("Exiting application state " + this);
        gameObject.SetActive(false);
    }
    
    virtual protected void receiveAndProcessNetworkMessages()
    {
        if (!Client.Channel.Connected)
        {
            Debug.LogWarning("Trying to receive network messages, but we are no longer connected.");
            return;
        }

        //while there are messages, we have no issues AAAND we haven't been disabled (important!!):
        //we need to check for gameObject.activeSelf because after sending a message and switching state,
        //we might get an immediate reply from the server. If we don't add this, the wrong state will be processing the message
        while (Client.Channel.HasMessage() && gameObject.activeSelf)
        {
            ASerializable message = Client.Channel.ReceiveMessage();
            handleNetworkMessage(message);
        }
    }

    /**
	 * Override/implement in a subclass
	 */
    abstract protected void handleNetworkMessage(ASerializable pMessage);
}
