using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;

/// <summary>
/// HierarchyHighlighter highlights GameObjects in the Unity Editor's Hierarchy view 
/// if they have a <see cref="HighlightMe"/> component.
/// </summary>
[UsedImplicitly]
[InitializeOnLoad]
public class HierarchyHighlighter
{
    /// <summary>
    /// Subscribes to the hierarchyWindowItemOnGUI event.
    /// </summary>
    static HierarchyHighlighter()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
    }

    /// <summary>
    /// This method is called for each GameObject in the Hierarchy. It checks if the GameObject has the <see cref="HighlightMe"/> component,
    /// and if so, it draws a rectangle with a specified color around the GameObject's name in the Hierarchy.
    /// </summary>
    /// <param name="instanceId">The instance ID of the GameObject in the Hierarchy.</param>
    /// <param name="selectionRect">The rectangle area representing the position and size of the Hierarchy item in the GUI.</param>
    private static void OnHierarchyItemGUI(int instanceId, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

        // Exit if the GameObject is not found.
        if (gameObject == null) return;

        // Attempt to retrieve the HighlightMe component.
        var highlightMe = gameObject.GetComponent<HighlightMe>();
        bool shouldHighlight = highlightMe != null;

        // Only proceed if the HighlightMe component is present.
        if (!shouldHighlight) return;

        HighlightItem(selectionRect, highlightMe, gameObject);
    }

    private static void HighlightItem(Rect selectionRect, HighlightMe highlightMe, Object gameObject)
    {
        // // Set icon size, typically 16x16 pixels which is commonly used in Unity's Hierarchy
        float iconSize = 16;
        float iconPadding = (selectionRect.height - iconSize) / 2; // Center the icon vertically

        // Adjust the highlight rect to exclude the icon
        Rect highlightRect = new(selectionRect.x + iconSize + iconPadding, selectionRect.y,
            selectionRect.width - iconSize - iconPadding, selectionRect.height);

        // Draw a colored rectangle over the highlight area, excluding the icon
        EditorGUI.DrawRect(highlightRect, highlightMe.highlightColor);

        // Draw the label next to the icon
        EditorGUI.LabelField(highlightRect, gameObject.name);
    }
}