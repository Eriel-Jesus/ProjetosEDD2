using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SistemaAgendaWinForms
{
    public partial class FormMain : Form
    {
        private Contatos minhaAgenda;
        private Contato contatoAtualEncontrado;

        // Componentes WinForms declarados
        private TextBox txtBusca, txtEmail, txtNome, txtTelTipo, txtTelNumero;
        private DateTimePicker dtpDataNasc;
        private CheckBox chkTelPrincipal;
        private Button btnPesquisar, btnAdicionar, btnAlterar, btnRemover, btnLimpar, btnSair;
        private DataGridView dgvContatos;

        public FormMain()
        {
            minhaAgenda = new Contatos();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Gerenciador de Agenda de Contatos";
            this.Size = new System.Drawing.Size(850, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Painel de Pesquisa
            GroupBox gbPesquisa = new GroupBox() { Text = "2. Pesquisar Contato", Location = new System.Drawing.Point(15, 12), Size = new System.Drawing.Size(800, 60) };
            Label lblBusca = new Label() { Text = "Nome ou E-mail:", Location = new System.Drawing.Point(15, 25), AutoSize = true };
            txtBusca = new TextBox() { Location = new System.Drawing.Point(120, 22), Width = 450 };
            btnPesquisar = new Button() { Text = "Pesquisar", Location = new System.Drawing.Point(580, 20), Width = 100 };
            btnPesquisar.Click += BtnPesquisar_Click;

            gbPesquisa.Controls.Add(lblBusca);
            gbPesquisa.Controls.Add(txtBusca);
            gbPesquisa.Controls.Add(btnPesquisar);

            // Painel de Cadastro e Edição
            GroupBox gbDados = new GroupBox() { Text = "Dados do Contato (Adicionar / Alterar)", Location = new System.Drawing.Point(15, 80), Size = new System.Drawing.Size(800, 180) };

            Label lblEmail = new Label() { Text = "E-mail:", Location = new System.Drawing.Point(15, 30), AutoSize = true };
            txtEmail = new TextBox() { Location = new System.Drawing.Point(80, 27), Width = 300 };

            Label lblNome = new Label() { Text = "Nome:", Location = new System.Drawing.Point(400, 30), AutoSize = true };
            txtNome = new TextBox() { Location = new System.Drawing.Point(460, 27), Width = 320 };

            Label lblData = new Label() { Text = "Data Nasc (DD/MM/AAAA):", Location = new System.Drawing.Point(15, 70), AutoSize = true };
            dtpDataNasc = new DateTimePicker() { Location = new System.Drawing.Point(160, 67), Format = DateTimePickerFormat.Short, Width = 120 };

            // Dados de Telefone
            Label lblTelTipo = new Label() { Text = "Tipo Tel:", Location = new System.Drawing.Point(300, 70), AutoSize = true };
            txtTelTipo = new TextBox() { Location = new System.Drawing.Point(360, 67), Width = 100 };

            Label lblTelNum = new Label() { Text = "Número:", Location = new System.Drawing.Point(470, 70), AutoSize = true };
            txtTelNumero = new TextBox() { Location = new System.Drawing.Point(530, 67), Width = 140 };

            chkTelPrincipal = new CheckBox() { Text = "Principal", Location = new System.Drawing.Point(680, 69), AutoSize = true, Checked = true };

            // Botões de Ação do Seletor
            btnAdicionar = new Button() { Text = "1. Adicionar", Location = new System.Drawing.Point(15, 125), Width = 130, Height = 35 };
            btnAlterar = new Button() { Text = "3. Alterar", Location = new System.Drawing.Point(160, 125), Width = 130, Height = 35 };
            btnRemover = new Button() { Text = "4. Remover", Location = new System.Drawing.Point(305, 125), Width = 130, Height = 35 };
            btnLimpar = new Button() { Text = "Limpar Campos", Location = new System.Drawing.Point(450, 125), Width = 130, Height = 35 };
            btnSair = new Button() { Text = "0. Sair", Location = new System.Drawing.Point(650, 125), Width = 130, Height = 35 };

            btnAdicionar.Click += BtnAdicionar_Click;
            btnAlterar.Click += BtnAlterar_Click;
            btnRemover.Click += BtnRemover_Click;
            btnLimpar.Click += (s, e) => LimparCampos();
            btnSair.Click += (s, e) => Application.Exit();

            gbDados.Controls.AddRange(new Control[] {
                lblEmail, txtEmail, lblNome, txtNome, lblData, dtpDataNasc,
                lblTelTipo, txtTelTipo, lblTelNum, txtTelNumero, chkTelPrincipal,
                btnAdicionar, btnAlterar, btnRemover, btnLimpar, btnSair
            });

            // Painel da Lista de Contatos
            GroupBox gbLista = new GroupBox() { Text = "5. Listar Contatos", Location = new System.Drawing.Point(15, 270), Size = new System.Drawing.Size(800, 270) };
            dgvContatos = new DataGridView()
            {
                Location = new System.Drawing.Point(15, 25),
                Size = new System.Drawing.Size(770, 230),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvContatos.SelectionChanged += DgvContatos_SelectionChanged;

            gbLista.Controls.Add(dgvContatos);

            this.Controls.Add(gbPesquisa);
            this.Controls.Add(gbDados);
            this.Controls.Add(gbLista);

            AtualizarGrid();
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Preencha ao menos o Nome e E-mail.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime dt = dtpDataNasc.Value;
            Data dataNasc = new Data(dt.Day, dt.Month, dt.Year);

            Contato novo = new Contato(txtEmail.Text.Trim(), txtNome.Text.Trim(), dataNasc);

            if (!string.IsNullOrWhiteSpace(txtTelNumero.Text))
            {
                novo.adicionarTelefone(new Telefone(txtTelTipo.Text.Trim(), txtTelNumero.Text.Trim(), chkTelPrincipal.Checked));
            }

            if (minhaAgenda.adicionar(novo))
            {
                MessageBox.Show("Contato adicionado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                AtualizarGrid();
            }
            else
            {
                MessageBox.Show("Não foi possível adicionar. E-mail ou Nome já cadastrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPesquisar_Click(object sender, EventArgs e)
        {
            string termo = txtBusca.Text.Trim();
            if (string.IsNullOrWhiteSpace(termo))
            {
                MessageBox.Show("Informe um e-mail ou nome para pesquisar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Contato buscaAux = new Contato { email = termo, nome = termo };
            Contato encontrado = minhaAgenda.pesquisar(buscaAux);

            if (encontrado != null)
            {
                CarregarContatoNosCampos(encontrado);
                MessageBox.Show("Contato encontrado e carregado nos campos!", "Pesquisa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Contato não encontrado.", "Pesquisa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            if (contatoAtualEncontrado == null)
            {
                MessageBox.Show("Selecione ou pesquise um contato primeiro para alterar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime dt = dtpDataNasc.Value;
            contatoAtualEncontrado.nome = txtNome.Text.Trim();
            contatoAtualEncontrado.email = txtEmail.Text.Trim();
            contatoAtualEncontrado.dtNasc.setData(dt.Day, dt.Month, dt.Year);

            if (!string.IsNullOrWhiteSpace(txtTelNumero.Text))
            {
                contatoAtualEncontrado.adicionarTelefone(new Telefone(txtTelTipo.Text.Trim(), txtTelNumero.Text.Trim(), chkTelPrincipal.Checked));
            }

            MessageBox.Show("Contato alterado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparCampos();
            AtualizarGrid();
        }

        private void BtnRemover_Click(object sender, EventArgs e)
        {
            if (contatoAtualEncontrado == null)
            {
                MessageBox.Show("Selecione ou pesquise um contato para remover.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Deseja realmente remover o contato '{contatoAtualEncontrado.nome}'?", "Confirmar Remoção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                minhaAgenda.remover(contatoAtualEncontrado);
                MessageBox.Show("Contato removido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                AtualizarGrid();
            }
        }

        private void CarregarContatoNosCampos(Contato c)
        {
            contatoAtualEncontrado = c;
            txtEmail.Text = c.email;
            txtNome.Text = c.nome;

            if (c.dtNasc != null && c.dtNasc.ano > 1900)
            {
                dtpDataNasc.Value = new DateTime(c.dtNasc.ano, c.dtNasc.mes, c.dtNasc.dia);
            }

            if (c.telefones.Count > 0)
            {
                var tel = c.telefones[0];
                txtTelTipo.Text = tel.tipo;
                txtTelNumero.Text = tel.numero;
                chkTelPrincipal.Checked = tel.principal;
            }
            else
            {
                txtTelTipo.Text = "";
                txtTelNumero.Text = "";
                chkTelPrincipal.Checked = true;
            }
        }

        private void LimparCampos()
        {
            contatoAtualEncontrado = null;
            txtBusca.Text = "";
            txtEmail.Text = "";
            txtNome.Text = "";
            txtTelTipo.Text = "";
            txtTelNumero.Text = "";
            dtpDataNasc.Value = DateTime.Today;
            chkTelPrincipal.Checked = true;
        }

        private void AtualizarGrid()
        {
            var bindingList = minhaAgenda.Agenda.Select(c => new
            {
                Nome = c.nome,
                Email = c.email,
                Nascimento = c.dtNasc.ToString(),
                Idade = c.getIdade(),
                TelefonePrincipal = c.getTelefonePrincipal()
            }).ToList();

            dgvContatos.DataSource = null;
            dgvContatos.DataSource = bindingList;
        }

        private void DgvContatos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvContatos.SelectedRows.Count > 0)
            {
                int index = dgvContatos.SelectedRows[0].Index;
                if (index >= 0 && index < minhaAgenda.Agenda.Count)
                {
                    CarregarContatoNosCampos(minhaAgenda.Agenda[index]);
                }
            }
        }
    }
}