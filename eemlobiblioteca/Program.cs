using System;

namespace SistemaSimples
{
    class Program
    {
        static void Main(string[] args)
        {
            Gerenciador sistema = new Gerenciador();

            while (true)
            {
                Console.WriteLine("\n--- CONTROLE DE BIBLIOTECA ---");
                Console.WriteLine("1 - Cadastrar Livro");
                Console.WriteLine("2 - Ver Livros");
                Console.WriteLine("3 - Novo Leitor");
                Console.WriteLine("4 - Ver Leitores");
                Console.WriteLine("5 - Emprestar (Email + ID Livro)");
                Console.WriteLine("6 - Devolver (Email)");
                Console.WriteLine("0 - Sair");
                Console.Write("Opcao: ");
                string op = Console.ReadLine();

                try
                {
                    switch (op)
                    {
                        case "1":
                            Console.Write("Titulo: ");
                            string t = Console.ReadLine();
                            Console.Write("Autor: ");
                            string a = Console.ReadLine();
                            Console.Write("Quantidade: ");
                            int qtd = int.Parse(Console.ReadLine());
                            sistema.NovoLivro(t, a, qtd);
                            break;
                        case "2":
                            sistema.VerLivros();
                            break;
                        case "3":
                            Console.Write("Nome: ");
                            string n = Console.ReadLine();
                            Console.Write("Email: ");
                            string e = Console.ReadLine();
                            sistema.NovoLeitor(n, e);
                            break;
                        case "4":
                            sistema.VerLeitores();
                            break;
                        case "5":
                            Console.Write("Email do Leitor: ");
                            string mailEmp = Console.ReadLine();
                            Console.Write("ID do Livro (Veja na lista 2): ");
                            int idLivro = int.Parse(Console.ReadLine());
                            sistema.PegarLivro(mailEmp, idLivro);
                            break;
                        case "6":
                            Console.Write("Email do Leitor: ");
                            string mailDev = Console.ReadLine();
                            sistema.DevolverLivro(mailDev);
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opcao invalida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Tratamento genérico de erro pra não fechar o programa
                    Console.WriteLine("Ocorreu um erro: " + ex.Message);
                }
            }
        }
    }
}