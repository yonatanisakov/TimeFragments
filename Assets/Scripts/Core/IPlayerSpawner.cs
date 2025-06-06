using UnityEngine;

public interface IPlayerSpawner
{
    Player SpawnPlayer();
    void DespawnAllPlayers();
}