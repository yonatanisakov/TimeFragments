
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawner : IPlayerSpawner
{
    private Player.Factory _playerFactory;
    private List<Player> _players;

    [Inject]
    public PlayerSpawner(Player.Factory playerFactory)
    {
        _playerFactory = playerFactory;
        _players = new List<Player>();
    }
    public void DespawnAllPlayers()
    {
    foreach(Player player in _players)
        {
            if (player!=null)
                Object.Destroy(player.gameObject);
        }
    _players.Clear();
    }

    public Player SpawnPlayer()
    {
        Player player = _playerFactory.Create();
        _players.Add(player);
        return player;
    }
}