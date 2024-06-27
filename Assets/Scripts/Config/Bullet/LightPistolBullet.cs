using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/BulletConfig/LightPistolBullet")]
public class LightPistolBullet : BulletConfig
{

    public override void Fire(Vector3 posSpawn, Vector3 target, BulletManager bulletManager)
    {
        Vector3 direction = (target - posSpawn).normalized;
        GameObject obj = GameObject.Instantiate(bulletPrefab, posSpawn, Quaternion.identity);
        BulletInfo newBullet = new BulletInfo(obj.transform, direction, speed);
        bulletManager.bulletInfoList.Add(newBullet);
    }
}