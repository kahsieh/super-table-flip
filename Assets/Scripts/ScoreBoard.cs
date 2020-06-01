using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        GetComponent<Text>().text = "Score: " + 0;
    }

    void Update()
    {
        GetComponent<Text>().text = "Score: " + GetScore().ToString("F2");
    }

    // Gets the reward/score from the player.
    double GetScore()
    {
        if (GameObject.Find("Player").GetComponent<ArmAgent>().enabled)
        {
            return player.GetComponent<ArmAgent>().GetScore();
        }
        else
        {
            return player.GetComponent<HumanArmAgent>().GetScore();
        }
    }
}
