using TMPro;
using UnityEngine;

// TODO: summarize right/wrong with a graphic showing each question
public class QuestionInterstitial : MonoBehaviour
{
    [SerializeField] private RoundState roundState;
    [SerializeField] private TextMeshProUGUI summary;

    private void OnEnable()
    {
        var soFar = roundState.RightAnswers + roundState.WrongAnswers;
        summary.text = $"<color=\"green\">right: {roundState.RightAnswers}/{soFar}</color>";
        if (roundState.WrongAnswers > 0)
            summary.text += $"\n<color=\"red\">wrong: {roundState.WrongAnswers}/{soFar}</color>";
        summary.text += $"\nremaining: {roundState.TotalAnswers - soFar}";
    }
}