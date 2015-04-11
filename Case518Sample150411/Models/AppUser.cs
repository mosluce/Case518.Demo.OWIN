namespace Case518Sample150411.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AppUser
    {
        public enum UserGender
        {
            Male,
            Female
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ActivationCode { get; set; }

        public bool Activated { get; set; }

        public UserGender Gender { get; set; }

        public bool Subscribe { get; set; }

        public DateTime? RegisterDate { get; set; }

        public DateTime? ActivatedDate { get; set; }
    }
}
