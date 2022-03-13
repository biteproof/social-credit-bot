using Npgsql;

namespace RottenBot.DataAccess.Postgres.Configurations
{
	public sealed class PostgresOptions
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string Database { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public string ConnectionString
		{
			get
			{
				// use database specific connection string builder to gain correct connection string
				var connectionStringBuilder = new NpgsqlConnectionStringBuilder
				{
					Host = Host,
					Port = Port,
					Database = Database,
					Username = Username,
					Password = Password,
					IncludeErrorDetail = true
				};

				// build connection string
				return connectionStringBuilder.ConnectionString;
			}
		}
	}
}
