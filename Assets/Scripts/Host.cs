using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using crass;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Host : MonoBehaviour
{
    [SerializeField] private float timePerQuestion = 30;
    [SerializeField] private int questionsPerRound;
    [SerializeField] private List<WeightedQuestion> questions;

    [SerializeField] private Transform questionParent;
    [SerializeField] private GameObject roundInterstitialContainer;
    [SerializeField] private int interstitialFrameDelay;

    [SerializeField] private RoundState roundState;
    [SerializeField] private Timer timer;

    private BagRandomizer<Question> _questionBag;

    private IEnumerator Start()
    {
        InitializeQuestionBag();
        while (true) yield return DoRound();
        // ReSharper disable once IteratorNeverReturns - infinite game loop
    }

    private void Update()
    {
        timer.Update();
    }

    private void InitializeQuestionBag()
    {
        _questionBag = new BagRandomizer<Question>
        {
            Items = new List<Question>()
        };

        foreach (var wq in questions)
            // could do fancy math trying to minimize the number of copies added while still respecting relative weights
            _questionBag.Items.AddRange(Enumerable.Repeat(wq.Question, wq.Weight));
    }

    private IEnumerator DoRound()
    {
        roundState.Initialize(questionsPerRound);
        roundInterstitialContainer.SetActive(false);

        for (var _ = 0; _ < questionsPerRound; _++) yield return AskNextQuestion();

        yield return WaitOnInterstitial(roundInterstitialContainer);
    }

    private IEnumerator AskNextQuestion()
    {
        var question = Instantiate(_questionBag.GetNext(), questionParent);

        var answer = question.Ask();
        timer.Begin(timePerQuestion);
        yield return new WaitWhile(() => timer.Ticking && !answer.Status.IsCompleted());
        timer.Stop(); // in case it's still ticking
        // TODO: cancel task when timer runs out

        if (question.AnsweredCorrectly())
            roundState.RightAnswers++;
        else
            roundState.WrongAnswers++;

        yield return question.GiveFeedback().ToCoroutine();

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

    [Serializable]
    public struct WeightedQuestion
    {
        public Question Question;
        public int Weight;
    }
}