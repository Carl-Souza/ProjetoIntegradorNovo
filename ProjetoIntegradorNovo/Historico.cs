using System;

namespace ProjetoIntegradorNovo

{
    public class Historico
    {
        public DateTime Transacao { get; set; }
        public string TipoMovimentacao { get; set; } 
        public int QuantidadeMovimentacao { get; set; }
        public int SaldoAtual { get; set; }
    }
}
