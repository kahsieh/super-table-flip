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

    void OnCollisionEnter(Collision collision)
    {
        // Reward only first touch so it will learn to flip the table hard.
        if (collision.gameObject.tag == "LeftArm" && !_leftArmCollided)
        {
            SetReward(5f);
            Debug.Log("LeftArm Reward");
            _leftArmCollided = true;
        }
        if (collision.gameObject.tag == "RightArm" && !_rightArmCollided)
        {
            SetReward(5f);
            Debug.Log("RightArm Reward");
            _rightArmCollided = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "LeftArm" && !_leftArmExit)
        {
            SetReward(1f);
            Debug.Log("LeftArm Reward 2");
            _leftArmExit = true;
        }
        if (collision.gameObject.tag == "RightArm" && !_rightArmExit)
        {
            SetReward(1f);
            Debug.Log("RightArm Reward 2");
            _rightArmExit = true;
        }
    }

    void SetReward(float reward) 
    {
        if (GameObject.Find("Player").GetComponent<ArmAgent>().enabled)
        {
            GameObject.Find("Player").GetComponent<ArmAgent>().externalSetReward(reward);
        }
        else
        {
            GameObject.Find("Player").GetComponent<HumanArmAgent>().externalSetReward(reward);
        }
    }
}
