using System;
using System.Linq;

namespace ProjetoControleAcessos
{
    class Program
    {
        static Cadastro sistema = new Cadastro();

        static void Main(string[] args)
        {
            sistema.download(); // Carrega os dados persistidos

            int opcao = -1;
            while (opcao != 0)
            {
                Console.WriteLine("\n=== Controle de Acessos ===");
                Console.WriteLine("0. Sair");
                Console.WriteLine("1. Cadastrar ambiente");
                Console.WriteLine("2. Consultar ambiente");
                Console.WriteLine("3. Excluir ambiente");
                Console.WriteLine("4. Cadastrar usuário");
                Console.WriteLine("5. Consultar usuário");
                Console.WriteLine("6. Excluir usuário");
                Console.WriteLine("7. Conceder permissão de acesso ao usuário");
                Console.WriteLine("8. Revogar permissão de acesso ao usuário");
                Console.WriteLine("9. Registrar acesso");
                Console.WriteLine("10. Consultar logs de acesso");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao)) continue;
                Console.WriteLine();

                switch (opcao)
                {
                    case 1: CadastrarAmbiente(); break;
                    case 2: ConsultarAmbiente(); break;
                    case 3: ExcluirAmbiente(); break;
                    case 4: CadastrarUsuario(); break;
                    case 5: ConsultarUsuario(); break;
                    case 6: ExcluirUsuario(); break;
                    case 7: ConcederPermissao(); break;
                    case 8: RevogarPermissao(); break;
                    case 9: RegistrarAcesso(); break;
                    case 10: ConsultarLogs(); break;
                    case 0:
                        sistema.upload(); // Salva os dados antes de fechar
                        Console.WriteLine("Dados salvos. Encerrando...");
                        break;
                    default: Console.WriteLine("Opção inválida!"); break;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void CadastrarAmbiente()
        {
            Console.Write("ID do Ambiente: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Nome do Ambiente: ");
            string nome = Console.ReadLine();

            if (sistema.pesquisarAmbiente(new Ambiente(id)) == null)
            {
                sistema.adicionarAmbiente(new Ambiente(id, nome));
                Console.WriteLine("Ambiente cadastrado com sucesso.");
            }
            else Console.WriteLine("Erro: Ambiente já existe.");
        }

        static void ConsultarAmbiente()
        {
            Console.Write("ID do Ambiente: ");
            Ambiente a = sistema.pesquisarAmbiente(new Ambiente(int.Parse(Console.ReadLine())));
            if (a != null) Console.WriteLine($"ID: {a.id} | Nome: {a.nome} | Logs registrados: {a.logs.Count}");
            else Console.WriteLine("Ambiente não encontrado.");
        }

        static void ExcluirAmbiente()
        {
            Console.Write("ID do Ambiente a excluir: ");
            if (sistema.removerAmbiente(new Ambiente(int.Parse(Console.ReadLine()))))
                Console.WriteLine("Ambiente excluído com sucesso.");
            else
                Console.WriteLine("Falha ao excluir. Ambiente não encontrado.");
        }

        static void CadastrarUsuario()
        {
            Console.Write("ID do Usuário: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Nome do Usuário: ");
            string nome = Console.ReadLine();

            if (sistema.pesquisarUsuario(new Usuario(id)) == null)
            {
                sistema.adicionarUsuario(new Usuario(id, nome));
                Console.WriteLine("Usuário cadastrado com sucesso.");
            }
            else Console.WriteLine("Erro: Usuário já existe.");
        }

        static void ConsultarUsuario()
        {
            Console.Write("ID do Usuário: ");
            Usuario u = sistema.pesquisarUsuario(new Usuario(int.Parse(Console.ReadLine())));
            if (u != null)
            {
                Console.WriteLine($"ID: {u.id} | Nome: {u.nome}");
                Console.WriteLine("Ambientes com permissão:");
                foreach (var amb in u.ambientes) Console.WriteLine($" - {amb.nome} (ID: {amb.id})");
                if (u.ambientes.Count == 0) Console.WriteLine(" - Nenhuma permissão");
            }
            else Console.WriteLine("Usuário não encontrado.");
        }

        static void ExcluirUsuario()
        {
            Console.Write("ID do Usuário a excluir: ");
            bool sucesso = sistema.removerUsuario(new Usuario(int.Parse(Console.ReadLine())));
            if (sucesso) Console.WriteLine("Usuário excluído com sucesso.");
            else Console.WriteLine("Falha ao excluir. O usuário não existe ou ainda possui permissões ativas.");
        }

        static void ConcederPermissao()
        {
            Console.Write("ID do Usuário: ");
            Usuario u = sistema.pesquisarUsuario(new Usuario(int.Parse(Console.ReadLine())));
            Console.Write("ID do Ambiente: ");
            Ambiente a = sistema.pesquisarAmbiente(new Ambiente(int.Parse(Console.ReadLine())));

            if (u != null && a != null)
            {
                if (u.concederPermissao(a)) Console.WriteLine("Permissão concedida com sucesso.");
                else Console.WriteLine("O usuário já tem permissão para este ambiente.");
            }
            else Console.WriteLine("Usuário ou Ambiente não encontrados.");
        }

        static void RevogarPermissao()
        {
            Console.Write("ID do Usuário: ");
            Usuario u = sistema.pesquisarUsuario(new Usuario(int.Parse(Console.ReadLine())));
            Console.Write("ID do Ambiente: ");
            Ambiente a = sistema.pesquisarAmbiente(new Ambiente(int.Parse(Console.ReadLine())));

            if (u != null && a != null)
            {
                if (u.revogarPermissao(a)) Console.WriteLine("Permissão revogada com sucesso.");
                else Console.WriteLine("O usuário não possuía permissão para este ambiente.");
            }
            else Console.WriteLine("Usuário ou Ambiente não encontrados.");
        }

        static void RegistrarAcesso()
        {
            Console.Write("ID do Ambiente: ");
            Ambiente a = sistema.pesquisarAmbiente(new Ambiente(int.Parse(Console.ReadLine())));
            Console.Write("ID do Usuário tentando acessar: ");
            Usuario u = sistema.pesquisarUsuario(new Usuario(int.Parse(Console.ReadLine())));

            if (a != null && u != null)
            {
                bool autorizado = u.ambientes.Contains(a); // Verifica permissão
                Log novoLog = new Log(DateTime.Now, u, autorizado);
                a.registrarLog(novoLog);

                if (autorizado) Console.WriteLine("ACESSO AUTORIZADO. Log registrado.");
                else Console.WriteLine("ACESSO NEGADO! Usuário sem permissão. Log registrado.");
            }
            else Console.WriteLine("Usuário ou Ambiente não encontrados.");
        }

        static void ConsultarLogs()
        {
            Console.Write("ID do Ambiente: ");
            Ambiente a = sistema.pesquisarAmbiente(new Ambiente(int.Parse(Console.ReadLine())));

            if (a != null)
            {
                Console.WriteLine("Filtrar logs por (A = Autorizados / N = Negados / T = Todos): ");
                string filtro = Console.ReadLine().ToUpper();

                var logsFiltrados = a.logs.AsEnumerable();

                if (filtro == "A") logsFiltrados = logsFiltrados.Where(l => l.tipoAcesso == true);
                else if (filtro == "N") logsFiltrados = logsFiltrados.Where(l => l.tipoAcesso == false);

                Console.WriteLine($"\n--- Logs do Ambiente {a.nome} ---");
                if (!logsFiltrados.Any()) Console.WriteLine("Nenhum log corresponde ao filtro.");
                
                foreach (var log in logsFiltrados)
                {
                    Console.WriteLine(log.ToString());
                }
            }
            else Console.WriteLine("Ambiente não encontrado.");
        }
    }
}