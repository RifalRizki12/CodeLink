﻿using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_accounts")]
    public class Account : BaseEntity
    {
        [Column("password", TypeName = "nvarchar(max)")]
        public string Password { get; set; }

        [Column("otp")]
        public int Otp { get; set; }

        [Column("status")]
        public StatusLevel Status { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; }

        [Column("expired_time")]
        public DateTime ExpiredTime { get; set; }

        //kardinalitas
        public Employee? Employee { get; set; }
        public ICollection<AccountRole>? AccountRoles { get; set; }

    }
}