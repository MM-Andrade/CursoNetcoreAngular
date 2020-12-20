using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }
        [Required (ErrorMessage="O campo {0} é obrigatório")]
        public string Nome { get; set; }
        [Required (ErrorMessage="O campo {0} é obrigatório")]
        public decimal Preco { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }

        [Range(2,1200, ErrorMessage="A quantidade de pessoas é entre {1} e {2}")]
        public int Quantidade { get; set; }
    }
}
