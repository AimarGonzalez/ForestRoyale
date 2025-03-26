using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace Raven.editor
{
	/// <summary>
	/// Utility class containing common functions used by the Raven Inspector drawers.
	/// </summary>
	public static class RavenInspectorUtility
	{
		/// <summary>
		/// Evaluates a boolean condition on a target object using reflection.
		/// </summary>
		/// <param name="target">The target object to evaluate the condition on</param>
		/// <param name="condition">The name of the property, field, or method to evaluate</param>
		/// <param name="defaultValue">The default value to return if the condition can't be evaluated</param>
		/// <returns>The boolean result of the condition or the default value if it can't be evaluated</returns>
		public static bool EvaluateCondition(object target, string condition, bool defaultValue = true)
		{
			try
			{
				if (string.IsNullOrEmpty(condition) || target == null)
				{
					return defaultValue;
				}

				// Get the property info for the condition
				PropertyInfo propertyInfo = target.GetType().GetProperty(condition, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (propertyInfo != null)
				{
					// Check the return type
					if (propertyInfo.PropertyType == typeof(bool))
					{
						return (bool)propertyInfo.GetValue(target);
					}
					else
					{
						Debug.LogWarning($"Condition property '{condition}' on {target.GetType().Name} must return bool, but returns {propertyInfo.PropertyType.Name}");
						return defaultValue;
					}
				}

				// Try to get the field info
				FieldInfo fieldInfo = target.GetType().GetField(condition, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (fieldInfo != null)
				{
					// Check the field type
					if (fieldInfo.FieldType == typeof(bool))
					{
						return (bool)fieldInfo.GetValue(target);
					}
					else
					{
						Debug.LogWarning($"Condition field '{condition}' on {target.GetType().Name} must be bool, but is {fieldInfo.FieldType.Name}");
						return defaultValue;
					}
				}

				// Try to get the method info
				MethodInfo methodInfo = target.GetType().GetMethod(condition, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (methodInfo != null)
				{
					// Check the return type and parameter count
					if (methodInfo.ReturnType == typeof(bool) && methodInfo.GetParameters().Length == 0)
					{
						return (bool)methodInfo.Invoke(target, null);
					}
					else
					{
						Debug.LogWarning($"Condition method '{condition}' on {target.GetType().Name} must return bool and take no parameters");
						return defaultValue;
					}
				}

				Debug.LogWarning($"Condition '{condition}' not found on {target.GetType().Name}");
				return defaultValue;
			}
			catch (Exception ex)
			{
				Debug.LogError($"Error evaluating condition '{condition}': {ex.Message}");
				return defaultValue;
			}
		}

		/// <summary>
		/// Evaluates a boolean condition on a SerializedProperty's target object using reflection.
		/// </summary>
		/// <param name="property">The SerializedProperty whose target object will be evaluated</param>
		/// <param name="condition">The name of the property, field, or method to evaluate</param>
		/// <param name="defaultValue">The default value to return if the condition can't be evaluated</param>
		/// <returns>The boolean result of the condition or the default value if it can't be evaluated</returns>
		public static bool EvaluateCondition(SerializedProperty property, string condition, bool defaultValue = true)
		{
			if (property == null || property.serializedObject == null)
			{
				return defaultValue;
			}

			return EvaluateCondition(property.serializedObject.targetObject, condition, defaultValue);
		}
	}
}