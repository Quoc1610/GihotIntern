using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Transform playerTrans;
    public string name;
    public string id;
    public int gunId;
    public GunConfig gunConfig;
    public Player(string name, string id, int gunId)
    {
        this.name = name;
        this.id = id;
        this.gunId = gunId;

    }
}

public class PlayerManager
{
    public Dictionary<string, Player> dictPlayers = new Dictionary<string, Player>();
    public PlayerManager()
    {

    }

    public void AddPlayer(string name, string id, int gunId)
    {
        Player newPlayer = new Player(name, id, gunId);
        // newPlayer.name = name;
        // newPlayer.id = id;
        dictPlayers.Add(id,newPlayer);
    }
}