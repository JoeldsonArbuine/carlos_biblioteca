namespace SistemaSimples
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int QtdTotal { get; set; }
        public int QtdAtual { get; set; }
    }

    public class Leitor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int? IdLivro { get; set; }
    }
}