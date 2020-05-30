using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControls : MonoBehaviour
{
   public GameObject player; 
   int n;
   void Start()
    {
        n = 0;
    }
    public void OnButtonPress()
    {
      Debug.Log("Button clicked");
      if (n == 0) 
      {
          n++;
          player.GetComponent<ArmAgent>().enabled = false;
          player.GetComponent<HumanArmAgent>().enabled = true;
      }
      else
      {
          n = 0;
          player.GetComponent<ArmAgent>().enabled = true;
          player.GetComponent<HumanArmAgent>().enabled = false;
      }
    }
}
