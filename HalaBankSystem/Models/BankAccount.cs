using System;
using System.Collections.Generic;

namespace HalaBankSystem.Models;

public partial class BankAccount
{
    public int AccountId { get; set; }

    public int? UserId { get; set; }

    public double? Balance { get; set; }

    public string Passwords { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? RoleName { get; set; }

    public string? MyImage { get; set; }

    public string? Fullname { get; set; }

    public string? Token { get; set; }

    public virtual ICollection<Loan> Loans { get; } = new List<Loan>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual ICollection<Transfer> TransferFromCustomers { get; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferToCustomers { get; } = new List<Transfer>();

    public virtual User? User { get; set; }
}
