using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private float lookSensitivty = 2.0f;
    private PlayerMotor motor;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
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

        Vector3 _cameraRotation = new Vector3(xRot, 0f, 0f) * lookSensitivty;

        //實現視角轉動
        motor.RotateCamera(_cameraRotation);

    }

}
