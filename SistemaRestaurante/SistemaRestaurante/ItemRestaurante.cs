namespace SistemaRestaurante
{
    public class Item
    {
        public int id { get; set; }
        public string descricao { get; set; }
        public double preco { get; set; }

        public Item(int id, string descricao, double preco)
        {
            this.id = id;
            this.descricao = descricao;
            this.preco = preco;
        }
    }
}