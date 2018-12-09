using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestGenerator.model
{
	public class QuestionType
	{
		public int Id;
		public float Difficulty;
		[XmlIgnore]
		public List<Question> Questions;

		public QuestionType() { }

		public QuestionType(int id) : this(id, null) { }

		public QuestionType(int id, List<Question> questions) : this(id, 0, questions) { }

		public QuestionType(int id, float difficulty, List<Question> questions)
		{
			Id = id;
			Difficulty = difficulty;
			Questions = questions;
		}

		public void SetQuestions(List<Question> questions)
		{
			Questions = questions;
		}

		// previousQuestions must be sorted by increasing 
		public Question GetRandom(List<Question> previousQuestions)
		{
			List<Question> sortedQuestions = new List<Question>();

			foreach (Question q in previousQuestions)
			{
				if (!sortedQuestions.Contains(q))
					sortedQuestions.Add(q);
			}

			int historyQuestionsCount = sortedQuestions.Count;

			double cumulativeProbability = (historyQuestionsCount * (historyQuestionsCount + 1) * (2 * historyQuestionsCount + 1)) / 6f;

			foreach (Question q in Questions)
			{
				if (!sortedQuestions.Contains(q))
					sortedQuestions.Add(q);
			}

			int nonHistoryQuestionsCount = sortedQuestions.Count - historyQuestionsCount;

			double rnd = (new Random()).NextDouble() * (cumulativeProbability > 0 ? (nonHistoryQuestionsCount + 1) * cumulativeProbability : nonHistoryQuestionsCount);
			
			double probability = 0;
			for (int i = 0; i < sortedQuestions.Count; i++)
			{
				if (i < historyQuestionsCount)
					probability += Math.Pow((i + 1), 2);
				else
					probability += cumulativeProbability > 0 ? cumulativeProbability : 1;
				if (rnd < probability)
					return sortedQuestions[i];
				
			}
			return null;
		}

		public String GetPreview()
		{
			if (Questions.Count > 0)
			{
				return Questions[0].Content;
			}
			return "This question type does not have any questions.";
		}

		public void OnDeserialized(QuestionPool pool)
		{
			Questions = pool.GetQuestions(this);
			foreach (Question question in Questions)
			{
				Question q = pool.GetQuestion(question.Type.Id, question.Id);
				question.Path = q.Path;
				question.Type = this;
			}
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is QuestionType)) return false;
			return ((QuestionType)obj).Id == Id;
		}
	}
}
