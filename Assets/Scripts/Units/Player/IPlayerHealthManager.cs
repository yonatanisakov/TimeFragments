public interface IPlayerHealthManager
{
    int currentLives { get; }
    void OnPlayerHit();
    bool IsInvulnerable { get; }
    void GrantInvulnerability(float duration);
    void ClearInvulnerability();

}