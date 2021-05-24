using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.1f;
    public float mouseSensitivity = 1f;
    public Animator animator;

    public Transform CameraTr;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        CameraTr = transform.Find("Main Camera");
        // 커서 숨기기
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Mouse1))
        {
            // 줌인
            Camera.main.fieldOfView = 10;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            // 줌아웃
            Camera.main.fieldOfView = 60;
        }

        // 무기 사용
        UseWeapon();

        // WASD, W위로, A왼쪽,S아래, D오른쪽
        Move();

        CameraRotae();
    }

    public GameObject bullet;   //총알
    public GameObject grenade;  // 수류탄
    public Transform bulletSpawnPosition;  // 수류탄
    private void UseWeapon()
    {
        // 마우스 클릭하면 총알 발사.
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(bullet, bulletSpawnPosition.position, CameraTr.rotation);

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Instantiate(grenade, bulletSpawnPosition.position, CameraTr.rotation);
        }

        // g키 누르면 수류탄 발사.
    }

    private void CameraRotae()
    {
        // 카메라 로테이션을 바꾸자 -> 마우스 이동량에 따라.
        float mouseMoveX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseMoveY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        var worldUp = CameraTr.InverseTransformDirection(Vector3.up);
        var rotation = CameraTr.rotation *
                       Quaternion.AngleAxis(mouseMoveX, worldUp) *
                       Quaternion.AngleAxis(mouseMoveY, Vector3.left);
        transform.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
        CameraTr.rotation = rotation;
    }

    private void Move()
    {
        float moveX = 0;
        float moveZ = 0;
        // || -> or
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveZ = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveZ = -1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1;

        // 이 트랜스폼의 앞쪽으로 움직여야 한다.
        Vector3 move = transform.forward * moveZ + transform.right * moveX;
        move.Normalize(); // 길이 1로 만들어 줌.
        transform.Translate(move * speed * Time.deltaTime, Space.World); // Space.Self : 로컬방향으로 움직임 Space.World : 월드방향으로 움직임
        //Vector3 position = transform.position;
        //position.x = position.x + moveX * speed * Time.deltaTime;
        //position.z = position.z + moveZ * speed * Time.deltaTime;
        //transform.position = position;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") == false)
        {
            if (moveX != 0 || moveZ != 0)
                animator.Play("run");
            else
                animator.Play("idle");
        }
    }
}
