using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    public Text playerPoints;
    public Text enemyPoints;
    private int playerScore = 0;
    private int enemyScore = 0;

    public void Scored(bool player)
    {
        if (player)
        {
            playerScore++;
            playerPoints.text = playerScore.ToString();
        }
        else
        {
            enemyScore++;
            enemyPoints.text = enemyScore.ToString();
        }
    }
}
