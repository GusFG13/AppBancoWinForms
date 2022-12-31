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
        public double SaldoAnterior { get; set; }
        public double SaltoAtual { get; set; }

        public Extrato(DateTime dataTransacao, string tipoMovimento, int numeroContaOrigem, int numeroContaDestino, double valor, double taxaCobrada, double saldoAnterior, double saltoAtual)
        { // para transferencias
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = numeroContaDestino;
            Valor = valor;
            TaxaCobrada = taxaCobrada;
            SaldoAnterior = saldoAnterior;
            SaltoAtual = saltoAtual;
        }
        public Extrato(DateTime dataTransacao, string tipoMovimento, int numeroContaOrigem, double valor, double taxaCobrada, double saldoAnterior, double saltoAtual)
        { // para saques
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = 0;
            Valor = valor;
            TaxaCobrada = taxaCobrada;
            SaldoAnterior = saldoAnterior;
            SaltoAtual = saltoAtual;
        }
        public Extrato(DateTime dataTransacao, string tipoMovimento, int numeroContaOrigem, double valor, double saldoAnterior, double saltoAtual)
        { // para depósitos
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = 0;
            Valor = valor;
            TaxaCobrada = 0.0;
            SaldoAnterior = saldoAnterior;
            SaltoAtual = saltoAtual;
        }

        public override string ToString()
        {
            return DataTransacao.ToString("yyyy-MM-ddTHH:mm:ssZ")
                + ";" + TipoMovimento
                + ";" + NumeroContaOrigem.ToString()
                + ";" + (NumeroContaDestino != 0 ? NumeroContaDestino.ToString() : "")
                + ";" + Valor.ToString("F2")
                + ";" + (TaxaCobrada != 0 ? TaxaCobrada.ToString("F2") : "")
                + ";" + SaldoAnterior.ToString("F2")
                + ";" + SaltoAtual.ToString("F2");
        }
    }
}
