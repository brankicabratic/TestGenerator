using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestGenerator.model
{
	public class Question
	{
		public int Id;
		public QuestionType Type;
		[XmlIgnore]
		public string Path;
		[XmlIgnore]
		public string Content { get { return Doc.Text; } }
		[XmlIgnore]
		public DocX Doc { get { return DocX.Load(Path); } }

		public Question() { }

		public Question(int id, QuestionType type, String path)
		{
			Id = id;
			Type = type;
			Path = path;
		}

		public override int GetHashCode()
		{
			return Type.GetHashCode() ^ Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Question)) return false;
			Question q = (Question)obj;
			return q.Id == Id && q.Type.Equals(Type);
		}
	}
}
