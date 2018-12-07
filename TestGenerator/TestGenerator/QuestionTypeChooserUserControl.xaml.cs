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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestGenerator.model;

namespace TestGenerator
{
	/// <summary>
	/// Interaction logic for QuestionTypeChooserUserControl.xaml
	/// </summary>
	public partial class QuestionTypeChooserUserControl : UserControl
	{
		private QuestionType questionType;

		public QuestionTypeChooserUserControl(TestPattern testPattern, QuestionType questionType)
		{
			InitializeComponent();
			cb.IsChecked = testPattern.ContainsQuestionType(questionType);
			lbl.Content = questionType.Id;
			tooltip.Text = questionType.GetPreview();
			this.questionType = questionType;
		}

		public bool IsChecked()
		{
			return cb.IsChecked.Value;
		}

		public QuestionType GetQuestionType()
		{
			if (!float.TryParse(tbPoints.Text, out questionType.Difficulty))
			{
				MessageBox.Show("Invalid points entry of question type with ID " + questionType.Id + ".");
				return null;
			}
			return questionType;
		}
	}
}
