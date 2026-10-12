using System.Collections.Generic;

namespace SistemaAgenda
{
    public class Contatos
    {   
        private readonly List<Contato> agenda;

        public List<Contato> Agenda => agenda;

        public Contatos()
        {
            this.agenda = new List<Contato>();
        }

        public bool adicionar(Contato c)
        {
            if (c == null) return false;

            // Evita duplicação usando o Equals sobrescrito
            if (pesquisar(c) != null) return false;

            agenda.Add(c);
            return true;
        }

        public Contato pesquisar(Contato c)
        {
            if (c == null) return null;

            foreach (var item in agenda)
            {
                // 1. Tenta comparar por Equals (se e-mail bater exato)
                if (item.Equals(c))
                {
                    return item;
                }

                // 2. Se a busca veio preenchida no campo 'nome' ou 'email', testa igualdade direta ignorando maiúsculas/minúsculas
                bool nomeBate = !string.IsNullOrEmpty(c.nome) &&
                                string.Equals(item.nome, c.nome, StringComparison.OrdinalIgnoreCase);

                bool emailBate = !string.IsNullOrEmpty(c.email) &&
                                 string.Equals(item.email, c.email, StringComparison.OrdinalIgnoreCase);

                if (nomeBate || emailBate)
                {
                    return item;
                }
            }
            return null;
        }

        public bool alterar(Contato c)
        {
            Contato cadastrado = pesquisar(c);
            if (cadastrado == null) return false;

            cadastrado.nome = c.nome;
            cadastrado.dtNasc = c.dtNasc;
            cadastrado.telefones = c.telefones;
            return true;
        }

        public bool remover(Contato c)
        {
            Contato cadastrado = pesquisar(c);
            if (cadastrado == null) return false;

            return agenda.Remove(cadastrado);
        }
    }
}