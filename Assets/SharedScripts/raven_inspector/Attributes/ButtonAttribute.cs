using UnityEngine;
using System;

namespace Raven.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
    public class ButtonAttribute : Attribute
    {
        public string ButtonText { get; private set; }
        public ButtonSizes ButtonSize { get; private set; }
        public bool Expanded { get; private set; }
        public bool DrawResult { get; private set; }
        public string ShowIf { get; private set; }
        public string HideIf { get; private set; }
        public string DisableIf { get; private set; }
        public string EnableIf { get; private set; }
        public string MethodName { get; private set; }

        public ButtonAttribute(string buttonText = null, ButtonSizes buttonSize = ButtonSizes.Small, bool expanded = true, bool drawResult = false, string showIf = null, string hideIf = null, string disableIf = null, string enableIf = null, string methodName = null)
        {
            ButtonText = buttonText;
            ButtonSize = buttonSize;
            Expanded = expanded;
            DrawResult = drawResult;
            ShowIf = showIf;
            HideIf = hideIf;
            DisableIf = disableIf;
            EnableIf = enableIf;
            MethodName = methodName;
        }
    }

    public enum ButtonSizes
    {
        Small,
        Medium,
        Large
    }
} 