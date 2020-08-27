using System.Data;
using System.Data.SqlClient;

namespace Book
{
    internal class SQL
    {
        //Global variable for later use
        private readonly string connectionString;

        //Constructor takes the connection, for flexible input
        public SQL(string ConnectionString)
        {
            this.connectionString = ConnectionString;
        }

        public bool CheckConnection()
        {
            var con = new SqlConnection(connectionString);
            return con.State.Equals(ConnectionState.Open);
        }

        public DataSet GetAllBooks()
        {
            var dset = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select id, Name, Author, Genre, Available from Books", con))
                    new SqlDataAdapter(cmd.CommandText, con).Fill(dset);
                con.Close();
            }
            return dset;
        }

        public bool Insert(Book book)
        {
            var results = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string command = $"insert into books (Name, Author, Genre, Available) " +
                    $"values (@Name, @Author, @Genre, @Available)";
                using (SqlCommand cmd = new SqlCommand(command, con))
                {
                    cmd.Parameters.AddWithValue("@Name", book.Name);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Genre", book.Genre);
                    cmd.Parameters.AddWithValue("@Available", book.Available);
                    results = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return results != 0;
        }

        public bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand($"Delete from Books where id={id}", con))
            {
                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }
    }
}
