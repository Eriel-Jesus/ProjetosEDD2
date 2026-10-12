using System;

namespace SistemaRestaurante
{
    class Program
    {
        static void Main(string[] args)
        {
            Restaurante restaurante = new Restaurante();
            int opcao = -1;

            while (opcao != 0)
            {
                Console.WriteLine("\n=== Opções do Seletor ===");
                Console.WriteLine("0. Sair");
                Console.WriteLine("1. Criar novo pedido");
                Console.WriteLine("2. Adicionar item ao pedido");
                Console.WriteLine("3. Remover item do pedido");
                Console.WriteLine("4. Consultar pedido (mostrar id, cliente, itens e valor total)");
                Console.WriteLine("5. Cancelar pedido");
                Console.WriteLine("6. Listar todos os pedidos (com id, valores totais e soma geral do dia)");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida! Digite um número.");
                    continue;
                }

                Console.WriteLine();

                switch (opcao)
                {
                    case 1: // Criar novo pedido
                        Console.Write("Informe o nome do cliente: ");
                        string nomeCliente = Console.ReadLine();
                        
                        int novoId = restaurante.getProximoId();
                        Pedido novoPed = new Pedido(novoId, nomeCliente);

                        if (restaurante.novoPedido(novoPed))
                        {
                            Console.WriteLine($"Pedido #{novoId} criado com sucesso para {nomeCliente}!");
                        }
                        else
                        {
                            Console.WriteLine("Erro: Limite de 50 pedidos atingido para o dia.");
                        }
                        break;

                    case 2: // Adicionar item ao pedido
                        Console.Write("Informe o ID do pedido: ");
                        if (int.TryParse(Console.ReadLine(), out int idPedAdd))
                        {
                            Pedido pAddAux = new Pedido(idPedAdd, "");
                            Pedido pEncontrado = restaurante.buscarPedido(pAddAux);

                            if (pEncontrado != null)
                            {
                                Console.Write("ID do Item: ");
                                int idItem = int.Parse(Console.ReadLine());
                                Console.Write("Descrição do Item: ");
                                string descItem = Console.ReadLine();
                                Console.Write("Preço do Item: ");
                                double precoItem = double.Parse(Console.ReadLine());

                                Item novoItem = new Item(idItem, descItem, precoItem);

                                if (pEncontrado.adicionarItem(novoItem))
                                {
                                    Console.WriteLine("Item adicionado ao pedido com sucesso!");
                                }
                                else
                                {
                                    Console.WriteLine("Erro: O pedido já possui o limite de 10 itens.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Pedido não encontrado.");
                            }
                        }
                        break;

                    case 3: // Remover item do pedido
                        Console.Write("Informe o ID do pedido: ");
                        if (int.TryParse(Console.ReadLine(), out int idPedRem))
                        {
                            Pedido pRemAux = new Pedido(idPedRem, "");
                            Pedido pEncontrado = restaurante.buscarPedido(pRemAux);

                            if (pEncontrado != null)
                            {
                                Console.Write("Informe o ID do item a remover: ");
                                int idItemRem = int.Parse(Console.ReadLine());

                                Item itemAux = new Item(idItemRem, "", 0);

                                if (pEncontrado.removerItem(itemAux))
                                {
                                    Console.WriteLine("Item removido com sucesso!");
                                }
                                else
                                {
                                    Console.WriteLine("Item não encontrado no pedido.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Pedido não encontrado.");
                            }
                        }
                        break;

                    case 4: // Consultar pedido
                        Console.Write("Informe o ID do pedido: ");
                        if (int.TryParse(Console.ReadLine(), out int idPedCons))
                        {
                            Pedido pConsAux = new Pedido(idPedCons, "");
                            Pedido pEncontrado = restaurante.buscarPedido(pConsAux);

                            if (pEncontrado != null)
                            {
                                Console.WriteLine(pEncontrado.dadosDoPedido());
                            }
                            else
                            {
                                Console.WriteLine("Pedido não encontrado.");
                            }
                        }
                        break;

                    case 5: // Cancelar pedido
                        Console.Write("Informe o ID do pedido a cancelar: ");
                        if (int.TryParse(Console.ReadLine(), out int idPedCanc))
                        {
                            Pedido pCancAux = new Pedido(idPedCanc, "");

                            if (restaurante.cancelarPedido(pCancAux))
                            {
                                Console.WriteLine($"Pedido #{idPedCanc} cancelado e removido do sistema.");
                            }
                            else
                            {
                                Console.WriteLine("Pedido não encontrado.");
                            }
                        }
                        break;

                    case 6: // Listar todos os pedidos
                        Console.WriteLine("=== HISTÓRICO DE PEDIDOS DO DIA ===");
                        Pedido[] todos = restaurante.getTodosPedidos();
                        bool algumPedido = false;

                        foreach (var p in todos)
                        {
                            if (p != null)
                            {
                                Console.WriteLine($"ID: {p.id} | Cliente: {p.cliente} | Total: R$ {p.calcularTotal():F2}");
                                algumPedido = true;
                            }
                        }

                        if (!algumPedido)
                        {
                            Console.WriteLine("Nenhum pedido cadastrado no momento.");
                        }

                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine($"SOMA GERAL DO DIA: R$ {restaurante.calcularSomaGeral():F2}");
                        break;

                    case 0:
                        Console.WriteLine("Saindo do sistema...");
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}