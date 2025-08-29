using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjetoIntegradorNovo.Data

{
    public class AlunoRepositorio
    {
        private SqlConnection _conn;
        public AlunoRepositorio(SqlConnection conn)
        {
            _conn = conn;
        }
        public string Inserir(Aluno aluno)
        {
            try
            {
                string sql =
                            """
                    INSERT INTO Aluno (
                        Nome, 
                        Cpf
                    ) VALUES (
                        @Nome, 
                        @Cpf
                    );
                    """;

                SqlCommand comando = new SqlCommand(sql, _conn);
                comando.Parameters.AddWithValue("@Nome", aluno.Nome);
                comando.Parameters.AddWithValue("@Cpf", aluno.Cpf);

                if (comando.ExecuteNonQuery() > 0)
                {
                    return "Aluno inserido com sucesso!";
                }
                else
                {
                    return "Não foi possivel inserir Aluno!";
                }
            }
            catch (Exception ex)
            {
                return "Erro ao inserir Aluno: " + ex.Message;
            }
        }

        public List<Aluno> BuscarAlunos()
        {
            try
            {
                string sql = 
                    "SELECT " +
                    "   Nome, " +
                    "   Cpf " +
                    "FROM " +
                    "   Aluno";
                SqlCommand comando = new SqlCommand(sql, _conn);

                List<Aluno> alunos = new List<Aluno>();

                using (var reader = comando.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var nomeDb = reader.GetString(reader.GetOrdinal("Nome"));
                        var cpfDb = reader.GetString(reader.GetOrdinal("Cpf"));

                        alunos.Add(new Aluno()
                        {
                            Nome = nomeDb,
                            Cpf = cpfDb
                        });

                    }
                    return alunos;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
