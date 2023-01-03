using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBancoWinForms.Entities;
using AppBancoWinForms.Entities.Enums;

namespace AppBancoWinForms.Utils
{
    internal class Cadastros
    {
        /*Verificar a possibilidade da classe Cadastros herdar de Conta
         * para um maior uso de POO
         */
        public static Cliente CadastrarCliente(string path, string cpf, string nome, string sobrenome, string senha)
        {
            int novoCodigo = 1;
            // Descobrir próximo código para cadastro de novo cliente
            if (File.Exists(path))
            {
                novoCodigo = int.Parse(EscreverArquivosBD.LerUltimaLinha(path).Split(';')[0]) + 1;
            }
            Cliente novoCliente = new Cliente(novoCodigo, cpf, nome, sobrenome,PerfilInvestidor.Indefinido, senha);

            EscreverArquivosBD.EscreverNovoItem(path, novoCliente.ToString());
            return novoCliente;
        }


        public static Conta CadastrarConta(string path, TipoConta tipoConta, int numCliente, double saldo, DateTime dataCriacao, Holerite holerite)
        {
            Conta novaConta;
            int novoCodigo = 1;
            // Descobrir próximo código para cadastro de novo cliente
            if (File.Exists(path))
            {
                novoCodigo = int.Parse(EscreverArquivosBD.LerUltimaLinha(path).Split(';')[0]) + 1;
            }
            
            switch (tipoConta)
            {
                //Verificar possibilidade de transformar em metodo logica de seleção de conta
                case TipoConta.ContaPoupanca:
                    novaConta = new ContaPoupanca(novoCodigo, tipoConta, numCliente, saldo, dataCriacao);
                    break;
                case TipoConta.ContaInvestimento:
                    novaConta = new ContaInvestimento(novoCodigo, tipoConta, numCliente, saldo, dataCriacao);
                    break;
                case TipoConta.ContaSalario:
                    novaConta = new ContaSalario(novoCodigo, tipoConta, numCliente, saldo, dataCriacao, holerite);
                    break;
                default:
                    novaConta = null;
                    break;
            }

            EscreverArquivosBD.EscreverNovoItem(path, novaConta.ToString());
            return novaConta;
        }

    }
}
