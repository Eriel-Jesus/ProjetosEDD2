using System;
using System.Collections.Generic;

namespace SistemaBiblioteca
{
    public class Livro
    {
        public int isbn { get; set; }
        public string titulo { get; set; }
        public string autor { get; set; }
        public string editora { get; set; }
        public List<Exemplar> exemplares { get; set; }

        public Livro()
        {
            this.exemplares = new List<Exemplar>();
        }

        public Livro(int isbn, string titulo, string autor, string editora)
        {
            this.isbn = isbn;
            this.titulo = titulo;
            this.autor = autor;
            this.editora = editora;
            this.exemplares = new List<Exemplar>();
        }

        public void adicionarExemplar(Exemplar exemplar)
        {
            if (exemplar != null)
            {
                exemplares.Add(exemplar);
            }
        }

        public int qtdeExemplares()
        {
            return exemplares.Count;
        }

        public int qtdeDisponiveis()
        {
            int cont = 0;
            foreach (var ex in exemplares)
            {
                if (ex.disponivel()) cont++;
            }
            return cont;
        }

        public int qtdeEmprestimos()
        {
            int total = 0;
            foreach (var ex in exemplares)
            {
                total += ex.qtdeEmprestimos();
            }
            return total;
        }

        public double percDisponibilidade()
        {
            int totalEx = qtdeExemplares();
            if (totalEx == 0) return 0.0;

            return ((double)qtdeDisponiveis() / totalEx) * 100.0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Livro)) return false;
            Livro outro = (Livro)obj;
            return this.isbn == outro.isbn;
        }
    }
}