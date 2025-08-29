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
        Console.WriteLine("0 - Sair");

        Console.Write("\nEscolha: ");
        int opcao = int.Parse(Console.ReadLine());
        if (opcao == 0) break;
        Console.Clear();
        try
        {
            switch (opcao)
            {
                case 1:
                    Console.Write("Nome: ");
                    string nome = Console.ReadLine();
                    Console.Write("CPF: ");
                    string cpf = Console.ReadLine();
                if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cpf))
                {
                    Console.WriteLine("Nome e CPF não podem ser vazios.");
                    break;
                }
                if(cpf.Length != 11)
                {
                    Console.WriteLine("CPF deve ter 11 dígitos.");
                    break;
                }
                if (alunoRepositorio.BuscarAlunos().Exists(a => a.Cpf == cpf))
                {
                    Console.WriteLine("Aluno com este CPF já cadastrado.");
                    break;
                }

                alunoRepositorio.Inserir(new Aluno { Nome = nome, Cpf = cpf });
                    Console.WriteLine("Aluno cadastrado!");
                    break;


                case 2:
                    Console.Write("CPF do aluno: ");
                    string codCompra = Console.ReadLine();
                    Console.Write("Quantidade (25 ou 50): ");
                    int qtdCompra = int.Parse(Console.ReadLine());
                    if (qtdCompra != 25 && qtdCompra != 50)
                        Console.WriteLine("Pacote inválido!");
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
                        int qtdImp = int.Parse(Console.ReadLine());
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
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro: " + ex.Message);
        }
    }
