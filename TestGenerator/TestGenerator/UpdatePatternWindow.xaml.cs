using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using TestGenerator.model;

namespace TestGenerator
{
	/// <summary>
	/// Interaction logic for UpdatePatternWindow.xaml
	/// </summary>
	public partial class UpdatePatternWindow : Window
	{
		private PatternManagerUserControl patternManager;

		public UpdatePatternWindow(TestPattern testPattern)
		{
			InitializeComponent();
			lblError.Content = null;
			lblQuestionsPath.Content = testPattern.QuestionsPath;
			QuestionPool questionPool = new QuestionPool(testPattern.QuestionsPath);
			List<String> errors = Messages.Instance.RetrieveErrors(typeof(QuestionPool));
			if (errors.Count == 0)
			{
				patternManager = new PatternManagerUserControl(testPattern, questionPool);
				spContainer.Children.Add(patternManager);
			}
			else
			{
				lblError.Content = String.Join("\n", errors);
			}
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			if (patternManager == null)
			{
				System.Windows.MessageBox.Show("There is nothing to be saved.");
				return;
			}

			if (patternManager.Save())
			{
				Close();
			}
		}
	}
}
