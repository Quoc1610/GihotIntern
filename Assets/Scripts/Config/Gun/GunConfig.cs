using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Config/Gun")]
public class GunConfig : ScriptableObject
{
   public List<GunType> lsGunType = new List<GunType>();
    public LightPistolBullet lightPistolBullet;

    public void Load()
    {
        lsGunType.Add(new GunType(1, 3, Resources.Load("Configs/Bullet/LightPistolBullet") as BulletConfig, 10, null, Resources.Load("Prefabs/Bullet/Bullet") as GameObject));
        lightPistolBullet = Resources.Load("Configs/Bullet/LightPistolBullet") as LightPistolBullet;
        lightPistolBullet.bulletPrefab = Resources.Load("Prefabs/Bullet/Bullet") as GameObject;
    }
}

[System.Serializable]
public class GunType
{
    public int numberOfBullet;
    public float Firerate;
    public BulletConfig bulletConfig;
    public float FireRange;
    public GameObject gunPrefab;
    public GameObject bulletPrefab;

    public  GunType( int numberOfBullet, float Firerate, BulletConfig bulletConfig, float FireRange, GameObject gunPrefab, GameObject bulletPrefab)
    {
        this.numberOfBullet = numberOfBullet;
        this.Firerate = Firerate;
        this.bulletConfig = bulletConfig;
        this.FireRange = FireRange;
        this.gunPrefab = gunPrefab;
        this.bulletPrefab = bulletPrefab;
    }
}