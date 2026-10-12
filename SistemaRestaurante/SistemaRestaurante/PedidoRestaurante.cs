namespace SistemaRestaurante
{
    public class Pedido
    {
        public int id { get; set; }
        public string cliente { get; set; }
        public Item[] itens { get; set; }

        public Pedido(int id, string cliente)
        {
            this.id = id;
            this.cliente = cliente;
            this.itens = new Item[10]; // Array estático de no máximo 10 itens
        }

        public bool adicionarItem(Item item)
        {
            for (int i = 0; i < itens.Length; i++)
            {
                if (itens[i] == null)
                {
                    itens[i] = item;
                    return true;
                }
            }
            return false; // Array cheio (máximo 10 itens)
        }

        public bool removerItem(Item item)
        {
            if (item == null) return false;

            for (int i = 0; i < itens.Length; i++)
            {
                if (itens[i] != null && itens[i].id == item.id)
                {
                    // Shift para a esquerda para preencher a lacuna
                    for (int j = i; j < itens.Length - 1; j++)
                    {
                        itens[j] = itens[j + 1];
                    }
                    itens[itens.Length - 1] = null; // Limpa a última posição
                    return true;
                }
            }
            return false; // Item não encontrado
        }

        public double calcularTotal()
        {
            double total = 0;
            foreach (var item in itens)
            {
                if (item != null)
                {
                    total += item.preco;
                }
            }
            return total;
        }

        public string dadosDoPedido()
        {
            string info = $"--- Pedido #{id} ---\n";
            info += $"Cliente: {cliente}\n";
            info += "Itens:\n";

            bool temItens = false;
            foreach (var item in itens)
            {
                if (item != null)
                {
                    info += $"  - [ID: {item.id}] {item.descricao} | R$ {item.preco:F2}\n";
                    temItens = true;
                }
            }

            if (!temItens)
            {
                info += "  (Nenhum item adicionado)\n";
            }

            info += $"Valor Total: R$ {calcularTotal():F2}\n";
            return info;
        }
    }
}