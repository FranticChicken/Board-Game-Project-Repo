using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Board : MonoBehaviour
{
    //this allows us to place a game object into the board's componant
    //to signify where the start point for players will be
    [SerializeField] private Transform initialTransform;

    Vector2[] tilePositions = new Vector2[40];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector2[] GetTilePositions()
    {
        return tilePositions; 
    }

    //setting up the directions of our board
    public void InitTilePositions()
    {
        bool reverse = false;

        //the first tile will be where the initial transfrom is located
        tilePositions[0] = new Vector2(initialTransform.position.x, initialTransform.position.y);

        //for the first tile to the 40th tile
        for(int i = 1; i<40; i++)
        {
            //if the remainder of the tile divided by 8 = 0, the position for the tile will be 1y below
            if (i % 8 == 0)
            {
                tilePositions[i] = tilePositions[i - 1] + new Vector2(0f, -1f);
            }
            //otherwise if board direction is to the right, add 1 to x to make the position of the next tile
            else if (!reverse)
            {
                tilePositions[i] = tilePositions[i - 1] + new Vector2(1f, 0f);
            }
            //otherwise if board direction is to the left, subtract 1 from x to make the position of the next tile
            else if (reverse)
            {
                tilePositions[i] = tilePositions[i - 1] + new Vector2(-1f, 0f);
            }

            //if the next tile has a remainder of 0 when devided by 8, set the direction of tiles to the left
            if((i+1) % 8 == 0)
            {
                reverse = !reverse;
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
