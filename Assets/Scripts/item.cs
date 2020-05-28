using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "object" || collision.gameObject.tag == "floor")
        {
            GameObject.Find("Player").GetComponent<ArmAgent>().externalSetReward(1f);
            Debug.Log("item Reward");
        }
    }
}
