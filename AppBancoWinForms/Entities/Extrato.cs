using AppBancoWinForms.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBancoWinForms.Entities
{
    public class Extrato
    {
        public DateTime DataTransacao { get; set; }
        public string TipoMovimento { get; set; }
        public int NumeroContaOrigem { get; set; }
        public int NumeroContaDestino { get; set; }
        public double Valor { get; set; }
        public double TaxaCobrada { get; set; }

        public Extrato(DateTime dataTransacao, string tipoMovimento, int numeroContaOrigem, int numeroContaDestino, double valor, double taxaCobrada)
        {
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = numeroContaDestino;
            Valor = valor;
            TaxaCobrada = taxaCobrada;
        }
        public Extrato(DateTime dataTransacao, string tipoMovimento, int numeroContaOrigem, double valor, double taxaCobrada)//para saque
        {
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = 0;
            Valor = valor;
            TaxaCobrada = taxaCobrada;
        }
        public Extrato(DateTime dataTransacao, string tipoMovimento, int numeroContaOrigem, double valor) // para depósitos
        {
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = 0;
            Valor = valor;
            TaxaCobrada = 0.0;
        }

        public override string ToString()
        {
            return DataTransacao.ToString("yyyy-MM-ddTHH:mm:ssZ")
                + ";" + TipoMovimento
                + ";" + NumeroContaOrigem.ToString()
                + ";" + (NumeroContaDestino != 0 ? NumeroContaDestino.ToString() : "")
                + ";" + Valor.ToString("F2")
                + ";" + (TaxaCobrada != 0 ? TaxaCobrada.ToString("F2") : "");
        }
    }
}
