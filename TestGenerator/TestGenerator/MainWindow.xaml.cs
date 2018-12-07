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
		public MainWindow()
		{
			InitializeComponent();
		}

		private void btnNewPattern_Click(object sender, RoutedEventArgs e)
		{
			NewPatternWindow newPatternWindow = new NewPatternWindow();
			newPatternWindow.Show();
		}

		private void btnGenerateTest_Click(object sender, RoutedEventArgs e)
		{
			if (cbTestPatterns.SelectedItem == null)
			{
				MessageBox.Show("Test pattern must be selected.");
				return;
			}

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Word Files (*.docx) | *.docx";
			saveFileDialog.DefaultExt = "docx";
			saveFileDialog.AddExtension = true;
			if (saveFileDialog.ShowDialog() == true)
			{
				TestPattern testPattern = (TestPattern)cbTestPatterns.SelectedItem;
				testPattern.GenerateTest().OutputDocument(testPattern.HeaderTemplatePath, testPattern.FooterTemplatePath, saveFileDialog.FileName);
			}
		}
	}
}
