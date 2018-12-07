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
	/// Interaction logic for NewPatternWindow.xaml
	/// </summary>
	public partial class NewPatternWindow : Window
	{
		private PatternManagerUserControl patternManager = null;

		public NewPatternWindow()
		{
			InitializeComponent();
		}

		private void btnBrowse_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

			if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				txtboxQuestionsPath.Text = folderBrowserDialog.SelectedPath;
			}
		}

		private void btnLoad_Click(object sender, RoutedEventArgs e)
		{
			lblError.Content = null;
			QuestionPool questionPool = new QuestionPool(txtboxQuestionsPath.Text);
			List<String> errors = Messages.Instance.RetrieveErrors(typeof(QuestionPool));
			if (patternManager != null)
			{
				spContainer.Children.Remove(patternManager);
			}
			if (errors.Count == 0)
			{
				TestPattern testPattern = new TestPattern(Guid.NewGuid().GetHashCode(), txtboxQuestionsPath.Text);
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
