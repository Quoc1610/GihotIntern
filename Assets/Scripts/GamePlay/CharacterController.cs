using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject prefabBullet;
    public Transform gunTransform;
    public int gunId = AllManager.Instance().bulletManager.GetGunId(); //temporary until be able to get the playerID
    public static CharacterController _instance { get; private set; }
    public static CharacterController Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindAnyObjectByType<CharacterController>();
        }
        return _instance;
    }

    private void Start(){

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
    }
    public void SetTargetShoot(GameObject target)
    {
        Debug.Log("SetTargetShoot");
        //int gunId = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].gunId;
        // AllManager.Instance().bulletManager.SpawnBullet(gunTransform.position, target.transform.position, gunId);
        AllManager.Instance().bulletManager.SetTarget(target);
    }
}
