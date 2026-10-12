namespace SistemaRestaurante
{
    public class Restaurante
    {
        private int proxPedido;
        private Pedido[] pedidos;

        public Restaurante()
        {
            this.proxPedido = 1;
            this.pedidos = new Pedido[50]; // Array estático de no máximo 50 pedidos
        }

        public int getProximoId()
        {
            return proxPedido;
        }

        public bool novoPedido(Pedido pedido)
        {
            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] == null)
                {
                    pedidos[i] = pedido;
                    proxPedido++; // Incrementa o contador de ID do próximo pedido
                    return true;
                }
            }
            return false; // Capacidade máxima atingida (50 pedidos)
        }

        public Pedido buscarPedido(Pedido pedido)
        {
            if (pedido == null) return null;

            foreach (var p in pedidos)
            {
                if (p != null && p.id == pedido.id)
                {
                    return p;
                }
            }
            return null;
        }

        public bool cancelarPedido(Pedido pedido)
        {
            if (pedido == null) return false;

            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] != null && pedidos[i].id == pedido.id)
                {
                    // Shift para manter a integridade do array
                    for (int j = i; j < pedidos.Length - 1; j++)
                    {
                        pedidos[j] = pedidos[j + 1];
                    }
                    pedidos[pedidos.Length - 1] = null;
                    return true;
                }
            }
            return false;
        }

        public Pedido[] getTodosPedidos()
        {
            return pedidos;
        }

        public double calcularSomaGeral()
        {
            double soma = 0;
            foreach (var p in pedidos)
            {
                if (p != null)
                {
                    soma += p.calcularTotal();
                }
            }
            return soma;
        }
    }
}