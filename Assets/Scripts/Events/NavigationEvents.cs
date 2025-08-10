using EventBusScripts;

/// <summary>
/// Navigation events for game flow control
/// Keep UI simple by using events for navigation
/// </summary>

/// <summary>
/// Fired when player wants to return to main menu from gameplay
/// </summary>
public class MainMenuRequestedEvent : Event<object> { }

/// <summary>
/// Fired when player wants to continue to next level
/// </summary>
public class NextLevelRequestedEvent : Event<object> { }