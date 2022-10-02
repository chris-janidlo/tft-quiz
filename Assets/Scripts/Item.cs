using Newtonsoft.Json;

[JsonObject(IsReference = true)]
public class Item
{
    public string Name, Effect;

    public override string ToString()
    {
        return $"{Name}: {Effect}";
    }
}