using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Raven.Attributes;
using UnityEditor;
using UnityEngine;

namespace Raven.Edtr
{
	/// <summary>
	/// Base inspector class that handles custom ordering of properties based on PropertyOrderAttribute.
	/// Extend this class to create inspectors that respect property order attributes.
	/// </summary>
	public class RavenInspectorBase : Editor
	{
		private Dictionary<string, int> propertyOrder = new Dictionary<string, int>();
		private bool propertiesOrdered = false;

		protected virtual void OnEnable()
		{
			// Get all properties with PropertyOrderAttribute and store their order
			propertiesOrdered = false;
			propertyOrder.Clear();
		}

		public override void OnInspectorGUI()
		{
			// Initialize the serialized object
			serializedObject.Update();

			// Get the ordered properties
			if (!propertiesOrdered)
			{
				CollectPropertyOrder();
			}

			// Draw the properties in order
			DrawPropertiesInOrder();

			// Apply changes
			serializedObject.ApplyModifiedProperties();
		}

		private void CollectPropertyOrder()
		{
			// Get the target type
			Type targetType = target.GetType();

			// Get all fields in the type hierarchy
			List<FieldInfo> fields = new List<FieldInfo>();
			Type currentType = targetType;

			while (currentType != null && currentType != typeof(MonoBehaviour) && currentType != typeof(ScriptableObject))
			{
				fields.AddRange(currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly));
				currentType = currentType.BaseType;
			}

			// Check each field for PropertyOrderAttribute
			foreach (var field in fields)
			{
				var orderAttribute = field.GetCustomAttribute<PropertyOrderAttribute>();
				if (orderAttribute != null)
				{
					propertyOrder[field.Name] = orderAttribute.Order;
				}
				else
				{
					// Assign a default high order to ensure it comes after ordered properties
					propertyOrder[field.Name] = int.MaxValue;
				}
			}

			propertiesOrdered = true;
		}

		private void DrawPropertiesInOrder()
		{
			// Get all visible properties
			SerializedProperty iterator = serializedObject.GetIterator();
			bool enterChildren = true;

			// Skip the Script field
			if (iterator.NextVisible(enterChildren))
			{
				// Skip script property
				if (iterator.name.Equals("m_Script"))
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.PropertyField(iterator);
					EditorGUI.EndDisabledGroup();
					enterChildren = false;
				}
			}

			// Collect all visible properties
			List<SerializedProperty> properties = new List<SerializedProperty>();
			while (iterator.NextVisible(enterChildren))
			{
				properties.Add(serializedObject.FindProperty(iterator.name));
				enterChildren = false;
			}

			// Sort properties based on the order dictionary
			properties = properties.OrderBy(p =>
			{
				if (propertyOrder.TryGetValue(p.name, out int order))
				{
					return order;
				}

				return int.MaxValue;
			}).ToList();

			// Draw the properties
			foreach (var property in properties)
			{
				EditorGUILayout.PropertyField(property, true);
			}
		}
	}
}