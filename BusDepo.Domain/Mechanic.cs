using System;
using System.Collections.Generic;
using System.Text;

namespace BusDepo.Domain
{
	public class Mechanic
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; }
	}
}
