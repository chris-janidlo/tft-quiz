using System.Collections.Generic;
using System.Linq;
using crass;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildsIntoQuestion : Question
{
    [SerializeField] private int numComponents;
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

    public override async UniTask<bool> Ask()
    {
        correctAnswerTitle.SetActive(false);
        incorrectAnswerTitle.SetActive(false);
        HashSet<CraftedItem> correctAnswers = new();

        List<Item> seenComponents = new();
        for (var _ = 0; _ < numComponents; _++)
        {
            var component = data.Values.Components.PickRandom();
            Instantiate(componentPrefab, componentContainer.transform).sprite = component.Icon;

            foreach (var seen in seenComponents)
                correctAnswers.Add(data.GetCombination(component, seen));

            seenComponents.Add(component);
        }

        _answerButtons = new List<MultipleChoiceItemButton>();
        foreach (var crafted in data.Values.Crafted.OrderBy(c => c.Name))
        {
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