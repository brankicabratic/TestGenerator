using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestGenerator.model
{
	public class QuestionTypeProbabilities
	{
		public int QuestionType;
		public List<QuestionTypeProbability> Probabilities = new List<QuestionTypeProbability>();

		public QuestionTypeProbabilities() { }

		public QuestionTypeProbabilities(int questionType)
		{
			QuestionType = questionType;
		}

		public void Add(int questionType, double probability)
		{
			Probabilities.Add(new QuestionTypeProbability(questionType, probability));
		}

		public double Get(int questionType)
		{
			foreach (QuestionTypeProbability probability in Probabilities)
				if (probability.QuestionType == questionType)
					return probability.Probability;
			return 0;
		}
	}

	public class QuestionTypeProbability
	{
		public int QuestionType;
		public double Probability;

		public QuestionTypeProbability() { }

		public QuestionTypeProbability(int questionType, double probability)
		{
			QuestionType = questionType;
			Probability = probability;
		}
	}

	public class QuestionTypesMatching
	{
		public List<QuestionTypeProbabilities> Probabilities;

		public QuestionTypesMatching() { }

		public QuestionTypesMatching(List<QuestionType> questionTypes)
		{
			Probabilities = new List<QuestionTypeProbabilities>();
			foreach (QuestionType qt1 in questionTypes)
			{
				QuestionTypeProbabilities qtProbabilites = new QuestionTypeProbabilities(qt1.Id);
				foreach (QuestionType qt2 in questionTypes)
				{
					qtProbabilites.Add(qt2.Id, 1);
				}
				Probabilities.Add(qtProbabilites);
			}
		}

		public double[] GetProbabilities(List<QuestionType> qt1, List<QuestionType> qt2)
		{
			double[] probabilities = new double[qt1.Count];
			for (int i = 0; i < qt1.Count; i++)
			{
				QuestionType type1 = qt1[i];
				double probability = 1;
				foreach (QuestionType type2 in qt2)
				{
					probability *= getProbability(type1.Id, type2.Id);
				}
				probabilities[i] = probability;
			}
			return probabilities;
		}

		private double getProbability(int qt1, int qt2)
		{
			foreach (QuestionTypeProbabilities qtProbabilities in Probabilities)
			{
				if (qtProbabilities.QuestionType == qt1)
					return qtProbabilities.Get(qt2);
			}
			return 0;
		}
	}
}
