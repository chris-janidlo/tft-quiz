using TMPro;
using UnityEngine;

// TODO: make this a nice summary like with the question interstitial
public class RoundInterstitial : MonoBehaviour
{
    [SerializeField] private RoundState roundState;
    [SerializeField] private TextMeshProUGUI summary;

    private void OnEnable()
    {
        summary.text = $"<color=\"green\">right: {roundState.RightAnswers}/{roundState.TotalAnswers}</color>";
        if (roundState.WrongAnswers > 0)
            summary.text += $"\n<color=\"red\">wrong: {roundState.WrongAnswers}/{roundState.TotalAnswers}</color>";
    }
}