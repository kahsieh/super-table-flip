using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    bool _leftArmCollided = false;
    bool _rightArmCollided = false;
    bool _leftArmExit = false;
    bool _rightArmExit = false;

    public void Reset()
    {
        _leftArmCollided = false;
        _rightArmCollided = false;
        _leftArmExit = false;
        _rightArmExit = false;
    }

    // +5 reward for each arm's first table touch.
    void OnCollisionEnter(Collision collision)
    {
        // Reward only first touch so it will learn to flip the table hard.
        if (collision.gameObject.tag == "LeftArm" && !_leftArmCollided)
        {
            SetRewardAndScore(5f);
            Debug.Log("LeftArm/Table reward 1");
            _leftArmCollided = true;
        }
        if (collision.gameObject.tag == "RightArm" && !_rightArmCollided)
        {
            SetRewardAndScore(5f);
            Debug.Log("RightArm/Table reward 1");
            _rightArmCollided = true;
        }
    }

    // +1 reward for finishing each arm's first table touch.
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "LeftArm" && !_leftArmExit)
        {
            SetRewardAndScore(1f);
            Debug.Log("LeftArm/Table reward 2");
            _leftArmExit = true;
        }
        if (collision.gameObject.tag == "RightArm" && !_rightArmExit)
        {
            SetRewardAndScore(1f);
            Debug.Log("RightArm/Table reward 2");
            _rightArmExit = true;
        }
    }

    // Adds the specified reward/score to the player.
    void SetRewardAndScore(float reward)
    {
        if (GameObject.Find("Player").GetComponent<ArmAgent>().enabled)
        {
            GameObject.Find("Player").GetComponent<ArmAgent>()
                                     .SetRewardAndScore(reward);
        }
        else
        {
            GameObject.Find("Player").GetComponent<HumanArmAgent>()
                                     .SetRewardAndScore(reward);
        }
    }
}
