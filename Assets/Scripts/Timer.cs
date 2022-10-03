using UnityEngine;

[CreateAssetMenu(fileName = "newTimer.asset", menuName = "TFT Quiz/Timer")]
public class Timer : ScriptableObject
{
    public float RemainingTime { get; private set; }
    public float MaximumTime { get; private set; }

    public bool Ticking { get; private set; }


    public void Begin(float maxTime)
    {
        if (Ticking) Debug.LogWarning($"{name} is already running");

        RemainingTime = MaximumTime = maxTime;
        Ticking = true;
    }

    public void Update()
    {
        if (!Ticking) return;

        RemainingTime -= Time.deltaTime;
        if (RemainingTime <= 0) Stop();
    }

    public void Stop()
    {
        if (!Ticking) return;

        Ticking = false;
    }
}