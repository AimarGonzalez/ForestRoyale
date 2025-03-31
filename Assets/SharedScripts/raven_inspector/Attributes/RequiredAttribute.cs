using UnityEngine;

namespace Raven.Attributes
{
	public class RequiredAttribute : PropertyAttribute
	{
		public string ErrorMessage { get; private set; }
		public bool ShowError { get; private set; }

		public RequiredAttribute(string errorMessage = "This field is required", bool showError = true)
		{
			ErrorMessage = errorMessage;
			ShowError = showError;
		}
	}
}