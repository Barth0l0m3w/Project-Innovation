using System;
using System.Collections.Generic;
using UnityEngine;
using shared;
using UnityEngine.XR;

public class Client : MonoBehaviour
{
    private static Client _instance;
    //make it a singleton, don't destroy on load
    [SerializeField] private ClientState _startState = null; //needs to have one state as the starting one
    private Dictionary<Type, ClientState> _states = new Dictionary<Type, ClientState>();
    private ClientState _currentState = null;
    private bool _playerOneClicked;
    private bool _playerTwoClicked;
    private bool _isPlayerOneReady;
    private bool _isPlayerTwoReady;

    public bool IsPlayerOneReady
    {
        get => _isPlayerOneReady;
        set => _isPlayerOneReady = value;
    }

    public bool IsPlayerTwoReady
    {
        get => _isPlayerTwoReady;
        set => _isPlayerTwoReady = value;
    }

    public bool PlayerOneClicked
    {
        get => _playerOneClicked;
        set => _playerOneClicked = value;
    }

    public bool PlayerTwoClicked
    {
        get => _playerTwoClicked;
        set => _playerTwoClicked = value;
    }

    private void Update()
    {
        if (_playerOneClicked)
        {
            Debug.Log("Player one got selected");
        } else if (_playerTwoClicked)
        {
            Debug.Log("Player two got selected");
        }
    }


    private TcpMessageChannel _channel;

    public TcpMessageChannel Channel
    {
        get => _channel;
        set => _channel = value;
    }

    public static Client Instance => _instance;
    

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
        _channel = new TcpMessageChannel();
        Debug.Log("Initializing Client:" + this);

        //All the states MUST BE children of the client object
        ClientState[] clientStates = GetComponentsInChildren<ClientState>(true);
        foreach (ClientState state in clientStates)
        {
            _states.Add(state.GetType(), state);
            state.InitializeState(this);
        }
    }

    private void Start()
    {
        SetState(_startState.GetType());
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetState<T>() where T : ClientState
    {
        SetState(typeof(T));
    }
    
    /// <summary>
    /// Set state of the client
    /// First checks if the state is not already set
    /// Exit the previous state if the client has a state already
    /// If the state exists in the game, change to this state
    /// </summary>
    /// <param name="pType"></param>
    public void SetState(Type pType)
    {
        if (_currentState != null && _currentState.GetType() == pType) return;
        
        if (_currentState != null)
        {
            _currentState.ExitState();
            _currentState = null;
        }

        if (pType != null && _states.ContainsKey(pType))
        {
            _currentState = _states[pType];
            _currentState.EnterState();
        }
    }

    public void OnApplicationQuit()
    {
        SetState(null);
        _channel.Close();
    }
}