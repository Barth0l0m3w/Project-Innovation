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
    //LOGIN SCREEN
    private bool _playerOneClicked;
    private bool _playerTwoClicked;
    private bool _isPlayerOneReady;
    private bool _isPlayerTwoReady;
    //GAME SCREEN
    private bool _isDoorVisibleP1;
    private bool _isDoorVisibleP2;
    private int _buttonClicked;
    private int _cameraNumberPlayer;
    private int _roomP1 = 0;
    private int _roomP2 = 0;
    private int _playerRoom;

    private bool _puzzleSolved;
    private bool _newPuzzle;

    private bool _blowDust;

    public bool BlowDust
    {
        get => _blowDust;
        set => _blowDust = value;
    }

    public bool NewPuzzle
    {
        get => _newPuzzle;
        set => _newPuzzle = value;
    }

    public bool PuzzleSolved
    {
        get => _puzzleSolved;
        set => _puzzleSolved = value;
    }

    private Dictionary<GameObject, bool> _interactions = new Dictionary<GameObject, bool>();

    public Dictionary<GameObject, bool> Interactions
    {
        get => _interactions;
        set => _interactions = value;
    }

    public int PlayerRoom
    {
        get => _playerRoom;
        set => _playerRoom = value;
    }

    private int _playerNumber;
    private bool _lockCorrect;
    private bool _lockPickedPhone;
    private bool _lockPickedLaptop;
    private bool _safeOpened;

    public bool SafeOpened
    {
        get => _safeOpened;
        set => _safeOpened = value;
    }

    public bool LockCorrect
    {
        get => _lockCorrect;
        set => _lockCorrect = value;
    }

    public int PlayerNumber
    {
        get => _playerNumber;
        set => _playerNumber = value;
    }

    public bool LockPickedLaptop
    {
        get => _lockPickedLaptop;
        set => _lockPickedLaptop = value;
    }

    public bool LockPickedPhone
    {
        get => _lockPickedPhone;
        set => _lockPickedPhone = value;
    }


    public int RoomP1
    {
        get => _roomP1;
        set => _roomP1 = value;
    }

    public int RoomP2
    {
        get => _roomP2;
        set => _roomP2 = value;
    }


    public int CameraNumberPlayer
    {
        get => _cameraNumberPlayer;
        set => _cameraNumberPlayer = value;
    }


    public int ButtonClicked
    {
        get => _buttonClicked;
        set => _buttonClicked = value;
    }

    private int _buttonP1;
    private int _buttonP2;

    public int ButtonP1
    {
        get => _buttonP1;
        set => _buttonP1 = value;
    }

    public int ButtonP2
    {
        get => _buttonP2;
        set => _buttonP2 = value;
    }


    public bool IsDoorVisibleP2
    {
        get => _isDoorVisibleP2;
        set => _isDoorVisibleP2 = value;
    }

    public bool ShowDoorButton
    {
        get => _showDoorButton;
        set => _showDoorButton = value;
    }

    private bool _showDoorButton;

    public bool IsDoorVisibleP1
    {
        get => _isDoorVisibleP1;
        set => _isDoorVisibleP1 = value;
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