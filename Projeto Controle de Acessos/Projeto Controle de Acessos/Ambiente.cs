using System.Collections.Generic;

namespace ProjetoControleAcessos
{
    public class Ambiente
    {
        public int id { get; set; }
        public string nome { get; set; }
        public Queue<Log> logs { get; set; }

        public Ambiente(int id, string nome = "")
        {
            this.id = id;
            this.nome = nome;
            this.logs = new Queue<Log>();
        }

        public void registrarLog(Log log)
        {
            // Garante que o ambiente registrará, no máximo, 100 logs
            if (logs.Count >= 100)
            {
                logs.Dequeue(); // Descarta o mais antigo
            }
            logs.Enqueue(log);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Ambiente)) return false;
            return this.id == ((Ambiente)obj).id;
        }
    }
}