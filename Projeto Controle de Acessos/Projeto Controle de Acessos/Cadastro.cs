using System;
using System.Collections.Generic;
using System.IO;

namespace ProjetoControleAcessos
{
    public class Cadastro
    {
        public List<Usuario> usuarios { get; set; }
        public List<Ambiente> ambientes { get; set; }

        public Cadastro()
        {
            usuarios = new List<Usuario>();
            ambientes = new List<Ambiente>();
        }

        public void adicionarUsuario(Usuario usuario)
        {
            if (usuario != null && pesquisarUsuario(usuario) == null)
                usuarios.Add(usuario);
        }

        public bool removerUsuario(Usuario usuario)
        {
            Usuario encontrado = pesquisarUsuario(usuario);
            // Usuários só podem ser removidos se estiverem sem permissões
            if (encontrado != null && encontrado.ambientes.Count == 0)
            {
                return usuarios.Remove(encontrado);
            }
            return false;
        }

        public Usuario pesquisarUsuario(Usuario usuario)
        {
            if (usuario == null) return null;
            return usuarios.Find(u => u.Equals(usuario));
        }

        public void adicionarAmbiente(Ambiente ambiente)
        {
            if (ambiente != null && pesquisarAmbiente(ambiente) == null)
                ambientes.Add(ambiente);
        }

        public bool removerAmbiente(Ambiente ambiente)
        {
            Ambiente encontrado = pesquisarAmbiente(ambiente);
            if (encontrado != null)
            {
                // Remover permissões deste ambiente em todos os usuários antes de excluir
                foreach (var u in usuarios)
                {
                    u.revogarPermissao(encontrado);
                }
                return ambientes.Remove(encontrado);
            }
            return false;
        }

        public Ambiente pesquisarAmbiente(Ambiente ambiente)
        {
            if (ambiente == null) return null;
            return ambientes.Find(a => a.Equals(ambiente));
        }

        // --- Persistência de Dados (Modelo Relacional em Arquivos) ---

        public void upload()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("ambientes.csv"))
                {
                    foreach (var a in ambientes) sw.WriteLine($"{a.id};{a.nome}");
                }

                using (StreamWriter sw = new StreamWriter("usuarios.csv"))
                {
                    foreach (var u in usuarios) sw.WriteLine($"{u.id};{u.nome}");
                }

                using (StreamWriter sw = new StreamWriter("permissoes.csv"))
                {
                    foreach (var u in usuarios)
                    {
                        foreach (var a in u.ambientes)
                        {
                            sw.WriteLine($"{u.id};{a.id}");
                        }
                    }
                }

                using (StreamWriter sw = new StreamWriter("logs.csv"))
                {
                    foreach (var a in ambientes)
                    {
                        foreach (var log in a.logs)
                        {
                            int idUser = log.usuario != null ? log.usuario.id : 0;
                            sw.WriteLine($"{a.id};{log.dtAcesso:O};{idUser};{log.tipoAcesso}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar os dados: " + ex.Message);
            }
        }

        public void download()
        {
            try
            {
                ambientes.Clear();
                usuarios.Clear();

                if (File.Exists("ambientes.csv"))
                {
                    foreach (string linha in File.ReadAllLines("ambientes.csv"))
                    {
                        string[] dados = linha.Split(';');
                        ambientes.Add(new Ambiente(int.Parse(dados[0]), dados[1]));
                    }
                }

                if (File.Exists("usuarios.csv"))
                {
                    foreach (string linha in File.ReadAllLines("usuarios.csv"))
                    {
                        string[] dados = linha.Split(';');
                        usuarios.Add(new Usuario(int.Parse(dados[0]), dados[1]));
                    }
                }

                if (File.Exists("permissoes.csv"))
                {
                    foreach (string linha in File.ReadAllLines("permissoes.csv"))
                    {
                        string[] dados = linha.Split(';');
                        Usuario u = pesquisarUsuario(new Usuario(int.Parse(dados[0])));
                        Ambiente a = pesquisarAmbiente(new Ambiente(int.Parse(dados[1])));
                        if (u != null && a != null) u.concederPermissao(a);
                    }
                }

                if (File.Exists("logs.csv"))
                {
                    foreach (string linha in File.ReadAllLines("logs.csv"))
                    {
                        string[] dados = linha.Split(';');
                        Ambiente a = pesquisarAmbiente(new Ambiente(int.Parse(dados[0])));
                        if (a != null)
                        {
                            DateTime dt = DateTime.ParseExact(dados[1], "O", null);
                            Usuario u = pesquisarUsuario(new Usuario(int.Parse(dados[2])));
                            bool tipo = bool.Parse(dados[3]);
                            
                            // Se o usuário foi removido da base, cria um genérico apenas para histórico
                            if (u == null) u = new Usuario(int.Parse(dados[2]), "Usuário Removido");

                            a.registrarLog(new Log(dt, u, tipo));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar os dados: " + ex.Message);
            }
        }
    }
}