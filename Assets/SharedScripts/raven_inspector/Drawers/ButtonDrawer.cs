using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace raven
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : PropertyDrawer
    {
        private ButtonAttribute ButtonAttribute => (ButtonAttribute)attribute;
        private SerializedProperty _currentProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _currentProperty = property;

            // Check if we should show the button
            if (!string.IsNullOrEmpty(ButtonAttribute.ShowIf) && !RavenInspectorUtility.EvaluateCondition(property, ButtonAttribute.ShowIf))
            {
                return;
            }

            if (!string.IsNullOrEmpty(ButtonAttribute.HideIf) && RavenInspectorUtility.EvaluateCondition(property, ButtonAttribute.HideIf))
            {
                return;
            }

            // Check if we should disable the button
            bool disabled = false;
            if (!string.IsNullOrEmpty(ButtonAttribute.DisableIf))
            {
                disabled = RavenInspectorUtility.EvaluateCondition(property, ButtonAttribute.DisableIf);
            }

            if (!string.IsNullOrEmpty(ButtonAttribute.EnableIf))
            {
                disabled = !RavenInspectorUtility.EvaluateCondition(property, ButtonAttribute.EnableIf);
            }

            // Calculate button size
            float buttonHeight = GetButtonHeight();
            Rect buttonRect = position;
            buttonRect.height = buttonHeight;

            // Draw the button
            EditorGUI.BeginDisabledGroup(disabled);
            if (GUI.Button(buttonRect, GetButtonText()))
            {
                InvokeMethod(property);
            }
            EditorGUI.EndDisabledGroup();

            // Draw the result if enabled
            if (ButtonAttribute.DrawResult)
            {
                Rect resultRect = position;
                resultRect.y += buttonHeight + 5;
                resultRect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(resultRect, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Check if the button is hidden
            if (!string.IsNullOrEmpty(ButtonAttribute.ShowIf) && !RavenInspectorUtility.EvaluateCondition(property, ButtonAttribute.ShowIf))
            {
                return 0;
            }

            if (!string.IsNullOrEmpty(ButtonAttribute.HideIf) && RavenInspectorUtility.EvaluateCondition(property, ButtonAttribute.HideIf))
            {
                return 0;
            }
            
            float height = GetButtonHeight();

            if (ButtonAttribute.DrawResult)
            {
                height += EditorGUIUtility.singleLineHeight + 5;
            }

            return height;
        }

        private float GetButtonHeight()
        {
            switch (ButtonAttribute.ButtonSize)
            {
                case ButtonSizes.Small:
                    return EditorGUIUtility.singleLineHeight;
                case ButtonSizes.Medium:
                    return EditorGUIUtility.singleLineHeight * 1.5f;
                case ButtonSizes.Large:
                    return EditorGUIUtility.singleLineHeight * 2f;
                default:
                    return EditorGUIUtility.singleLineHeight;
            }
        }

        private string GetButtonText()
        {
            if (!string.IsNullOrEmpty(ButtonAttribute.ButtonText))
            {
                return ButtonAttribute.ButtonText;
            }

            if (!string.IsNullOrEmpty(ButtonAttribute.MethodName))
            {
                return ButtonAttribute.MethodName;
            }

            return _currentProperty.name;
        }

        private void InvokeMethod(SerializedProperty property)
        {
            try
            {
                object target = property.serializedObject.targetObject;
                string methodName = ButtonAttribute.MethodName ?? property.name;

                MethodInfo methodInfo = target.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (methodInfo != null)
                {
                    methodInfo.Invoke(target, null);
                }
                else
                {
                    Debug.LogWarning($"Method '{methodName}' not found on {target.GetType().Name}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error invoking button method: {ex.Message}");
            }
        }
    }
} 