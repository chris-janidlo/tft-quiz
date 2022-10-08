using System;
using System.Collections.Generic;
using System.Linq;
using crass;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildsIntoQuestion : Question
{
    [Flags]
    public enum ExclusionOptions
    {
        None = 0,
        TierX = 1 << 0,
        TierS = 1 << 1,
        TierA = 1 << 2,
        TierB = 1 << 3,
        TierC = 1 << 4,
        TierD = 1 << 5,
        Spatula = 1 << 6
    }

    [SerializeField] private int numComponents;
    [SerializeField] private ExclusionOptions exclusions;
    [SerializeField] private string submitPromptText, continuePromptText;

    [SerializeField] private LayoutGroup componentContainer, craftedItemsContainer;
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI actionButtonPrompt;
    [SerializeField] private GameObject prompt, correctAnswerTitle, incorrectAnswerTitle;

    [SerializeField] private Data data;
    [SerializeField] private MultipleChoiceItemButton buttonPrefab;
    [SerializeField] private Image componentPrefab;

    private List<MultipleChoiceItemButton> _answerButtons;
    private bool _correct;

    private static ExclusionOptions ToFlag(Tier tier)
    {
        return (ExclusionOptions)(1 << (int)tier);
    }

    public override async UniTask<bool> Ask()
    {
        correctAnswerTitle.SetActive(false);
        incorrectAnswerTitle.SetActive(false);
        HashSet<CraftedItem> correctAnswers = new();

        var noSpat = exclusions.HasFlag(ExclusionOptions.Spatula);

        var availableComponents = data.Values.Components;
        if (noSpat)
            availableComponents = availableComponents.Where(c => c.Name != "Spatula").ToList();

        List<Item> seenComponents = new();
        for (var _ = 0; _ < numComponents; _++)
        {
            var component = availableComponents.PickRandom();
            Instantiate(componentPrefab, componentContainer.transform).sprite = component.Icon;

            foreach (var seen in seenComponents)
            {
                var crafted = data.GetCombination(component, seen);
                var flag = ToFlag(crafted.Tier);

                if (!exclusions.HasFlag(flag)) correctAnswers.Add(crafted);
            }

            seenComponents.Add(component);
        }

        _answerButtons = new List<MultipleChoiceItemButton>();
        foreach (var crafted in data.Values.Crafted.OrderBy(c => c.Name))
        {
            if (exclusions.HasFlag(ToFlag(crafted.Tier)) ||
                (noSpat && crafted.Recipe.Any(i => i.Name == "Spatula")))
                continue;

            var button = Instantiate(buttonPrefab, craftedItemsContainer.transform);
            var isAnswer = correctAnswers.Contains(crafted);
            button.Initialize(crafted, isAnswer);
            _answerButtons.Add(button);
        }

        actionButtonPrompt.text = submitPromptText;
        await actionButton.OnClickAsync();

        return _correct = !_answerButtons.Any(b => b.CorrectnessState is
            MultipleChoiceItemButton.Correctness.IncorrectlyDeselected or
            MultipleChoiceItemButton.Correctness.IncorrectlySelected);
    }

    public override async UniTask GiveFeedback()
    {
        prompt.SetActive(false);

        if (_correct)
            correctAnswerTitle.SetActive(true);
        else
            incorrectAnswerTitle.SetActive(true);

        foreach (var button in _answerButtons)
            button.RevealGrade();

        actionButtonPrompt.text = continuePromptText;
        await actionButton.OnClickAsync();
    }
}