using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Vector2 _position;
    Transform _trans; 
    int _currentTile = 1;
    bool _isPlayerTurn;

    private void Start(){
        _trans = GetComponent<Transform>();
        _position = _trans.position; 
        Debug.Log(_position);
    }

    private void Update(){
        _trans.position = _position;
    }

    public Vector2 GetPosition(){
        return _position; 
    }

    public void Setposition(Vector2 pos){
        _position = pos; 

    }

    public bool GetisPlayerTurn(){
        return _isPlayerTurn; 
    }

    public void SetisPlayerTurn(bool isTurn){
        _isPlayerTurn = isTurn; 
    }

    public int GetCurrentTile(){
        return _currentTile; 
    }

    public void SetCurrentTile(int currentTile){
        _currentTile = currentTile; 

    }
}
