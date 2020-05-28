using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ArmAgent : Agent
{
    public float speed = 100;
    public float timer = 0;
    public bool startTimer = false;

    GameObject _leftArm;
    GameObject _rightArm;
    GameObject _table;
    GameObject[] _items;

    Dictionary<int, Vector3> _idToPosition;
    Dictionary<int, Quaternion> _idToRotation;

    void Start()
    {
        // Initialize GameObject references.
        _leftArm = GameObject.Find("Left Arm");
        _rightArm = GameObject.Find("Right Arm");
        _table = GameObject.Find("Table");
        _items = GameObject.FindGameObjectsWithTag("Item");

        // Initialize and populate dictionaries.
        _idToPosition = new Dictionary<int, Vector3>();
        _idToRotation = new Dictionary<int, Quaternion>();
        foreach (GameObject obj in _items)
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
            Debug.Log(timer);
        }
    }

    public override void OnEpisodeBegin()
    {
        // Reset Table component.
        _table.GetComponent<Table>().Reset();

        // Reset item positions.
        Debug.Log("Start");
        foreach (GameObject obj in _items)
        {
            obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            obj.transform.localPosition = _idToPosition[obj.GetInstanceID()];
            obj.transform.rotation = _idToRotation[obj.GetInstanceID()];
        }

        // Start timer.
        timer = 0f;
        startTimer = true;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (GameObject obj in _items)
        {
            sensor.AddObservation(obj.transform.position);
            sensor.AddObservation(obj.GetComponent<Rigidbody>().velocity);
            sensor.AddObservation(obj.transform.rotation);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Take actions.
        Vector3 leftSignal = Vector3.zero;
        leftSignal.x = vectorAction[0];
        leftSignal.y = vectorAction[1];
        leftSignal.z = vectorAction[2];
        _leftArm.GetComponent<Rigidbody>().AddForce(leftSignal * speed);
        Debug.Log(leftSignal);

        Vector3 rightSignal = Vector3.zero;
        rightSignal.x = vectorAction[3];
        rightSignal.y = vectorAction[4];
        rightSignal.z = vectorAction[5];
        _rightArm.GetComponent<Rigidbody>().AddForce(rightSignal * speed);

        // End if timeout.
        if (timer > 10f)
        {
            startTimer = false;
            Debug.Log("End");
            EndEpisode();
        }

        // End if an arm moves too far away.
        // TODO: Should have a contraint between the two arms.
        if (_leftArm.transform.localPosition.x > 3 ||
            _leftArm.transform.localPosition.x < -3 ||
            _leftArm.transform.localPosition.z > 3 ||
            _leftArm.transform.localPosition.z < -3 ||
            _leftArm.transform.localPosition.y > 3 ||
            _leftArm.transform.localPosition.y < -3 ||
            _rightArm.transform.localPosition.x > 3 ||
            _rightArm.transform.localPosition.x < -3 ||
            _rightArm.transform.localPosition.z > 3 ||
            _rightArm.transform.localPosition.z < -3 ||
            _rightArm.transform.localPosition.y > 3 ||
            _rightArm.transform.localPosition.y < -3)
        {
            EndEpisode();
        }

        // Give a negative reward every timeframe (probably unnecessary).
        SetReward(-0.1f);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        actionsOut[2] = Input.GetAxis("Horizontal");
        actionsOut[3] = Input.GetAxis("Vertical");
        actionsOut[4] = Input.GetAxis("Horizontal");
        actionsOut[5] = Input.GetAxis("Vertical");
    }

    // For external objects' use.
    public void externalSetReward(float reward)
    {
        SetReward(reward);
    }
}
