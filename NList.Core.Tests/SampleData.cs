using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using NFluent;

namespace NList.Core.Tests
{

	public static class SampleData
	{
		public static List<User> Source = new List<User> {
			new User (1, "tata"),
			new User (2, "tete"),
			new User (3, "titi"),
			new User (4, "toto")
		};
		public static List<User> Modified = new List<User> {
			new User (1, "tata"),
			new User (3, "titi"),
			new User (4, "totoz"),
			new User (5, "tutu")
		};
	}

}
