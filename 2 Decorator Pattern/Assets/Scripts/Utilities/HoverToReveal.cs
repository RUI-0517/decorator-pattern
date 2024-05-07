using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 当鼠标悬停时显示工具提示，并处理相关的UI更新。
/// </summary>
public class HoverToReveal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [Header("Refs.")] [SerializeField] private RectTransform _tooltip = null;
    [SerializeField] private TextMeshProUGUI _contentRef;

    private bool _isHovering;

    /// <summary>
    /// 初始化时获取组件引用。
    /// </summary>
    [UsedImplicitly]
    private void Awake()
    {
        _contentRef = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateTooltipAndCursorVisibility(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpdateTooltipAndCursorVisibility(false);
    }

    /// <summary>
    /// 处理鼠标移动事件，如果正在悬停，更新工具提示的位置。
    /// </summary>
    public void OnPointerMove(PointerEventData eventData)
    {
        if (!_isHovering) return;
        _tooltip.position = eventData.position;
    }
    
    /// <summary>
    /// 更新工具提示和光标的可见性。
    /// </summary>
    /// <param name="isHovering">是否悬停</param>
    private void UpdateTooltipAndCursorVisibility(bool isHovering)
    {
        _tooltip.gameObject.SetActive(isHovering);
        _isHovering = isHovering;
        Cursor.visible = !isHovering;
    }

    /// <summary>
    /// 更新显示的文本内容。
    /// </summary>
    /// <param name="text">要显示的新文本。</param>
    [UsedImplicitly]
    public void UpdateText(string text)
    {
        _contentRef.text = text;
    }
}