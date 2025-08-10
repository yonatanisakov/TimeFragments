using EventBusScripts;

// === SHIELD EVENTS ===

/// <summary>
/// Event fired when shield power-up is activated
/// </summary>
public class ShieldActivatedEvent : Event
{
}

/// <summary>
/// Event fired when shield power-up is deactivated/consumed
/// </summary>
public class ShieldDeactivatedEvent : Event
{
}
public class ShieldConsumedEvent : Event
{
}
// === TIME SCALE EVENTS ===

/// <summary>
/// Data for time scale modifications
/// </summary>
public struct TimeScaleData
{
    public float fragmentTimeScale;
    public bool isApply; // true = apply effect, false = remove effect
}

/// <summary>
/// Event fired when time scale changes (for slow motion effects)
/// </summary>
public class TimeScaleChangedEvent : Event<TimeScaleData>
{
}

// === MAGNET EVENTS ===

/// <summary>
/// Data for magnet effects
/// </summary>
public struct MagnetData
{
    public float force;
    public float range;
}

/// <summary>
/// Event fired when magnet power-up is activated
/// </summary>
public class MagnetActivatedEvent : Event<MagnetData>
{
}

/// <summary>
/// Event fired when magnet power-up is deactivated
/// </summary>
public class MagnetDeactivatedEvent : Event
{
}

/// <summary>
/// Event fired every frame while magnet is active (for continuous effect)
/// </summary>
public class MagnetUpdateEvent : Event<MagnetData>
{
}

// === CONTROLS EVENTS ===

/// <summary>
/// Event fired when player controls are inverted/restored
/// </summary>
public class ControlsInvertedEvent : Event<bool> // true = inverted, false = normal
{
}