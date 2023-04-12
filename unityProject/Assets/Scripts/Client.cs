using System;
using System.Collections.Generic;
using UnityEngine;
using shared;
using UnityEngine.XR;

public class Client : MonoBehaviour
{
    private static Client _instance;
    [Tooltip("Please remember to add one state as a start one (preferably LoginScreen)")]
    [SerializeField] private ClientState _startState = null;
    
    private Dictionary<Type, ClientState> _states = new Dictionary<Type, ClientState>();
    private ClientState _currentState = null;
    private TcpMessageChannel _channel;
    private bool _playerOneClicked;
    private bool _playerTwoClicked;
    private bool _isPlayerOneReady;
    private bool _isPlayerTwoReady;
    private bool _isDoorVisibleP1P1;

    public bool IsDoorVisibleP1
    {
        get => _isDoorVisibleP1P1;
        set => _isDoorVisibleP1P1 = value;
    }

    private bool _isCurrentStateNotNull;

    /// <summary>
    /// Those variables are used mostly to connect client to buttons and objects in the actual game
    /// The names might still change
    /// </summary>
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

    //For some reason if I change it, it does not work - TODO: ask Hans
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