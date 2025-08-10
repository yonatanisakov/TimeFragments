using EventBusScripts;

public class FragmentTimeScaleService : IFragmentTimeScaleService
{
    public float CurrentFragmentTimeScale { get; private set; } = 1f;
    public bool IsSlowMotionActive { get; private set; } = false;
    public FragmentTimeScaleService()
    {
        EventBus.Get<TimeScaleChangedEvent>().Subscribe(OnTimeScaleChanged);
        EventBus.Get<RestartGameEvent>().Subscribe(OnGameRestart);
        EventBus.Get<LevelCompletedEvent>().Subscribe(OnLevelCompleted);

    }
    private void OnTimeScaleChanged(TimeScaleData data)
    {
        CurrentFragmentTimeScale = data.isApply ? data.fragmentTimeScale : 1f;
        IsSlowMotionActive = data.isApply;
    }

    private void OnGameRestart()
    {
        ResetTimeScale();
    }

    private void OnLevelCompleted(LevelCompletionData data)
    {
        ResetTimeScale();
    }

    private void ResetTimeScale()
    {
        CurrentFragmentTimeScale = 1f;
        IsSlowMotionActive = false;

        // Fire event to notify all fragments to reset their time scale
        EventBus.Get<TimeScaleChangedEvent>().Invoke(new TimeScaleData
        {
            fragmentTimeScale = 1f,
            isApply = false
        });
    }

    ~FragmentTimeScaleService()
    {
        EventBus.Get<TimeScaleChangedEvent>().Unsubscribe(OnTimeScaleChanged);
        EventBus.Get<RestartGameEvent>().Unsubscribe(OnGameRestart);
        EventBus.Get<LevelCompletedEvent>().Unsubscribe(OnLevelCompleted);
    }
}