using UnityEngine;

[CreateAssetMenu(fileName = "newRoundState.asset", menuName = "TFT Quiz/Round State")]
public class RoundState : ScriptableObject
{
    public int RightAnswers { get; set; }
    public int WrongAnswers { get; set; }
    public int TotalAnswers { get; private set; }

    public void Initialize(int totalAnswers)
    {
        RightAnswers = WrongAnswers = 0;
        TotalAnswers = totalAnswers;
    }
}