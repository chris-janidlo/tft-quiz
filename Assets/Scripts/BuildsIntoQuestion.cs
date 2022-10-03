using System;
using System.Collections.Generic;
using System.Linq;
using crass;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BuildsIntoQuestion : Question
{
    [SerializeField] private int numComponents;

    [SerializeField] private LayoutGroup componentContainer, craftedItemsContainer;
    [SerializeField] private Button submitButton;

    [SerializeField] private Data data;
    [SerializeField] private MultipleChoiceItemButton buttonPrefab;
    [SerializeField] private Image componentPrefab;


    public override async UniTask<bool> Ask()
    {
        HashSet<CraftedItem> selectedAnswers = new(), expectedAnswers = new();

        void Callback(Item i)
        {
            if (i is not CraftedItem crafted) throw new Exception("this shouldn't happen");

            if (selectedAnswers.Contains(crafted))
                selectedAnswers.Remove(crafted);
            else
                selectedAnswers.Add(crafted);
        }

        Dictionary<(Item, Item), CraftedItem> recipesToResults = new();
        foreach (var crafted in data.Values.Crafted.OrderBy(c => c.Name))
        {
            var button = Instantiate(buttonPrefab, craftedItemsContainer.transform);
            button.Initialize(crafted, Callback);

            recipesToResults[SortedRecipeTuple(crafted.Recipe)] = crafted;
        }

        List<Item> seenComponents = new();
        for (var _ = 0; _ < numComponents; _++)
        {
            var component = data.Values.Components.PickRandom();
            Instantiate(componentPrefab, componentContainer.transform).sprite = component.Icon;

            foreach (var seen in seenComponents)
                expectedAnswers.Add(recipesToResults[SortedRecipeTuple(component, seen)]);

            seenComponents.Add(component);
        }

        await submitButton.OnClickAsync();

        return selectedAnswers.SetEquals(expectedAnswers);
    }

    private static (Item, Item) SortedRecipeTuple(IReadOnlyList<Item> recipe)
    {
        return SortedRecipeTuple(recipe[0], recipe[1]);
    }

    private static (Item, Item) SortedRecipeTuple(Item a, Item b)
    {
        return string.Compare(a.Name, b.Name, StringComparison.Ordinal) < 0
            ? (a, b)
            : (b, a);
    }
}