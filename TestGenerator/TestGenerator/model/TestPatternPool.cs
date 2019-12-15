using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestGenerator.model
{
	public class TestPatternPool
	{
		public static TestPatternPool Instance
		{
			get
			{
				if (instance == null)
					instance = new TestPatternPool();
				return instance;
			}
		}

		private static TestPatternPool instance;

		public List<TestPattern> Patterns = new List<TestPattern>();

		public event Action<TestPattern> OnTestPatternAdded;
		public event Action<TestPattern> OnTestPatternDeleted;

		public TestPatternPool()
		{
			foreach (string testPatternFilename in Directory.GetFiles(Environment.CurrentDirectory + Path.DirectorySeparatorChar + Properties.Settings.Default.TestPatternsPath))
			{
				Patterns.Add(Load(testPatternFilename));
			}
		}

		public void AddTestPattern(TestPattern testPattern)
		{
			if (!Patterns.Contains(testPattern))
			{
				Patterns.Add(testPattern);
				OnTestPatternAdded(testPattern);
			}
		}

		public void DeleteTestPattern(TestPattern testPattern)
		{
			if (Patterns.Contains(testPattern))
			{
				Patterns.Remove(testPattern);
				testPattern.Delete();
				OnTestPatternDeleted(testPattern);
			}
		}

		private TestPattern Load(String filename)
		{
			XmlSerializer mySerializer = new XmlSerializer(typeof(TestPattern));
			FileStream myFileStream = new FileStream(filename, FileMode.Open);
			TestPattern testPattern = (TestPattern)mySerializer.Deserialize(myFileStream);
			testPattern.OnDeserialized();
			myFileStream.Close();
			return testPattern;
		}
	}
}
