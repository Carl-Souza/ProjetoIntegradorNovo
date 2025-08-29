using System.Data.SqlClient;
namespace ProjetoIntegradorNovo.Data
{
    public class Conexao
    {
        public SqlConnection conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ProjetoIntegrador;Integrated Security=True;TrustServerCertificate=True;");

        public void Conectar()
        {
            conn.Open();
        }

        public void Desconectar()
        {
            conn.Close();
        }
    }
}

