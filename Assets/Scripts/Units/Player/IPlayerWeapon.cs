
using Unity.VisualScripting;
using UnityEngine;

public interface IPlayerWeapon
{
    void Initialize(float cooldownTime, Transform muzzle);
    void TryShoot();
    void SetCooldown();
    void UpdateCooldown();
    void CancelCooldown();
}