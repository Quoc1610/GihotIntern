using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private GameObject prefabBullet;
    public Transform gunTransform;
    [SerializeField] int gunId = AllManager.Instance().bulletManager.GetGunId(); //temporary until be able to get the playerID
    [SerializeField] LayerMask creepLayerMask;
    GameObject curCreepTarget = null;

    public static CharacterController _instance { get; private set; }
    public static CharacterController Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindAnyObjectByType<CharacterController>();
        }
        return _instance;
    }

    private void Awake()
    {
        //  SocketCommunication.GetInstance();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        transform.position += direction * speed * Time.deltaTime;
        Shoot();
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

        Debug.Log("find target" + targetObj?.GetInstanceID().ToString());
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

        AllManager.Instance().bulletManager.SpawnBullet(transform.position, gunId);
    }

    public void SetTargetShoot(GameObject target)
    {
        Debug.Log("SetTargetShoot");
        //int gunId = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].gunId;
        // AllManager.Instance().bulletManager.SpawnBullet(gunTransform.position, target.transform.position, gunId);
        AllManager.Instance().bulletManager.SetTarget(target);
    }
}
