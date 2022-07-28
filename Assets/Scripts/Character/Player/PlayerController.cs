using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private float lookSensitivty = 2.0f;
    [SerializeField]
    private float thrusterForce = 1000f;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [SerializeField]
    private LayerMask environmentsMask;

    [Header("Spring setting")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;
    private PlayerMotor motor;
    private ConfigurableJoint joint;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        joint =GetComponent<ConfigurableJoint>();
    }
    void Start()
    {
        SetJointSettings(jointSpring);
    }
    void Update()
    {
        RaycastHit _hit;
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, environmentsMask))
        {
            joint.targetPosition = new Vector3(0f, _hit.point.y, 0f);
        }
        else
        {
             joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

        //最終位移向量
        Vector3 _velocity = (movHorizontal + movVertical).normalized * speed;

        //實現移動
        motor.Move(_velocity);
        
        //視角參數
        float yRot =Input.GetAxisRaw("Mouse X");

        Vector3 _rotaion = new Vector3(0f, yRot, 0f) * lookSensitivty;

        //實現視角轉動
        motor.Rotate(_rotaion);

        float xRot =Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = xRot * lookSensitivty;

        //實現視角轉動
        motor.RotateCamera(_cameraRotationX);

        Vector3 _thrusterForce = Vector3.zero;
        if(Input.GetButton("Jump") && thrusterFuelAmount >0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

            SetJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);
        //apply thrusterForce
        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce,
        };
    }

}
