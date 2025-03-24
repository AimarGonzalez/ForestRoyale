using UnityEngine;

namespace raven
{
    public class LabelTextAttribute : PropertyAttribute
    {
        public string Label { get; private set; }
        public bool Bold { get; private set; }
        public bool Italic { get; private set; }
        public Color Color { get; private set; }

        public LabelTextAttribute(string label, bool bold = false, bool italic = false, float r = 0, float g = 0, float b = 0)
        {
            Label = label;
            Bold = bold;
            Italic = italic;
            Color = new Color(r, g, b);
        }
    }
} 