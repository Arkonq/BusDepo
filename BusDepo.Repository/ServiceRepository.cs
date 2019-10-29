using BusDepo.Domain;
using Dapper;
using System;
using System.Data.SqlClient;

namespace BusDepo.Repository
{
	public class ServiceRepository
	{
		private readonly string connectionString;

		public ServiceRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public void SetRepair(Service service)
		{
			string sql = "Insert into Services (Id, BusId, MechanicId) Values (@Id, @BusId, @MechanicId)";

			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, service);
				if (rowAffected != 1)   // так как вставка всего на 1 строку
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}

		public void DelRepair(Guid BusId)
		{
			string sql = "DELETE FROM Services " +
				"WHERE BusId = @BusId";

			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, new { BusId = BusId});
				if (rowAffected != 1)   // так как вставка всего на 1 строку
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}

		public Guid GetMechanicId(Guid BusId)
		{
			string sql = "Select * From Services " +
				"WHERE BusId = @BusId";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Service>(sql, new { BusId = BusId }).MechanicId;				
			}
		}

		public Mechanic GetMechanic(Mechanic mechanic)
		{
			string sql = "Select * from Services " +
				"Where MechanicId = @MechanicId";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Mechanic>(sql, new { MechanicId = mechanic.Id});
			}
		}
	}
}
