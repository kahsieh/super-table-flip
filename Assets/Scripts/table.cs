using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class table : MonoBehaviour
{
    bool left_collided = false;
    bool right_collided = false;
    bool left_exit = false;
    bool right_exit = false;


    public void Reset()
    {
        left_collided = false;
        right_collided = false;
        left_exit = false;
        right_exit = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        //reward only first touch -> hope it will learn to flip the table hard.
        if (collision.gameObject.tag == "leftarm" && !left_collided)
        {
            GameObject.Find("Player").GetComponent<ArmAgent>().externalSetReward(5f);
            Debug.Log("left Reward");
            left_collided = true;
        }
        if (collision.gameObject.tag == "rightarm" && !right_collided)
        {
            GameObject.Find("Player").GetComponent<ArmAgent>().externalSetReward(5f);
            Debug.Log("right Reward");
            right_collided = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "leftarm" && !left_exit)
        {
            GameObject.Find("Player").GetComponent<ArmAgent>().externalSetReward(1f);
            Debug.Log("left2 Reward");
            left_exit = true;
        }
        if (collision.gameObject.tag == "rightarm" && !right_exit)
        {
            GameObject.Find("Player").GetComponent<ArmAgent>().externalSetReward(1f);
            Debug.Log("right2 Reward");
            right_exit = true;
        }
    }
}
