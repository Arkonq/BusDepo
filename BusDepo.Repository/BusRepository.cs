using BusDepo.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BusDepo.Repository
{
	public class BusRepository
	{
		private readonly string connectionString;

		public BusRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public Bus GetByNum(string Num)
		{
			var sql = "Select * From Buses " +
								$"Where Num = @Num ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Bus>(sql, new { Num = Num });
			}
		}

		public void Update(Guid Id, string Status)
		{
			string sql = "UPDATE Buses SET Status = @Status WHERE Id = @Id;";
			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, new { Status = Status, Id = Id});
				if (rowAffected != 1)
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}

		public ICollection<Bus> GetAll()
		{
			var sql = "Select * From Buses";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Bus>(sql).ToList();
			}
		}

	}
}
