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


        protected Conta(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo)
        {
            NumeroConta = numeroConta;
            TipoConta = tipoConta;
            NumeroCliente = numeroConta;
            Saldo = saldo;
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
