using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    public Transform gunTransform;

    [SerializeField] private GameObject prefabBullet;
    public GameObject goChar;
    [SerializeField] public int gunId; 
    [SerializeField] LayerMask creepLayerMask;
    private FixedJoystick joystick;
    GameObject curCreepTarget = null;
    public string id;
    int frame = 0;
    public Vector3 velocity = new Vector3(0, 0, 0);
    float lastFireTime = 0f;
    [SerializeField] private Animator charAnim;
    Vector3 colllide_velocity = new Vector3(0, 0, 0);
    bool collision = false;
    public CharacterController characterController;
    //public static CharacterControl _instance { get; private set; }
    //public static CharacterControl Instance()
    //{
    //    if (_instance == null)
    //    {
    //        _instance = GameObject.FindAnyObjectByType<CharacterControl>();
    //    }
    //    return _instance;
    //}

    private void Awake()
    {
        //  SocketCommunication.GetInstance();
        //gunId = GameObject.FindAnyObjectByType<SceneUpdater>().bulletManager.GetGunId();
    }
    private void Start(){
        
        joystick = UIManager._instance._joystick;
    }

    public void SetGunAndBullet()
    {
        GunType gunType = AllManager.Instance().bulletManager.gunConfig.lsGunType[gunId];
        prefabBullet = gunType.bulletPrefab;
        GameObject currentGunPrefab = gunType.gunPrefab;
        GameObject gun = Instantiate(currentGunPrefab, transform.position, Quaternion.identity);
        gun.transform.SetParent(transform.Find("Gun"));
    }

    private void Update()
    {
        characterController.Move(velocity);
        Shoot();
        if (velocity != Vector3.zero)
        {
            charAnim.SetBool("isRun", true);
            
        }
        else
        {
            charAnim.SetBool("isRun", false);
        }
        if (id != Player_ID.MyPlayerID) return;
    
        if (frame % 3 == 0)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;
        
            Vector3 direction = new Vector3(horizontal, 0, vertical);
            Vector3 velocity = direction * speed * Time.deltaTime;
            SendData<MoveEvent> data = new SendData<MoveEvent>(new MoveEvent(velocity, transform.position,Quaternion.LookRotation(direction)));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }
        frame++;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            // Ignore collisions with enemies
            Physics.IgnoreCollision(hit.collider, characterController);
        }
    }

    private void FixedUpdate()
    {

    }


    GameObject GetTagetObj()
    {
        GunType gunType = AllManager.Instance().gunConfig.lsGunType[gunId];

        Collider[] creepColliders = Physics.OverlapSphere(transform.position, gunType.FireRange, creepLayerMask);

        GameObject targetObj = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider creepCollider in creepColliders)
        {
            float distance = Vector3.Distance(transform.position, creepCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetObj = creepCollider.gameObject;
            }
        }

        //Debug.Log("find target" + targetObj?.GetInstanceID().ToString());
        return targetObj;
    }

    void Shoot()
    {
        GameObject targetObj = GetTagetObj();

        if (targetObj == null) 
        {
            return;
        }

        if (targetObj != curCreepTarget) 
        {
            CreepManager creepManager = AllManager.Instance().creepManager;
            creepManager.MarkTargetCreepById(targetObj.GetInstanceID());
            if (curCreepTarget != null) 
            {
                creepManager.UnmarkTargetCreepById(curCreepTarget.GetInstanceID());
            } 
            curCreepTarget = targetObj;
        }

        Vector3 directionToTarget = (targetObj.transform.position - gunTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        gunTransform.rotation = lookRotation;
        lastFireTime = AllManager.Instance().bulletManager.SpawnBullet(gunTransform.position, curCreepTarget, gunId, lastFireTime, "PlayerBullet");

        //float angle = Vector3.Angle(gunTransform.forward, directionToTarget);
        //if (angle < 10f)
        //{
        //    AllManager.Instance().bulletManager.SpawnBullet(gunTransform.position, curCreepTarget, gunId);
        //}
    }

}