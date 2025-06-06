using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;
using EventBus = EventBusScripts.EventBus;
public class LevelManager : ILevelManager
{
    private readonly LevelConfig _levelConfig;
    private readonly IFragmentSpawner _fragmentSpawner;
    private readonly ILevelInitializer _levelInitializer;
    private readonly IPowerUpDrop _powerUpDrop;

    private int _fragmentsRemaining;
    private float _timeLeft;

    [Inject]
    public LevelManager(LevelConfig levelConfig, IFragmentSpawner fragmentSpawner, ILevelInitializer levelInitializer, IPowerUpDrop powerUpDrop)
    {
        _levelConfig = levelConfig;
        _fragmentSpawner = fragmentSpawner;
        _levelInitializer = levelInitializer;
        _powerUpDrop = powerUpDrop;
    }
    public void Initialize()
    {
        _timeLeft = _levelConfig.timeLimit;
        _levelInitializer.InitializeLevel(_levelConfig);

        _fragmentsRemaining = 0;
        foreach (var fragmentConfig in _levelConfig.fragments)
        {
            _fragmentSpawner.SpawnFragment(fragmentConfig);
            _fragmentsRemaining += _fragmentSpawner.CalculateTotalFragments(fragmentConfig.recipe.splitDepth);
        }
        EventBus.Get<FragmentPoppedEvent>().Subscribe(OnFragmentPopped);
    }
    public void Tick()
    {
        if (_levelConfig.timeLimit > 0)
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                EventBus.Get<LevelFailEvent>().Invoke();
                CleanUp();
            }
        }
    }

    void OnFragmentPopped(Vector3 powerUpPos)
    {
        _fragmentsRemaining--;
        _powerUpDrop.TryDropPowerUp(powerUpPos, _levelConfig.powerUpDrops);
        if (_fragmentsRemaining == 0 && _levelConfig.scoreToWin == 0)
        {
            EventBus.Get<LevelWinEvent>().Invoke();
            CleanUp();
        }
    }
    void CleanUp()
    {
        EventBus.Get<FragmentPoppedEvent>().Unsubscribe(OnFragmentPopped);
    }
}
