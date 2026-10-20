using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoAtendimento
{
    public partial class FormMain : Form
    {
        private Senhas controleSenhas;
        private Guiches controleGuiches;

        // Controles de Interface
        private Button btnGerarSenha;
        private Button btnAdicionarGuiche;
        private Button btnChamarSenha;
        private Button btnListarSenhas;
        private Button btnListarAtendimentos;

        private Label lblGuiche;
        private Label lblQtdGuiches;
        private TextBox txtGuicheId;

        private ListBox lstSenhasEmEspera;
        private ListBox lstAtendimentosGuiche;

        public FormMain()
        {
            controleSenhas = new Senhas();
            controleGuiches = new Guiches();
            ConfigurarTela();
        }

        private void ConfigurarTela()
        {
            this.Text = "Central de Atendimento";
            this.Size = new Size(820, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // 1. Botão Gerar Senha
            btnGerarSenha = new Button()
            {
                Text = "Gerar Senha",
                Location = new Point(20, 20),
                Size = new Size(160, 40)
            };
            btnGerarSenha.Click += BtnGerarSenha_Click;

            // ListBox para senhas geradas (aguardando atendimento)
            Label lblTituloEspera = new Label() { Text = "Senhas em Espera:", Location = new Point(20, 75), AutoSize = true };
            lstSenhasEmEspera = new ListBox() { Location = new Point(20, 95), Size = new Size(240, 320) };

            btnListarSenhas = new Button() { Text = "Listar Senhas", Location = new Point(20, 425), Size = new Size(240, 30) };
            btnListarSenhas.Click += (s, e) => AtualizarListaSenhas();

            // 2. Controles de Guichê e Chamada
            lblGuiche = new Label() { Text = "Guichê:", Location = new Point(300, 25), AutoSize = true };
            txtGuicheId = new TextBox() { Location = new Point(355, 22), Width = 60 };

            btnChamarSenha = new Button() { Text = "Chamar Próxima Senha", Location = new Point(430, 20), Size = new Size(160, 40) };
            btnChamarSenha.Click += BtnChamarSenha_Click;

            btnAdicionarGuiche = new Button() { Text = "Adicionar Guichê", Location = new Point(610, 20), Size = new Size(160, 40) };
            btnAdicionarGuiche.Click += BtnAdicionarGuiche_Click;

            lblQtdGuiches = new Label() { Text = "Guichês cadastrados: 0", Location = new Point(610, 65), AutoSize = true };

            // ListBox para atendimentos por guichê
            Label lblTituloAtend = new Label() { Text = "Atendimentos Realizados pelo Guichê:", Location = new Point(300, 75), AutoSize = true };
            lstAtendimentosGuiche = new ListBox() { Location = new Point(300, 95), Size = new Size(470, 320) };

            btnListarAtendimentos = new Button() { Text = "Listar Atendimentos do Guichê", Location = new Point(300, 425), Size = new Size(470, 30) };
            btnListarAtendimentos.Click += BtnListarAtendimentos_Click;

            // Adicionando controles na tela
            this.Controls.AddRange(new Control[] {
                btnGerarSenha, lblTituloEspera, lstSenhasEmEspera, btnListarSenhas,
                lblGuiche, txtGuicheId, btnChamarSenha, btnAdicionarGuiche, lblQtdGuiches,
                lblTituloAtend, lstAtendimentosGuiche, btnListarAtendimentos
            });
        }

        private void BtnGerarSenha_Click(object sender, EventArgs e)
        {
            controleSenhas.gerar();
            AtualizarListaSenhas();
        }

        private void BtnAdicionarGuiche_Click(object sender, EventArgs e)
        {
            int novoId = controleGuiches.ListaGuiches.Count + 1;
            Guiche g = new Guiche(novoId);
            controleGuiches.adicionar(g);

            lblQtdGuiches.Text = $"Guichês cadastrados: {controleGuiches.ListaGuiches.Count}";
            MessageBox.Show($"Guichê nº {novoId} cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnChamarSenha_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtGuicheId.Text, out int idGuiche))
            {
                MessageBox.Show("Informe um ID de Guichê válido no campo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Guiche guicheEncontrado = controleGuiches.ListaGuiches.Find(g => g.id == idGuiche);

            if (guicheEncontrado == null)
            {
                MessageBox.Show($"Guichê {idGuiche} não encontrado! Cadastre o guichê primeiro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (guicheEncontrado.chamar(controleSenhas.FilaSenhas))
            {
                AtualizarListaSenhas();
                AtualizarListaAtendimentos(guicheEncontrado);
                MessageBox.Show($"Senha chamada com sucesso pelo Guichê {idGuiche}!", "Atendimento", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Não há senhas na fila de espera.", "Fila Vazia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnListarAtendimentos_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtGuicheId.Text, out int idGuiche))
            {
                MessageBox.Show("Informe o ID do Guichê para consultar o histórico.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Guiche guicheEncontrado = controleGuiches.ListaGuiches.Find(g => g.id == idGuiche);

            if (guicheEncontrado != null)
            {
                AtualizarListaAtendimentos(guicheEncontrado);
            }
            else
            {
                MessageBox.Show($"Guichê {idGuiche} não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarListaSenhas()
        {
            lstSenhasEmEspera.Items.Clear();
            foreach (Senha s in controleSenhas.FilaSenhas)
            {
                lstSenhasEmEspera.Items.Add(s.dadosParciais());
            }
        }

        private void AtualizarListaAtendimentos(Guiche g)
        {
            lstAtendimentosGuiche.Items.Clear();
            foreach (Senha s in g.atendimentos)
            {
                lstAtendimentosGuiche.Items.Add(s.dadosCompletos());
            }
        }
    }
}
