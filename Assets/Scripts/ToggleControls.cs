using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControls : MonoBehaviour
{
   public GameObject player; 
   bool humanEnabled = true;
   void Start()
    {
        //player.GetComponent<ArmAgent>().Start();
        //player.GetComponent<HumanArmAgent>().Start();
        //player.GetComponent<HumanArmAgent>().ResetItems();
    }
    public void OnButtonPress()
    {
      Debug.Log("Button clicked");
      if (humanEnabled) 
      {
          player.GetComponent<HumanArmAgent>().ResetItems();
          player.GetComponent<ArmAgent>().enabled = true;
          player.GetComponent<HumanArmAgent>().enabled = false;
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
