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
        public DateTime DataTransacao { get; protected set; }
        public string TipoMovimento { get; protected set; }
        public int NumeroContaOrigem { get; protected set; }
        public int NumeroContaDestino { get; protected set; }
        public double Valor { get; protected set; }
        public double TaxaCobrada { get; protected set; }
        public double SaldoAnterior { get; protected set; }
        public double SaldoAtual { get; protected set; }

        public Extrato(DateTime dataTransacao, string tipoMovimento, int numeroContaOrigem, int numeroContaDestino, double valor, double taxaCobrada, double saldoAnterior, double saldoAtual)
        { // para transferencias feitas
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = numeroContaDestino;
            Valor = valor;
            TaxaCobrada = taxaCobrada;
            SaldoAnterior = saldoAnterior;
            SaldoAtual = saldoAtual;
        }
        public Extrato(DateTime dataTransacao, string tipoMovimento, int numContaRecebeu, int numContaPagou, double valor, double saldoAtual)
        { // para transferencias recebidas
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem  = numContaRecebeu; // conta que recebeu
            NumeroContaDestino = numContaPagou; // conta que transferiu
            Valor = valor;
            SaldoAtual = saldoAtual;
            SaldoAnterior = SaldoAtual - valor;
        }
        public Extrato(DateTime dataTransacao, string tipoMovimento, int numeroContaOrigem, double valor, double taxaCobrada, double saldoAnterior, double saldoAtual)
        { // para saques
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = 0;
            Valor = valor;
            TaxaCobrada = taxaCobrada;
            SaldoAnterior = saldoAnterior;
            SaldoAtual = saldoAtual;
        }
        public Extrato(DateTime dataTransacao, string tipoMovimento, int numeroContaOrigem, double valor, double saldoAnterior, double saldoAtual)
        { // para depósitos
            DataTransacao = dataTransacao;
            TipoMovimento = tipoMovimento;
            NumeroContaOrigem = numeroContaOrigem;
            NumeroContaDestino = 0;
            Valor = valor;
            TaxaCobrada = 0.0;
            SaldoAnterior = saldoAnterior;
            SaldoAtual = saldoAtual;
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
                + ";" + SaldoAtual.ToString("F2");
        }
    }
}
