using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using NFluent;

namespace NList.Core.Tests
{

	public class User
	{
		public User (int i, char c)
		{
			throw new NotImplementedException ();
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public User (int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
	
}
