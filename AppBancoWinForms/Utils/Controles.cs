using System.Windows.Forms;
using System;
using AppBancoWinForms.Entities;
using AppBancoWinForms.Entities.Enums;

namespace AppBancoWinForms.Utils
{
    internal class Controles
    {
        public static void LimpaCaixasTextos(Control.ControlCollection parentControl)
        {
            foreach (Control c in parentControl)
            {
                if (c.HasChildren)
                {
                    //Recursively loop through the child controls
                    LimpaCaixasTextos(c.Controls);
                }
                else
                {
                    if (c is TextBox || c is MaskedTextBox)
                    {
                        c.Text = "";
                    }
                }
            }
        }


        public static Conta SelecionarConta(string c)
        {
            string[] dadosConta = c.Split(';');
            int numConta = int.Parse(dadosConta[0]);
            TipoConta tipoConta = (TipoConta)Enum.Parse(typeof(TipoConta), dadosConta[1]);
            int numcliente = int.Parse(dadosConta[2]);
            double saldo = double.Parse(dadosConta[3]);
            DateTime dataCriacao = DateTime.Parse(dadosConta[4]);

            Conta conta;

            switch (tipoConta)
            {
                case TipoConta.ContaPoupanca:
                    conta = new ContaPoupanca(numConta, tipoConta, numcliente, saldo, dataCriacao);
                    break;
                case TipoConta.ContaInvestimento:
                    conta = new ContaInvestimento(numConta, tipoConta, numcliente, saldo, dataCriacao);
                    break;
                case TipoConta.ContaSalario:
                    Holerite holerite = new Holerite();
                    holerite.Cnpj = dadosConta[5];
                    holerite.NomeFontePagadora = dadosConta[6];
                    conta = new ContaSalario(numConta, tipoConta, numcliente, saldo, dataCriacao, holerite);
                    break;
                default:
                    conta = null;
                    break;
            }

            return conta;
        }



    }

}
