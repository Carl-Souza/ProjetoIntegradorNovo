using System;

namespace ProjetoIntegradorNovo

{
    public class Historico
    {
        public DateTime Transacao { get; set; }
        public string TipoMovimentacao { get; set; } 
        public int QuantidadeMovimentacao { get; set; }
        public decimal SaldoAtual { get; set; }
        public string cpfAluno { get; set; } = string.Empty;
        public string nomeAluno { get; set; } = string.Empty;
    }
}
