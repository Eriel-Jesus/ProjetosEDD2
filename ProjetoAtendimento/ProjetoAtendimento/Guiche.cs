using System;
using System.Collections.Generic;

namespace ProjetoAtendimento
{
    public class Guiche
    {
        public int id { get; set; }
        public Queue<Senha> atendimentos { get; set; }

        public Guiche()
        {
            this.id = 0;
            this.atendimentos = new Queue<Senha>();
        }

        public Guiche(int id)
        {
            this.id = id;
            this.atendimentos = new Queue<Senha>();
        }

        public bool chamar(Queue<Senha> filaSenhas)
        {
            if (filaSenhas == null || filaSenhas.Count == 0)
            {
                return false;
            }

            // Desenfileira a próxima senha em espera
            Senha senhaAtendida = filaSenhas.Dequeue();

            // Registra a data/hora do atendimento
            DateTime agora = DateTime.Now;
            senhaAtendida.dataAtend = agora.Date;
            senhaAtendida.horaAtend = agora;

            // Enfileira no histórico do guichê
            atendimentos.Enqueue(senhaAtendida);
            return true;
        }
    }
}
