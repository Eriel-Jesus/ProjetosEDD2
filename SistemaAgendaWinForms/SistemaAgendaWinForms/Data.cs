using System;

namespace SistemaAgendaWinForms
{
    public class Data
    {
        public int dia { get; set; }
        public int mes { get; set; }
        public int ano { get; set; }

        public Data() { }

        public Data(int dia, int mes, int ano)
        {
            setData(dia, mes, ano);
        }

        public void setData(int dia, int mes, int ano)
        {
            this.dia = dia;
            this.mes = mes;
            this.ano = ano;
        }

        public override string ToString()
        {
            return $"{dia:D2}/{mes:D2}/{ano:D4}";
        }
    }
}