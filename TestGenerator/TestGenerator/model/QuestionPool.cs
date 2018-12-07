using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.model
{
	public class QuestionPool
	{
		private List<QuestionType> pool = new List<QuestionType>();

		public QuestionPool(String path)
		{
			if (!Directory.Exists(path))
			{
				Messages.Instance.LogError("Invalid path.", GetType());
				return;
			}
			foreach (string questionTypeIdStr in Directory.GetDirectories(path))
			{
				DirectoryInfo questionTypeDirectoryInfo = new DirectoryInfo(questionTypeIdStr);
				int questionTypeId = -1;
				if (!int.TryParse(questionTypeDirectoryInfo.Name, out questionTypeId))
				{
					Messages.Instance.LogError("Invalid id (folder name) for question type " + questionTypeDirectoryInfo.Name + ". Id must be integer value.", GetType());
					continue;
				}
				QuestionType questionType = new QuestionType(questionTypeId);
				List<Question> questions = new List<Question>();
				foreach (string questionIdStr in Directory.GetFiles(questionTypeIdStr))
				{
					FileInfo questionFileInfo = new FileInfo(questionIdStr);
					int questionId;
					if (!int.TryParse(Path.GetFileNameWithoutExtension(questionFileInfo.Name), out questionId))
					{
						Messages.Instance.LogError("Invalid id (file name) for question " + Path.GetFileNameWithoutExtension(questionFileInfo.Name) + ". Id must be integer value.", GetType());
						continue;
					}
					questions.Add(new Question(questionId, questionType, questionIdStr));
				}
				questionType.SetQuestions(questions);
				pool.Add(questionType);
			}
		}

		public List<QuestionType> GetQuestionTypes()
		{
			return pool;
		}

		public List<Question> GetQuestions(QuestionType type)
		{
			foreach (QuestionType t in pool)
			{
				if (type.Equals(t))
				{
					return t.Questions;
				}
			}
			return new List<Question>();
		}

		public Question GetQuestion(int typeId, int questionId)
		{
			foreach (QuestionType t in pool)
			{
				if (t.Id == typeId)
				{
					foreach (Question q in t.Questions)
					{
						if (q.Id == questionId)
							return q;
					}
				}
			}
			return null;
		}
	}
}
