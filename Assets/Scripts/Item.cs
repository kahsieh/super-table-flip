using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Item" || collision.gameObject.tag == "Floor")
        {
            GameObject.Find("Player").GetComponent<ArmAgent>().externalSetReward(1f);
            Debug.Log("Item reward");
        }
    }
}
