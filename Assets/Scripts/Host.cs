using System.Collections;
using crass;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Host : MonoBehaviour
{
    [SerializeField] private float timePerQuestion = 30;
    [SerializeField] private int questionsPerRound;
    [SerializeField] private BagRandomizer<Question> questions;

    [SerializeField] private Transform questionParent;
    [SerializeField] private GameObject roundInterstitialContainer, questionInterstitialContainer;
    [SerializeField] private int interstitialFrameDelay;

    [SerializeField] private RoundState roundState;
    [SerializeField] private Timer timer;

    private IEnumerator Start()
    {
        while (true) yield return DoRound();
        // ReSharper disable once IteratorNeverReturns - infinite game loop
    }

    private void Update()
    {
        timer.Update();
    }

    private IEnumerator DoRound()
    {
        roundState.Initialize(questionsPerRound);
        roundInterstitialContainer.SetActive(false);
        questionInterstitialContainer.SetActive(false);

        for (var _ = 0; _ < questionsPerRound; _++)
        {
            yield return AskNextQuestion();
            // TODO: instead of a static interstitial, add a method to Questions for showing results (either explaining what you got wrong or saying "Correct!")
            yield return WaitOnInterstitial(questionInterstitialContainer);
        }

        yield return WaitOnInterstitial(roundInterstitialContainer);
    }

    private IEnumerator AskNextQuestion()
    {
        var question = Instantiate(questions.GetNext(), questionParent);

        var answer = question.Ask();
        timer.Begin(timePerQuestion);
        yield return new WaitWhile(() => timer.Ticking && !answer.Status.IsCompleted());
        timer.Stop(); // in case it's still ticking
        // TODO: cancel task when timer runs out

        if (answer.Status.IsCompleted() && answer.GetAwaiter().GetResult())
            roundState.RightAnswers++;
        else
            roundState.WrongAnswers++;

        question.Cleanup();
    }

    private IEnumerator WaitOnInterstitial(GameObject container)
    {
        container.SetActive(true);

        var i = interstitialFrameDelay;
        while (true)
        {
            if (i-- <= 0 && Input.anyKey) break;
            yield return null;
        }

        container.SetActive(false);
    }
}