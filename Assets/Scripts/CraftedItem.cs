using System.Collections.Generic;
using System.Linq;

public class CraftedItem : Item
{
    public List<Item> Recipe;
    public string Tag;
    public Tier Tier;

    public override string ToString()
    {
        return @$"{Name} ({Tier})
{(string.IsNullOrWhiteSpace(Tag) ? "" : Tag)}

{string.Join('\n', Recipe.Select(r => r.Name))}

{Effect}";
    }
}