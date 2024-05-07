using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �������ͣʱ��ʾ������ʾ����������ص�UI���¡�
/// </summary>
public class HoverToReveal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [Header("Refs.")] [SerializeField] private RectTransform _tooltip = null;
    [SerializeField] private TextMeshProUGUI _contentRef;

    private bool _isHovering;

    /// <summary>
    /// ��ʼ��ʱ��ȡ������á�
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
    /// ��������ƶ��¼������������ͣ�����¹�����ʾ��λ�á�
    /// </summary>
    public void OnPointerMove(PointerEventData eventData)
    {
        if (!_isHovering) return;
        _tooltip.position = eventData.position;
    }
    
    /// <summary>
    /// ���¹�����ʾ�͹��Ŀɼ��ԡ�
    /// </summary>
    /// <param name="isHovering">�Ƿ���ͣ</param>
    private void UpdateTooltipAndCursorVisibility(bool isHovering)
    {
        _tooltip.gameObject.SetActive(isHovering);
        _isHovering = isHovering;
        Cursor.visible = !isHovering;
    }

    /// <summary>
    /// ������ʾ���ı����ݡ�
    /// </summary>
    /// <param name="text">Ҫ��ʾ�����ı���</param>
    [UsedImplicitly]
    public void UpdateText(string text)
    {
        _contentRef.text = text;
    }
}