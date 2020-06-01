using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControls : MonoBehaviour
{
    public GameObject player;
    bool humanEnabled = true;

    void Start()
    {
    }

    // Toggles between human and AI player.
    public void OnButtonPress()
    {
        Debug.Log("Button clicked");
        if (humanEnabled)
        {
            player.GetComponent<HumanArmAgent>().ResetItems();
            player.GetComponent<HumanArmAgent>().enabled = false;
            player.GetComponent<ArmAgent>().enabled = true;
            humanEnabled = false;
        }
        else
        {
            player.GetComponent<ArmAgent>().enabled = false;
            player.GetComponent<HumanArmAgent>().enabled = true;
            player.GetComponent<HumanArmAgent>().ResetItems();
            humanEnabled = true;
        }
    }
}
