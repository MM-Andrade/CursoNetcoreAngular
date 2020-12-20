using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class EventoDto
    {
        public int Id { get;  set; }
        [Required(ErrorMessage = "Local obrigatório")]
        [StringLength(100,MinimumLength=4,ErrorMessage="é entre {2} e {1} caracteres")]
        public string Local { get;  set; }
        public DateTime DataEvento { get; set; }
        [Required (ErrorMessage = "O tema é obrigatório")]
        public string Tema { get;  set; }
        [Range(2,120000, ErrorMessage="A quantidade de pessoas é entre {1} e {2}")]
        public int QtdPessoas { get;  set; }
        public string ImagemURL { get;  set; }
        [Phone]
        public string Telefone { get;  set; }
        [EmailAddress]
        public string Email { get;  set; }
        public List<LoteDto> Lotes { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<PalestranteDto> Palestrantes { get;  set; }
    }
}