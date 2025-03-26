using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Raven.Attributes;

namespace Raven.Drawers
{
	[CustomPropertyDrawer(typeof(TabGroupAttribute))]
	public class TabGroupDrawer : PropertyDrawer
	{
		private static readonly Dictionary<string, int> _selectedTabs = new Dictionary<string, int>();

		private static readonly Dictionary<string, bool> _expandedTabs = new Dictionary<string, bool>();

		// Dictionary<groupKey>
		private static readonly Dictionary<string, HashSet<string>> s_AllTabsCatalog = new Dictionary<string, HashSet<string>>();

		private TabGroupAttribute _currentTabGroupAttribute => (TabGroupAttribute)attribute;

		// Register the editor callback to clear dictionaries when playmode changes or editor reloads
		[InitializeOnLoadMethod]
		private static void RegisterCallbacks()
		{
			EditorApplication.playModeStateChanged += CleanupOnPlayModeChange;
		}

		private static void CleanupOnPlayModeChange(PlayModeStateChange state)
		{
			// DISABLED: I'm not sure this is desirable. Do object instance Ids change when entering playmode? And when comming back from playmode?
			/*
			if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.EnteredEditMode)
			{
			    _selectedTabs.Clear();
			    _expandedTabs.Clear();
			    s_AllTabsCatalog.Clear();
			}
			*/
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			string groupKey = $"{property.serializedObject.targetObject.GetInstanceID()}_{_currentTabGroupAttribute.GroupName}";

			// Register this tab in the group's tab collection
			if (!s_AllTabsCatalog.ContainsKey(groupKey))
			{
				s_AllTabsCatalog[groupKey] = new HashSet<string>();
			}

			s_AllTabsCatalog[groupKey].Add(_currentTabGroupAttribute.TabName);

			if (!_selectedTabs.ContainsKey(groupKey))
			{
				_selectedTabs[groupKey] = 0;
			}

			if (!_expandedTabs.ContainsKey(groupKey))
			{
				_expandedTabs[groupKey] = true;
			}

			// Draw the tab buttons
			Rect tabRect = position;
			tabRect.height = EditorGUIUtility.singleLineHeight;

			string[] tabs = GetTabs(groupKey);
			int selectedTab = _selectedTabs[groupKey];

			// Ensure the selected tab is valid
			if (selectedTab >= tabs.Length)
			{
				selectedTab = 0;
				_selectedTabs[groupKey] = 0;
			}

			// Draw the tab buttons
			EditorGUI.BeginChangeCheck();
			selectedTab = GUI.Toolbar(tabRect, selectedTab, tabs);
			if (EditorGUI.EndChangeCheck())
			{
				_selectedTabs[groupKey] = selectedTab;
				// Force a repaint to update the inspector immediately
				EditorUtility.SetDirty(property.serializedObject.targetObject);
			}

			// Only draw the property if it's in the selected tab
			if (tabs[selectedTab] == _currentTabGroupAttribute.TabName)
			{
				Rect propertyRect = position;
				propertyRect.y += EditorGUIUtility.singleLineHeight + 5;
				propertyRect.height = EditorGUI.GetPropertyHeight(property, label, true);

				if (_currentTabGroupAttribute.UseFixedHeight && _currentTabGroupAttribute.Height > 0)
				{
					propertyRect.height = _currentTabGroupAttribute.Height;
				}

				EditorGUI.PropertyField(propertyRect, property, label, true);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			string groupKey = $"{property.serializedObject.targetObject.GetInstanceID()}_{_currentTabGroupAttribute.GroupName}";
			if (!_selectedTabs.ContainsKey(groupKey) || !_expandedTabs[groupKey])
			{
				return EditorGUIUtility.singleLineHeight;
			}

			string[] tabs = GetTabs(groupKey);
			if (tabs.Length == 0)
			{
				return EditorGUIUtility.singleLineHeight;
			}

			int selectedTab = _selectedTabs[groupKey];
			if (selectedTab >= tabs.Length)
			{
				selectedTab = 0;
				_selectedTabs[groupKey] = 0;
			}

			if (tabs[selectedTab] != _currentTabGroupAttribute.TabName)
			{
				return EditorGUIUtility.singleLineHeight;
			}

			float propertyHeight = _currentTabGroupAttribute.UseFixedHeight && _currentTabGroupAttribute.Height > 0
				? _currentTabGroupAttribute.Height
				: EditorGUI.GetPropertyHeight(property, label, true);

			return EditorGUIUtility.singleLineHeight + 5 + propertyHeight;
		}

		private string[] GetTabs(string groupKey)
		{
			if (!s_AllTabsCatalog.ContainsKey(groupKey))
			{
				return new string[0];
			}

			// Return all unique tab names for this group, sorted alphabetically
			return s_AllTabsCatalog[groupKey].OrderBy(tab => tab).ToArray();
		}
	}
}