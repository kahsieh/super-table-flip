using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanArmAgent : MonoBehaviour
{
    public float xSpeed = 150f;
    public float ySpeed = 75f;
    public float zSpeed = 75f;
    public float timer = 0f;
    public bool startTimer = false;
    public float score = 0f;

    // TODO: These probably need to change.
    public float speed = 2f;
    public float jumpHeight = 10f;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;

    GameObject _leftArm;
    GameObject _rightArm;
    GameObject _table;
    List<GameObject> _objects;

    Dictionary<int, Vector3> _idToPosition;
    Dictionary<int, Quaternion> _idToRotation;

    // For keyboard controls.
    Rigidbody _leftBody;
    Rigidbody _rightBody;
    Vector3 _leftDirection = Vector3.zero;
    Vector3 _rightDirection = Vector3.zero;

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

        // Initialize Rigidbodys.
        _leftBody = _leftArm.GetComponent<Rigidbody>();
        _rightBody = _rightArm.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
        UpdateLeft();
        UpdateRight();
    }

    // Update the left arm.
    void UpdateLeft()
    {
        // Basic directional controls.
        _leftDirection = Vector3.zero;
        _leftDirection.x -= Input.GetKey(KeyCode.A) ? 1 : 0;  // left
        _leftDirection.x += Input.GetKey(KeyCode.D) ? 1 : 0;  // right
        _leftDirection.z -= Input.GetKey(KeyCode.S) ? 1 : 0;  // forwards
        _leftDirection.z += Input.GetKey(KeyCode.W) ? 1 : 0;  // backwards
        _leftDirection.Normalize();

        // Jump control.
        bool isGrounded = Physics.CheckSphere(_leftBody.position,
            groundDistance, groundLayer, QueryTriggerInteraction.Ignore);
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            // Use a projectile motion equation to calculate initial y-velocity
            // needed for the body to reach the specified jump height.
            float vyi = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            _leftBody.AddForce(Vector3.up * vyi, ForceMode.VelocityChange);
        }
    }

    // Update the right arm.
    void UpdateRight()
    {
        // Basic directional controls.
        _rightDirection = Vector3.zero;
        _rightDirection.x -= Input.GetKey(KeyCode.L) ? 1 : 0;  // left
        _rightDirection.x += Input.GetKey(KeyCode.Quote) ? 1 : 0;  // right
        _rightDirection.z -= Input.GetKey(KeyCode.Semicolon) ? 1 : 0;  // forwards
        _rightDirection.z += Input.GetKey(KeyCode.P) ? 1 : 0;  // backwards
        _rightDirection.Normalize();

        // Jump control.
        bool isGrounded = Physics.CheckSphere(_rightBody.position,
            groundDistance, groundLayer, QueryTriggerInteraction.Ignore);
        if (Input.GetKeyDown(KeyCode.RightShift) && isGrounded)
        {
            // Use a projectile motion equation to calculate initial y-velocity
            // needed for the body to reach the specified jump height.
            float vyi = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            _rightBody.AddForce(Vector3.up * vyi, ForceMode.VelocityChange);
        }
    }

    // TODO: This needs work need to not hard code speed.
    void FixedUpdate()
    {
        Vector3 leftDisplacement = _leftDirection * speed * Time.fixedDeltaTime;
        _leftBody.MovePosition(_leftBody.position + leftDisplacement);
        Vector3 rightDisplacement = _rightDirection * speed * Time.fixedDeltaTime;
        _rightBody.MovePosition(_rightBody.position + rightDisplacement);
    }

    // Places objects back in their original positions. Also resets score and
    // timer.
    public void ResetItems()
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

    // Adds the specified reward/score to the player.
    public void SetRewardAndScore(float reward)
    {
        // Reward not applicable for human controls.
        score += reward;
    }

    // Gets the reward/score from the player.
    public double GetScore()
    {
        return score;
    }
}
