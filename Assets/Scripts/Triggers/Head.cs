using UnityEngine;

public class Head : MonoBehaviour, IBody
{
    private TriggerType type = TriggerType.Head;

    public TriggerType Type => type;
}
