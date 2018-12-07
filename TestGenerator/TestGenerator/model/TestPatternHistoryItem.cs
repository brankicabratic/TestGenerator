using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.model
{
	public class TestPatternHistoryItem
	{
		public DateTime Date;
		public Test Test;

		public TestPatternHistoryItem() { }

		public Question Get(QuestionType type)
		{
			return Test.Get(type);
		}

		public void OnDeserialized(QuestionPool pool)
		{
			Test.OnDeserialized(pool);
		}
	}
}
