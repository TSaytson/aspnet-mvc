using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace introduction.Models
{
  public class PersonModel
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "CPF is required")]
    [StringLength(11)]
    public string Cpf { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    [MaxLength(20)]
    [DisplayName("Surname")]
    public string SurName { get; set; }

    [DisplayName("Birthdate")]
    [DataType(DataType.DateTime)]
    
    public DateTime BirthDate { get; set; }

    [EmailAddress, MaxLength(20), Required]
    public string Email { get; set; }

    [Phone]
    public string Phone { get; set; }
  }
}