using AppBancoWinForms.Entities;
using AppBancoWinForms.Utils;
using AppBancoWinForms.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.IO;
using System.Collections;

namespace AppBancoWinForms
{
    public partial class Form1 : Form
    {
        //Cliente cliente = new Cliente();
        Cliente cliente;
        Conta contaAtual = new Conta();
        int contaSelecionada = 0;
        List<string> listaContas;
        readonly static string path = @"c:\DadosAppBanco";
        readonly static string pathClientes = path + @"\clientes.csv";
        readonly static string pathContas = path + @"\contas.csv";
        readonly static string pathTransacoes = path + @"\trasacoes.csv";
        readonly static string pathInvestimentos = path + @"\investimentos.csv";


        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            //this.MinimizeBox = false;
            lblErroLogin.Text = "";

            // Esconde abas do tabCtrlTipoConta (na aba Abrir Nova Conta de tabCtrlTelasApp)
            tabCtrlTipoConta.Appearance = TabAppearance.FlatButtons;
            tabCtrlTipoConta.ItemSize = new Size(0, 1);
            tabCtrlTipoConta.SizeMode = TabSizeMode.Fixed;

            // Esconde abas do tabCtrlTelasApp (tela principal)
            tabCtrlTelasApp.Appearance = TabAppearance.FlatButtons;
            tabCtrlTelasApp.ItemSize = new Size(0, 1);
            tabCtrlTelasApp.SizeMode = TabSizeMode.Fixed;



            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    //Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                Application.Exit();
            }
        }

        private void btEnter_Click(object sender, EventArgs e)
        {
            lblErroLogin.Text = "";
            //string numerosCPF;

            //mtbCPFLogin.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            //numerosCPF = mtbCPFLogin.Text;
            // if (numerosCPF.Length == 11 || double.TryParse(numerosCPF, out _))
            if (mtbCPFLogin.MaskFull)
            {
                //mtbCPFLogin.TextMaskFormat = MaskFormat.IncludeLiterals;
                string dadosCliente = EscreverArquivosBD.RetornarDadosCliente(pathClientes, mtbCPFLogin.Text);
                if (dadosCliente != "")
                {
                    string[] aux = dadosCliente.Split(';');
                    if (aux[5] == mtbSenhaLogin.Text)
                    {
                        cliente = new Cliente(int.Parse(aux[0]), aux[1], aux[2], aux[3], (PerfilInvestidor)Enum.Parse(typeof(PerfilInvestidor), aux[4]), aux[5]);
                        //cliente.Codigo = int.Parse(aux[0]);
                        //cliente.Cpf = aux[1];
                        //cliente.Nome = aux[2];
                        //cliente.Sobrenome = aux[3];
                        //cliente.Perfil = (PerfilInvestidor)Enum.Parse(typeof(PerfilInvestidor), aux[4]);
                        //cliente.Senha = aux[5];

                        tabCtrlTelasApp.SelectedTab = tabPageMovContas; // ir para a página de seleção de conta
                        
                    }
                    else
                    {
                        lblErroLogin.Text = "Senha incorreta";
                        mtbSenhaLogin.Focus();
                    }
                }
                else
                {
                    lblErroLogin.Text = "CPF não encontrado no cadastro";
                    mtbCPFLogin.Focus();
                }
            }
            else
            {
                mtbCPFLogin.Focus();
                lblErroLogin.Text = "Formato do CPF incorreto";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                mtbSenhaCadastro.UseSystemPasswordChar = false;
                mtbSenhaCadastroRepetir.UseSystemPasswordChar = false;
            }
            else
            {
                mtbSenhaCadastro.UseSystemPasswordChar = true;
                mtbSenhaCadastroRepetir.UseSystemPasswordChar = true;
            }

        }

        private void btCadastrar_Click(object sender, EventArgs e)
        {
            bool dadosOK = true;
            //string numerosCPF;
            //mtbCPFCadastro.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            //numerosCPF = mtbCPFCadastro.Text;
            //if (numerosCPF.Length != 11 || !double.TryParse(numerosCPF, out _)) // melhorar validação
            if (!mtbCPFCadastro.MaskFull) // melhorar validação
            {
                dadosOK = false;
                MessageBox.Show("Por favor, preencha o campo CPF corretamente.", "CPF Inválido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mtbCPFCadastro.Focus();
            }
            else if (String.IsNullOrWhiteSpace(tbNomeCadastro.Text))
            {
                dadosOK = false;
                tbNomeCadastro.Focus();
            }
            else if (String.IsNullOrWhiteSpace(tbSobrenomeCadastro.Text))
            {
                dadosOK = false;
                tbSobrenomeCadastro.Focus();
            }
            else if (mtbSenhaCadastro.Text.Length < 6)
            {
                dadosOK = false;
                mtbSenhaCadastro.Focus();
                MessageBox.Show("Senha deve ter 6 números", "Senha Inválida", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (mtbSenhaCadastro.Text != mtbSenhaCadastroRepetir.Text)
                {
                    dadosOK = false;
                    mtbSenhaCadastro.Focus();
                    MessageBox.Show("Campos senhas não coincidem", "Senhas Diferentes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (dadosOK) // Gravar novo usuário no arquivo c:\DadosAppBanco\clientes.csv
            {



                //mtbCPFCadastro.TextMaskFormat = MaskFormat.IncludeLiterals;
                string cpf = mtbCPFCadastro.Text;
                string nome = tbNomeCadastro.Text;
                string sobrenome = tbSobrenomeCadastro.Text;
                string senha = mtbSenhaCadastro.Text;


                // cadastrar novo
                if (EscreverArquivosBD.ProcurarCliente(pathClientes, cpf))
                {
                    MessageBox.Show("CPF já cadastrado!", "Falha no Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Console.WriteLine("CPF já cadastrado");
                }
                else
                {
                    /**!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
                    cliente = Cadastros.CadastrarCliente(pathClientes, cpf, nome, sobrenome, senha);
                    MessageBox.Show("Novo cadastro realizado", "Cadastro concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Console.WriteLine("NOVO CADASTRO: " + cliente.ToString());
                    tabCtrlTelasApp.SelectedTab = tabPageAbrirNovaConta; // Ir para a tela de criação de conta
                    btVoltarParaMenu.Visible = false;
                }
            }
        }

        private void btNovoCadastro_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPageNovoCadCliente;
            mtbCPFCadastro.Focus();
        }

        private void tabPageExtrato_Enter(object sender, EventArgs e)
        {
            rb7dias.Checked = true;
            rtbExtrato.Text = string.Empty;
        }

        private void rb7dias_CheckedChanged(object sender, EventArgs e)
        {
            if (rb7dias.Checked)
            {
                DateTime hoje = DateTime.Now;
                dtpInicio.ValueChanged -= dtpInicio_ValueChanged;
                dtpInicio.Value = hoje.AddDays(-7);
                dtpInicio.ValueChanged += dtpInicio_ValueChanged;
                dtpFim.ValueChanged -= dtpFim_ValueChanged;
                dtpFim.Value = hoje;
                dtpFim.ValueChanged += dtpFim_ValueChanged;
                rtbExtrato.Text = "";
            }
        }

        private void rb15dias_CheckedChanged(object sender, EventArgs e)
        {
            if (rb15dias.Checked)
            {
                DateTime hoje = DateTime.Now;
                dtpInicio.ValueChanged -= dtpInicio_ValueChanged;
                dtpInicio.Value = hoje.AddDays(-15);
                dtpInicio.ValueChanged += dtpInicio_ValueChanged;
                dtpFim.ValueChanged -= dtpFim_ValueChanged;
                dtpFim.Value = hoje;
                dtpFim.ValueChanged += dtpFim_ValueChanged;
                rtbExtrato.Text = "";
            }
        }

        private void rb30dias_CheckedChanged(object sender, EventArgs e)
        {
            if (rb30dias.Checked)
            {
                DateTime hoje = DateTime.Now;
                dtpInicio.ValueChanged -= dtpInicio_ValueChanged;
                dtpInicio.Value = hoje.AddDays(-30);
                dtpInicio.ValueChanged += dtpInicio_ValueChanged;
                dtpFim.ValueChanged -= dtpFim_ValueChanged;
                dtpFim.Value = hoje;
                dtpFim.ValueChanged += dtpFim_ValueChanged;
                rtbExtrato.Text = "";
            }
        }

        private void dtpInicio_ValueChanged(object sender, EventArgs e)
        {
            rb7dias.Checked = false;
            rb15dias.Checked = false;
            rb30dias.Checked = false;
            rtbExtrato.Text = "";
        }

        private void dtpFim_ValueChanged(object sender, EventArgs e)
        {
            rb7dias.Checked = false;
            rb15dias.Checked = false;
            rb30dias.Checked = false;
            rtbExtrato.Text = "";
        }

        private void btGerarextrato_Click(object sender, EventArgs e)
        {
            if (dtpInicio.Value > dtpFim.Value)
            {
                MessageBox.Show("Final do período escolhido anterior ao início.\nPor favor, escolher outro período.", "Período Inválido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpInicio.Focus();
            }
            else
            {
                string tempCnpj = "";
                if (contaAtual is ContaSalario)
                {
                    ContaSalario cTemp = contaAtual as ContaSalario;
                    tempCnpj = cTemp.Holerite.Cnpj;
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Nome titular: {cliente.Nome} {cliente.Sobrenome}; CPF: {cliente.Cpf}; Nº cadastro: {cliente.Codigo}");
                sb.AppendLine($"Operações em Conta Nº {contaAtual.NumeroConta}; Tipo Conta: {contaAtual.TipoConta.ToString().Replace("Conta", "")}{(tempCnpj != "" ? ("; CNPJ Fonte Pagadora: " + tempCnpj) : "")}");
                sb.AppendLine($"Período selecionado: de {dtpInicio.Value.Date.ToString("dd/MM/yyyy")} a {dtpFim.Value.Date.ToString("dd/MM/yyyy")}");
                sb.AppendLine($"Extrato gerado em: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                List<string> listaTransacoes = EscreverArquivosBD.BuscarTransacoes(pathTransacoes, contaAtual.NumeroConta, dtpInicio.Value.Date, dtpFim.Value.Date);
                if (listaTransacoes.Count > 0) // cria tabela
                {

                    sb.AppendLine("+--------------------+----------------------+---------+--------------+--------------+----------------+---------------+");
                    sb.AppendLine("|                    |         Tipo         |         |              | Taxa Cobrada | Saldo Anterior |  Saldo Final  |");
                    sb.AppendLine("|    Data - Hora     |       Operação       |  D / O  |  Valor (R$)  |     (R$)     |       (R$)     |      (R$)     |");
                    sb.AppendLine("+--------------------+----------------------+---------+--------------+--------------+----------------+---------------+");
                    var table = new DataTable();
                    foreach (string item in listaTransacoes)
                    {
                        string[] strAux = item.Split(';');
                        //sb.AppendLine(item);
                        DateTime dataHoraAux = DateTime.Parse(strAux[0]).ToLocalTime();
                        sb.AppendLine($"| {dataHoraAux.ToString("dd/MM/yyyy - HH:mm")} | {strAux[1].PadRight(20)} | {PadBoth(strAux[3], 7)} | {strAux[4].PadLeft(12)} | {strAux[5].PadLeft(12)} | {strAux[6].PadLeft(14)} | {strAux[7].PadLeft(13)} |");
                    }
                    sb.AppendLine("+--------------------+----------------------+---------+--------------+--------------+----------------+---------------+");
                    sb.AppendLine("D / O: Número da conta Destinatária ou Origem da movimentação");
                }
                else
                {
                    sb.AppendLine("\n*** Não foram encontradas movimentações nesta conta no período selecionado. ***");
                }
                rtbExtrato.Text = sb.ToString();
            }
        }
        public string PadBoth(string source, int length)
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft).PadRight(length);

        }

        private void btExportarExtrato_Click(object sender, EventArgs e)
        {
            if (rtbExtrato.Text.Length == 0)
            {
                MessageBox.Show("Por favor, gerar o extrato para o período.", "Extrato Indisponível", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Arquivo de texto | *.txt";
                sfd.Title = "Salvar Extrato Gerado";
                //sfd.ShowDialog();

                string horaExtratoSalvo = "";
                int pos = rtbExtrato.Find("Extrato gerado em: ");
                if (pos != 1)
                {
                    if (DateTime.TryParse(rtbExtrato.Text.Substring(pos + 19, 19), out DateTime horaExtratoGerado))
                    { // conseguiu ler do texto a hora que o extrato foi gerado
                        horaExtratoSalvo = horaExtratoGerado.ToString("yyyyMMddHHmmss");
                    }
                    else
                    { // senão salva pega a hora atual para criar o nome
                        horaExtratoSalvo = DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                }
                //sfd.FileName = $"Extrato_Conta_{contaAtual.NumeroConta}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                sfd.FileName = $"Extrato_Conta_{contaAtual.NumeroConta}_{horaExtratoSalvo}";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, rtbExtrato.Text);
                }

            }
        }

        /******************** Seleção tipo de conta a ser criada ********************/
        private void rbPoupanca_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPoupanca.Checked)
            {
                tabCtrlTipoConta.SelectedTab = tabPagePoupanca;
                tbDepInicialPoup.Focus();
            }
        }

        private void rbSalario_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSalario.Checked)
            {
                tabCtrlTipoConta.SelectedTab = tabPageSalario;
                mtbCnpjCadastroContaSal.Focus();
            }

        }

        private void rbInvestimento_CheckedChanged(object sender, EventArgs e)
        {
            if (rbInvestimento.Checked)
            {
                tabCtrlTipoConta.SelectedTab = tabPageInvestimento;
                rbPerfil1.Focus();
            }

        }
        /**************************************************************************/
        private void btIrParaPagExtrato_Click(object sender, EventArgs e)
        {
            contaSelecionada = cbContaSelecionada.SelectedIndex;
            tabCtrlTelasApp.SelectedTab = tabPageExtrato;
        }

        private void cbContaSelecionada_SelectedIndexChanged(object sender, EventArgs e)
        {

            rbDepositar.Checked = false;
            rbDepositar.Enabled = false;
            rbSacar.Checked = false;
            rbSacar.Enabled = false;
            rbTransferencia.Checked = false;
            rbTransferencia.Enabled = false;
            btInvestirAcoes.Enabled = false;

            contaSelecionada = cbContaSelecionada.SelectedIndex;
            contaAtual = Controles.SelecionarConta(listaContas[contaSelecionada]);
            lblNumConta.Text = contaAtual.NumeroConta.ToString();
            lblDataAbertura.Text = contaAtual.DataCriacao.ToLocalTime().ToString("dd/MM/yyyy");
            lblSaldoAtual.Text = "R$ " + contaAtual.Saldo.ToString("F2");
            switch (contaAtual.TipoConta)
            {
                case TipoConta.ContaPoupanca:
                    rbDepositar.Enabled = true;
                    rbSacar.Enabled = true;
                    rbTransferencia.Enabled = true;
                    lblTipoConta.Text = "Conta Poupança";
                    break;
                case TipoConta.ContaSalario:
                    rbSacar.Enabled = true;
                    rbTransferencia.Enabled = true;
                    lblTipoConta.Text = "Conta Salário";
                    break;
                case TipoConta.ContaInvestimento:
                    rbDepositar.Enabled = true;
                    rbSacar.Enabled = true;
                    rbTransferencia.Enabled = true;
                    btInvestirAcoes.Enabled = true;
                    lblTipoConta.Text = "Conta Investimento";
                    break;
                default: break;
            }
        }

        private void tabPageMovContas_Enter(object sender, EventArgs e)
        {
            DateTime agora = DateTime.Now;
            string msg = "";
            if (agora.Hour < 12)
                msg = "Bom dia, ";
            else if (agora.Hour < 18)
                msg = "Boa Tarde, ";
            else
                msg = "Boa noite, ";
            msg += cliente.Nome + "!";
            lblErroMovimentacao.Text = "";
            lblSaudacao.Text = msg;
            chkBoxMostrarSaldo.Checked = false;
            lblSaldoAtual.Visible = false;
            pnlNumContaDestino.Enabled = false;
            btMovimentoConta.Text = "Ok";

            /******* Buscar contas do cliente adicionar no comboBox ***********/
            cbContaSelecionada.Items.Clear();
            listaContas = EscreverArquivosBD.ProcurarContasCliente(pathContas, cliente.Codigo);
            foreach (string c in listaContas)
            {
                cbContaSelecionada.Items.Add(c.Split(';')[0] + " - " + c.Split(';')[1]);
            }
            /********************************************************************/
            if (cbContaSelecionada.Items.Count > 0)
            {
                //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                cbContaSelecionada.SelectedIndex = contaSelecionada;
            }
            else
            {
                //cbContaSelecionada.Visible = false;
                //cbContaSelecionada.Enabled = false;
                tabCtrlTelasApp.SelectedTab = tabPageAbrirNovaConta;
            }
        }

        private void btVoltar_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPageMovContas;
        }

        private void btVoltarParaMenu_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPageMovContas;
        }

        private void btAbrirNovaConta_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPageAbrirNovaConta;
            btVoltarParaMenu.Visible = true;
        }

        private void btCadastrarNovaConta_Click(object sender, EventArgs e)
        {
            //Fazer validações
            bool requisitosOK = true;
            double saldoInicial = 0.0;
            TipoConta tConta = TipoConta.ContaPoupanca;
            Holerite holerite = new Holerite();

            if (rbPoupanca.Checked)
            {
                tConta = TipoConta.ContaPoupanca;

                if (!tbDepInicialPoup.Text.Contains(".") && double.TryParse(tbDepInicialPoup.Text, out saldoInicial))
                {
                    if (saldoInicial < 500.00)
                    {
                        requisitosOK = false;
                        tbDepInicialPoup.Focus();
                    }

                }
                else
                {
                    requisitosOK = false;
                    tbDepInicialPoup.Focus();
                }
            }
            else if (rbSalario.Checked)
            {
                if (mtbCnpjCadastroContaSal.MaskFull) // cnpj preenchido?
                {
                    if (String.IsNullOrWhiteSpace(tbNomeFontePag.Text)) // nome preenchido?
                    {
                        tbNomeFontePag.Focus();
                        requisitosOK = false;
                    }
                    else
                    {
                        tConta = TipoConta.ContaSalario;
                        holerite.Cnpj = mtbCnpjCadastroContaSal.Text;
                        holerite.NomeFontePagadora = tbNomeFontePag.Text;
                    }
                }
                else
                {
                    mtbCnpjCadastroContaSal.Focus();
                    requisitosOK = false;
                }
            }
            else if (rbInvestimento.Checked)
            {
                tConta = TipoConta.ContaInvestimento;
                if (!tbDepIniInvest.Text.Contains(".") && double.TryParse(tbDepIniInvest.Text, out saldoInicial))
                {
                    if (saldoInicial >= 0)
                    {
                        //verificar radioButton selecionado e atualizar cadastro do cliente em clientes.csv
                        if (rbPerfil1.Checked)
                        {
                            cliente.Perfil = (PerfilInvestidor)Enum.Parse(typeof(PerfilInvestidor), rbPerfil1.Text);
                        }
                        else if (rbPerfil2.Checked)
                        {
                            cliente.Perfil = (PerfilInvestidor)Enum.Parse(typeof(PerfilInvestidor), rbPerfil2.Text);
                        }
                        else if (rbPerfil3.Checked)
                        {
                            cliente.Perfil = (PerfilInvestidor)Enum.Parse(typeof(PerfilInvestidor), rbPerfil3.Text);
                        }
                        else
                        {
                            requisitosOK = false;
                        }
                    }
                    else
                    {
                        tbDepIniInvest.Focus();
                        requisitosOK = false;
                    }
                }
                else
                {
                    requisitosOK = false;
                    tbDepIniInvest.Focus();
                }
            }
            if (requisitosOK)
            {
                contaAtual = Cadastros.CadastrarConta(pathContas, tConta, cliente.Codigo, saldoInicial, DateTime.UtcNow, holerite);
                if (rbInvestimento.Checked) //atualizar perfil investidor no cadastro do cliente (clientes.csv)
                {
                    cliente.EditarCadastroCliente(pathClientes);
                }
                contaSelecionada = cbContaSelecionada.Items.Count;
                tabCtrlTelasApp.SelectedTab = tabPageMovContas;
            }
        }

        private void btLogout_Click(object sender, EventArgs e)
        {
            DialogResult resposta = MessageBox.Show("Deseja realmente sair?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resposta == DialogResult.Yes)
            {
                //mtbCPFLogin.Text = string.Empty;
                //mtbSenhaLogin.Text = string.Empty;
                cliente = new Cliente();
                contaAtual = new Conta();
                cbContaSelecionada.Items.Clear();
                contaSelecionada = 0;
                listaContas.Clear();
                Controles.LimpaCaixasTextos(tabCtrlTelasApp.Controls);
                tabCtrlTelasApp.SelectedTab = tabPageLogin;
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {// evita fechamento do app por engano ao clicar no botão fechar janela ou pressionar Alt + F4
            DialogResult resposta = MessageBox.Show("Deseja realmente fechar o programa?", "Fechar App", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resposta == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void chkBoxMostrarSaldo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxMostrarSaldo.Checked)
            {
                lblSaldoAtual.Visible = true;
            }
            else
            {
                lblSaldoAtual.Visible = false;
            }

        }

        private void rbDepositar_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDepositar.Checked)
            {
                btMovimentoConta.Text = "Depositar";
                lblErroMovimentacao.Text = "";
            }
        }

        private void rbSacar_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSacar.Checked)
            {
                btMovimentoConta.Text = "Sacar";
                lblErroMovimentacao.Text = "";
            }
        }

        private void rbTransferenciaPoup_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTransferencia.Checked)
            {
                btMovimentoConta.Text = "Transferir";
                pnlNumContaDestino.Enabled = true;
                lblErroMovimentacao.Text = "";
            }
            else
            {
                pnlNumContaDestino.Enabled = false;
            }
        }

        private void btMovimentoConta_Click(object sender, EventArgs e)
        {
            if (double.TryParse(tbValorMovimento.Text.Replace('.', ','), out double val))
            {
                if (val > 0)
                {
                    DateTime horaTransacao = DateTime.UtcNow;
                    double saldoAnterior = contaAtual.Saldo;// salvar saldo anterior para exibir no extrato
                    bool movConcluido = false;
                    if (rbDepositar.Checked)
                    {
                        contaAtual.Depositar(val);
                        EscreverArquivosBD.GravarSaldoContaAtualizado(pathContas, contaAtual);
                        Extrato movimento = new Extrato(horaTransacao, "Depósito", contaAtual.NumeroConta, val, saldoAnterior, contaAtual.Saldo);
                        EscreverArquivosBD.EscreverNovoItem(pathTransacoes, movimento.ToString());
                        movConcluido = true;
                    }
                    else if (rbSacar.Checked)
                    {
                        double taxaCobrada = contaAtual.CalcularValorTarifa(val);
                        string pergunta = $"Será cobrada uma taxa de R$ {taxaCobrada.ToString("F2")} nesta operação.\nDeseja continuar?";
                        DialogResult resp = MessageBox.Show(pergunta, "Continuar?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (resp == DialogResult.Yes)
                        {
                            //if (val > 0 && val <= contaAtual.Saldo) //permite saldo negativo se valor + taxa > saldo
                            if (contaAtual.Saldo >= (val + taxaCobrada)) //considera se valor da operação (valor + taxa) está disponível
                            {
                                contaAtual.Sacar(val);
                                EscreverArquivosBD.GravarSaldoContaAtualizado(pathContas, contaAtual);
                                //double taxaCobrada = contaAtual.CalcularValorTarifa(val);
                                // Escrever movimentação em transacoes.csv
                                Extrato movimento = new Extrato(horaTransacao, "Saque", contaAtual.NumeroConta, val, taxaCobrada, saldoAnterior, contaAtual.Saldo);
                                EscreverArquivosBD.EscreverNovoItem(pathTransacoes, movimento.ToString());
                                movConcluido = true;
                            }
                            else
                            {
                                tbValorMovimento.Focus();
                                lblErroMovimentacao.Text = $"Saldo em conta insuficiente! \n Valor + taxa: R$ {(val + taxaCobrada).ToString("F2")}";
                            }
                        }
                    }
                    else if (rbTransferencia.Checked)
                    {
                        if (!String.IsNullOrEmpty(tbContaDestino.Text) && int.TryParse(tbContaDestino.Text, out int numContaDest))
                        {
                            //if (val > 0 && val <= contaAtual.Saldo && numContaDest != contaAtual.NumeroConta)
                            if (numContaDest != contaAtual.NumeroConta)
                            {
                                double taxaCobrada = contaAtual.CalcularValorTarifa(val);// cobra apenas de onde o dinheiro saiu 

                                if (contaAtual.Saldo >= (val + taxaCobrada))
                                {
                                    string pergunta = $"Será cobrada uma taxa de R$ {taxaCobrada.ToString("F2")} nesta operação.\nDeseja continuar?";
                                    DialogResult resp = MessageBox.Show(pergunta, "Continuar?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (resp == DialogResult.Yes)
                                    {
                                        Conta contaDest = contaAtual.TranferirParaConta(val, numContaDest, pathContas);
                                        if (contaDest != null)
                                        {
                                            EscreverArquivosBD.GravarSaldoContaAtualizado(pathContas, contaAtual);// grava no contas.csv
                                                                                                                  // Escrever movimentação em transacoes.csv
                                            Extrato movimento = new Extrato(horaTransacao, "Transferência", contaAtual.NumeroConta, numContaDest, val, taxaCobrada, saldoAnterior, contaAtual.Saldo);
                                            EscreverArquivosBD.EscreverNovoItem(pathTransacoes, movimento.ToString());
                                            // Escrever movimentação em transacoes.csv
                                            double saldoAnteriorContaDest = contaDest.Saldo - val;// nesse ponto a transferencia já foi feita, então saldo anterior é o atual menos valor transferido
                                            EscreverArquivosBD.GravarSaldoContaAtualizado(pathContas, contaDest);
                                            //Extrato(DateTime dataTransacao, string tipoMovimento, int numContaRecebeu, int numContaPagou, double valor, double saldoAtual)
                                            Extrato movimentoContaDest = new Extrato(horaTransacao, "Transf. Recebida", numContaDest, contaAtual.NumeroConta, val, contaDest.Saldo);
                                            EscreverArquivosBD.EscreverNovoItem(pathTransacoes, movimentoContaDest.ToString());
                                            movConcluido = true;
                                        }
                                        else
                                        {
                                            tbContaDestino.Focus();
                                            lblErroMovimentacao.Text = "Conta destinatária inexistente \nou não permite receber transferência";
                                        }

                                    }
                                }
                                else
                                {
                                    tbValorMovimento.Focus();
                                    lblErroMovimentacao.Text = $"Saldo em conta insuficiente! \n Valor + taxa: R$ {(val + taxaCobrada).ToString("F2")}";
                                }
                            }
                            else
                            {
                                tbContaDestino.Focus();
                                lblErroMovimentacao.Text = "Não é possível transferir para\nmesma conta.";
                            }
                        }
                        else
                        {
                            lblErroMovimentacao.Text = "Entrada inválida";
                            tbContaDestino.Focus();
                        }
                    }
                    else
                    {
                        lblErroMovimentacao.Text = "Selecione uma operação";
                    }
                    if (movConcluido)
                    {
                        MessageBox.Show("Operação concluída!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tabCtrlTelasApp.SelectedTab = tabPageExtrato;
                        tabCtrlTelasApp.SelectedTab = tabPageMovContas;
                        //tabPageMovContas.Refresh();
                        tbContaDestino.Text = "";
                        tbValorMovimento.Text = "";
                        lblErroMovimentacao.Text = "";
                        lblSaldoAtual.Text = "R$ " + contaAtual.Saldo.ToString("F2");
                    }
                }
                else
                {
                    tbValorMovimento.Focus();
                    lblErroMovimentacao.Text = "Insira um valor maior que zero.";
                }
            }
            else
            {
                lblErroMovimentacao.Text = "Valor inválido";
                tbValorMovimento.Focus();
            }
        }

        private void btCancelarNovoCadastro_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPageLogin;
        }

        private void tabPageInvestimento_Enter(object sender, EventArgs e)
        {
            rbPerfil1.Text = PerfilInvestidor.Conservador.ToString();
            rbPerfil2.Text = PerfilInvestidor.Moderado.ToString();
            rbPerfil3.Text = PerfilInvestidor.Arrojado.ToString();
            rbPerfil1.Checked = true;

        }

        private void btInvestirAcoes_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPageAcoes;
        }

        private void btCancelarPagAcoes_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPageMovContas;
        }

        private void tabPageAcoes_Enter(object sender, EventArgs e)
        {
            lblSaldoDisponivel.Text = contaAtual.Saldo.ToString("F2");
            lblSaldoInvestido.Text = "";
            lblValorVenda.Text = "";
            lblValorCompra.Text = "";
            // inicialização tabela ações
            Random precoAcao = new Random();
            dtGridViewAcoes.Rows.Clear();
            dtGridViewAcoes.Rows.Add(false, "ABC", (10.0 * precoAcao.NextDouble()).ToString("F4"), 0);
            dtGridViewAcoes.Rows.Add(false, "DEF", (10.0 * precoAcao.NextDouble()).ToString("F4"), 0);
            dtGridViewAcoes.Rows.Add(false, "GHI", (10.0 * precoAcao.NextDouble()).ToString("F4"), 0);
            dtGridViewAcoes.Rows.Add(false, "JKL", (10.0 * precoAcao.NextDouble()).ToString("F4"), 0);
            dtGridViewAcoes.Rows.Add(false, "MNO", (10.0 * precoAcao.NextDouble()).ToString("F4"), 0);
            dtGridViewAcoes.Rows.Add(false, "PQR", (10.0 * precoAcao.NextDouble()).ToString("F4"), 0);
            dtGridViewAcoes.Rows.Add(false, "STU", (10.0 * precoAcao.NextDouble()).ToString("F4"), 0);
            dtGridViewAcoes.Rows.Add(false, "VWX", (10.0 * precoAcao.NextDouble()).ToString("F4"), 0);
            dtGridViewAcoes.Rows.Add(false, "YZA", (10.0 * precoAcao.NextDouble()).ToString("F4"), 0);
            Dictionary<string, double> dictAcoes = new Dictionary<string, double>();
            foreach (DataGridViewRow row in dtGridViewAcoes.Rows)
            {
                dictAcoes.Add(Convert.ToString(row.Cells[1].Value), Convert.ToDouble(row.Cells[2].Value));
            }
            // inicialização tabela carteira
            dtGridViewCarteira.Rows.Clear();
            string carteiraAcoes = EscreverArquivosBD.RetornarCarteiraAcoes(pathInvestimentos, contaAtual.NumeroConta);
            double saldoInvestido = 0;
            if (carteiraAcoes != "")
            {
                string[] dadosCarteira = carteiraAcoes.Split(';');
                if (dadosCarteira.Length > 1)
                {
                    Dictionary<string, Acoes> dictAcoesEmCarteira = new Dictionary<string, Acoes>();
                    for (int i = 1; i < dadosCarteira.Length; i += 3)
                    {
                        dictAcoesEmCarteira.Add(dadosCarteira[i], new Acoes(int.Parse(dadosCarteira[i + 1]), double.Parse(dadosCarteira[i + 2])));
                    }
                    foreach (KeyValuePair<string, Acoes> ac in dictAcoesEmCarteira)
                    {

                        double valorTotalAcaoAtual = dictAcoes[ac.Key] * ac.Value.Quantidade;
                        dtGridViewCarteira.Rows.Add(false,
                             ac.Key,
                             ac.Value.Valor,
                             ac.Value.Quantidade,
                             (ac.Value.Valor * ac.Value.Quantidade).ToString("F2"),
                             dictAcoes[ac.Key],
                             (valorTotalAcaoAtual).ToString("F2"),
                             0);
                        saldoInvestido += valorTotalAcaoAtual;
                    }
                }
            }
            lblSaldoInvestido.Text = saldoInvestido.ToString("F2");
            //StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<string, double> ac in dictAcoes)
            //{
            //    sb.AppendLine($"{ac.Key} : {ac.Value.ToString("F4")}"); 
            //}
            //MessageBox.Show(sb.ToString());
        }

        private void dtGridViewCarteira_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == dtGridViewCarteira.Columns["TabCarteiraQtdVender"].Index) // 1 should be your column index
            {
                int num;
                if (!int.TryParse(Convert.ToString(e.FormattedValue), out num) || num < 0)
                {
                    e.Cancel = true;
                }
                else
                {
                    int qtdDisponivel = int.Parse(dtGridViewCarteira.Rows[e.RowIndex].Cells["TabCarteiraQtdEmCart"].Value.ToString());
                    if (num > qtdDisponivel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
        private void dtGridViewCarteira_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            double totalVenda = 0;
            foreach (DataGridViewRow row in dtGridViewCarteira.Rows)
            {
                if (Convert.ToBoolean(row.Cells["TabCarteiraChk"].Value))
                {
                    totalVenda += Convert.ToDouble(row.Cells["TabCarteiraValAtual"].Value) * Convert.ToDouble(row.Cells["TabCarteiraQtdVender"].Value);
                }
            }
            lblValorVenda.Text = totalVenda.ToString("F2");
        }

        private void dtGridViewAcoes_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == dtGridViewAcoes.Columns["TabMercadoQtdComprar"].Index)
            {
                if (!int.TryParse(Convert.ToString(e.FormattedValue), out int num) || num < 0)
                {
                    e.Cancel = true;
                }
            }
        }
        private void dtGridViewAcoes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            double totalCompra = 0;
            foreach (DataGridViewRow row in dtGridViewAcoes.Rows)
            {
                //convert.toint32(datagridview1.rows[i].cells[1].value)
                if (Convert.ToBoolean(row.Cells["TabMercadoChk"].Value))
                {
                    //messagebox.show(row.cells[2].value.tostring());
                    totalCompra += Convert.ToDouble(row.Cells["TabMercadoValorAcao"].Value) * Convert.ToDouble(row.Cells["TabMercadoQtdComprar"].Value);

                }
            }
            lblValorCompra.Text = totalCompra.ToString("F2");
            if (totalCompra > contaAtual.Saldo)
            {
                lblAvisoSaldoIndisp.Visible = true;
            }
            else
            {
                lblAvisoSaldoIndisp.Visible = false;
            }
        }
        /******************************** compra e venda de ações ********************************/

        private void btComprarAcoes_Click(object sender, EventArgs e)
        {
            double valCompra = 0;
            StringBuilder mensagem = new StringBuilder();
            mensagem.AppendLine("Deseja realmente comprar as ações abaixo?\n");


            //verifica ações em carteira
            Dictionary<string, Acoes> dictAcoesEmCarteira = new Dictionary<string, Acoes>();
            for (int i = 0; i < dtGridViewCarteira.RowCount; i++)
            {
                dictAcoesEmCarteira.Add(dtGridViewCarteira.Rows[i].Cells["TabCarteiraAcoes"].Value.ToString(),
                    new Acoes(Convert.ToInt32(dtGridViewCarteira.Rows[i].Cells["TabCarteiraQtdEmCart"].Value),
                    Convert.ToDouble(dtGridViewCarteira.Rows[i].Cells["TabCarteiraValorMedPago"].Value)));
            }
            foreach (DataGridViewRow row in dtGridViewAcoes.Rows)
            {
                //convert.toint32(datagridview1.rows[i].cells[1].value)
                if (Convert.ToBoolean(row.Cells["TabMercadoChk"].Value))
                {
                    string nomeAcao = row.Cells["TabMercadoNomeAcao"].Value.ToString();
                    double valAcao = Convert.ToDouble(row.Cells["TabMercadoValorAcao"].Value);
                    int qtdComprar = Convert.ToInt32(row.Cells["TabMercadoQtdComprar"].Value);
                    //messagebox.show(row.cells[2].value.tostring());
                    valCompra += valAcao * qtdComprar;
                    if (qtdComprar > 0)
                    {
                        // Verificar se ação já existe em carteira e calcular o valor médio
                        if (dictAcoesEmCarteira.ContainsKey(nomeAcao))
                        {// calcula preço médio das ações
                            double novoValorMedio = ((dictAcoesEmCarteira[nomeAcao].Quantidade * dictAcoesEmCarteira[nomeAcao].Valor) + (qtdComprar * valAcao)) / (dictAcoesEmCarteira[nomeAcao].Quantidade + qtdComprar);
                            //sb.Append($";{nomeAcao};{qtdComprar + dictAcoesEmCarteira[nomeAcao].Quantidade};{novoValorMedio.ToString("F4")}");
                            dictAcoesEmCarteira[nomeAcao].Valor = novoValorMedio;
                            dictAcoesEmCarteira[nomeAcao].Quantidade += qtdComprar;
                        }
                        else
                        {
                            dictAcoesEmCarteira.Add(nomeAcao, new Acoes(qtdComprar, valAcao));
                            //sb.Append($";{nomeAcao};{qtdComprar};{valAcao.ToString("F4")}");
                        }
                        mensagem.AppendLine($"{nomeAcao} (qtd.: {qtdComprar}) por R$ {(valAcao * qtdComprar).ToString("F2")}");
                    }
                }
            }
            if (valCompra <= contaAtual.Saldo && valCompra > 0)
            {
                mensagem.AppendLine($"\nValor total: R$ {valCompra.ToString("F2")}");
                DialogResult confirmarCompra = MessageBox.Show(mensagem.ToString(), "Confirmar Venda", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmarCompra == DialogResult.Yes)
                {
                    DateTime horaTransacao = DateTime.UtcNow;
                    double saldoAnterior = contaAtual.Saldo;
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{contaAtual.NumeroConta}");
                    foreach (KeyValuePair<string, Acoes> ac in dictAcoesEmCarteira)
                    {
                        sb.Append($";{ac.Key};{ac.Value.Quantidade};{ac.Value.Valor.ToString("F4")}");
                    }
                    // Gravar operações nos bancos (.csv)

                    //if (contaAtual is ContaInvestimento)
                    //{
                    //    ContaInvestimento cTemp = contaAtual as ContaInvestimento;
                    //    cTemp.ComprarAcao(valCompra);
                    //}

                    ContaInvestimento cTemp = contaAtual as ContaInvestimento;
                    cTemp.ComprarAcao(valCompra);
                    EscreverArquivosBD.GravarSaldoContaAtualizado(pathContas, contaAtual);
                    Extrato movimento = new Extrato(horaTransacao, "Compra Ações", cTemp.NumeroConta, valCompra, saldoAnterior, cTemp.Saldo);
                    EscreverArquivosBD.EscreverNovoItem(pathTransacoes, movimento.ToString());
                    EscreverArquivosBD.GravarCarteiraAtualizada(pathInvestimentos, contaAtual.NumeroConta, sb.ToString());

                    tabCtrlTelasApp.SelectedTab = tabPageMovContas;
                    tabCtrlTelasApp.SelectedTab = tabPageAcoes;
                    //tabPageAcoes.Refresh();
                    MessageBox.Show("Operação concluída!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                //MessageBox.Show($"{mensagem.ToString()}\n\n{sb.ToString()} ");
                lblAvisoSaldoIndisp.Visible = false;
            }
            else
            {
                if (valCompra != 0)
                    lblAvisoSaldoIndisp.Visible = true;
            }
        }

        private void btVenderAcoes_Click(object sender, EventArgs e)
        {

            double valVenda = 0;
            StringBuilder mensagem = new StringBuilder();
            mensagem.AppendLine("Deseja realmente vender as ações abaixo?\n");
            StringBuilder sb = new StringBuilder();
            sb.Append($"{contaAtual.NumeroConta}");
            foreach (DataGridViewRow row in dtGridViewCarteira.Rows)
            {
                int qtdRestante = Convert.ToInt32(row.Cells["TabCarteiraQtdEmCart"].Value);
                string nomeAcao = row.Cells["TabCarteiraAcoes"].Value.ToString();
                double valorMedioPago = Convert.ToDouble(row.Cells["TabCarteiraValorMedPago"].Value);
                double valorParaVenda = Convert.ToDouble(row.Cells["TabCarteiraValAtual"].Value);
                if (Convert.ToBoolean(row.Cells["TabCarteiraChk"].Value))
                {
                    int qtdVendida = Convert.ToInt32(row.Cells["TabCarteiraQtdVender"].Value);
                    //messagebox.show(row.cells[2].value.tostring());
                    valVenda += valorParaVenda * qtdVendida;
                    qtdRestante -= qtdVendida;
                    if (qtdRestante > 0)
                    {
                        sb.Append($";{nomeAcao};{qtdRestante};{valorMedioPago.ToString("F4")}");
                    }
                    mensagem.AppendLine($"{nomeAcao} (qtd.: {qtdVendida}) por R$ {(valorParaVenda * qtdVendida).ToString("F2")}");
                }
                else
                {
                    if (qtdRestante > 0)
                    {
                        sb.Append($";{nomeAcao};{qtdRestante};{valorMedioPago.ToString("F4")}");
                    }
                }

            }

            if (valVenda > 0)
            {
                mensagem.AppendLine($"\nValor total: R$ {valVenda.ToString("F2")}");
                DialogResult confirmarVenda = MessageBox.Show(mensagem.ToString(), "Confirmar Venda", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmarVenda == DialogResult.Yes)
                {
                    DateTime horaTransacao = DateTime.UtcNow;
                    double saldoAnterior = contaAtual.Saldo;
                    contaAtual.Depositar(valVenda);
                    EscreverArquivosBD.GravarSaldoContaAtualizado(pathContas, contaAtual);
                    Extrato movimento = new Extrato(horaTransacao, "Venda Ações", contaAtual.NumeroConta, valVenda, saldoAnterior, contaAtual.Saldo);
                    EscreverArquivosBD.EscreverNovoItem(pathTransacoes, movimento.ToString());
                    EscreverArquivosBD.GravarCarteiraAtualizada(pathInvestimentos, contaAtual.NumeroConta, sb.ToString());
                    //GravarCarteiraAtualizada(string path, int numConta, string dadosAtualizados)
                    tabCtrlTelasApp.SelectedTab = tabPageMovContas;
                    tabCtrlTelasApp.SelectedTab = tabPageAcoes;
                    lblAvisoSelecionarParaVenda.Visible = false;
                    //tabPageAcoes.Refresh();
                    MessageBox.Show("Operação concluída!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //sb.AppendLine();
                    //sb.AppendLine($"Valor total da venda: {valVenda.ToString("F2")}");
                    //MessageBox.Show(sb.ToString());
                }
            }
            else
            {
                lblAvisoSelecionarParaVenda.Visible = true;
            }
        }
        /*****************************************************************************************/
        private void btCancelarTelaDepSal_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPageLogin;
        }

        private void btIrParaDepSalario_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabDepEmContaSalario;
            tbNumContaDepSal.Focus();
        }

        private void btDepositarSalario_Click(object sender, EventArgs e)
        {
            bool camposOk = true;
            int numContaSalDep = 0;
            double valorDeposito = 0;
            // testa preenchimento dos campos
            if (!int.TryParse(tbNumContaDepSal.Text, out numContaSalDep))
            {
                lblErroTelaDepSal.Text = "Insira um Nº válido.";
                tbNumContaDepSal.Focus();
                camposOk = false;
            }
            else if (!mtbCpfDepSalario.MaskFull)
            {
                lblErroTelaDepSal.Text = "Campo CPF incorreto.";
                mtbCpfDepSalario.Focus();
                camposOk = false;
            }
            else if (!mtbCnpjDepSalario.MaskFull)
            {
                lblErroTelaDepSal.Text = "Campo CNPJ incorreto.";
                mtbCnpjDepSalario.Focus();
                camposOk = false;
            }
            else if (!double.TryParse(tbValorSalario.Text.Replace('.', ','), out valorDeposito))
            {
                lblErroTelaDepSal.Text = "Valor inválido";
                tbValorSalario.Focus();
                camposOk = false;
            }

            if (camposOk)
            {
                if (valorDeposito > 0)
                {
                    string infoCliente = EscreverArquivosBD.RetornarDadosCliente(pathClientes, mtbCpfDepSalario.Text);
                    if (infoCliente != "")
                    {
                        ContaSalario contaSalDeposito = EscreverArquivosBD.BuscarContaSalario(pathContas, numContaSalDep);
                        if (contaSalDeposito != null)
                        {
                            if (contaSalDeposito.Holerite.Cnpj == mtbCnpjDepSalario.Text)
                            {
                                if (int.Parse(infoCliente.Split(';')[0]) == contaSalDeposito.NumeroCliente)
                                {
                                    //ok para depósito
                                    DateTime horaTransacao = DateTime.UtcNow;
                                    double saldoAnterior = contaSalDeposito.Saldo;// salvar saldo anterior para exibir no extrato
                                    contaSalDeposito.Depositar(valorDeposito);
                                    EscreverArquivosBD.GravarSaldoContaAtualizado(pathContas, contaSalDeposito);
                                    Extrato movimento = new Extrato(horaTransacao, "Depósito Salário", contaSalDeposito.NumeroConta, valorDeposito, saldoAnterior, contaSalDeposito.Saldo);
                                    EscreverArquivosBD.EscreverNovoItem(pathTransacoes, movimento.ToString());

                                    MessageBox.Show("Operação concluída!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblErroTelaDepSal.Text = "";
                                    tbNumContaDepSal.Clear();
                                    mtbCpfDepSalario.Clear();
                                    mtbCnpjDepSalario.Clear();
                                    tbValorSalario.Clear();
                                }
                                else
                                {
                                    lblErroTelaDepSal.Text = "CPF não corresponde ao cadastro\nda conta informada.";
                                    mtbCpfDepSalario.Focus();
                                }
                            }
                            else
                            {
                                lblErroTelaDepSal.Text = "CNPJ não corresponde ao cadastro\nda conta informada.";
                                mtbCnpjDepSalario.Focus();
                            }
                        }
                        else
                        {
                            lblErroTelaDepSal.Text = "Nº informado não encontrado\nou não é uma Conta Salário.";
                            tbNumContaDepSal.Focus();
                        }
                    }
                    else
                    {
                        lblErroTelaDepSal.Text = "CPF informado não encontrado\nno cadastro de clientes.";
                    }
                }
                else
                {
                    lblErroTelaDepSal.Text = "Insira um valor maior que zero.";
                    tbValorSalario.Focus();
                }
            }
        }// btDepositarSalario_Click

        private void tabCtrlTelasApp_Enter(object sender, EventArgs e)
        {
            lblErroTelaDepSal.Text = "";
        }

        private void btFecharPrograma_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void btExibirDadosCliente_Click(object sender, EventArgs e)
        {
            cliente.MostrarDadosCliente();
        }

    }
}
