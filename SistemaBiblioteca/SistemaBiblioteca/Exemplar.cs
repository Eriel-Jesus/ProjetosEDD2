using System;
using System.Collections.Generic;

namespace SistemaBiblioteca
{
    public class Exemplar
    {
        public int tombo { get; set; }
        public List<Emprestimo> emprestimos { get; set; }

        public Exemplar()
        {
            this.emprestimos = new List<Emprestimo>();
        }

        public Exemplar(int tombo)
        {
            this.tombo = tombo;
            this.emprestimos = new List<Emprestimo>();
        }

        public bool disponivel()
        {
            if (emprestimos.Count == 0) return true;

            // O exemplar está disponível se o último empréstimo registrado tiver uma data de devolução válida
            Emprestimo ultimoEmprestimo = emprestimos[emprestimos.Count - 1];
            return ultimoEmprestimo.dtDevolucao != DateTime.MinValue;
        }

        public bool emprestar()
        {
            if (!disponivel()) return false;

            emprestimos.Add(new Emprestimo(DateTime.Now));
            return true;
        }

        public bool devolver()
        {
            if (disponivel()) return false;

            Emprestimo ultimoEmprestimo = emprestimos[emprestimos.Count - 1];
            ultimoEmprestimo.dtDevolucao = DateTime.Now;
            return true;
        }

        public int qtdeEmprestimos()
        {
            return emprestimos.Count;
        }
    }
}