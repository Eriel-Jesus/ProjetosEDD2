using System;

namespace SistemaAgenda
{
    class Program
    {
        static Contatos minhaAgenda = new Contatos();

        static void Main(string[] args)
        {
            int opcao = -1;

            while (opcao != 0)
            {
                Console.WriteLine("\n=== Opções do Seletor ===");
                Console.WriteLine("0. Sair");
                Console.WriteLine("1. Adicionar contato");
                Console.WriteLine("2. Pesquisar contato");
                Console.WriteLine("3. Alterar contato");
                Console.WriteLine("4. Remover contato");
                Console.WriteLine("5. Listar contatos");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida!");
                    opcao = -1;
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                Console.WriteLine();

                switch (opcao)
                {
                    case 1:
                        AdicionarContato();
                        break;
                    case 2:
                        PesquisarContato();
                        break;
                    case 3:
                        AlterarContato();
                        break;
                    case 4:
                        RemoverContato();
                        break;
                    case 5:
                        ListarContatos();
                        break;
                    case 0:
                        Console.WriteLine("Saindo do sistema...");     
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");                
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void AdicionarContato()
        {
            Console.WriteLine("--- Adicionar Contato ---");
            Console.Write("E-mail: ");
            string email = Console.ReadLine();

            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Dia de Nasc: ");
            int dia = int.Parse(Console.ReadLine());
            Console.Write("Mês de Nasc: ");
            int mes = int.Parse(Console.ReadLine());
            Console.Write("Ano de Nasc: ");
            int ano = int.Parse(Console.ReadLine());

            Contato c = new Contato(email, nome, new Data(dia, mes, ano));

            Console.Write("Deseja adicionar um telefone? (s/n): ");
            if (Console.ReadLine().ToLower() == "s")
            {
                Console.Write("Tipo (Ex: Celular, Res): ");
                string tipo = Console.ReadLine();
                Console.Write("Número: ");
                string numero = Console.ReadLine();
                Console.Write("É o principal? (s/n): ");
                bool principal = Console.ReadLine().ToLower() == "s";

                c.adicionarTelefone(new Telefone(tipo, numero, principal));
            }

            if (minhaAgenda.adicionar(c))
            {
                Console.WriteLine("Contato adicionado com sucesso!");
            }
            else
            {
                Console.WriteLine("Erro: Contato já cadastrado (e-mail duplicado).");
            }
        }

        static void PesquisarContato()
        {
            Console.WriteLine("--- Pesquisar Contato ---");
            Console.Write("Informe o e-mail ou nome para busca: ");
            string busca = Console.ReadLine();

            Contato aux = new Contato { email = busca, nome = busca };
            Contato encontrado = minhaAgenda.pesquisar(aux);

            if (encontrado != null)
            {
                Console.WriteLine("\n[Contato Encontrado]");
                Console.WriteLine(encontrado.ToString());
            }
            else
            {
                Console.WriteLine("Contato não encontrado.");
            }
        }

        static void AlterarContato()
        {
            Console.WriteLine("--- Alterar Contato ---");
            Console.Write("Informe o e-mail ou nome do contato que deseja alterar: ");
            string busca = Console.ReadLine();

            Contato aux = new Contato { email = busca, nome = busca };
            Contato cadastrado = minhaAgenda.pesquisar(aux);

            if (cadastrado != null)
            {
                Console.WriteLine("\nDeixe em branco e pressione ENTER caso não queira alterar o campo.");

                // 1. Alterar E-mail
                Console.Write($"Novo E-mail (Atual: {cadastrado.email}): ");
                string novoEmail = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novoEmail))
                {
                    cadastrado.email = novoEmail;
                }

                // 2. Alterar Nome
                Console.Write($"Novo Nome (Atual: {cadastrado.nome}): ");
                string novoNome = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novoNome))
                {
                    cadastrado.nome = novoNome;
                }

                // 3. Alterar Data de Nascimento
                Console.Write($"Deseja alterar a Data de Nascimento? (Atual: {cadastrado.dtNasc}) (s/n): ");
                if (Console.ReadLine().ToLower() == "s")
                {
                    Console.Write("Novo Dia Nasc: ");
                    int dia = int.Parse(Console.ReadLine());
                    Console.Write("Novo Mês Nasc: ");
                    int mes = int.Parse(Console.ReadLine());
                    Console.Write("Novo Ano Nasc: ");
                    int ano = int.Parse(Console.ReadLine());

                    cadastrado.dtNasc.setData(dia, mes, ano);
                }

                // 4. Gerenciar Telefones
                Console.WriteLine($"\nTelefones cadastrados: {cadastrado.telefones.Count}");
                Console.Write("Deseja gerenciar/adicionar telefones? (s/n): ");
                if (Console.ReadLine().ToLower() == "s")
                {
                    Console.WriteLine("1. Adicionar um novo telefone");
                    Console.WriteLine("2. Limpar telefones e cadastrar um novo");
                    Console.Write("Escolha uma opção: ");
                    string opTel = Console.ReadLine();

                    if (opTel == "2")
                    {
                        cadastrado.telefones.Clear();
                    }

                    Console.Write("Tipo (Ex: Celular, Res): ");
                    string tipo = Console.ReadLine();
                    Console.Write("Número: ");
                    string numero = Console.ReadLine();
                    Console.Write("É o principal? (s/n): ");
                    bool principal = Console.ReadLine().ToLower() == "s";

                    cadastrado.adicionarTelefone(new Telefone(tipo, numero, principal));
                }

                Console.WriteLine("\nContato atualizado com sucesso!");
            }
            else
            {
                Console.WriteLine("Contato não encontrado.");
            }
        }

        static void RemoverContato()
        {
            Console.WriteLine("--- Remover Contato ---");
            Console.Write("Informe o e-mail ou nome do contato a remover: ");
            string busca = Console.ReadLine();

            Contato aux = new Contato { email = busca, nome = busca };

            if (minhaAgenda.remover(aux))
            {
                Console.WriteLine("Contato removido com sucesso!");
            }
            else
            {
                Console.WriteLine("Contato não encontrado.");
            }
        }

        static void ListarContatos()
        {
            Console.WriteLine("--- Lista de Contatos ---");
            if (minhaAgenda.Agenda.Count == 0)
            {
                Console.WriteLine("Nenhum contato cadastrado.");
                return;
            }

            foreach (var c in minhaAgenda.Agenda)
            {
                Console.WriteLine(c.ToString());
                Console.WriteLine("-------------------------");
            }
        }
    }
}