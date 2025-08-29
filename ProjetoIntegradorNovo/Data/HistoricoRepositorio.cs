using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace ProjetoIntegradorNovo.Data

{
    public class HistoricoRepositorio
    {
        private SqlConnection _conn;
        public HistoricoRepositorio(SqlConnection conn)
        {
            _conn = conn;
        }
        public Historico ConsultarSaldo(string cpf)
        {
            try
            {
                string sql = @"
                    SELECT 
                        a.nome AS NomeAluno,
                        a.cpf AS Cpf,
                        h.saldoAtual AS SaldoAtual
                    FROM Aluno a
                    OUTER APPLY (
                        SELECT TOP 1 h.saldoAtual
                        FROM Historico h
                        WHERE h.cpf = a.cpf
                        ORDER BY h.transacao DESC, h.codigo DESC
                    ) h
                    WHERE a.cpf = @cpf;";

                using (SqlCommand comando = new SqlCommand(sql, _conn))
                {
                    comando.Parameters.AddWithValue("@cpf", cpf);

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Historico
                            {

                                SaldoAtual = reader.IsDBNull(reader.GetOrdinal("SaldoAtual")) 
                                    ? 0 
                                    : Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("SaldoAtual")))
                            };
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RegistrarMovimentacao(string cpf, string tipo, int quantidade)
        {
            try
            {
                var historico = ConsultarSaldo(cpf);
                decimal saldoAtual = historico != null ? historico.SaldoAtual : 0;

                if (tipo == "COMPRA")
                    saldoAtual += quantidade;
                else if (tipo == "IMPRESSAO")
                {
                    if (saldoAtual < quantidade)
                        throw new Exception("Saldo insuficiente para impressão!");
                    saldoAtual -= quantidade;
                }
                string sql =
                    @"INSERT INTO 
                        Historico 
                        (cpf, 
                        tipoMovimentacao, 
                        quantidadeMovimentacao, 
                        saldoAtual) 
                      VALUES 
                        (@cpf, 
                        @tipo, 
                        @qtd,
                        @saldo)";

                SqlCommand comando = new SqlCommand(sql, _conn);
                comando.Parameters.AddWithValue("@Cpf", cpf);
                comando.Parameters.AddWithValue("@tipo", tipo);
                comando.Parameters.AddWithValue("@qtd", quantidade);
                comando.Parameters.AddWithValue("@saldo", saldoAtual);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir Aluno: " + ex.Message, ex);
            }
        }
        public List<Historico> ListarPorAluno(string cpf)
        {
            try
            {
                string sql =
                    @"SELECT 
                        transacao, 
                        tipoMovimentacao, 
                        quantidadeMovimentacao, 
                        saldoAtual 
                      FROM 
                        Historico 
                      WHERE 
                        cpf = @cpf
                      ORDER BY 
                        transacao DESC, codigo DESC";
                SqlCommand comando = new SqlCommand(sql, _conn);
                comando.Parameters.AddWithValue("@cpf", cpf);
                var lista = new List<Historico>();
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var historico = new Historico
                        {
                            Transacao = reader.GetDateTime(reader.GetOrdinal("transacao")),
                            TipoMovimentacao = reader.GetString(reader.GetOrdinal("tipoMovimentacao")),
                            QuantidadeMovimentacao = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("quantidadeMovimentacao"))),
                            SaldoAtual = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("saldoAtual")))
                        };
                        lista.Add(historico);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar histórico: " + ex.Message, ex);
            }
        }
    }
}
    
        

