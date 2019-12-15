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

		public void OutputDocument(string headerTemplatePath, string footerTemplatePath, string outputPath, int desiredPoints)
		{
			if (File.Exists(headerTemplatePath) && File.Exists(footerTemplatePath))
			{
				Questions = Questions.OrderBy(x => x.Type.Id).ToList();

				int ratio;
				float[] points = getQuestionsPoints(desiredPoints, out ratio);

				DocX header, footer, doc;

				try
				{
					header = DocX.Load(headerTemplatePath);
					footer = DocX.Load(footerTemplatePath);
					doc = DocX.Create(outputPath);
				}
				catch (Exception e)
				{
					return;
				}

				DateTime dt = DateTime.Now;
				int year = dt.Month > 9 ? dt.Year : dt.Year - 1;
				header.ReplaceText(Properties.Settings.Default.HeaderYearToken, year + "/" + (year + 1));

				doc.InsertDocument(header);

				int i = 0;
				foreach (Question q in Questions)
				{
					DocX questionDoc = q.Doc;
					questionDoc.ReplaceText(Properties.Settings.Default.QuestionNumberToken, "" + (i+1));
					questionDoc.ReplaceText(Properties.Settings.Default.QuestionPointsToken, "" + points[i]);
					doc.InsertDocument(questionDoc);
					i++;
				}

				footer.ReplaceText(Properties.Settings.Default.FooterMaxPointsToken, "" + (desiredPoints*ratio));
				footer.ReplaceText(Properties.Settings.Default.FooterDivPointsToken, "" + ratio);
				footer.ReplaceText(Properties.Settings.Default.FooterRealPointsToken, "" + desiredPoints);
				doc.InsertDocument(footer);

				try
				{
					doc.Save();
				}
				catch
				{
					return;
				}
			}
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

		private float[] getQuestionsPoints(int desiredPoints, out int ratio)
		{
			ratio = (int)(Points / desiredPoints);
			int lowerPointsTarget = ratio * desiredPoints;
			int upperPointsTarget = (ratio + 1) * desiredPoints;
			ratio = (upperPointsTarget - Points) < (Points - lowerPointsTarget) ? ratio + 1 : ratio;
			int targetPoints = ratio * desiredPoints;

			float[] points = new float[Questions.Count];
			float roundedPointsSum = 0;
			int i = 0;
			foreach (Question q in Questions)
			{
				float v1 = (int)(q.Type.Difficulty) * 10;
				float v2 = (float)Math.Round(q.Type.Difficulty * 10);
				if (v2 - v1 > 3 && v2 - v1 < 7)
				{
					v1 += 5;
				}
				else if (v2 - v1 >= 7)
				{
					v1 += 10;
				}
				points[i++] = v1 / 10;
				roundedPointsSum += v1 / 10;
			}

			float pointsDiff = targetPoints - roundedPointsSum;
			List<float> pointsToAssign = new List<float>();
			for (i = 0; i < Math.Abs(pointsDiff) * 2; i++)
			{
				pointsToAssign.Add(Math.Sign(pointsDiff) * 0.5f);
			}

			while (pointsToAssign.Count > Questions.Count)
			{
				if (pointsToAssign.Count < 2) break;
				float v1 = pointsToAssign[0];
				float v2 = pointsToAssign[1];
				pointsToAssign.RemoveAt(0);
				pointsToAssign.RemoveAt(0);
				pointsToAssign.Add(v1 + v2);
			}

			HashSet<int> updatedQuestions = new HashSet<int>();
			while (pointsToAssign.Count > 0)
			{
				float pToAssign = pointsToAssign.Last();
				pointsToAssign.RemoveAt(pointsToAssign.Count - 1);
				int maxIndex = -1;
				float maxValue = -1;
				int index = 0;
				foreach (float p in points)
				{
					if (p > maxValue && !updatedQuestions.Contains(index))
					{
						maxIndex = index;
						maxValue = p;
					}
					index++;
				}
				points[maxIndex] += pToAssign;
				updatedQuestions.Add(maxIndex);
			}

			return points;
		}
	}
}
