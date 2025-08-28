using System.Data.SqlClient;
namespace ProjetoIntegradorNovo
{
    public class Conexao
    {
        public SqlConnection conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=JovemProgramador;Integrated Security=True;TrustServerCertificate=True;");

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

}
