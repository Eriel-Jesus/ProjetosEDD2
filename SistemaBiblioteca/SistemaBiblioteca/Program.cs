using System;

namespace SistemaBiblioteca
{
    class Program
    {
        static Livros acervoGeral = new Livros();

        static void Main(string[] args)
        {
            int opcao = -1;

            while (opcao != 0)
            {
                Console.WriteLine("\n=== Opções do Seletor ===");
                Console.WriteLine("0. Sair");
                Console.WriteLine("1. Adicionar livro");
                Console.WriteLine("2. Pesquisar livro (sintético)");
                Console.WriteLine("3. Pesquisar livro (analítico)");
                Console.WriteLine("4. Adicionar exemplar");
                Console.WriteLine("5. Registrar empréstimo");
                Console.WriteLine("6. Registrar devolução");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida!");
                    continue;
                    Console.ReadKey();
                    Console.Clear();
                }

                Console.WriteLine();

                switch (opcao)
                {
                    case 1:
                        AdicionarLivro();
                        break;
                    case 2:
                        PesquisarLivroSintetico();
                        break;
                    case 3:
                        PesquisarLivroAnalitico();
                        break;
                    case 4:
                        AdicionarExemplar();
                        break;
                    case 5:
                        RegistrarEmprestimo();
                        break;
                    case 6:
                        RegistrarDevolucao();
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

        static void AdicionarLivro()
        {
            Console.WriteLine("--- Adicionar Livro ---");
            Console.Write("ISBN: ");
            int isbn = int.Parse(Console.ReadLine());

            Console.Write("Título: ");
            string titulo = Console.ReadLine();

            Console.Write("Autor: ");
            string autor = Console.ReadLine();

            Console.Write("Editora: ");
            string editora = Console.ReadLine();

            Livro novoLivro = new Livro(isbn, titulo, autor, editora);

            if (acervoGeral.pesquisar(novoLivro) == null)
            {
                acervoGeral.adicionar(novoLivro);
                Console.WriteLine("Livro cadastrado com sucesso!");
            }
            else
            {
                Console.WriteLine("Erro: Livro com este ISBN já está cadastrado.");
            }
        }

        static void PesquisarLivroSintetico()
        {
            Console.WriteLine("--- Pesquisar Livro (Sintético) ---");
            Console.Write("Informe o ISBN do livro: ");
            int isbn = int.Parse(Console.ReadLine());

            Livro encontrado = acervoGeral.pesquisar(new Livro { isbn = isbn });

            if (encontrado != null)
            {
                Console.WriteLine($"\n[Livro ISBN: {encontrado.isbn}]");
                Console.WriteLine($"Título: {encontrado.titulo}");
                Console.WriteLine($"Autor: {encontrado.autor} | Editora: {encontrado.editora}");
                Console.WriteLine($"Total de Exemplares: {encontrado.qtdeExemplares()}");
                Console.WriteLine($"Exemplares Disponíveis: {encontrado.qtdeDisponiveis()}");
                Console.WriteLine($"Total de Empréstimos: {encontrado.qtdeEmprestimos()}");
                Console.WriteLine($"Percentual de Disponibilidade: {encontrado.percDisponibilidade():F2}%");
            }
            else
            {
                Console.WriteLine("Livro não encontrado no acervo.");
            }
        }

        static void PesquisarLivroAnalitico()
        {
            Console.WriteLine("--- Pesquisar Livro (Analítico) ---");
            Console.Write("Informe o ISBN do livro: ");
            int isbn = int.Parse(Console.ReadLine());

            Livro encontrado = acervoGeral.pesquisar(new Livro { isbn = isbn });

            if (encontrado != null)
            {
                // Exibe primeiramente os dados sintéticos
                Console.WriteLine($"\n========================================");
                Console.WriteLine($"[LIVRO ISBN: {encontrado.isbn}] {encontrado.titulo}");
                Console.WriteLine($"Autor: {encontrado.autor} | Editora: {encontrado.editora}");
                Console.WriteLine($"Total Exemplares: {encontrado.qtdeExemplares()} | Disponíveis: {encontrado.qtdeDisponiveis()} | Empréstimos Totais: {encontrado.qtdeEmprestimos()}");
                Console.WriteLine($"Percentual de Disponibilidade: {encontrado.percDisponibilidade():F2}%");
                Console.WriteLine($"----------------------------------------");
                Console.WriteLine("DETALHES DOS EXEMPLARES:");

                if (encontrado.exemplares.Count == 0)
                {
                    Console.WriteLine("Nenhum exemplar cadastrado para este livro.");
                }

                foreach (var ex in encontrado.exemplares)
                {
                    string status = ex.disponivel() ? "Disponível" : "Emprestado";
                    Console.WriteLine($"\n  -> Exemplar Tombo: {ex.tombo} | Status: {status} | Total Empréstimos: {ex.qtdeEmprestimos()}");

                    if (ex.emprestimos.Count > 0)
                    {
                        Console.WriteLine("     Histórico de Empréstimos:");
                        foreach (var emp in ex.emprestimos)
                        {
                            string dataDev = emp.dtDevolucao == DateTime.MinValue ? "Em aberto" : emp.dtDevolucao.ToString("dd/MM/yyyy HH:mm");
                            Console.WriteLine($"       - Empréstimo: {emp.dtEmprestimo:dd/MM/yyyy HH:mm} | Devolução: {dataDev}");
                        }
                    }
                }
                Console.WriteLine($"========================================");
            }
            else
            {
                Console.WriteLine("Livro não encontrado no acervo.");
            }
        }

        static void AdicionarExemplar()
        {
            Console.WriteLine("--- Adicionar Exemplar ---");
            Console.Write("Informe o ISBN do livro: ");
            int isbn = int.Parse(Console.ReadLine());

            Livro livro = acervoGeral.pesquisar(new Livro { isbn = isbn });

            if (livro != null)
            {
                Console.Write("Informe o número de Tombo do exemplar: ");
                int tombo = int.Parse(Console.ReadLine());

                livro.adicionarExemplar(new Exemplar(tombo));
                Console.WriteLine("Exemplar adicionado com sucesso ao livro!");
            }
            else
            {
                Console.WriteLine("Livro não encontrado.");
            }
        }

        static void RegistrarEmprestimo()
        {
            Console.WriteLine("--- Registrar Empréstimo ---");
            Console.Write("Informe o ISBN do livro: ");
            int isbn = int.Parse(Console.ReadLine());

            Livro livro = acervoGeral.pesquisar(new Livro { isbn = isbn });

            if (livro != null)
            {
                Console.Write("Informe o número de Tombo do exemplar: ");
                int tombo = int.Parse(Console.ReadLine());

                Exemplar exemplarEncontrado = null;
                foreach (var ex in livro.exemplares)
                {
                    if (ex.tombo == tombo)
                    {
                        exemplarEncontrado = ex;
                        break;
                    }
                }

                if (exemplarEncontrado != null)
                {
                    if (exemplarEncontrado.emprestar())
                    {
                        Console.WriteLine("Empréstimo registrado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Erro: Este exemplar já está emprestado no momento.");
                    }
                }
                else
                {
                    Console.WriteLine("Exemplar com este tombo não pertence a este livro.");
                }
            }
            else
            {
                Console.WriteLine("Livro não encontrado.");
            }
        }

        static void RegistrarDevolucao()
        {
            Console.WriteLine("--- Registrar Devolução ---");
            Console.Write("Informe o ISBN do livro: ");
            int isbn = int.Parse(Console.ReadLine());

            Livro livro = acervoGeral.pesquisar(new Livro { isbn = isbn });

            if (livro != null)
            {
                Console.Write("Informe o número de Tombo do exemplar: ");
                int tombo = int.Parse(Console.ReadLine());

                Exemplar exemplarEncontrado = null;
                foreach (var ex in livro.exemplares)
                {
                    if (ex.tombo == tombo)
                    {
                        exemplarEncontrado = ex;
                        break;
                    }
                }

                if (exemplarEncontrado != null)
                {
                    if (exemplarEncontrado.devolver())
                    {
                        Console.WriteLine("Devolução registrada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Erro: Este exemplar não consta como emprestado no momento.");
                    }
                }
                else
                {
                    Console.WriteLine("Exemplar com este tombo não pertence a este livro.");
                }
            }
            else
            {
                Console.WriteLine("Livro não encontrado.");
            }
        }
    }
}