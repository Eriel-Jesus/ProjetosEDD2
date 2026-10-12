using System;

namespace SistemaBiblioteca
{
    public class Emprestimo
    {
        public DateTime dtEmprestimo { get; set; }
        public DateTime dtDevolucao { get; set; }

        public Emprestimo()
        {
            this.dtEmprestimo = DateTime.Now;
            this.dtDevolucao = DateTime.MinValue; // MinValue indica que ainda não foi devolvido
        }

        public Emprestimo(DateTime dtEmprestimo)
        {
            this.dtEmprestimo = dtEmprestimo;
            this.dtDevolucao = DateTime.MinValue;
        }
    }
}