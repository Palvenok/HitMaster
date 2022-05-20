using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    public void Launch(float force, Vector3 targetPoint)
    {
        transform.DOLookAt(targetPoint, 0);
        transform.DOMove(targetPoint, force);
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.parent = other.transform;
    }
}
