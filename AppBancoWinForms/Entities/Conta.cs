using System;
using System.Collections.Generic;
using AppBancoWinForms.Entities.Enums;

namespace AppBancoWinForms.Entities
{
    internal class Conta
    {
        public int NumeroConta { get; set; }
        public TipoConta TipoConta { get; set; }
        public int NumeroCliente { get; set; }
        public double Saldo { get; set; }
        public DateTime DataCriacao { get; set; }

        public Conta()
        {
        }

        protected Conta(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo, DateTime dataCriacao)
        {
            NumeroConta = numeroConta;
            TipoConta = tipoConta;
            NumeroCliente = numeroCliente;
            Saldo = saldo;
            DataCriacao = dataCriacao;
        }

        public void Depositar(double valor)
        {
            Saldo += valor;
        }

        public void Sacar(double valor)
        {
            Saldo -= valor;
        }
    }
}
