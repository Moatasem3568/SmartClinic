using System;
using System.Data.SqlClient;

namespace SmartClinic
{
    public static class DbHelper
    {
        private static readonly string _connectionString =
            "Data Source=ENG_MOATASEM;Initial Catalog=clinic;Integrated Security=True;TrustServerCertificate=True";

        public static string ConnectionString => _connectionString;

        public static SqlConnection GetConnection()
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open(); // فتح الاتصال للتحقق منه
                connection.Close(); // إغلاقه بعد التحقق
                return new SqlConnection(_connectionString);
            }
            catch (SqlException ex)
            {
                throw new Exception($"خطأ في الاتصال بقاعدة البيانات: {ex.Message}", ex);
            }
        }

        // طريقة مساعدة للتحقق من الاتصال
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}