using UnityEngine;

namespace raven
{
    public class TabGroupAttribute : PropertyAttribute
    {
        public string GroupName { get; private set; }
        public string TabName { get; private set; }
        public bool UseFixedHeight { get; private set; }
        public float Height { get; private set; }
        public Color Color { get; private set; }

        public TabGroupAttribute(string groupName, string tabName, bool useFixedHeight = false, float height = 0, float r = 0, float g = 0, float b = 0)
        {
            GroupName = groupName;
            TabName = tabName;
            UseFixedHeight = useFixedHeight;
            Height = height;
            Color = new Color(r, g, b);
        }
    }
} 