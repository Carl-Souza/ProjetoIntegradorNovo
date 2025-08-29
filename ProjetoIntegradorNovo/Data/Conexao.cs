using System.Data.SqlClient;
namespace ProjetoIntegradorNovo.Data
{
    public class Conexao
    {
        public SqlConnection conn = new SqlConnection(@"Data Source=(Local);Initial Catalog=ProjetoIntegrador;Integrated Security=True;TrustServerCertificate=True;");

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

///////
////CREATE DATABASE ProjetoIntegrador;
////GO

////USE ProjetoIntegrador;
////GO


////CREATE TABLE Aluno (
////    Codigo INT IDENTITY(1,1) PRIMARY KEY, 
////    Nome NVARCHAR(150) NOT NULL,
////    Cpf CHAR(11) NOT NULL UNIQUE
////);
////GO


////CREATE TABLE Historico (
////    Codigo INT IDENTITY(1,1) PRIMARY KEY,
////    Cpf CHAR(11) NOT NULL,               
////    Transacao DATETIME NOT NULL DEFAULT GETDATE(), 
////    TipoMovimentacao NVARCHAR(50) NOT NULL,       
////    QuantidadeMovimentacao DECIMAL(18,2) NOT NULL, 
////    SaldoAtual DECIMAL(18,2) NOT NULL,             

////    CONSTRAINT FK_Historico_Aluno FOREIGN KEY (Cpf) REFERENCES Aluno(Cpf)
////);
////GO


