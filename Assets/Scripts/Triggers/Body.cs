using UnityEngine;

public class Body : MonoBehaviour, IBody
{
    private TriggerType type = TriggerType.Body;

    public TriggerType Type => type;
}

public enum TriggerType
{
    None,
    Body,
    Head,
    Arm,
    Leg
}

public interface IBody
{
    public TriggerType Type { get; }
}
