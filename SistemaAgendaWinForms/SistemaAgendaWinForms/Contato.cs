using System;
using System.Collections.Generic;

namespace SistemaAgendaWinForms
{
    public class Contato
    {
        public string email { get; set; }
        public string nome { get; set; }
        public Data dtNasc { get; set; }
        public List<Telefone> telefones { get; set; }

        public Contato()
        {
            this.dtNasc = new Data();
            this.telefones = new List<Telefone>();
        }

        public Contato(string email, string nome, Data dtNasc)
        {
            this.email = email;
            this.nome = nome;
            this.dtNasc = dtNasc;
            this.telefones = new List<Telefone>();
        }

        public int getIdade()
        {
            if (dtNasc == null) return 0;

            DateTime hoje = DateTime.Today;
            int idade = hoje.Year - dtNasc.ano;

            if (hoje.Month < dtNasc.mes || (hoje.Month == dtNasc.mes && hoje.Day < dtNasc.dia))
            {
                idade--;
            }

            return idade;
        }

        public void adicionarTelefone(Telefone t)
        {
            if (t == null) return;

            if (t.principal)
            {
                foreach (var tel in telefones)
                {
                    tel.principal = false;
                }
            }

            telefones.Add(t);
        }

        public string getTelefonePrincipal()
        {
            foreach (var tel in telefones)
            {
                if (tel.principal)
                {
                    return $"({tel.tipo}) {tel.numero}";
                }
            }

            if (telefones.Count > 0)
            {
                return $"({telefones[0].tipo}) {telefones[0].numero}";
            }

            return "Sem telefone";
        }

        public override string ToString()
        {
            return $"{nome} | {email} | Nasc: {dtNasc} ({getIdade()} anos) | Tel: {getTelefonePrincipal()}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Contato)) return false;

            Contato outro = (Contato)obj;

            bool emailIgual = !string.IsNullOrWhiteSpace(this.email) &&
                              !string.IsNullOrWhiteSpace(outro.email) &&
                              this.email.Equals(outro.email, StringComparison.OrdinalIgnoreCase);

            bool nomeIgual = !string.IsNullOrWhiteSpace(this.nome) &&
                             !string.IsNullOrWhiteSpace(outro.nome) &&
                             this.nome.Equals(outro.nome, StringComparison.OrdinalIgnoreCase);

            return emailIgual || nomeIgual;
        }
    }
}