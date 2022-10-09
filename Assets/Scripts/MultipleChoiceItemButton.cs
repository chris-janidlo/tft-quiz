using System;
using System.Text;
using crass;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceItemButton : MonoBehaviour, ITooltipHoverable
{
    public enum Correctness
    {
        CorrectlySelected,
        CorrectlyDeselected,
        IncorrectlySelected,
        IncorrectlyDeselected
    }

    [Flags]
    public enum TooltipField
    {
        None = 0,
        Name = 1 << 0,
        Tier = 1 << 1,
        Recipe = 1 << 2,
        Effect = 1 << 3
    }

    public TooltipField tooltipInclusions;

    [SerializeField] private Image itemIcon, feedbackIcon;
    [SerializeField] private TextMeshProUGUI tier;
    [SerializeField] private Button button;
    [SerializeField] private Color selectedTint, deselectedTint;
    [SerializeField] private EnumMap<Correctness, Sprite> feedbackIcons;
    private readonly StringBuilder _tooltipSB = new();

    private Item _item;
    private bool _selected, _isAnswer;

    public Correctness CorrectnessState { get; private set; }

    private void Start()
    {
        itemIcon.color = deselectedTint;
        button.onClick.AddListener(OnButtonClick);
    }

    public string GetTooltipText()
    {
        _tooltipSB.Clear();

        var crafted = _item as CraftedItem;
        var isCrafted = crafted != null;

        if (tooltipInclusions.HasFlag(TooltipField.Name))
        {
            _tooltipSB.Append(_item.Name);
            if (isCrafted && tooltipInclusions.HasFlag(TooltipField.Tier)) _tooltipSB.Append($" ({crafted.Tier} Tier)");
            _tooltipSB.Append("\n\n");
        }

        if (isCrafted && tooltipInclusions.HasFlag(TooltipField.Recipe))
        {
            foreach (var comp in crafted.Recipe)
            {
                _tooltipSB.Append(comp.Name);
                _tooltipSB.Append('\n');
            }

            _tooltipSB.Append('\n');
        }

        if (tooltipInclusions.HasFlag(TooltipField.Effect)) _tooltipSB.Append(_item.Effect);

        return _tooltipSB.ToString();
    }

    public void Initialize(Item item, bool isAnswer)
    {
        _isAnswer = isAnswer;
        _item = item;
        itemIcon.sprite = item.Icon;
        if (item is CraftedItem crafted) tier.text = crafted.Tier.ToString();

        UpdateCorrectness();
    }

    public void GiveFeedback()
    {
        feedbackIcon.sprite = feedbackIcons[CorrectnessState];
        if (CorrectnessState is Correctness.IncorrectlyDeselected or Correctness.IncorrectlySelected)
            Debug.Log($"{CorrectnessState}: {_item}");
    }

    private void OnButtonClick()
    {
        _selected = !_selected;
        itemIcon.color = _selected ? selectedTint : deselectedTint;

        UpdateCorrectness();
    }

    private void UpdateCorrectness()
    {
        CorrectnessState = _selected switch
        {
            true when _isAnswer => Correctness.CorrectlySelected,
            true when !_isAnswer => Correctness.IncorrectlySelected,
            false when _isAnswer => Correctness.IncorrectlyDeselected,
            false when !_isAnswer => Correctness.CorrectlyDeselected,
            _ => CorrectnessState
        };
    }
}