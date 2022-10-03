using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private Timer timer;

    private void Update()
    {
        if (timer.Ticking)
            bar.fillAmount = 1 - timer.RemainingTime / timer.MaximumTime;
    }
}