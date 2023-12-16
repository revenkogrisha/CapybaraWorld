using UnityEngine;

namespace Core.Editor
{
    public static class ToolbarStyles
    {
        public static GUIStyle GetCommandButtonStyle(int fontSize = 12, float width = 100f, FontStyle fontStyle = FontStyle.Bold)
        {
            GUIStyle style = new GUIStyle("Command")
            {
                stretchWidth = true,
                fixedWidth = width,
                fontSize = fontSize,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = fontStyle
            };

            return style;
        }
    }
}