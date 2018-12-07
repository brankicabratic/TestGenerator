using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.model
{
	class Messages
	{
		public static Messages Instance
		{
			get
			{
				if (instance == null)
					instance = new Messages();
				return instance;
			}
		}

		private static Messages instance;

		Dictionary<String, List<String>> errors = new Dictionary<string, List<string>>();

		public void LogError(string message, Type context = null)
		{
			string contextKey = getContextKey(context);
			if (!errors.ContainsKey(contextKey))
				errors.Add(contextKey, new List<String>());
			errors[contextKey].Add(message);
		}

		public List<String> RetrieveErrors(Type context = null)
		{
			String contextKey = getContextKey(context);
			if (!errors.ContainsKey(contextKey)) return new List<String>();
			List<String> errorsList = new List<string>(errors[contextKey]);
			errors[contextKey].Clear();
			return errorsList;
		}

		public void ClearErrors(Type context = null)
		{
			String contextKey = getContextKey(context);
			if (!errors.ContainsKey(contextKey)) return;
			errors[contextKey].Clear();
		}

		private string getContextKey(Type context)
		{
			return context == null ? "" : context.Name;
		}
	}
}
