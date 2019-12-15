using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string placeholderText;

		public MainWindow()
		{
			InitializeComponent();
			placeholderText = tbPoints.Text;
		}

		private void btnNewPattern_Click(object sender, RoutedEventArgs e)
		{
			NewPatternWindow newPatternWindow = new NewPatternWindow();
			newPatternWindow.Show();
		}

		private void btnUpdateTest_Click(object sender, RoutedEventArgs e)
		{
			if (cbTestPatterns.SelectedItem is TestPattern)
			{
				UpdatePatternWindow updatePatternWindow = new UpdatePatternWindow((TestPattern)cbTestPatterns.SelectedItem);
				updatePatternWindow.Show();
			}
			else
			{
				MessageBox.Show("Select the pattern you want to update.");
			}
		}

		private void btnDeleteTest_Click(object sender, RoutedEventArgs e)
		{
			if (cbTestPatterns.SelectedItem is TestPattern)
			{
				TestPatternPool.Instance.DeleteTestPattern((TestPattern)cbTestPatterns.SelectedItem);
			}
			else
			{
				MessageBox.Show("Select the pattern you want to delete.");
			}
		}

		private void btnGenerateTest_Click(object sender, RoutedEventArgs e)
		{
			if (cbTestPatterns.SelectedItem == null)
			{
				MessageBox.Show("Test pattern must be selected.");
				return;
			}

			int points;
			if (!int.TryParse(tbPoints.Text, out points))
			{
				MessageBox.Show("Points value must be a number.");
				return;
			}

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Word Files (*.docx) | *.docx";
			saveFileDialog.DefaultExt = "docx";
			saveFileDialog.AddExtension = true;
			if (saveFileDialog.ShowDialog() == true)
			{
				TestPattern testPattern = (TestPattern)cbTestPatterns.SelectedItem;
				testPattern.GenerateTest().OutputDocument(testPattern.HeaderTemplatePath, testPattern.FooterTemplatePath, saveFileDialog.FileName, points);
			}
		}

		private void tbPoints_GotFocus(object sender, RoutedEventArgs e)
		{
			if (placeholderText == tbPoints.Text) tbPoints.Text = "";
		}

		private void tbPoints_LostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(tbPoints.Text)) tbPoints.Text = placeholderText;
		}
	}
}
