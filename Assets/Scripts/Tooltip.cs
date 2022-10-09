using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using crass;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public enum OrientationZone
    {
        Top,
        Right,
        Bottom,
        Left
    }

    [SerializeField] private EnumMap<OrientationZone, OrientationSpec> orientations;
    [SerializeField] private float delay;

    [SerializeField] private GameObject visualsParent;
    [SerializeField] private TextMeshProUGUI textContainer;

    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    private readonly List<RaycastResult> _raycastResults = new();
    private ITooltipHoverable _currentHoverable;
    private PointerEventData _pointerEventData;

    private void Start()
    {
        _pointerEventData = new PointerEventData(eventSystem);
        visualsParent.SetActive(false);
    }

    private void Update()
    {
        UpdatePosition();
        UpdateDisplay();
        UpdateText();
    }

    private void UpdatePosition()
    {
        var visualTransform = visualsParent.transform as RectTransform;
        visualTransform!.position = Input.mousePosition;

        float
            xPercent = Input.mousePosition.x / Screen.width,
            yPercent = Input.mousePosition.y / Screen.height;

        OrientationSpec
            top = orientations[OrientationZone.Top],
            right = orientations[OrientationZone.Right],
            bottom = orientations[OrientationZone.Bottom],
            left = orientations[OrientationZone.Left];

        void ModifyPivot(float? _ = null, float? x = null, float? y = null)
        {
            if (x == null && y == null) return;

            var pivot = visualTransform.pivot;
            pivot = new Vector2
            (
                x ?? pivot.x,
                y ?? pivot.y
            );
            visualTransform.pivot = pivot;
        }

        // TODO: this happens even if the tooltip isn't visible
        if (yPercent > top.boundaryPercentage) ModifyPivot(y: top.pivotComponent);
        if (yPercent < bottom.boundaryPercentage) ModifyPivot(y: bottom.pivotComponent);
        if (xPercent > right.boundaryPercentage) ModifyPivot(x: right.pivotComponent);
        if (xPercent < left.boundaryPercentage) ModifyPivot(x: left.pivotComponent);
    }

    private void UpdateDisplay()
    {
        var hoverable = GetHovered();
        if (hoverable == _currentHoverable) return;

        _currentHoverable = hoverable;

        StopAllCoroutines();
        StartCoroutine(DisplayRoutine());
    }

    private void UpdateText()
    {
        textContainer.text = _currentHoverable?.GetTooltipText() ?? "";
    }

    private ITooltipHoverable GetHovered()
    {
        _pointerEventData.position = Input.mousePosition;
        _raycastResults.Clear();
        raycaster.Raycast(_pointerEventData, _raycastResults);

        return _raycastResults.SelectMany(result => result.gameObject.GetComponents<MonoBehaviour>())
            .OfType<ITooltipHoverable>()
            .FirstOrDefault();
    }

    private IEnumerator DisplayRoutine()
    {
        visualsParent.SetActive(false);

        if (_currentHoverable == null) yield break;
        yield return new WaitForSeconds(delay);

        visualsParent.SetActive(true);
    }

    [Serializable]
    public struct OrientationSpec
    {
        public float boundaryPercentage;
        public float pivotComponent;
    }
}