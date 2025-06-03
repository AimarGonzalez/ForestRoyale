using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestLib.MonoBehaviours
{
	[HideMonoScript]
	public class Notes : MonoBehaviour
	{
		[Serializable]
		public class Note
		{
			public Component Component;

			[TextArea(10, 1000)]
			public string NotesText;
		}

		[SerializeField]
		[HideLabel]
		private List<Note> _notes = new();
	}
}
