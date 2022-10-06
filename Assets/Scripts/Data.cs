using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "newDataContainer.asset", menuName = "TFT Quiz/Data Container")]
public class Data : ScriptableObject
{
    [SerializeField] private TextAsset dataSource;

    private Dictionary<(Item, Item), CraftedItem> _recipesToResults;

    public Format Values { get; private set; }

    public void Load()
    {
        if (Values != null) return;

        Values = JsonConvert.DeserializeObject<Format>(dataSource.text);

        _recipesToResults = Values.Crafted
            .OrderBy(c => c.Name)
            .ToDictionary(k => SortedRecipeTuple(k.Recipe), v => v);
    }

    public CraftedItem GetCombination(Item a, Item b)
    {
        return _recipesToResults.TryGetValue(SortedRecipeTuple(a, b), out var result)
            ? result
            : null;
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

    public class Format
    {
        public List<Item> Components;
        public List<CraftedItem> Crafted;
    }
}