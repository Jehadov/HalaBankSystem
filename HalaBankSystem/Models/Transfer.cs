using System;
using System.Collections.Generic;

namespace HalaBankSystem.Models;

public partial class Transfer
{
    public int TransferId { get; set; }

    public int? FromCustomerId { get; set; }

    public int? ToCustomerId { get; set; }

    public double? Amount { get; set; }

    public DateTime? TransferDate { get; set; }

    public virtual BankAccount? FromCustomer { get; set; }

    public virtual BankAccount? ToCustomer { get; set; }
}
