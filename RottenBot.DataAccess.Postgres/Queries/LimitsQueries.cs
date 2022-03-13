namespace RottenBot.DataAccess.Postgres.Queries
{
	public class LimitsQueries
	{
		#region SELECT

		public const string GetUserLimit = @"SELECT	sc_limit
												FROM limits
												WHERE user_id = @UserId";

		public const string GetUserIds = @"SELECT DISTINCT user_id FROM limits";

		#endregion

		#region INSERT

		public const string UpsertUserLimit = @"INSERT INTO limits
                            (user_id, sc_limit, updated_at)
                        VALUES 
                            (@UserId, @Limit, @UpdatedAt)
                        ON CONFLICT (user_id)
                        DO UPDATE SET
                            (user_id, sc_limit, updated_at) =
                            (@UserId, @Limit, @UpdatedAt)";

		public const string UpdateAllLimitsFromValue =
			@"UPDATE limits SET sc_limit = @ScLimit, updated_at = @UpdatedAt";

		#endregion
	}
}
