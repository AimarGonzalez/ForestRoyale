using UnityEngine;

namespace raven
{
    public class OnValueChangedAttribute : PropertyAttribute
    {
        public string MethodName { get; private set; }
        public bool IncludeChildren { get; private set; }
        public bool InvokeOnInitialize { get; private set; }

        public OnValueChangedAttribute(string methodName, bool includeChildren = false, bool invokeOnInitialize = false)
        {
            MethodName = methodName;
            IncludeChildren = includeChildren;
            InvokeOnInitialize = invokeOnInitialize;
        }
    }
} 