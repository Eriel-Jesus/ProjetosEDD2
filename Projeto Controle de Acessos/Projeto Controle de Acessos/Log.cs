using System;

namespace ProjetoControleAcessos
{
    public class Log
    {
        public DateTime dtAcesso { get; set; }
        public Usuario usuario { get; set; }
        public bool tipoAcesso { get; set; } // true = Autorizado, false = Negado

        public Log(DateTime dtAcesso, Usuario usuario, bool tipoAcesso)
        {
            this.dtAcesso = dtAcesso;
            this.usuario = usuario;
            this.tipoAcesso = tipoAcesso;
        }

        public override string ToString()
        {
            string status = tipoAcesso ? "Autorizado" : "Negado";
            string nomeUser = usuario != null ? usuario.nome : "Desconhecido";
            return $"[{dtAcesso:dd/MM/yyyy HH:mm:ss}] Usuário: {nomeUser} | Status: {status}";
        }
    }
}