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
        if(Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }
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
