﻿using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Creep
{
    public Transform creepTrans;
    public CreepConfig config;
    public CreepManager.CreepType type;
    public int hp;
    public float speed;
    public int dmg;

    public Creep(Transform creepTrans, CreepManager.CreepType type, CreepConfig config)
    {
        this.creepTrans = creepTrans;
        this.config = config;
        this.type = type;
        creepTrans.gameObject.SetActive(false);
    }

    public void Set(Vector3 pos, float time)
    {
        hp = config.BaseHp + (int)(time / 60) * 5;
        dmg = config.BaseDmg + (int)(time / 60) * 2;
        speed = config.BaseSpeed;
        creepTrans.position = pos;
        creepTrans.gameObject.SetActive(true);
    }

    public void UnSet() 
    {
        creepTrans.gameObject.SetActive(false);
    }

    public void Move()
    {
        config.Move(creepTrans, speed);
    }

    public void Attack()
    {
        config.Attack(this);
    }

    public void OnDead()
    {
        config.OnDead(creepTrans);
    }

    public void ProcessDmg(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            CreepManager creepManager = AllManager.Instance().creepManager;
            creepManager.AddToDeactivateList(this);
        }
    }
}

public class CreepManager
{
    private Dictionary<int, Creep> creepActiveDict = new Dictionary<int, Creep>();
    List<Creep>[] creepNotActiveByTypeList = new List<Creep>[7];
    private List<int> creepIdsToDeactivate = new List<int>();
    //private List<CreepSpawnInfo> needSpawnCreeps = new List<CreepSpawnInfo>();
    private CreepConfig[] creepConfigs;

    public enum CreepType
    {
        Creep1,
        Creep2, 
        Creep3,
        Creep4,
        Creep5,
        Creep6,
        Creep7
    }

    private void SpawnCreep(CreepType creepType)
    {
        CreepConfig config = GetCreepConfigByType(creepType);
        GameObject creepObj = GameObject.Instantiate(config.CreepPrefab);
        Creep creep = new Creep(creepObj.transform, creepType, config);
        creepNotActiveByTypeList[(int) creepType].Add(creep);
    }

    public CreepManager(AllCreepConfig allCreepConfig)
    {
        creepConfigs = allCreepConfig.CreepConfigs;
        
        for (int i = 0; i < creepNotActiveByTypeList.Length; i++) 
        {
            creepNotActiveByTypeList[i] = new List<Creep>();
        }

        for (CreepType type = CreepType.Creep1; type <= CreepType.Creep7; type++)
        {
            for (int i = 0; i < Constants.MaxCreepForEachType; i++)
            {
                SpawnCreep(type);
            }
        }
    }

    public CreepConfig GetCreepConfigByType(CreepType type)
    {
        return creepConfigs[(int)type];
    }

    public void AddToDeactivateList(Creep creep)
    {
        creepIdsToDeactivate.Add(creep.creepTrans.gameObject.GetInstanceID());
    }

    public void ActivateCreep(Vector3 spawnPos, CreepType creepType, float time)
    {
        // All creep of same type have been activated => Spawn new
        if (creepNotActiveByTypeList[(int)creepType].Count == 0)
        {
            SpawnCreep(creepType);
        }

        Creep creepNeedActive = creepNotActiveByTypeList[(int)creepType][0];
            
        creepNeedActive.Set(spawnPos, time);
        creepActiveDict.Add(creepNeedActive.creepTrans.gameObject.GetInstanceID(), creepNeedActive);
            
        creepNotActiveByTypeList[(int)creepType].RemoveAt(0);
    }

    public Creep GetActiveCreepById(int id)
    {
        Creep creep;
        if (creepActiveDict.TryGetValue(id, out creep))
        {
            return creep;
        }
        return null;
    }

    public void MyUpdate()
    {
        /*if (_timer >= 2)
        {
            SpawnCreep(new Vector3(Random.Range(-38, 38), 1, Random.Range(-38, 38)), (CreepType) Random.Range(0, 7), _timer);     
            _timer = 0;
        } else
        {
            _timer += Time.deltaTime;
        }*/

        foreach (var pair in creepActiveDict)
        {
            Creep creep = pair.Value;
            creep.Attack();
            creep.Move();
        }
    }

    private void DeactivateCreep(Creep creep)
    {
        creep.UnSet();
        creepNotActiveByTypeList[(int) creep.type].Add(creep);
        creepActiveDict.Remove(creep.creepTrans.gameObject.GetInstanceID());
    }

    public void LateUpdate()
    {
        for (int i = 0; i < creepIdsToDeactivate.Count; i++)
        {
            Creep creep = GetActiveCreepById(creepIdsToDeactivate[i]);

            if (creep == null)
            {
                creepIdsToDeactivate.RemoveAt(i);
                continue;
            }

            //AllManager.Instance.effectManager.SpawnEffect(EffectManager.EffectType.EXPLOSION, enemyInfo.enemyTrans.position);
            creep.OnDead();
            
            DeactivateCreep(creep);
        }

        creepIdsToDeactivate.Clear();
    }

    public void ProcessCollision(int creepId, GameObject colliderobject)
    {
        //TODO: Bullet Dmg
        //TODO: Sync 
        int dmg = 5;
        Creep creep = GetActiveCreepById(creepId);
        creep.ProcessDmg(dmg);
    }

    //public void Reset()
    //{
    //    enemyIdsToDestroy.Clear();
    //    enemyInfoDict.Clear();
    //}

    //public void DeleteAllObj()
    //{
    //    foreach (var pair in enemyInfoDict)
    //    {
    //        AddToDestroyList(pair.Value);
    //        for (int i = 0; i < enemyConfigs.Length; i++)
    //        {
    //            timeCountForSpawnEnemys[i] = 0;
    //        }
    //    }
    //}

    //public bool isClean()
    //{
    //    return needSpawnedNums.All(n => n == 0) && enemyInfoDict.Count == 0;
    //}
}
