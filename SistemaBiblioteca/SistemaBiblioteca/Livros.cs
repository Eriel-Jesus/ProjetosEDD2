using System.Collections.Generic;

namespace SistemaBiblioteca
{
    public class Livros
    {
        private List<Livro> acervo;

        public List<Livro> Acervo => acervo;

        public Livros()
        {
            this.acervo = new List<Livro>();
        }

        public void adicionar(Livro livro)
        {
            if (livro != null && pesquisar(livro) == null)
            {
                acervo.Add(livro);
            }
        }

        public Livro pesquisar(Livro livro)
        {
            if (livro == null) return null;

            foreach (var item in acervo)
            {
                if (item.Equals(livro))
                {
                    return item;
                }
            }
            return null;
        }
    }
}