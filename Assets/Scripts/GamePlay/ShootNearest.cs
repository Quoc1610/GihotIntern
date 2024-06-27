using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShootNearest : MonoBehaviour
{
    public GunConfig gunConfig;
    public int currentGunIndex = 0;
    private float searchRadius = 0f;
    public int maxColliders = 10;
    private ITarget currentTarget;

    private void Update()
    {
        // Debug logging to check the status of gunConfig and lsGunType
        if (gunConfig == null)
        {
            Debug.LogError("gunConfig is null!");
        }
        else
        {
            Debug.Log("gunConfig is not null.");
            Debug.Log("lsGunType count: " + gunConfig.lsGunType.Count);
            if (gunConfig.lsGunType.Count <= currentGunIndex)
            {
                Debug.LogError("currentGunIndex is out of range!");
            }
            else
            {
                Debug.Log("currentGunIndex is within range.");
                searchRadius = gunConfig.lsGunType[currentGunIndex].FireRange;
                Debug.Log("searchRadius set to: " + searchRadius);
            }
        }

        FindInRadius();
    }

    void FindInRadius()
    {
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, searchRadius, hitColliders);
        float closestDistance = Mathf.Infinity;
        ITarget closestTarget = null;

        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].TryGetComponent<ITarget>(out ITarget target))
            {
                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        if (currentTarget != closestTarget)
        {
            if (currentTarget != null)
            {
                currentTarget.UnTarget();
            }
            currentTarget = closestTarget;
            if (currentTarget != null)
            {
                currentTarget.Target();
                Debug.Log("Targeting: " + currentTarget);
            }
        }
    }
}