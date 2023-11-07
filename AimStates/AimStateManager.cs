using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;

public class AimStateManager : MonoBehaviour
{
    public AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();

    [SerializeField] float mouseSense = 1;
    //public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    float xAxis, yAxis;

    [HideInInspector] public Animator anim;//Reference to the animator
    [HideInInspector] public CinemachineVirtualCamera vCam;
    public float adsFov = 40;
    [HideInInspector] public float hipFov;  //Fov = field of view
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10;

    public Transform aimPos;
    //[HideInInspector] public Vector3 actualAimPos;//aimPos is getting Lerp 'ed so it won't be always looking at the center of the screen.So I make a new aimPos as actualAimPos
    [SerializeField] float aimSmoothSpeed = 20;//this variable value did not automatically added in unity.I spent more time on this error.so if error occurs in the future look out for variables like this
    [SerializeField] LayerMask aimMask;

    // for Camera smooth movement(when shooting)
    float xFollowPos;
    float yFollowPos, ogYPos;//ogYPos is to determine what height the camera should go
    [SerializeField] float crouchCamHeight = 0.6f;
    [SerializeField] float shoulderSwapSpeed = 10;//when shooting camera switching between the two sides of shoulders
    MovementStateManager moving;

    public GameObject bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        moving = GetComponent<MovementStateManager>();
        xFollowPos = camFollowPos.localPosition.x;
        ogYPos = camFollowPos.localPosition.y;
        yFollowPos = ogYPos;
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        hipFov = vCam.m_Lens.FieldOfView;
        anim = GetComponent<Animator>();
        SwitchState(Hip);
    }

    // Update is called once per frame
    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis,-80, 80);

        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);

        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);
        // ray is an infinite line. "Ray" in c# is a data structure used in c# to represent a "ray".It is used in game developing for like shooting projectiles.

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, aimMask))//this basically checks whether the ray intersect with an object along its way or not
        {
            
            //Math.Infinity sets the ray to an infinite distance or you can set it to a maximum distance
            //aimMask is a layer that determines which objects the ray intesect with.Only objects on the specified layers will be considered for the raycast


            EnemyControl enemy = hitInfo.transform.GetComponent<EnemyControl>();

            aimPos.position = Vector3.Lerp(aimPos.position, hitInfo.point, aimSmoothSpeed * Time.deltaTime);

            if(enemy != null)
            {
                enemy.enemyHitDamage(10f);
                GameObject impactGo = Instantiate(bloodEffect , hitInfo.point , Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGo,1f);
            }
        }

        MoveCamera();

        currentState.UpdateState(this);
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void MoveCamera()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) xFollowPos = -xFollowPos;
        if (moving.currentState == moving.Crouch) yFollowPos = crouchCamHeight;
        else yFollowPos = ogYPos;

        Vector3 newFollowPos = new Vector3(xFollowPos, yFollowPos, camFollowPos.localPosition.z);
        camFollowPos.localPosition = Vector3.Lerp(camFollowPos.localPosition, newFollowPos, shoulderSwapSpeed * Time.deltaTime);
    }
}
