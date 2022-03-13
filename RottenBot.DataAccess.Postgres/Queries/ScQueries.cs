namespace RottenBot.DataAccess.Postgres.Queries
{
	public class ScQueries
	{
		#region SELECT

		public const string SelectOverallUserRatingByName = @"SELECT
														SUM(rating) as Rating
												FROM user_social_rating_chat
												WHERE username = @UserName";

		public const string SelectChatUserById = @"SELECT id as Id,
       													username as Username, 
														rating as Rating,
       													created_at as CreatedAt,
       													updated_at as UpdatedAt,
       													chat_id as ChatId
												FROM user_social_rating_chat
												WHERE id = @Id and chat_id = @ChatId";

		public const string SelectChatUserRatingByName = @"SELECT 
														rating as Rating
												FROM user_social_rating_chat
												WHERE username = @UserName and chat_id = @ChatId";

		public const string SelectChatRanks = @"SELECT 
       													username as Username,
														rating as Rating
												FROM user_social_rating_chat
												WHERE chat_id = @ChatId
												ORDER BY rating DESC";

		#endregion

		#region INSERT

		public const string UpsertChatUser = @"INSERT INTO user_social_rating_chat
                            (id, username, rating, created_at, updated_at, chat_id)
                        VALUES 
                            (@Id, @Username, @Rating, @CreatedAt, @UpdatedAt, @ChatId)
                        ON CONFLICT (id, chat_id)
                        DO UPDATE SET
                            (username, rating, updated_at) =
                            (@Username, @Rating, @UpdatedAt)";

		#endregion
	}
}
