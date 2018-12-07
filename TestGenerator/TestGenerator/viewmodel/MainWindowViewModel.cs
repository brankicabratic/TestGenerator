using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGenerator.model;

namespace TestGenerator.viewmodel
{
	public class MainWindowViewModel
	{
		public ObservableCollection<TestPattern> AllPatterns { get; set; }

		public MainWindowViewModel()
		{
			AllPatterns = new ObservableCollection<TestPattern>();
			foreach (TestPattern tp in TestPatternPool.Instance.Patterns)
			{
				AllPatterns.Add(tp);
			}
			TestPatternPool.Instance.OnTestPatternAdded += (tp) => AllPatterns.Add(tp);
		}
	}
}
