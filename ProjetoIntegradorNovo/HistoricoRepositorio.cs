using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjetoIntegradorNovo

{
    public class HistoricoRepositorio
    {
        public int ConsultarSaldo(string cpf)
        {
            using (var conn = Conexao.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"DECLARE @cpf CHAR(11) = '12345678901';
                    SELECT 
                        a.nome as 'Nome do Aluno',
                        a.cpf as 'CPF',
                        h.saldoAtual as 'Saldo Atual'
                    FROM Alunos a
                    OUTER APPLY (
                    SELECT TOP 1 h.saldoAtual
                    FROM Historico h
                    WHERE h.codigoAluno = a.codigo
                    ORDER BY h.transacao DESC, h.codigo DESC
                    ) h
                    WHERE a.cpf = @cpf;", conn);

                cmd.Parameters.AddWithValue("@cpf", cpf);
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public void RegistrarMovimentacao(string cpf, string tipo, int quantidade)
        {
            int saldoAtual = ConsultarSaldo(cpf);

            if (tipo == "COMPRA")
                saldoAtual += quantidade;
            else if (tipo == "IMPRESSAO")
            {
                if (saldoAtual < quantidade)
                    throw new Exception("Saldo insuficiente para impressão!");
                saldoAtual -= quantidade;
            }

            using (var conn = Conexao.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"INSERT INTO 
                        Historico 
                        (@cpf, 
                        tipoMovimentacao, 
                        quantidadeMovimentacao, 
                        saldoAtual) 
                      VALUES 
                        (@cpf, 
                        @tipo, 
                        @qtd,
                        @saldo)", conn);
                cmd.Parameters.AddWithValue("@cpf", cpf);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@qtd", quantidade);
                cmd.Parameters.AddWithValue("@saldo", saldoAtual);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Historico> ListarPorAluno(string cpf)
        {
            var lista = new List<Historico>();
            using (var conn = Conexao.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"SELECT codigo, codigoAluno, transacao, tipoMovimentacao, quantidadeMovimentacao, saldoAtual 
                      FROM Historico 
                      WHERE codigoAluno=@aluno 
                      ORDER BY transacao", conn);

                cmd.Parameters.AddWithValue("@aluno", codigoAluno);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Historico
                    {
                        Codigo = reader.GetInt32(0),
                        CodigoAluno = reader.GetInt32(1),
                        Transacao = reader.GetDateTime(2),
                        TipoMovimentacao = reader.GetString(3),
                        QuantidadeMovimentacao = reader.GetInt32(4),
                        SaldoAtual = reader.GetInt32(5)
                    });
                }
            }
            return lista;
        }
    }
}
