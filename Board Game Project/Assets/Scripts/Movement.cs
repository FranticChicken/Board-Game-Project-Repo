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

    
    // Start is called before the first frame update
    void Start()
    {
        _board.InitTilePositions(); //Initializes the board movement (Board Script)
        _text.text = "Press 'Space' to roll the dice"; //For the first movement of the player
    }

    void CheckWin(){

        /*Check To See If Player Won*/

        if ((_players[0].GetCurrentTile() == 40 || _players[1].GetCurrentTile() == 40 || _players[2].GetCurrentTile() == 40 || _players[3].GetCurrentTile() == 40) && !_isMoving){  //When the current tile is 40, and the user is not moving the conditional statement will run.
            _gameIsover = true; //Boolean is set to true to be used elsewhere. 
            _text.text = $"Game Over! Player {_currentPlayer + 1 } Escaped! Press 'Space' to play again!"; //We add 1 to current player because element starts with 0 instead of 1. Similar to arrays 
        }
        
        /*If The Player Did Win*/

        if(_gameIsover && Input.GetKeyDown(KeyCode.Space)){ //When the boolean of the _gameisover is set to true and when the user presses space, a new scene/New game will start from the beginning. 
            SceneManager.LoadScene("BoardGame"); 
        } 

    }

    // Update is called once per frame
    void Update(){

    CheckWin(); //Runs the method CheckWin(); to see if the user has reached tile 40. 
    if (!_gameIsover){ //Will run when the game is not over
        _currentPos = _players[_currentPlayer].GetPosition(); //Gets the players current position

        /*Player Turn Setting*/

        if(_tileMovementAmount == 0 && !_isMoving){ //When the dice is not rolled yet, and we are not moving it will run this statement. 
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

        if (Input.GetKeyDown(KeyCode.Space) && _tileMovementAmount == 0 ){ //If the spacekey is pressed and the tilemovementamount is set to 0 (Which it should be since there is no initial die value), it will roll the dice. 
            _tileMovementAmount = _die.RollDice(); //Dice will be rolled from the die script and is set tothe tilemovement variable 
            _text.text = "You rolled a " + _tileMovementAmount.ToString(); //Text to show what the user rolled converted ToString(); since its in text form. 
            _turnStarted = true; //When we are rolling the dice, we dont want other actions to be done, unless stated. So the turn has started 

        }

        /*Player Color Setting*/

        if (_currentPlayer == 0 ){ //Color red when its player 1's turn
            _text.color = Color.red; 
        }
        else if(_currentPlayer == 1 ){ //Color grey when its player 2's turn
            _text.color = Color.grey; 
        } 
        else if(_currentPlayer == 2 ){ //Color blue when its player 3's turn
            _text.color = Color.blue; 
        }
        else if(_currentPlayer == 3 ){ //Color green when its player 4's turn
            _text.color = Color.green; 
        }

        /*Player Single Tile Movement*/

        if(_tileMovementAmount > 0 && !_isMoving){ //When the rolled dice amount is greater than 0, and is not moving, we are moving one tile based on the board script. This allows to move one tile over to another. Not a fluid transformation. 
            MoveOneTile(); 
        }

        /*Player Position Update Afrer Movement*/

        if(_isMoving){ //When the player is not moving and has finished moving from moving one tile, it will update its position from the method UpdatePosition();
            UpdatePostion(); 
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
}
