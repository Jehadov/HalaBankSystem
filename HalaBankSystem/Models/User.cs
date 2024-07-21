using System;
using System.Collections.Generic;

namespace HalaBankSystem.Models;

public partial class User
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public int Age { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public int? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public string Passwords { get; set; } = null!;

    public string? Access { get; set; }

    public string? RoleName { get; set; }

    public string? MyImage { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; } = new List<BankAccount>();
}
