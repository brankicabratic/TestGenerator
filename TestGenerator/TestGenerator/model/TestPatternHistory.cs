using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.model
{
	public class TestPatternHistory
	{
		// Sorted by date starting from more recent.
		public List<TestPatternHistoryItem> Items = new List<TestPatternHistoryItem>();

		public TestPatternHistory() { }

		public List<Question> Get(QuestionType questionType)
		{
			List<Question> result = new List<Question>();

			foreach (TestPatternHistoryItem item in Items) {
				Question q = item.Get(questionType);
				if (q != null)
				{
					result.Add(q);
				}
			}

			return result;
		}

		public void OnDeserialized(QuestionPool pool)
		{
			foreach (TestPatternHistoryItem item in Items)
				item.OnDeserialized(pool);
		}
	}
}
