using System;
using System.Data.SqlClient;
namespace ProjetoIntegradorNovo
{
    Conexao db = new Conexao();

    db.Conectar();
    class Program
    {
        static void Main(string[] args)
        {
            var alunoRepo = new AlunoRepositorio();
            var historicoRepo = new HistoricoRepositorio();

            while (true)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("1 - Cadastrar aluno");
                Console.WriteLine("2 - Comprar impressões");
                Console.WriteLine("3 - Realizar impressão");
                Console.WriteLine("4 - Consultar saldo");
                Console.WriteLine("5 - Consultar histórico");
                Console.WriteLine("0 - Sair");

                Console.Write("\nEscolha: ");
                int opcao = int.Parse(Console.ReadLine());
                if (opcao == 0) break;

                try
                {
                    switch (opcao)
                    {
                        case 1:
                            Console.Write("Nome: ");
                            string nome = Console.ReadLine();
                            Console.Write("CPF: ");
                            string cpf = Console.ReadLine();
                            alunoRepo.Inserir(new Aluno { Nome = nome, Cpf = cpf });
                            Console.WriteLine("Aluno cadastrado!");
                            break;

                        case 2:
                            Console.Write("Código do aluno: ");
                            int codCompra = int.Parse(Console.ReadLine());
                            Console.Write("Quantidade (25 ou 50): ");
                            int qtdCompra = int.Parse(Console.ReadLine());
                            if (qtdCompra != 25 && qtdCompra != 50)
                                Console.WriteLine("Pacote inválido!");
                            else
                            {
                                historicoRepo.RegistrarMovimentacao(codCompra, "COMPRA", qtdCompra);
                                Console.WriteLine("Compra registrada!");
                            }
                            break;

                        case 3:
                            Console.Write("Código do aluno: ");
                            int codImp = int.Parse(Console.ReadLine());
                            Console.Write("Quantidade de páginas: ");
                            int qtdImp = int.Parse(Console.ReadLine());
                            historicoRepo.RegistrarMovimentacao(codImp, "IMPRESSAO", qtdImp);
                            Console.WriteLine("Impressão registrada!");
                            break;

                        case 4:
                            Console.Write("Código do aluno: ");
                            int codSaldo = int.Parse(Console.ReadLine());
                            int saldo = historicoRepo.ConsultarSaldo(codSaldo);
                            Console.WriteLine($"Saldo atual: {saldo} impressões");
                            break;

                        case 5:
                            Console.Write("Código do aluno: ");
                            int codHist = int.Parse(Console.ReadLine());
                            var lista = historicoRepo.ListarPorAluno(codHist);
                            foreach (var h in lista)
                                Console.WriteLine($"[{h.Transacao}] {h.TipoMovimentacao} {h.QuantidadeMovimentacao} → Saldo: {h.SaldoAtual}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
        }
    }
}
