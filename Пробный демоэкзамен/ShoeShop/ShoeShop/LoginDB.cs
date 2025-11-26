using System;
using System.Data.SqlClient;

namespace ShoeShop
{
    internal class LoginDB
    {
        string connectionString = "Data Source=ADCLG1;Initial Catalog=ShoeShop;Integrated Security=True;Encrypt=False";

        public (bool Success, string Role) AuthenticateUser(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                        SELECT [Роль сотрудника] 
                        FROM Users 
                        WHERE Логин = @Login AND Пароль = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["[Роль сотрудника]"].ToString();
                                return (true, role);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                    System.Diagnostics.Debug.WriteLine($"Ошибка аутентификации: {ex.Message}");
                    return (false, null);
                }
            }

            return (false, null);
        }
    }
}
