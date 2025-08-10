public interface IFragmentTimeScaleService
{
    float CurrentFragmentTimeScale { get; }
    bool IsSlowMotionActive { get; }
}
