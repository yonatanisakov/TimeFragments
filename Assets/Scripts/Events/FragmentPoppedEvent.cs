using EventBusScripts;
using UnityEngine;
public struct FragmentHitData
{
    public Vector3 position;
    public float radius;
    public int splitDepth;
    public int basePoints;
}

public class FragmentPoppedEvent : Event<FragmentHitData>
{
}