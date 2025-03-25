using UnityEngine;

namespace Raven.Attributes
{
    public class FoldoutGroupAttribute : PropertyAttribute
    {
        public string GroupName { get; private set; }
        public bool Expanded { get; private set; }
        public bool ShowTitle { get; private set; }
        public bool CenterTitle { get; private set; }
        public Color Color { get; private set; }

        public FoldoutGroupAttribute(string groupName, bool expanded = true, bool showTitle = true, bool centerTitle = false, float r = 0, float g = 0, float b = 0)
        {
            GroupName = groupName;
            Expanded = expanded;
            ShowTitle = showTitle;
            CenterTitle = centerTitle;
            Color = new Color(r, g, b);
        }
    }
} 