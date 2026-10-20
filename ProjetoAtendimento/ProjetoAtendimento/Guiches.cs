using System;
using System.Collections.Generic;

namespace ProjetoAtendimento
{
    public class Guiches
    {
        private List<Guiche> listaGuiches;

        public List<Guiche> ListaGuiches => listaGuiches;

        public Guiches()
        {
            this.listaGuiches = new List<Guiche>();
        }

        public void adicionar(Guiche guiche)
        {
            if (guiche != null)
            {
                listaGuiches.Add(guiche);
            }
        }
    }
}
