using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreBoard : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "Score : " + 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = 
            "Score : " + player.GetComponent<ArmAgent>().getScore().ToString("F2");
    }
}
