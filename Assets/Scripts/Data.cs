using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "newDataContainer.asset", menuName = "TFT Quiz/Data Container")]
public class Data : ScriptableObject
{
    [SerializeField] private TextAsset dataSource;

    public Format Values { get; private set; }


    public void Load()
    {
        if (Values != null) return;

        Values = JsonConvert.DeserializeObject<Format>(dataSource.text);
    }

    public class Format
    {
        public List<Item> Components;
        public List<CraftedItem> Crafted;
    }
}