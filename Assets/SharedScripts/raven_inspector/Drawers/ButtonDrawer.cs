using UnityEngine;
using UnityEditor;
using System.Reflection;
using Raven.Attributes;

namespace Raven.Drawers
{
    // Note: This drawer is kept for backward compatibility with ButtonAttribute on fields
    // For methods, the attribute will be handled by RavenMonoBehaviourEditor
    [CustomPropertyDrawer(typeof(PropertyWithButtonAttribute))]
    public class ButtonDrawer : PropertyDrawer
    {
        // Rename to PropertyWithButtonAttribute for fields
        private class PropertyWithButtonAttribute : PropertyAttribute
        {
            // Empty subclass for compatibility
        }
        
        private ButtonAttribute GetButtonAttribute()
        {
            // Get the ButtonAttribute from the field
            FieldInfo field = GetFieldInfoFromProperty();
            return field?.GetCustomAttribute<ButtonAttribute>();
        }
        
        private FieldInfo GetFieldInfoFromProperty()
        {
            // Try to get the field from the property path
            string propertyPath = fieldInfo.Name;
            
            // Extract the field name from the property path
            string fieldName = propertyPath;
            int lastDotIndex = propertyPath.LastIndexOf('.');
            if (lastDotIndex >= 0)
            {
                fieldName = propertyPath.Substring(lastDotIndex + 1);
            }
            
            // Get the field info from the target type
            System.Type targetType = fieldInfo.DeclaringType;
            return targetType.GetField(fieldName, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ButtonAttribute buttonAttribute = GetButtonAttribute();
            if (buttonAttribute == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            // Check if we should show the button
            if (!string.IsNullOrEmpty(buttonAttribute.ShowIf) && !EvaluateCondition(property, buttonAttribute.ShowIf))
            {
                return;
            }

            if (!string.IsNullOrEmpty(buttonAttribute.HideIf) && EvaluateCondition(property, buttonAttribute.HideIf))
            {
                return;
            }

            // Check if we should disable the button
            bool disabled = false;
            if (!string.IsNullOrEmpty(buttonAttribute.DisableIf))
            {
                disabled = EvaluateCondition(property, buttonAttribute.DisableIf);
            }

            if (!string.IsNullOrEmpty(buttonAttribute.EnableIf))
            {
                disabled = !EvaluateCondition(property, buttonAttribute.EnableIf);
            }

            // Calculate button size
            float buttonHeight = GetButtonHeight(buttonAttribute);
            Rect buttonRect = position;
            buttonRect.height = buttonHeight;

            // Draw the button
            EditorGUI.BeginDisabledGroup(disabled);
            if (GUI.Button(buttonRect, GetButtonText(property, buttonAttribute)))
            {
                InvokeMethod(property, buttonAttribute);
            }
            EditorGUI.EndDisabledGroup();

            // Draw the result if enabled
            if (buttonAttribute.DrawResult)
            {
                Rect resultRect = position;
                resultRect.y += buttonHeight + 5;
                resultRect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(resultRect, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ButtonAttribute buttonAttribute = GetButtonAttribute();
            if (buttonAttribute == null)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            
            // Check if the button is hidden
            if (!string.IsNullOrEmpty(buttonAttribute.ShowIf) && !EvaluateCondition(property, buttonAttribute.ShowIf))
            {
                return 0;
            }

            if (!string.IsNullOrEmpty(buttonAttribute.HideIf) && EvaluateCondition(property, buttonAttribute.HideIf))
            {
                return 0;
            }
            
            float height = GetButtonHeight(buttonAttribute);

            if (buttonAttribute.DrawResult)
            {
                height += EditorGUIUtility.singleLineHeight + 5;
            }

            return height;
        }

        private float GetButtonHeight(ButtonAttribute buttonAttribute)
        {
            switch (buttonAttribute.ButtonSize)
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

        private string GetButtonText(SerializedProperty property, ButtonAttribute buttonAttribute)
        {
            if (!string.IsNullOrEmpty(buttonAttribute.ButtonText))
            {
                return buttonAttribute.ButtonText;
            }

            if (!string.IsNullOrEmpty(buttonAttribute.MethodName))
            {
                return buttonAttribute.MethodName;
            }

            return property.name;
        }

        private void InvokeMethod(SerializedProperty property, ButtonAttribute buttonAttribute)
        {
            try
            {
                object target = property.serializedObject.targetObject;
                string methodName = buttonAttribute.MethodName ?? property.name;

                MethodInfo methodInfo = target.GetType().GetMethod(methodName, 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    
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
        
        private bool EvaluateCondition(SerializedProperty property, string conditionName)
        {
            // Get the target object
            object target = property.serializedObject.targetObject;
            System.Type targetType = target.GetType();
            
            // Try as method first
            MethodInfo conditionMethod = targetType.GetMethod(conditionName, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (conditionMethod != null && conditionMethod.ReturnType == typeof(bool))
            {
                return (bool)conditionMethod.Invoke(target, null);
            }
            
            // Try as property
            PropertyInfo conditionProperty = targetType.GetProperty(conditionName, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (conditionProperty != null && conditionProperty.PropertyType == typeof(bool))
            {
                return (bool)conditionProperty.GetValue(target);
            }
            
            // Try as field
            FieldInfo conditionField = targetType.GetField(conditionName, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (conditionField != null && conditionField.FieldType == typeof(bool))
            {
                return (bool)conditionField.GetValue(target);
            }
            
            Debug.LogWarning($"Condition '{conditionName}' not found or not a boolean.");
            return true; // Default to showing/enabling if condition not found
        }
    }
} 