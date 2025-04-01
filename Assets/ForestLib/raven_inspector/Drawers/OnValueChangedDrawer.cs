using System;
using System.Collections.Generic;
using System.Reflection;
using Raven.Attributes;
using UnityEditor;
using UnityEngine;

namespace Raven.Drawers
{
	[CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
	public class OnValueChangedDrawer : PropertyDrawer
	{
		private OnValueChangedAttribute OnValueChangedAttribute => (OnValueChangedAttribute)attribute;

		// Track the last values by property path per object instance
		private static readonly Dictionary<string, object> LastValues = new Dictionary<string, object>();
		private static readonly HashSet<string> InitializedProperties = new HashSet<string>();

		// Register cleanup callback
		[InitializeOnLoadMethod]
		private static void RegisterCleanupCallback()
		{
			EditorApplication.playModeStateChanged += (state) =>
			{
				if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.EnteredEditMode)
				{
					LastValues.Clear();
					InitializedProperties.Clear();
				}
			};
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Create a unique key for this property instance
			string key = $"{property.serializedObject.targetObject.GetInstanceID()}_{property.propertyPath}";

			// Store the current value
			object currentValue = GetPropertyValue(property);

			// Check if we should invoke on initialize
			if (OnValueChangedAttribute.InvokeOnInitialize && !InitializedProperties.Contains(key))
			{
				InvokeMethod(property);
				InitializedProperties.Add(key);
			}

			// Draw the property
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(position, property, label, true);

			if (EditorGUI.EndChangeCheck())
			{
				// Apply changes to the serialized object immediately
				property.serializedObject.ApplyModifiedProperties();

				// Get the new value
				object newValue = GetPropertyValue(property);

				// Only invoke if the value has actually changed
				if (!Equals(currentValue, newValue))
				{
					// Value has changed, invoke the method
					InvokeMethod(property);
				}
			}

			// Update last value
			LastValues[key] = currentValue;
		}

		private object GetPropertyValue(SerializedProperty property)
		{
			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					return property.intValue;
				case SerializedPropertyType.Boolean:
					return property.boolValue;
				case SerializedPropertyType.Float:
					return property.floatValue;
				case SerializedPropertyType.String:
					return property.stringValue;
				case SerializedPropertyType.Color:
					return property.colorValue;
				case SerializedPropertyType.ObjectReference:
					return property.objectReferenceValue;
				case SerializedPropertyType.LayerMask:
					return property.intValue;
				case SerializedPropertyType.Enum:
					return property.enumValueIndex;
				case SerializedPropertyType.Vector2:
					return property.vector2Value;
				case SerializedPropertyType.Vector3:
					return property.vector3Value;
				case SerializedPropertyType.Vector4:
					return property.vector4Value;
				case SerializedPropertyType.Rect:
					return property.rectValue;
				case SerializedPropertyType.ArraySize:
					return property.arraySize;
				case SerializedPropertyType.Character:
					return property.stringValue;
				case SerializedPropertyType.AnimationCurve:
					return property.animationCurveValue;
				case SerializedPropertyType.Bounds:
					return property.boundsValue;
				case SerializedPropertyType.Gradient:
					return property.gradientValue;
				case SerializedPropertyType.Quaternion:
					return property.quaternionValue;
				default:
					return null;
			}
		}

		private void InvokeMethod(SerializedProperty property)
		{
			try
			{
				object target = property.serializedObject.targetObject;
				string methodName = OnValueChangedAttribute.MethodName;

				// Check if we should include children
				if (OnValueChangedAttribute.IncludeChildren && property.hasChildren)
				{
					// Iterate through all child properties
					var childProperty = property.Copy();
					var enterChildren = true;

					while (childProperty.NextVisible(enterChildren) &&
					       (enterChildren || childProperty.propertyPath.StartsWith(property.propertyPath + ".")))
					{
						// Let's not recurse further
						enterChildren = false;
					}
				}

				// Invoke the method
				MethodInfo methodInfo = target.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (methodInfo != null)
				{
					// Check if the method takes a parameter of the property's type
					var parameters = methodInfo.GetParameters();
					if (parameters.Length == 0)
					{
						// No parameters
						methodInfo.Invoke(target, null);
					}
					else if (parameters.Length == 1)
					{
						// Try to pass the property value
						var value = GetPropertyValue(property);
						if (value != null && parameters[0].ParameterType.IsAssignableFrom(value.GetType()))
						{
							methodInfo.Invoke(target, new[] {value});
						}
						else
						{
							// Can't pass parameter, call without
							Debug.LogWarning($"Method '{methodName}' expects parameter of type {parameters[0].ParameterType}, but property is of type {(value != null ? value.GetType().ToString() : "null")}");
							methodInfo.Invoke(target, null);
						}
					}
					else
					{
						Debug.LogWarning($"Method '{methodName}' has too many parameters ({parameters.Length}). Expected 0 or 1.");
						methodInfo.Invoke(target, null);
					}
				}
				else
				{
					Debug.LogWarning($"Method '{methodName}' not found on {target.GetType().Name}");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"Error invoking OnValueChanged method: {ex.Message}");
			}
		}
	}
}