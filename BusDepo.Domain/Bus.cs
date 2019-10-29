using System;
using System.Collections.Generic;
using System.Text;

namespace BusDepo.Domain
{
	public class Bus
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Num { get; set; }
		public string Status { get; set; }
	}
}
