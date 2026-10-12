using System;
using System.Collections.Generic;

namespace SistemaAgendaWinForms
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

            if (pesquisar(c) != null) return false;

            agenda.Add(c);
            return true;
        }

        public Contato pesquisar(Contato c)
        {
            if (c == null) return null;

            foreach (var item in agenda)
            {
                bool emailBate = !string.IsNullOrWhiteSpace(c.email) &&
                                 string.Equals(item.email, c.email, StringComparison.OrdinalIgnoreCase);

                bool nomeBate = !string.IsNullOrWhiteSpace(c.nome) &&
                                string.Equals(item.nome, c.nome, StringComparison.OrdinalIgnoreCase);

                if (emailBate || nomeBate)
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