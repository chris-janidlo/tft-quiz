using System;
using Newtonsoft.Json;
using UnityEngine;

[JsonObject(IsReference = true)]
public class Item
{
    public static string ResourcesPathPrefix = "Item Icons";

    private Sprite _sprite;

    public string Name, Effect;

    public Sprite Icon
    {
        get
        {
            if (_sprite == null)
                _sprite = Resources.Load<Sprite>($"{ResourcesPathPrefix}/{Name}") ??
                          throw new ArgumentException($"could not find icon for {Name}");

            return _sprite;
        }
    }

    public override string ToString()
    {
        return $"{Name}: {Effect}";
    }
}