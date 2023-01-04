using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppBancoWinForms.Entities;
using System.Text;
using AppBancoWinForms.Entities.Enums;
using System.Drawing;

namespace AppBancoWinForms.Utils
{
    internal class EscreverArquivosBD
    {
        public static bool ProcurarCliente(string path, string CpfProcurado)
        {
            bool encontrou = false;
            if (File.Exists(path))
            {
                // Open the file to read from.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (s.Split(';')[1] == CpfProcurado)
                        {
                            sr.Close();
                            encontrou = true;
                            break;
                        }
                    }
                }
            }
            return encontrou;
        }

        public static string RetornarDadosCliente(string path, string CpfProcurado)
        {
            string dadosCliente = "";
            if (File.Exists(path))
            {
                // Open the file to read from.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (s.Split(';')[1] == CpfProcurado)
                        {
                            dadosCliente = s;
                            sr.Close();
                            break;
                        }
                    }
                }
            }
            return dadosCliente;
        }

        public static List<string> ProcurarContasCliente(string path, int numTitular)
        { // implementar código

            List<string> listaContas = new List<string>();
            if (File.Exists(path))
            {
                // Open the file to read from.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (int.Parse(s.Split(';')[2]) == numTitular)
                        {
                            listaContas.Add(s);
                            
                        }
                    }
                    sr.Close();
                }
            }
            return listaContas;
        }
        public static List<string> BuscarTransacoes(string path, int numConta, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            List<string> listaTransacoes = new List<string>();
            if (File.Exists(path))
            {
                // Open the file to read from.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (int.Parse(s.Split(';')[2]) == numConta)
                        {
                            DateTime dataMovimento = DateTime.Parse(s.Split(';')[0]).ToLocalTime().Date;
                            if (dataMovimento >= inicioPeriodo && dataMovimento <= fimPeriodo)
                            {
                                listaTransacoes.Add(s);
                            }
                        }
                    }
                    sr.Close();
                }
            }
            return listaTransacoes;
        }

        public static void EscreverNovoItem(string path, string dados)
        {
            // This text is added only once to the file.
            if (File.Exists(path))
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(dados);
                    sw.Close();
                }
            }
            else
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(dados);
                    sw.Close();
                }
            }
        }

        public static string LerUltimaLinha(string path)
        {
            // lê última linha
            if (File.Exists(path))
            {
                return File.ReadLines(path).Last();
            }
            else
            {
                return String.Empty;
            }
        }





        public static void GravarSaldoContaAtualizado(string path, Conta conta)
        {
            if (File.Exists(path))
            {
                string[] contasCSV = File.ReadAllLines(path);
                for (int i = 0; i < contasCSV.Length; i++)
                {
                    if (int.Parse(contasCSV[i].Split(';')[0]) == conta.NumeroConta)
                    {
                        contasCSV[i] = conta.ToString();
                        break;
                    }
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < contasCSV.Length; i++)
                {
                    sb.AppendLine(contasCSV[i]);
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(sb.ToString());
                }
            }
        }


        public static ContaSalario BuscarContaSalario(string path, int numContaSalario)
        {
            ContaSalario contaSalarioDestino = null;
            if (File.Exists(path))
            {
                string[] contasCSV = File.ReadAllLines(path);
                for (int i = 0; i < contasCSV.Length; i++)
                {
                    if (int.Parse(contasCSV[i].Split(';')[0]) == numContaSalario)
                    {
                        string[] dadosContaSalario = contasCSV[i].Split(';');
                        TipoConta tipoConta = (TipoConta)Enum.Parse(typeof(TipoConta), dadosContaSalario[1]);
                        if (tipoConta == TipoConta.ContaSalario)
                        {
                            int numCliente = int.Parse(dadosContaSalario[2]);
                            double saldo = double.Parse(dadosContaSalario[3]);
                            DateTime dataCriacao = DateTime.Parse(dadosContaSalario[4]);
                            Holerite dadosHolerite = new Holerite();
                            dadosHolerite.Cnpj = dadosContaSalario[5];
                            dadosHolerite.NomeFontePagadora = dadosContaSalario[6];
                            contaSalarioDestino = new ContaSalario(numContaSalario, tipoConta, numCliente, saldo, dataCriacao, dadosHolerite);
                        }
                        break;
                    }
                }
            }
            return contaSalarioDestino;
        }

        public static string RetornarCarteiraAcoes(string path, int numContaInvest)
        {
            string dadosCarteira = "";
            if (File.Exists(path))
            {
                // Open the file to read from.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (int.Parse(s.Split(';')[0]) == numContaInvest)
                        {
                            dadosCarteira = s;
                            sr.Close();
                            break;
                        }
                    }
                }
            }
            return dadosCarteira;
        }


        public static void GravarCarteiraAtualizada(string path, int numConta, string dadosAtualizados)
        {
            bool carteiraExiste = false;
            if (File.Exists(path))
            {
                string[] investimentosCSV = File.ReadAllLines(path);
                for (int i = 0; i < investimentosCSV.Length; i++)
                {
                    if (int.Parse(investimentosCSV[i].Split(';')[0]) == numConta)
                    {
                        investimentosCSV[i] = dadosAtualizados;
                        carteiraExiste = true;
                        break;
                    }
                } 
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < investimentosCSV.Length; i++)
                {
                    sb.AppendLine(investimentosCSV[i]);
                }
                if(!carteiraExiste) 
                { 
                    sb.AppendLine(dadosAtualizados); 
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(sb.ToString());
                }
            }
            else
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(dadosAtualizados);
                    sw.Close();
                }
            }
        }


    }
}
