using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AppBancoWinForms.Entities.Enums;
using AppBancoWinForms.Utils;

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


        public Conta TranferenciaParaPoupanca(double valor, int numContaDestino, string path)
        {
            ContaPoupanca contaDestinataria = null;
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
                int numConta = int.Parse(dadosConta[0]);
                TipoConta tipoConta = (TipoConta)Enum.Parse(typeof(TipoConta), dadosConta[1]);
                int numcliente = int.Parse(dadosConta[2]);
                double saldo = double.Parse(dadosConta[3]);
                DateTime dataCriacao = DateTime.Parse(dadosConta[4]);

                if (tipoConta == TipoConta.ContaPoupanca) 
                {
                    contaDestinataria = new ContaPoupanca(numConta, tipoConta, numcliente, saldo, dataCriacao);

                    Sacar(valor);
                    contaDestinataria.Depositar(valor);
                    
                }
                
            }
            return contaDestinataria;
        }
    }
}
