using System;

namespace ProjetoAtendimento
{
    public class Senha
    {
        public int id { get; set; }
        public DateTime dataGerac { get; set; }
        public DateTime horaGerac { get; set; }
        public DateTime dataAtend { get; set; }
        public DateTime horaAtend { get; set; }

        public Senha(int id)
        {
            this.id = id;
            DateTime agora = DateTime.Now;
            this.dataGerac = agora.Date;
            this.horaGerac = agora;
            this.dataAtend = DateTime.MinValue;
            this.horaAtend = DateTime.MinValue;
        }

        public string dadosParciais()
        {
            return $"{id} - {dataGerac.ToString("dd/MM/yyyy")} - {horaGerac.ToString("HH:mm:ss")}";
        }

        public string dadosCompletos()
        {
            string dataA = dataAtend == DateTime.MinValue ? "Pending" : dataAtend.ToString("dd/MM/yyyy");
            string horaA = horaAtend == DateTime.MinValue ? "Pending" : horaAtend.ToString("HH:mm:ss");

            return $"{id} - {dataGerac.ToString("dd/MM/yyyy")} - {horaGerac.ToString("HH:mm:ss")} - {dataA} - {horaA}";
        }
    }
}
