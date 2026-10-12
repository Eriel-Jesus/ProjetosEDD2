using System;
using System.Collections.Generic;

namespace SistemaAgenda
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

            // Se o novo telefone for marcado como principal, desmarca os anteriores
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
            // Retorna o telefone principal, se houver
            foreach (var tel in telefones)
            {
                if (tel.principal)
                {
                    return $"({tel.tipo}) {tel.numero}";
                }
            }

            // Caso nenhum esteja marcado explicitamente como principal
            if (telefones.Count > 0)
            {
                return $"({telefones[0].tipo}) {telefones[0].numero}";
            }

            return "Sem telefone cadastrado";
        }
        //ToString para exibir informações da forma correta.
        public override string ToString()
        {
            string info = $"Nome: {nome}\n";
            info += $"E-mail: {email}\n";
            info += $"Data Nasc: {dtNasc} ({getIdade()} anos)\n";
            info += $"Tel Principal: {getTelefonePrincipal()}";
            return info;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Contato)) return false;

            Contato outro = (Contato)obj;
            // Define a chave de busca priorizando o e-mail (ou o nome caso e-mail esteja vazio)
            if (!string.IsNullOrEmpty(this.email) && !string.IsNullOrEmpty(outro.email))
            {
                return this.email.Equals(outro.email, StringComparison.OrdinalIgnoreCase);
            }
            //StringComparison foi realmente útil para ignorar diferenças de maiúsculas e minúsculas na comparação.
            return this.nome != null && this.nome.Equals(outro.nome, StringComparison.OrdinalIgnoreCase);
        }
    }
}