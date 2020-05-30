using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanArmAgent : MonoBehaviour
{
    public float xSpeed = 150;
    public float ySpeed = 75;
    public float zSpeed = 75;
    public float timer = 0;
    public bool startTimer = false;
    public bool playerControl = false;

    public double score = 0;

    GameObject _leftArm;
    GameObject _rightArm;
    GameObject _table;
    List<GameObject> _objects;

    Dictionary<int, Vector3> _idToPosition;
    Dictionary<int, Quaternion> _idToRotation;

    void Start()
    {
        // Initialize GameObject references.
        _leftArm = GameObject.Find("Left Arm");
        _rightArm = GameObject.Find("Right Arm");
        _table = GameObject.Find("Table");
        _objects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Item"));
        _objects.Add(_leftArm);
        _objects.Add(_rightArm);
        _objects.Add(GameObject.Find("Left Elbow"));
        _objects.Add(GameObject.Find("Right Elbow"));

        // Initialize and populate dictionaries.
        _idToPosition = new Dictionary<int, Vector3>();
        _idToRotation = new Dictionary<int, Quaternion>();
        foreach (GameObject obj in _objects)
        {
            _idToPosition.Add(obj.GetInstanceID(), obj.transform.localPosition);
            _idToRotation.Add(obj.GetInstanceID(), obj.transform.rotation);
        }
    }

    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
    }
}
