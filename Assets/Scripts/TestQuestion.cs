using crass;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TestQuestion : Question
{
    [SerializeField] private Button answer;
    [SerializeField] private Image image;
    [SerializeField] private Data data;

    public override async UniTask<bool> Ask()
    {
        image.sprite = data.Values.Crafted.PickRandom().Icon;
        await answer.OnClickAsync();
        return true;
    }
}