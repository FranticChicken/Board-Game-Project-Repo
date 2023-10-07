using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int RollDice()
    {
        float diceRoll = Random.Range(1, 6);

        return (int)diceRoll;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
