using BusDepo.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BusDepo.Repository
{
	public class MechanicRepository
	{
		private readonly string connectionString;

		public MechanicRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public Mechanic GetByName(string Name)
		{
			var sql = "Select * From Mechanics " +
								$"Where Name = @Name ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Mechanic>(sql, new { Name = Name });
			}
		}

		public Mechanic GetById(Guid Id)
		{
			var sql = "Select * From Mechanics " +
								$"Where Id = @Id ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Mechanic>(sql, new { Id = Id });
			}
		}

		public ICollection<Mechanic> GetAll()
		{
			var sql = "Select * From Mechanics";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Mechanic>(sql).ToList();
			}
		}
	}
}
