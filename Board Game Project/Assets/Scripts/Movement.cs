using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    //Inspector Values
    [SerializeField] private List<Player> _players; //Number of players 
    public float _speed = 1;  //Speed of the players when they move
    [SerializeField] private Board _board; //Board thats being used 
    [SerializeField] private Text _text; //What text its going to display through
    [SerializeField] private Text _popUpText; //where event text will be displayed through

    //Positional Values
    Vector2 _currentPos; //Current Cord of Vector 2 Position 
    Vector2 _nextPos; //Empty Variable for the next Vector 2 Position 
    
    //Time Variables
    float _totalTime;
    float _dealtaT; 

    //Game Turn Data
    bool _isMoving; //Is the player moving?
    bool _gameIsover; //Is the game over?
    bool _turnStarted; //Has the turn started for the player? 

    //Current turn Information
    int _currentPlayer = 0; //Who the player is 0 = Player 1, 1 = Player 2, 2 = Player 3, 3 = Player 4
    int _tileMovementAmount; //How far or how many times its moving by 

    //Our Dice
    Die _die = new Die(); //Die script which includes ranges upto 1-6

    //event variables and bools
    bool _nextPlayerTurn;
    bool _moveOn;
    bool _chooseOne;
    int _randomNum;
    int _currentTile; 
    
    // Start is called before the first frame update
    void Start()
    {
        _board.InitTilePositions(); //Initializes the board movement (Board Script)
        _text.text = "Press 'Space' to roll the dice"; //For the first movement of the player

        //for event tile stuff
        _nextPlayerTurn = false;
        _moveOn = true;
        _chooseOne = true;
    }

    void CheckWin()
    {
        /*Check To See If Player Won*/
        if ((_players[0].GetCurrentTile() == 40 || _players[1].GetCurrentTile() == 40 || _players[2].GetCurrentTile() == 40 || _players[3].GetCurrentTile() == 40) && !_isMoving)
        {  //When the current tile is 40, and the user is not moving the conditional statement will run.
            _gameIsover = true; //Boolean is set to true to be used elsewhere. 
            _text.text = $"Game Over! Player {_currentPlayer + 1 } Escaped! Press 'Space' to play again!"; //We add 1 to current player because element starts with 0 instead of 1. Similar to arrays 
        }
        
        /*If The Player Did Win*/

        if(_gameIsover && Input.GetKeyDown(KeyCode.Space))
        { //When the boolean of the _gameisover is set to true and when the user presses space, a new scene/New game will start from the beginning. 
            SceneManager.LoadScene("BoardGame"); 
        } 

    }

    // Update is called once per frame
    void Update()
    {
        //if you want to restart the game at any point, click escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("BoardGame");
        }

        CheckWin(); //Runs the method CheckWin(); to see if the user has reached tile 40. 

        if (!_gameIsover)
        { //Will run when the game is not over
            _currentPos = _players[_currentPlayer].GetPosition(); //Gets the players current position

            /*Player Turn Setting*/

            if(_tileMovementAmount == 0 && !_isMoving && _nextPlayerTurn == true)
            { //When the dice is not rolled yet, and we are not moving it will run this statement. 
                _text.text = $" Player {_currentPlayer + 1}'s Turn! Press 'Space' to roll the dice"; //Text to the players guiding keys to press to move. 
 
                if (_turnStarted){ //Will check for turn bool and set it to false during this statement. As nothing is being moved. 
                    _turnStarted = false; 

                    if (_currentPlayer == 0){ //Player 1's turn is element 0, which is done initially before this "if" statement. After its done, _currentplayer will be set to element 1, which is Player 2. The player 2 will also go through the cycle below.   
                        _currentPlayer = 1; 
                    }
                    else if(_currentPlayer == 1){ //When player 2's turn is over, we move onto player 3's turn. It follows the same idea as above. 
                        _currentPlayer = 2; 
                    }

                    else if(_currentPlayer == 2){ // When player 3's turn is over, we move onto player 4's turn. It follows the same idea as above. 
                        _currentPlayer = 3;
                    }
                    else{
                        _currentPlayer = 0;  //After a cycle through all the players, the "else" statement will put the _currentPlyer back to elemnt 0, Player 1's turn. 
                    }
                }
            }

            /*Player Movement When "Space" is Pressed*/

            if (Input.GetKeyDown(KeyCode.Space) && _tileMovementAmount == 0 && _moveOn == true ){ //If the spacekey is pressed and the tilemovementamount is set to 0 (Which it should be since there is no initial die value), it will roll the dice. 
                _tileMovementAmount = _die.RollDice(); //Dice will be rolled from the die script and is set tothe tilemovement variable 
                _text.text = "You rolled a " + _tileMovementAmount.ToString(); //Text to show what the user rolled converted ToString(); since its in text form. 
                _turnStarted = true; //When we are rolling the dice, we dont want other actions to be done, unless stated. So the turn has started 

            }

            /*Player Color Setting*/

            if (_currentPlayer == 0 ){ //Color green when its player 1's turn
                _text.color = Color.green; 
            }
            else if(_currentPlayer == 1 ){ //Color grey when its player 2's turn
                _text.color = Color.grey; 
            } 
            else if(_currentPlayer == 2 ){ //Color red when its player 3's turn
                _text.color = Color.red; 
            }
            else if(_currentPlayer == 3 ){ //Color yellow when its player 4's turn
                _text.color = Color.yellow; 
            }

            /*Player Single Tile Movement*/

            if(_tileMovementAmount > 0 && !_isMoving){ //When the rolled dice amount is greater than 0, and is not moving, we are moving one tile based on the board script. This allows to move one tile over to another. Not a fluid transformation. 
                MoveOneTile();
                _nextPlayerTurn = false;
            }

            /*Player Position Update Afrer Movement*/

            if(_isMoving){ //When the player is not moving and has finished moving from moving one tile, it will update its position from the method UpdatePosition();
                UpdatePostion(); 
            }

            //if player has done their roll and moved, run OnEventTile() function to see if they are on event tile and make event run in accordance  
            if (_tileMovementAmount == 0 && !_isMoving && _nextPlayerTurn == false)
            {

                OnEventTile();

            }
        }
        
    }


    /*Method That Updates The Position Via Vector2 and Time*/
    void UpdatePostion(){
        _dealtaT += Time.deltaTime / _totalTime; 

        if (_dealtaT < 0f){
            _dealtaT = 0f;
        }

        if (_dealtaT >= 1f || _nextPos == _currentPos){
            _isMoving = false;
            _tileMovementAmount--; 
            _dealtaT = 0f;
        }

        _players[_currentPlayer].Setposition(Vector2.Lerp(_currentPos, _nextPos, _dealtaT)); 
    }

    /* Method That Moves Player Tile to Tile using speed stated above the script. It will go from 1 tile to another*/
    void MoveOneTile(){
        _nextPos = _board.GetTilePositions()[_players[_currentPlayer].GetCurrentTile()];
        _totalTime = (_nextPos - _currentPos / _speed).magnitude; 
        _isMoving = true; 
        _players[_currentPlayer].SetCurrentTile(_players[_currentPlayer].GetCurrentTile() + 1); 

    }

    //choose random number for event
    public int ChooseRandomNum()
    {
        float coolNum = Random.Range(1, 7);
        return (int)coolNum;

    }

    //make player move after event popup has shown
    public void EventMove()
    {

        //player must press space to get rid of event popup
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _popUpText.gameObject.SetActive(false);
            _moveOn = true;
        }

        //once pop up is gone, make player move for the event
        if (_moveOn == true)
        {
            _tileMovementAmount = 2;

        }
    }

    //6 functions below enable pop up with different messages for each 
    public void EventOne()
    {
        _moveOn = false;
        Debug.Log("Event One!");

        _popUpText.gameObject.SetActive(true);
        _popUpText.text = "You want to play on your PC at home so badly, it gives you an energy boost. Move forward 2 spaces. Press 'Space' to continue.";

        EventMove();

    }

    public void EventTwo()
    {
        _moveOn = false;
        Debug.Log("Event Two!");

        _popUpText.gameObject.SetActive(true);
        _popUpText.text = "A random prisoner cheers you on. With new found confidence, move forward two spaces. Press 'Space' to continue.";
        EventMove();
    }

    public void EventThree()
    {
        _moveOn = false;
        Debug.Log("Event Three!");
        _popUpText.gameObject.SetActive(true);
        _popUpText.text = "You steal and drink a guard's energy drink. Move forward 2 spaces. Press 'Space' to continue.";
        EventMove();
    }

    public void EventFour()
    {
        _moveOn = false;
        Debug.Log("Event Four!");
        _popUpText.gameObject.SetActive(true);
        _popUpText.text = "A fight breaks out, leaving the guards preoccupied. Move forward 2 spaces. Press 'Space' to continue.";
        EventMove();
    }

    public void EventFive()
    {
        _moveOn = false;
        Debug.Log("Event Five!");
        _popUpText.gameObject.SetActive(true);
        _popUpText.text = "You think you might have left the stove on at home. Worried, move forward 2 spaces. Press 'Space' to continue.";
        EventMove();
    }

    public void EventSix()
    {
        _moveOn = false;
        Debug.Log("Event Six!");
        _popUpText.gameObject.SetActive(true);
        _popUpText.text = "It's time for you to touch grass again. Move forward 2 spaces.";
        EventMove();
    }

    //if player is on an event tile, choose a random event function and call it
    //if they are not, it continues to the next player's turn 
    public void OnEventTile()
    {
        _currentTile = _players[_currentPlayer].GetCurrentTile();


        if (_currentTile == 5 || _currentTile == 11 || _currentTile == 21 || _currentTile == 31 || _currentTile == 38)
        {
            if (_chooseOne == true)
            {
                _randomNum = ChooseRandomNum();
                _chooseOne = false;
            }

            Debug.Log("Event!");

            if (_randomNum == 1)
            {
                EventOne();
            }
            else if (_randomNum == 2)
            {
                EventTwo();
            }
            else if (_randomNum == 3)
            {
                EventThree();
            }
            else if (_randomNum == 4)
            {
                EventFour();
            }
            else if (_randomNum == 5)
            {
                EventFive();
            }
            else if (_randomNum == 6)
            {
                EventSix();
            }
        }
        else
        {
            _chooseOne = true;
            _nextPlayerTurn = true;
        }

    }
}
