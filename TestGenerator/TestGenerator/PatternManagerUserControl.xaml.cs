using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestGenerator.model;

namespace TestGenerator
{
	/// <summary>
	/// Interaction logic for PatternManagerUserControl.xaml
	/// </summary>
	public partial class PatternManagerUserControl : UserControl
	{
		private TestPattern testPattern;
		private QuestionPool questionPool;
		private List<QuestionTypeChooserUserControl> questionTypeChoosers = new List<QuestionTypeChooserUserControl>();

		public PatternManagerUserControl(TestPattern testPattern, QuestionPool questionPool)
		{
			InitializeComponent();
			this.testPattern = testPattern;
			this.questionPool = questionPool;
			tbName.Text = testPattern.Name;
			tbHeaderTemplate.Text = testPattern.HeaderTemplatePath;
			tbFooterTemplate.Text = testPattern.FooterTemplatePath;
			tbPoints.Text = testPattern.Points + "";
			addQuestionTypesControls();
		}

		public bool Save()
		{
			testPattern.Name = tbName.Text;

			if (!File.Exists(tbHeaderTemplate.Text))
			{
				MessageBox.Show("Header template file does not exist.");
				return false;
			}

			if (!File.Exists(tbFooterTemplate.Text))
			{
				MessageBox.Show("Footer template file does not exist.");
				return false;
			}

			if (System.IO.Path.GetExtension(tbHeaderTemplate.Text) != ".docx")
			{
				MessageBox.Show("Extension of header template file must be \"docx\".");
				return false;
			}

			if (System.IO.Path.GetExtension(tbFooterTemplate.Text) != ".docx")
			{
				MessageBox.Show("Extension of footer template file must be \"docx\".");
				return false;
			}

			if (!int.TryParse(tbPoints.Text, out testPattern.Points))
			{
				MessageBox.Show("Incorect entry for field \"Points\"");
				return false;
			}

			testPattern.HeaderTemplatePath = tbHeaderTemplate.Text;
			testPattern.FooterTemplatePath = tbFooterTemplate.Text;

			testPattern.QuestionTypes.Clear();
			foreach(QuestionTypeChooserUserControl questionTypeChooser in questionTypeChoosers)
			{
				if (questionTypeChooser.IsChecked())
				{
					QuestionType questionType = questionTypeChooser.GetQuestionType();
					if (questionType == null) return false;
					testPattern.QuestionTypes.Add(questionType);
				}
			}
			testPattern.SetDefaultQuestionTypesMatching();
			testPattern.Save();
			TestPatternPool.Instance.AddTestPattern(testPattern);
			return true;
		}

		private void addQuestionTypesControls()
		{
			List<QuestionType> questionTypes = questionPool.GetQuestionTypes();
			questionTypes.Sort((x, y) => { return x.Id - y.Id; });
			foreach (QuestionType questionType in questionTypes)
			{
				QuestionTypeChooserUserControl questionTypeChooser = new QuestionTypeChooserUserControl(testPattern, questionType);
				questionTypeChoosers.Add(questionTypeChooser);
				spQuestionTypes.Children.Add(questionTypeChooser);
			}
		}

		private void btnBrowseHeaderTemplate_Click(object sender, RoutedEventArgs e)
		{
			browseTemplateFile(tbHeaderTemplate);
		}

		private void btnBrowseFooterTemplate_Click(object sender, RoutedEventArgs e)
		{
			browseTemplateFile(tbFooterTemplate);
		}

		private void browseTemplateFile(TextBox tb)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.DefaultExt = "docx";
			openFileDialog.Filter = "Word Files (*.docx) | *.docx";

			if (openFileDialog.ShowDialog() == true)
			{
				tb.Text = openFileDialog.FileName;
			}
		}
	}
}
