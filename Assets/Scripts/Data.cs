using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "newDataContainer.asset", menuName = "TFT Quiz/Data Container")]
public class Data : ScriptableObject
{
    [SerializeField] private TextAsset dataSource;
    [SerializeField] private string itemIconResourcePathPrefix;

    public Format Values { get; private set; }

    public void Load()
    {
        if (Values != null) return;

        Values = JsonConvert.DeserializeObject<Format>(dataSource.text);
    }

    public Sprite GetItemIcon(Item item)
    {
        return Resources.Load<Sprite>($"{itemIconResourcePathPrefix}/{item.Name}") ??
               throw new ArgumentException($"could not find sprite for {item.Name}");
    }

    public class Format
    {
        public List<Item> Components;
        public List<CraftedItem> Crafted;
    }
}