﻿using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Accounts;
public class AccountDto
{
    // Properti-properti DTO untuk objek Account
    public Guid Guid { get; set; } // GUID unik yang mengidentifikasi akun
    public string Password { get; set; }
    public int Otp { get; set; }
    public StatusLevel Status { get; set; }
    public bool IsUsed { get; set; }
    public DateTime ExpiredTime { get; set; }

    // Operator eksplisit digunakan untuk mengonversi objek Account ke AccountDto
    public static explicit operator AccountDto(Account account)
    {
        return new AccountDto
        {
            Guid = account.Guid, // Mengisi properti Guid dengan nilai GUID dari objek Account yang diberikan.
            Password = account.Password,
            Otp = account.Otp,
            Status = account.Status,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime,
        };
    }

    // Operator implisit digunakan untuk mengonversi objek AccountDto ke Account
    public static implicit operator Account(AccountDto accountDto)
    {
        return new Account
        {
            Guid = accountDto.Guid, // Mengisi properti Guid dengan nilai GUID dari objek AccountDto yang diberikan.
            Password = accountDto.Password,
            Otp = accountDto.Otp,
            Status = accountDto.Status,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime,
            ModifiedDate = DateTime.Now // Mengisi properti ModifiedDate dengan tanggal dan waktu saat ini.
        };
    }
}
