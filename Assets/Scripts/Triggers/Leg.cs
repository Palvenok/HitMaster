using UnityEngine;

public class Leg : MonoBehaviour, IBody
{
    private TriggerType type = TriggerType.Leg;
    public TriggerType Type => type;
}
