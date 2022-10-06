using crass;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceItemButton : MonoBehaviour
{
    public enum Correctness
    {
        CorrectlySelected,
        CorrectlyDeselected,
        IncorrectlySelected,
        IncorrectlyDeselected
    }

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI tier;
    [SerializeField] private Button button;
    [SerializeField] private Color selectedTint, deselectedTint;
    [SerializeField] private EnumMap<Correctness, Color> gradingTint;

    private bool _selected, _isAnswer;

    public Correctness CorrectnessState { get; private set; }

    private void Start()
    {
        icon.color = deselectedTint;
        button.onClick.AddListener(OnButtonClick);
    }

    public void Initialize(Item item, bool isAnswer)
    {
        _isAnswer = isAnswer;
        icon.sprite = item.Icon;
        if (item is CraftedItem crafted) tier.text = crafted.Tier.ToString();

        UpdateCorrectness();
    }

    public void RevealGrade()
    {
        icon.color = gradingTint[CorrectnessState];
    }

    private void OnButtonClick()
    {
        _selected = !_selected;
        icon.color = _selected ? selectedTint : deselectedTint;

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