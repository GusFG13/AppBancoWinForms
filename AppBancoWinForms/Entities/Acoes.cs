using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppBancoWinForms.Entities
{
    internal class Acoes
    {
        public int Quantidade { get; set; }
        public double Valor { get; set; }

        public Acoes(int quantidade, double valor)
        {
            Quantidade = quantidade;
            Valor = valor;
        }





        //public void InvestirAcoes(Acoes investir, double valorInvestido)
        //{
        //    Investir = investir;
        //    ValorInvestido = valorInvestido;
        //}

        //public void ResgatarAcoes(Acoes investir, double valorRasgatar)
        //{
        //    Random rnd = new Random();
        //    Investir = investir;
        //    ValorInvestido = valorInvestido;
        //    Saldo += valorInvestido * (rnd.NextDouble() * 2);
        //}


    }
}
