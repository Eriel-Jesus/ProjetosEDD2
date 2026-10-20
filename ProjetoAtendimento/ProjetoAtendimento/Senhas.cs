using System;
using System.Collections.Generic;

namespace ProjetoAtendimento
{
    public class Senhas
    {
        private int proximoAtendimento;
        private Queue<Senha> filaSenhas;

        public Queue<Senha> FilaSenhas => filaSenhas;
        public int ProximoAtendimento => proximoAtendimento;

        public Senhas()
        {
            this.filaSenhas = new Queue<Senha>();
            this.proximoAtendimento = 1;
        }

        public void gerar()
        {
            Senha novaSenha = new Senha(proximoAtendimento);
            filaSenhas.Enqueue(novaSenha);
            proximoAtendimento++;
        }
    }
}
