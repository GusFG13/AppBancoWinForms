using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBancoWinForms.Entities
{
    public class Extrato
    {
        //public Conta Conta { get; }
        //public double Valor { get; set; }

        //public Extrato(Conta conta, double valor)
        //{
        //    Conta = conta;
        //    Valor = valor;
        //}

        // data, conta origem, conta destino, valor

        public DateTime DataTransacao { get; set; }
        public int NumeroContaOrigem { get; set; }
        public int NumeroContaDestino { get; set; }
        public double Valor { get; set; }

        public Extrato(DateTime dataTransacao, int numeroContaOrigem, int numeroContaDestino, double valor)
        {
            DataTransacao = dataTransacao;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = numeroContaDestino;
            Valor = valor;
        }

        public override string ToString()
        {
            return DataTransacao.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
