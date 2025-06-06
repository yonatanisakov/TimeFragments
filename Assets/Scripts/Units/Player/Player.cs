using EventBusScripts;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private float _shootCoolDown = 0.2f;

    private IPlayerMovement _playerMovement;
    private IPlayerWeapon _playerWeapon;
    private IPlayerInput _playerInput;


    [Inject]
    public void Construct(IPlayerInput playerInput,
        IPlayerMovement playerMovement,
        IPlayerWeapon playerWeapon)
    {
        _playerInput = playerInput;
        _playerMovement = playerMovement;
        _playerWeapon = playerWeapon;

        // Configure the injected services with this player's specific data
        _playerMovement.Initialize(transform, _speed);
        _playerWeapon.Initialize(_shootCoolDown, _muzzle);
    }
    // Update is called once per frame
    void Update()
    {
        _playerMovement.Move(_playerInput.MoveInput);

        _playerWeapon.UpdateCooldown();

        if (_playerInput.ShootInput)
        {
            _playerWeapon.TryShoot();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Time Fragment"))
        {
            EventBus.Get<PlayerGetHitEvent>().Invoke();
        }
    }
    public class Factory : PlaceholderFactory<Player> { }
}
