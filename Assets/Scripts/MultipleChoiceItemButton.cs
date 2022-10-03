using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceItemButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI tier;
    [SerializeField] private Button button;
    [SerializeField] private Color selectedTint, deselectedTint;

    private bool _selected;

    private void Start()
    {
        icon.color = deselectedTint;
        button.onClick.AddListener(OnButtonClick);
    }

    public void Initialize(Item item, Action<Item> callback)
    {
        icon.sprite = item.Icon;
        if (item is CraftedItem crafted) tier.text = crafted.Tier.ToString();
        button.onClick.AddListener(() => callback(item));
    }

    private void OnButtonClick()
    {
        _selected = !_selected;
        icon.color = _selected ? selectedTint : deselectedTint;
    }
}