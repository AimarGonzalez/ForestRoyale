using UnityEngine;

namespace raven
{
    /// <summary>
    /// Groups properties inside a box with an optional title.
    /// </summary>
    public class BoxGroupAttribute : PropertyAttribute
    {
        public string GroupName { get; private set; }
        public bool ShowTitle { get; private set; }
        public bool CenterTitle { get; private set; }
        public float Padding { get; private set; }
        public Color Color { get; private set; }
        public bool HasCustomColor { get; private set; }

        /// <summary>
        /// Groups properties inside a box with an optional title.
        /// </summary>
        /// <param name="groupName">The name of the group (also used as title if shown)</param>
        /// <param name="showTitle">Whether to show the title</param>
        /// <param name="centerTitle">Whether to center the title</param>
        /// <param name="padding">The padding inside the box</param>
        /// <param name="r">Red component of the box color (0-1)</param>
        /// <param name="g">Green component of the box color (0-1)</param>
        /// <param name="b">Blue component of the box color (0-1)</param>
        public BoxGroupAttribute(string groupName, bool showTitle = true, bool centerTitle = false, float padding = 10f, float r = 0, float g = 0, float b = 0)
        {
            GroupName = groupName;
            ShowTitle = showTitle;
            CenterTitle = centerTitle;
            Padding = padding;
            
            // Only use custom color if any color component is non-zero
            HasCustomColor = r != 0 || g != 0 || b != 0;
            Color = HasCustomColor ? new Color(r, g, b) : Color.white;
        }
    }
} 