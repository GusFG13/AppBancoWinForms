using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppBancoWinForms.Entities;
using System.Text;

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
            //else
            //{
            //    Console.WriteLine("Arquivo informado não existe.");
            //}
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
            //else
            //{
            //    Console.WriteLine("Arquivo informado não existe.");
            //}
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
            //else
            //{
            //    Console.WriteLine("Arquivo informado não existe.");
            //}
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
            //string path = @"c:\DadosAppBanco\clientes.csv";
            // This text is added only once to the file.
            if (File.Exists(path))
            {
                // This text is always added, making the file longer over time
                // if it is not deleted.
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
                //Console.WriteLine("Arquivo informado não existe.");
                return String.Empty;
            }
        }

        public static void EditarCadastroCliente(string path, int numCliente, Cliente novosDadosCliente)
        {
            if (File.Exists(path))
            {
                string[] LinhasCSV = File.ReadAllLines(path);
                string[][] cadastros = new string[LinhasCSV.Length][]; //cria um array onde cada elemento também é um array
                // atribui vetores de palavras (colunas) para cada elemento da matrizDePalavrasCSV (linhas)
                for (int i = 0; i < LinhasCSV.Length; i++)
                {
                    cadastros[i] = LinhasCSV[i].Split(';');
                }
                for (int i = 0; i < LinhasCSV.Length; i++)
                {
                    if (int.Parse(cadastros[i][0]) == numCliente)
                    {
                        cadastros[i][1] = novosDadosCliente.Cpf;
                        cadastros[i][2] = novosDadosCliente.Nome;
                        cadastros[i][3] = novosDadosCliente.Sobrenome;
                        break;
                    }
                    cadastros[i] = LinhasCSV[i].Split(';');
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < cadastros.Length; i++)
                {
                    //sb.AppendJoin<string>(';', cadastros[i]); // funciona no aplicativo console
                    foreach (string item in cadastros[i])
                    {
                        sb.Append(item + ";");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendLine();
                    
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(sb.ToString());
                }
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


    }
}
