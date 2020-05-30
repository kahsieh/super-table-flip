using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ArmAgent : Agent
{
    public float xSpeed = 150f;
    public float ySpeed = 75f;
    public float zSpeed = 75f;
    public float timer = 0f;
    public bool startTimer = false;
    public float score = 0f;

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

    // Places objects back in their original positions. Also resets score and
    // timer.
    public override void OnEpisodeBegin()
    {
        // Reset Table component.
        _table.GetComponent<Table>().Reset();

        // Reset item positions.
        Debug.Log("Start");
        foreach (GameObject obj in _objects)
        {
            obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            obj.transform.localPosition = _idToPosition[obj.GetInstanceID()];
            obj.transform.rotation = _idToRotation[obj.GetInstanceID()];
        }
        // Initialize score to 0
        score = 0f;

        // Start timer.
        timer = 0f;
        startTimer = true;
    }

    // Observe the physical state of game objects.
    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (GameObject obj in _objects)
        {
            sensor.AddObservation(obj.transform.position);
            sensor.AddObservation(obj.GetComponent<Rigidbody>().velocity);
            sensor.AddObservation(obj.transform.rotation);
        }
    }

    // Try an action and possibly end the episode or give a reward.
    public override void OnActionReceived(float[] vectorAction)
    {
        Debug.Log("Taking action");

        // Take actions.
        Vector3 leftSignal = Vector3.zero;
        leftSignal.x = vectorAction[0] * xSpeed;
        leftSignal.y = vectorAction[1] * ySpeed;
        leftSignal.z = vectorAction[2] * zSpeed;
        _leftArm.GetComponent<Rigidbody>().AddForce(leftSignal);

        Vector3 rightSignal = Vector3.zero;
        rightSignal.x = vectorAction[3] * xSpeed;
        rightSignal.y = vectorAction[4] * ySpeed;
        rightSignal.z = vectorAction[5] * zSpeed;
        _rightArm.GetComponent<Rigidbody>().AddForce(rightSignal);

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

        // Y-axis bonus.
        Rigidbody rBody = _table.GetComponent<Rigidbody>();
        float vy = Mathf.Abs(rBody.velocity.y);
        if (vy > 0.2f) {
            Debug.Log("Y-axis bonus: " + vy);
            SetRewardAndScore(vy * 10);
        }

        // Give a negative reward every timeframe (probably unnecessary).
        SetRewardAndScore(-0.1f);
    }

    // public override void Heuristic(float[] actionsOut)
    // {
    //     actionsOut[0] = Input.GetAxis("Horizontal");
    //     actionsOut[1] = Input.GetAxis("Vertical");
    //     actionsOut[2] = Input.GetAxis("Horizontal");
    //     actionsOut[3] = Input.GetAxis("Vertical");
    //     actionsOut[4] = Input.GetAxis("Horizontal");
    //     actionsOut[5] = Input.GetAxis("Vertical");
    // }

    // Adds the specified reward/score to the player.
    public void SetRewardAndScore(float reward)
    {
        SetReward(reward);
        score += reward;
    }

    // Gets the reward/score from the player.
    public double GetScore()
    {
        return score;
    }
}
