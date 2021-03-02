using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ViteDotnetCore5.Models.EFCore
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            UserTokens = new HashSet<UserToken>();
        }

        public int SeqNo { get; set; }
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "numeric(1, 0)")]
        public decimal? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedUser { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedUser { get; set; }
        [Required]
        [StringLength(30)]
        public string Account { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(30)]
        public string TelHome { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(5)]
        public string PostCode { get; set; }
        [StringLength(10)]
        public string City { get; set; }
        [StringLength(10)]
        public string Town { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public bool? IsForgotPassword { get; set; }
        public Guid? CompanyId { get; set; }

        [InverseProperty(nameof(UserRole.User))]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        [InverseProperty(nameof(UserToken.User))]
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
