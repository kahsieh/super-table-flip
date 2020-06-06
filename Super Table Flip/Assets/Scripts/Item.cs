using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // +1 reward/score for colliding with an item or the floor.
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Item" ||
            collision.gameObject.tag == "Floor")
        {
            SetRewardAndScore(1f);
            Debug.Log("Item/floor reward");
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
