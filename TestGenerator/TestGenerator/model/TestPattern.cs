﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestGenerator.model
{
	public class TestPattern
	{
		public int Id;
		public string Name;
		public string QuestionsPath;
		public string HeaderTemplatePath;
		public string FooterTemplatePath;
		public List<QuestionType> QuestionTypes = new List<QuestionType>();
		public int Points;
		public QuestionTypesMatching Matching;
		public TestPatternHistory History = new TestPatternHistory();

		private string Filename
		{
			get { return Environment.CurrentDirectory + Path.DirectorySeparatorChar + Properties.Settings.Default.TestPatternsPath + Id + ".xml"; }
		}

		public TestPattern() { }

		public TestPattern(int id, string questionsPath)
		{
			Id = id;
			QuestionsPath = questionsPath;
		}

		public void SetDefaultQuestionTypesMatching()
		{
			Matching = new QuestionTypesMatching(QuestionTypes);
		}

		public void Save()
		{
			XmlSerializer ser = new XmlSerializer(typeof(TestPattern));
			TextWriter writer = new StreamWriter(Filename);
			ser.Serialize(writer, this);
			writer.Close();
		}

		public void Delete()
		{
			File.Delete(Filename);
		}

		public void OnDeserialized()
		{
			QuestionPool pool = new QuestionPool(QuestionsPath);
			foreach (QuestionType questionType in QuestionTypes)
			{
				questionType.OnDeserialized(pool);
			}
			History.OnDeserialized(pool);
		}

		public Test GenerateTest()
		{
			Random rnd = new Random();
			List<QuestionType> questionTypes = new List<QuestionType>(QuestionTypes);
			Test test = new Test();
			while (test.NeedMorePoints(Points) && questionTypes.Count > 0)
			{
				double[] probabilities = Matching.GetProbabilities(questionTypes, test.GetQuestionTypes());
				double probabilitiesSum = 0;
				foreach (double probability in probabilities) probabilitiesSum += probability;
				double rndDbl = rnd.NextDouble() * probabilitiesSum;
				double cumulativeProbability = 0;
				QuestionType type = null;
				int i = 0;
				foreach (double probability in probabilities)
				{
					cumulativeProbability += probability;
					if (rndDbl < cumulativeProbability)
					{
						type = questionTypes[i];
						break;
					}
					i++;
				}
				questionTypes.Remove(type);
				test.AddQuestion(type.GetRandom(History.Get(type)));
			}
			History.Add(test);
			Save();
			return test;
		}

		public bool ContainsQuestionType(QuestionType questionType)
		{
			foreach (QuestionType q in QuestionTypes)
				if (q.Id == questionType.Id)
					return true;
			return false;
		}

		public QuestionType GetQuestionType(QuestionType questionType)
		{
			foreach (QuestionType q in QuestionTypes)
				if (q.Id == questionType.Id)
					return q;
			return null;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
