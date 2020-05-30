using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Item" || collision.gameObject.tag == "Floor")
        {
            SetReward(1f);
            Debug.Log("Item reward");
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
