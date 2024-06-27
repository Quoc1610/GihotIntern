using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/BulletConfig/ShotgunBullet")]
public class ShotgunBullet : BulletConfig
{
    public override void Fire(Vector3 posSpawn, Vector3 target, BulletManager bulletManager)
    {
        Vector3 direction = (target - posSpawn).normalized;
        float spreadAngle = 10f;
        int initBulletCount = 3;
        for (int i = 0; i < initBulletCount; i++)
        {
            float angle = spreadAngle * (i - (initBulletCount - 1) / 2f);
            Vector3 spreadDirection = Quaternion.Euler(0, angle, 0) * direction;
            GameObject obj = GameObject.Instantiate(bulletPrefab, posSpawn, Quaternion.identity);
            BulletInfo newBullet = new BulletInfo(obj.transform, spreadDirection, speed);
            bulletManager.bulletInfoList.Add(newBullet);
        }

    }
}