using UnityEngine;

public class Arm : MonoBehaviour, IBody
{
    private TriggerType type = TriggerType.Arm;

    public TriggerType Type => type;
}
