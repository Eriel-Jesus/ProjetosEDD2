using System.Collections.Generic;

namespace ProjetoControleAcessos
{
    public class Usuario
    {
        public int id { get; set; }
        public string nome { get; set; }
        public List<Ambiente> ambientes { get; set; }

        public Usuario(int id, string nome = "")
        {
            this.id = id;
            this.nome = nome;
            this.ambientes = new List<Ambiente>();
        }

        public bool concederPermissao(Ambiente ambiente)
        {
            if (ambiente == null) return false;
            
            // Cada usuário só pode ter uma permissão para cada ambiente
            if (!ambientes.Contains(ambiente))
            {
                ambientes.Add(ambiente);
                return true;
            }
            return false;
        }

        public bool revogarPermissao(Ambiente ambiente)
        {
            if (ambiente == null) return false;
            return ambientes.Remove(ambiente);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Usuario)) return false;
            return this.id == ((Usuario)obj).id;
        }
    }
}