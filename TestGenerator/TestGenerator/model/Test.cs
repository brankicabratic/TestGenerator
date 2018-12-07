using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.model
{
	public class Test
	{
		public double Points = 0;
		public List<Question> Questions = new List<Question>();

		public Test() { }

		public bool OutputDocument(string headerTemplatePath, string footerTemplatePath, string outputPath)
		{			
			if (File.Exists(headerTemplatePath) && File.Exists(footerTemplatePath))
			{
				DocX header, footer, doc;

				try
				{
					header = DocX.Load(headerTemplatePath);
					footer = DocX.Load(footerTemplatePath);
					doc = DocX.Create(outputPath);
				}
				catch (Exception e)
				{
					return false;
				}
				
				doc.InsertDocument(header);

				foreach (Question q in Questions)
					doc.InsertDocument(q.Doc);

				doc.InsertDocument(footer);

				try
				{
					doc.Save();
				}
				catch
				{
					return false;
				}
			}

			return true;
		}

		public void AddQuestion(Question question)
		{
			Questions.Add(question);
			Points += question.Type.Difficulty;
		}

		public Question Get(QuestionType type)
		{
			foreach (Question question in Questions)
			{
				if (question.Type == type)
					return question;
			}
			return null;
		}

		public bool NeedMorePoints(double points)
		{
			return points > this.Points;
		}

		public List<QuestionType> GetQuestionTypes()
		{
			return Questions.Select(x => x.Type).ToList();
		}

		public void OnDeserialized(QuestionPool pool)
		{
			foreach (Question question in Questions)
			{
				Question q = pool.GetQuestion(question.Type.Id, question.Id);
				question.Path = q.Path;
				question.Type = q.Type;
			}
		}
	}
}
