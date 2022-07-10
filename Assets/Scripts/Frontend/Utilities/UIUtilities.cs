using System;
using System.Linq;
using TMPro;

namespace Frontend.Utilities
{
	public static class UIUtilities
	{
		public static void SetUpDropdown<T>(TMP_Dropdown dropdown, out T[] values)
		{
			dropdown.ClearOptions();
			values = Enum.GetValues(typeof(T))
				.Cast<T>()
				.ToArray();
			dropdown.AddOptions(values
				.Select(t => t.ToString())
				.ToList());
		}
	}
}