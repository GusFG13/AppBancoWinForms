using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AppBancoWinForms.Entities.Enums;
using AppBancoWinForms.Utils;
using AppBancoWinForms.Entities;

namespace AppBancoWinForms.Entities
{
    public class Conta
    {
        public int NumeroConta { get;protected set; }
        public TipoConta TipoConta { get; set; }
        public int NumeroCliente { get;protected set; }
        public double Saldo { get; protected set; }
        public DateTime DataCriacao { get; protected set; }

        public Conta()
        {
        }

        public Conta(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo, DateTime dataCriacao)
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
            Saldo -= (valor + CalcularValorTarifa(valor));
        }

        public virtual double CalcularValorTarifa(double valor)
        {
            return 0.0 * valor;
        }


        public Conta TranferirParaConta(double valor, int numContaDestino, string path)
        {
            Conta contaDestinataria = null;
            string dadosContaDestinataria = "";

            if (File.Exists(path))
            {
                string[] contasCSV = File.ReadAllLines(path);
                for (int i = 0; i < contasCSV.Length; i++)
                {
                    if (int.Parse(contasCSV[i].Split(';')[0]) == numContaDestino)
                    {
                        dadosContaDestinataria = contasCSV[i];
                        break;
                    }
                }
            }

            if (dadosContaDestinataria != "")
            {
                string[] dadosConta = dadosContaDestinataria.Split(';');
                int numContaDest = int.Parse(dadosConta[0]);
                TipoConta tipoContaDest = (TipoConta)Enum.Parse(typeof(TipoConta), dadosConta[1]);
                int numClienteDest = int.Parse(dadosConta[2]);
                double saldoDest = double.Parse(dadosConta[3]);
                DateTime dataCriacaoDest = DateTime.Parse(dadosConta[4]);


                switch (tipoContaDest)
                {
                    case TipoConta.ContaPoupanca:
                        contaDestinataria = new ContaPoupanca(numContaDest, tipoContaDest, numClienteDest, saldoDest, dataCriacaoDest);
                        break;
                    case TipoConta.ContaInvestimento:
                        contaDestinataria = new ContaInvestimento(numContaDest, tipoContaDest, numClienteDest, saldoDest, dataCriacaoDest);
                        break;
                    case TipoConta.ContaSalario:      
                    default:
                        contaDestinataria = null;
                        break;
                }

                if (contaDestinataria != null)
                {
                    Sacar(valor);
                    contaDestinataria.Depositar(valor);
                }
            }
            return contaDestinataria;
        }
    }
}
