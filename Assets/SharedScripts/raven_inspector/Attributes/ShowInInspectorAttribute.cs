using UnityEngine;

namespace Raven.Attributes
{
    public class ShowInInspectorAttribute : PropertyAttribute
    {
        public bool ReadOnly { get; private set; }
        public bool ShowIf { get; private set; }
        public string Condition { get; private set; }

        public ShowInInspectorAttribute(bool readOnly = false, bool showIf = false, string condition = null)
        {
            ReadOnly = readOnly;
            ShowIf = showIf;
            Condition = condition;
        }
    }
} 