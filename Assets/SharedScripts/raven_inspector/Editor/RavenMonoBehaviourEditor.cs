using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using raven;

/// <summary>
/// Custom editor for MonoBehaviours that supports property grouping using the BoxGroup attribute.
/// This editor automatically groups properties with the same BoxGroup attribute into a single box.
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class RavenMonoBehaviourEditor : Editor
{
    // Dictionary to store grouped properties
    private Dictionary<string, List<SerializedProperty>> _boxGroupedProperties = new Dictionary<string, List<SerializedProperty>>();
    private Dictionary<string, List<SerializedProperty>> _foldoutGroupedProperties = new Dictionary<string, List<SerializedProperty>>();
    // List to store properties without a group
    private List<SerializedProperty> _ungroupedProperties = new List<SerializedProperty>();
    // Dictionary to keep track of foldout states for each group
    private Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>();
    // List to store methods with ButtonAttribute
    private List<MethodInfo> _buttonMethods = new List<MethodInfo>();

    private void OnEnable()
    {
        // Initialize the dictionaries and lists
        _boxGroupedProperties.Clear();
        _foldoutGroupedProperties.Clear();
        _ungroupedProperties.Clear();
        _foldoutStates.Clear();
        _buttonMethods.Clear();
        
        // Get all methods with ButtonAttribute
        Type targetType = target.GetType();
        MethodInfo[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        
        foreach (MethodInfo method in methods)
        {
            ButtonAttribute buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
            if (buttonAttribute != null)
            {
                _buttonMethods.Add(method);
            }
        }
        
        // Get all properties and organize them
        SerializedProperty iterator = serializedObject.GetIterator();
        if (iterator.NextVisible(true))
        {
            do
            {
                // Skip script field
                if (iterator.name == "m_Script")
                    continue;

                // Create a copy of the property
                SerializedProperty property = serializedObject.FindProperty(iterator.propertyPath);
                
                // Get the matching field info
                FieldInfo fieldInfo = GetFieldInfo(target.GetType(), iterator.name);
                if (fieldInfo != null)
                {
                    // Check if the field has a BoxGroup attribute
                    BoxGroupAttribute boxGroupAttribute = fieldInfo.GetCustomAttribute<BoxGroupAttribute>();
                    FoldoutGroupAttribute foldoutGroupAttribute = fieldInfo.GetCustomAttribute<FoldoutGroupAttribute>();
                    
                    if (boxGroupAttribute != null)
                    {
                        // Add the property to the box group
                        string groupName = boxGroupAttribute.GroupName;
                        if (!_boxGroupedProperties.ContainsKey(groupName))
                        {
                            _boxGroupedProperties[groupName] = new List<SerializedProperty>();
                        }
                        _boxGroupedProperties[groupName].Add(property.Copy());
                    }
                    else if (foldoutGroupAttribute != null)
                    {
                        // Add the property to the foldout group
                        string groupName = foldoutGroupAttribute.GroupName;
                        if (!_foldoutGroupedProperties.ContainsKey(groupName))
                        {
                            _foldoutGroupedProperties[groupName] = new List<SerializedProperty>();
                            _foldoutStates[groupName] = foldoutGroupAttribute.Expanded; // Use the attribute's expansion state
                        }
                        _foldoutGroupedProperties[groupName].Add(property.Copy());
                    }
                    else
                    {
                        // Add the property to the ungrouped list
                        _ungroupedProperties.Add(property.Copy());
                    }
                }
                else
                {
                    // If we couldn't find the field info, just add it to ungrouped
                    _ungroupedProperties.Add(property.Copy());
                }
            }
            while (iterator.NextVisible(false));
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the script field
        using (new EditorGUI.DisabledScope(true))
        {
            SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");
            if (scriptProperty != null)
            {
                EditorGUILayout.PropertyField(scriptProperty, true);
            }
        }

        // Draw box grouped properties
        DrawBoxGroups();
        
        // Draw foldout grouped properties
        DrawFoldoutGroups();

        // Draw button methods
        DrawButtonMethods();

        // Draw ungrouped properties
        foreach (var property in _ungroupedProperties)
        {
            EditorGUILayout.PropertyField(property, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    private void DrawBoxGroups()
    {
        foreach (var groupPair in _boxGroupedProperties)
        {
            string groupName = groupPair.Key;
            List<SerializedProperty> properties = groupPair.Value;
            
            if (properties.Count == 0)
                continue;
                
            // Get the BoxGroup attribute from the first property
            FieldInfo fieldInfo = GetFieldInfo(target.GetType(), properties[0].name);
            BoxGroupAttribute boxAttribute = fieldInfo?.GetCustomAttribute<BoxGroupAttribute>();
            
            if (boxAttribute == null)
                continue;

            // Begin the box
            GUIStyle boxStyle = new GUIStyle(EditorStyles.helpBox);
            boxStyle.padding = new RectOffset(10, 10, 10, 10);
            
            // Draw the box with the specified color
            Color originalColor = GUI.color;
            GUI.color = boxAttribute.HasCustomColor ? boxAttribute.Color : new Color(0.8f, 0.8f, 0.8f, 0.2f);
            
            EditorGUILayout.BeginVertical(boxStyle);
            GUI.color = originalColor;
            
            // Draw the title if enabled
            if (boxAttribute.ShowTitle)
            {
                GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
                titleStyle.alignment = boxAttribute.CenterTitle ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
                titleStyle.padding = new RectOffset(0, 0, 5, 5);
                
                EditorGUILayout.LabelField(groupName, titleStyle);
            }
            
            // Draw the properties (always shown, no foldout)
            foreach (var property in properties)
            {
                EditorGUILayout.PropertyField(property, true);
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(2);
        }
    }
    
    private void DrawFoldoutGroups()
    {
        foreach (var groupPair in _foldoutGroupedProperties)
        {
            string groupName = groupPair.Key;
            List<SerializedProperty> properties = groupPair.Value;
            
            if (properties.Count == 0)
                continue;
                
            // Get the FoldoutGroup attribute from the first property
            FieldInfo fieldInfo = GetFieldInfo(target.GetType(), properties[0].name);
            FoldoutGroupAttribute foldoutAttribute = fieldInfo?.GetCustomAttribute<FoldoutGroupAttribute>();
            
            if (foldoutAttribute == null)
                continue;
            
            // Draw the foldout header
            GUIStyle headerStyle = new GUIStyle(EditorStyles.foldoutHeader);
            headerStyle.fontStyle = foldoutAttribute.ShowTitle ? FontStyle.Bold : FontStyle.Normal;
            headerStyle.alignment = foldoutAttribute.CenterTitle ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
            
            _foldoutStates[groupName] = EditorGUILayout.Foldout(_foldoutStates[groupName], 
                foldoutAttribute.ShowTitle ? groupName : "", true, headerStyle);
            
            // Draw the properties if expanded
            if (_foldoutStates[groupName])
            {
                EditorGUI.indentLevel++;
                foreach (var property in properties)
                {
                    EditorGUILayout.PropertyField(property, true);
                }
                EditorGUI.indentLevel--;
            }
            
            EditorGUILayout.Space(2);
        }
    }
    
    private void DrawButtonMethods()
    {
        if (_buttonMethods.Count == 0)
            return;
            
        foreach (MethodInfo method in _buttonMethods)
        {
            ButtonAttribute buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
            if (buttonAttribute == null)
                continue;

            // Check if conditions are met to show the button
            if (!string.IsNullOrEmpty(buttonAttribute.ShowIf))
            {
                bool showCondition = EvaluateCondition(buttonAttribute.ShowIf);
                if (!showCondition)
                    continue;
            }
            
            if (!string.IsNullOrEmpty(buttonAttribute.HideIf))
            {
                bool hideCondition = EvaluateCondition(buttonAttribute.HideIf);
                if (hideCondition)
                    continue;
            }
            
            // Check if the button should be disabled
            bool disabled = false;
            if (!string.IsNullOrEmpty(buttonAttribute.DisableIf))
            {
                disabled = EvaluateCondition(buttonAttribute.DisableIf);
            }
            
            if (!string.IsNullOrEmpty(buttonAttribute.EnableIf))
            {
                disabled = !EvaluateCondition(buttonAttribute.EnableIf);
            }
            
            // Get button text
            string buttonText = !string.IsNullOrEmpty(buttonAttribute.ButtonText) 
                ? buttonAttribute.ButtonText 
                : ObjectNames.NicifyVariableName(method.Name);
                
            // Calculate button height
            float buttonHeight = EditorGUIUtility.singleLineHeight;
            switch (buttonAttribute.ButtonSize)
            {
                case ButtonSizes.Medium:
                    buttonHeight = EditorGUIUtility.singleLineHeight * 1.5f;
                    break;
                case ButtonSizes.Large:
                    buttonHeight = EditorGUIUtility.singleLineHeight * 2f;
                    break;
            }
            
            // Draw the button
            EditorGUI.BeginDisabledGroup(disabled);
            
            GUILayout.Space(4);
            if (GUILayout.Button(buttonText, GUILayout.Height(buttonHeight)))
            {
                InvokeMethod(method);
            }
            GUILayout.Space(4);
            
            EditorGUI.EndDisabledGroup();
        }
    }
    
    private bool EvaluateCondition(string conditionName)
    {
        // Get the method/property/field that represents the condition
        Type targetType = target.GetType();
        
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
        FieldInfo conditionField = GetFieldInfo(targetType, conditionName);
        if (conditionField != null && conditionField.FieldType == typeof(bool))
        {
            return (bool)conditionField.GetValue(target);
        }
        
        Debug.LogWarning($"Condition '{conditionName}' not found or not a boolean.");
        return true; // Default to showing/enabling if condition not found
    }
    
    private void InvokeMethod(MethodInfo method)
    {
        if (method == null)
            return;
            
        try
        {
            // Check method parameter count
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length == 0)
            {
                method.Invoke(target, null);
            }
            else
            {
                Debug.LogWarning($"Method '{method.Name}' has parameters but button methods should have none.");
            }
            
            // Refresh the editor to show any changes
            Repaint();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error invoking button method '{method.Name}': {ex.Message}");
        }
    }

    private FieldInfo GetFieldInfo(Type type, string fieldName)
    {
        // First try direct field lookup
        FieldInfo field = type.GetField(fieldName, 
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        
        if (field != null)
            return field;
        
        // If not found, try looking in base classes
        while (type.BaseType != null)
        {
            type = type.BaseType;
            field = type.GetField(fieldName, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            if (field != null)
                return field;
        }
        
        return null;
    }
} 