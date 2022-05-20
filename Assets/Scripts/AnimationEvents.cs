using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public UnityEvent OnLaunch;

    private void LaunchArrow()
    {
        OnLaunch?.Invoke();
    }

    private void OnDestroy()
    {
        OnLaunch.RemoveAllListeners();
    }
}
