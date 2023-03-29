using shared;

/**
 * This is where we 'play' a game.
 */
public class GameState : ApplicationStateWithView<GameView>
{
    //just for fun we keep track of how many times a player clicked the board
    //note that in the current application you have no idea whether you are player 1 or 2
    //normally it would be better to maintain this sort of info on the server if it is actually important information
    private int player1MoveCount = 0;
    private int player2MoveCount = 0;
    private string player1Name;
    private string player2Name;

    public override void EnterState()
    {
        base.EnterState();
        
        view.gameBoard.OnCellClicked += _onCellClicked;
    }

    private void _onCellClicked(int pCellIndex)
    {
        MakeMoveRequest makeMoveRequest = new MakeMoveRequest();
        makeMoveRequest.move = pCellIndex;

        fsm.channel.SendMessage(makeMoveRequest);
    }

    public override void ExitState()
    {
        base.ExitState();
        view.gameBoard.OnCellClicked -= _onCellClicked;
    }

    private void Update()
    {
        receiveAndProcessNetworkMessages();
    }

    protected override void handleNetworkMessage(ASerializable pMessage)
    {
        if (pMessage is RoomEntered)
        {
            HandleRoomEntered(pMessage as RoomEntered);
        }
        if (pMessage is MakeMoveResult)
        {
            handleMakeMoveResult(pMessage as MakeMoveResult);
        }

        if (pMessage is GameFinished)
        {
            handleGameFinished(pMessage as GameFinished);
        }
    }

    private void handleMakeMoveResult(MakeMoveResult pMakeMoveResult)
    {
        view.gameBoard.SetBoardData(pMakeMoveResult.boardData);

        //some label display
        if (pMakeMoveResult.whoMadeTheMove == 1)
        {
            player1MoveCount++;
            view.playerLabel1.text = $"{player1Name} (Movecount: {player1MoveCount})";
        }
        if (pMakeMoveResult.whoMadeTheMove == 2)
        {
            player2MoveCount++;
            view.playerLabel2.text = $"{player2Name} (Movecount: {player2MoveCount})";
        }

    }

    private void HandleRoomEntered(RoomEntered roomEntered)
    {
        // player1Name = roomEntered.player1.name;
        // player2Name = roomEntered.player2.name;
        player1MoveCount = 0;
        player2MoveCount = 0;
        view.playerLabel2.text = $"{player2Name} (Movecount: {player2MoveCount})";
        view.playerLabel1.text = $"{player1Name} (Movecount: {player1MoveCount})";
    }
    
    private void handleGameFinished(GameFinished pMessage)
    {
        PlayerJoinRequest join = new PlayerJoinRequest();
            //join.name = pMessage.player.name;
        fsm.channel.SendMessage(join);
        fsm.ChangeState<LobbyState>();
    }
}
