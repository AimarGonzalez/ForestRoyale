using UnityEngine;

namespace raven
{
    public class PropertyOrderAttribute : PropertyAttribute
    {
        public int Order { get; private set; }

        public PropertyOrderAttribute(int order)
        {
            Order = order;
        }
    }
} 