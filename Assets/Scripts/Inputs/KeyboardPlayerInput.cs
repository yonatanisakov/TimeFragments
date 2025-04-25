using UnityEngine;
using UnityEngine.InputSystem;
using Zenject.SpaceFighter;

public class KeyboardPlayerInput : MonoBehaviour, IPlayerInput
{
    public Vector2 MoveInput { get; private set; }
    public bool ShootInput { get; private set; }
    public bool AbilityInput { get; private set; }

    private PlayerKeyboardControls controls;

    private void Awake()
    {
        controls = new PlayerKeyboardControls();

        controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        controls.Player.Shoot.performed += _ => ShootInput = true;
        controls.Player.UseAbility.performed += _ => AbilityInput = true;
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    private void LateUpdate()
    {
        // Reset the shoot and ability inputs after processing them
        ShootInput = false;
        AbilityInput = false;
    }
}
