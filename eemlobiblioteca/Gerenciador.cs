using System;
using MySql.Data.MySqlClient;

namespace SistemaSimples
{
    public class Gerenciador
    {
        // Conexão direta aqui para simplificar
        private string conexao = "server=localhost;uid=root;pwd=;database=biblio_simples";

        // Função genérica para conectar (evita repetição)
        private MySqlConnection Conectar()
        {
            var con = new MySqlConnection(conexao);
            con.Open();
            return con;
        }

        public void NovoLivro(string titulo, string autor, int qtd)
        {
            using (var con = Conectar())
            {
                // Inserção direta, sem verificar se já existe
                string sql = "INSERT INTO livros (titulo, autor, qtd_total, qtd_atual) VALUES (@t, @a, @q, @q)";
                var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@t", titulo);
                cmd.Parameters.AddWithValue("@a", autor);
                cmd.Parameters.AddWithValue("@q", qtd);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Livro salvo!");
            }
        }

        public void VerLivros()
        {
            using (var con = Conectar())
            {
                var cmd = new MySqlCommand("SELECT * FROM livros", con);
                var r = cmd.ExecuteReader();
                Console.WriteLine("\n--- LISTA DE LIVROS ---");
                while (r.Read())
                {
                    Console.WriteLine($"{r["id"]} - {r["titulo"]} (Disp: {r["qtd_atual"]})");
                }
            }
        }

        public void NovoLeitor(string nome, string email)
        {
            try
            {
                using (var con = Conectar())
                {
                    var cmd = new MySqlCommand("INSERT INTO leitores (nome, email) VALUES (@n, @e)", con);
                    cmd.Parameters.AddWithValue("@n", nome);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Leitor salvo!");
                }
            }
            catch { Console.WriteLine("Erro: Email provavelmente já existe."); }
        }

        public void VerLeitores()
        {
            using (var con = Conectar())
            {
                var cmd = new MySqlCommand("SELECT * FROM leitores", con);
                var r = cmd.ExecuteReader();
                Console.WriteLine("\n--- LISTA DE LEITORES ---");
                while (r.Read())
                {
                    string livroStatus = r.IsDBNull(r.GetOrdinal("id_livro")) ? "Livre" : "Com livro";
                    Console.WriteLine($"{r["nome"]} ({r["email"]}) - {livroStatus}");
                }
            }
        }

        public void PegarLivro(string email, int idLivro)
        {
            using (var con = Conectar())
            {
                // Lógica simplificada: Tenta atualizar direto. Se der erro no SQL ou lógica, pegamos no catch.
                // 1. Verifica se tem estoque
                var cmdCheck = new MySqlCommand("SELECT qtd_atual FROM livros WHERE id = @id", con);
                cmdCheck.Parameters.AddWithValue("@id", idLivro);
                int estoque = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (estoque > 0)
                {
                    // 2. Atualiza o leitor
                    var cmdUser = new MySqlCommand("UPDATE leitores SET id_livro = @lid WHERE email = @mail AND id_livro IS NULL", con);
                    cmdUser.Parameters.AddWithValue("@lid", idLivro);
                    cmdUser.Parameters.AddWithValue("@mail", email);

                    if (cmdUser.ExecuteNonQuery() > 0) // Se atualizou alguém
                    {
                        // 3. Tira do estoque
                        var cmdLivro = new MySqlCommand("UPDATE livros SET qtd_atual = qtd_atual - 1 WHERE id = @id", con);
                        cmdLivro.Parameters.AddWithValue("@id", idLivro);
                        cmdLivro.ExecuteNonQuery();
                        Console.WriteLine("Empréstimo feito!");
                    }
                    else Console.WriteLine("Erro: Leitor não existe ou já tem livro.");
                }
                else Console.WriteLine("Livro sem estoque.");
            }
        }

        public void DevolverLivro(string email)
        {
            using (var con = Conectar())
            {
                // Descobre qual livro o cara tem
                var cmdBusca = new MySqlCommand("SELECT id_livro FROM leitores WHERE email = @m", con);
                cmdBusca.Parameters.AddWithValue("@m", email);
                object resultado = cmdBusca.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    int idLivro = Convert.ToInt32(resultado);

                    // Devolve estoque
                    var cmdVolta = new MySqlCommand("UPDATE livros SET qtd_atual = qtd_atual + 1 WHERE id = @id", con);
                    cmdVolta.Parameters.AddWithValue("@id", idLivro);
                    cmdVolta.ExecuteNonQuery();

                    // Limpa leitor
                    var cmdLimpa = new MySqlCommand("UPDATE leitores SET id_livro = NULL WHERE email = @m", con);
                    cmdLimpa.Parameters.AddWithValue("@m", email);
                    cmdLimpa.ExecuteNonQuery();

                    Console.WriteLine("Devolvido!");
                }
                else Console.WriteLine("Leitor não tem livros para devolver.");
            }
        }
    }
}