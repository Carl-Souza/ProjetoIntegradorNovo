using System;
using System.Data.SqlClient;
using ProjetoIntegradorNovo.Data;

Conexao db = new Conexao();

    db.Conectar();

    AlunoRepositorio alunoRepositorio = new AlunoRepositorio(db.conn);
    HistoricoRepositorio historicoRepositorio = new HistoricoRepositorio(db.conn);

while (true)
{
    Console.WriteLine("\n--- MENU ---");
    Console.WriteLine("1 - Cadastrar aluno");
    Console.WriteLine("2 - Comprar impressões");
    Console.WriteLine("3 - Realizar impressão");
    Console.WriteLine("4 - Consultar saldo");
    Console.WriteLine("5 - Consultar histórico");
    Console.WriteLine("6 - Consultar saldo de todos alunos");
    Console.WriteLine("0 - Sair");

    Console.Write("\nEscolha: ");
    string input = Console.ReadLine();
    int opcao;

    if (!int.TryParse(input, out opcao))
    {
        Console.Clear();
        Console.WriteLine("Entrada inválida! Digite apenas números.");
        continue; 
    }

    if (opcao < 0 || opcao > 6)
    {
        Console.Clear();
        Console.WriteLine("Opção inválida! Escolha um número do menu.");
        continue;
    }

    if (opcao == 0) break;

    Console.Clear();
    try
        {
            switch (opcao)
            {
            case 1:
                Console.Write("Nome do aluno: ");
                string nome = Console.ReadLine();

                string cpf;
                do
                {
                    Console.Write("CPF do aluno (apenas números): ");
                    cpf = Console.ReadLine() ?? "";

                    if (!cpf.All(char.IsDigit))
                    {
                        Console.WriteLine("Digite apenas números!");
                    }

                    if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cpf))
                    {
                        Console.WriteLine("Nome e CPF não podem ser vazios.");
                    }

                    if (cpf.Length != 11)
                    {
                        Console.WriteLine("CPF deve ter 11 dígitos.");
                    }

                    if (alunoRepositorio.BuscarAlunos().Exists(a => a.Cpf == cpf))
                    {
                        Console.WriteLine("Aluno com este CPF já cadastrado.");
                    }
                    else
                    {
                        alunoRepositorio.Inserir(new Aluno { Nome = nome, Cpf = cpf });
                        Console.WriteLine("Aluno cadastrado!");
                        break;
                    }

                } while (!cpf.All(char.IsDigit) && string.IsNullOrEmpty(cpf));
                break;

            case 2:
                    Console.Write("CPF do aluno: ");
                    string codCompra = Console.ReadLine();
                    if (string.IsNullOrEmpty(codCompra))
                    {
                        Console.WriteLine("CPF do aluno não pode ser vazio.");
                        break;
                    }
                    if (historicoRepositorio.ConsultarSaldo(codCompra) == null)
                    {
                        Console.WriteLine("Aluno não encontrado.");
                        break;
                    }
                    Console.Write("Quantidade (25 ou 50): ");
                    int qtdCompra = int.Parse(Console.ReadLine());
                    if (qtdCompra != 25 && qtdCompra != 50)
                    {
                        Console.WriteLine("Pacote inválido!");
                        break;
                    }
                    else
                    {
                        historicoRepositorio.RegistrarMovimentacao(codCompra, "COMPRA", qtdCompra);
                        Console.WriteLine("Compra registrada!");
                    }
                    break;

            case 3:
                Console.Write("CPF do aluno: ");
                string codImp = Console.ReadLine();
                if (string.IsNullOrEmpty(codImp))
                {
                    Console.WriteLine("CPF do aluno não pode ser vazio.");
                    break;
                }
                if (historicoRepositorio.ConsultarSaldo(codImp) == null)
                {
                    Console.WriteLine("Aluno não encontrado.");
                    break;
                }
                if (historicoRepositorio.ConsultarSaldo(codImp).SaldoAtual == 0)
                {
                    Console.WriteLine("Saldo insuficiente para impressão.");
                    break;
                }
                else
                {
                    Console.Write("Quantidade de páginas: ");
                    string inpt = Console.ReadLine();
                    int qtdImp;
                    if (!int.TryParse(inpt, out qtdImp))
                    {
                        Console.WriteLine("Digite apenas números!");
                        continue; 
                    }
                    historicoRepositorio.RegistrarMovimentacao(codImp, "IMPRESSAO", qtdImp);
                    Console.WriteLine("Impressão registrada!");
                    break;
                }

            case 4:
                Console.Write("CPF do aluno: ");
                string codSaldo = Console.ReadLine();
                if (string.IsNullOrEmpty(codSaldo))
                {
                    Console.WriteLine("CPF do aluno não pode ser vazio.");
                    break;
                }
                if (historicoRepositorio.ConsultarSaldo(codSaldo) == null)
                {
                    Console.WriteLine("Aluno não encontrado.");
                    break;
                }
                var historico = historicoRepositorio.ConsultarSaldo(codSaldo);
                decimal saldo = historico != null ? historico.SaldoAtual : 0;
                Console.WriteLine($"Saldo atual: {saldo} impressões");
                break;

            case 5:
                Console.Write("CPF do aluno: ");
                string codHist = Console.ReadLine();
                var lista = historicoRepositorio.ListarPorAluno(codHist);
                foreach (var h in lista)
                    Console.WriteLine($"[{h.Transacao}] {h.TipoMovimentacao} {h.QuantidadeMovimentacao} | Saldo: {h.SaldoAtual}");
                break;
            case 6:
                var listaAlunos = historicoRepositorio.ListarTodosAlunos();
                if (listaAlunos.Count == 0) { Console.WriteLine("Nenhum aluno encontrado."); break; }
                if (listaAlunos[0].SaldoAtual == 0 && listaAlunos.TrueForAll(a => a.TipoMovimentacao != "COMPRA"))
                { Console.WriteLine("Nenhum aluno com saldo encontrado."); break; }
                foreach (var h in listaAlunos)
                        Console.WriteLine($"Aluno: {h.nomeAluno} | CPF: {h.cpfAluno} | Saldo: {h.SaldoAtual}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro: " + ex.Message);
        }
    }
