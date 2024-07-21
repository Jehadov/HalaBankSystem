using System;
using System.Collections.Generic;

namespace HalaBankSystem.Models;

public partial class Loan
{
    public int LoanId { get; set; }

    public int? AccountId { get; set; }

    public string? LoanType { get; set; }

    public double? LoanAmount { get; set; }

    public DateTime? LoanStartDate { get; set; }

    public DateTime? LoanEndDate { get; set; }

    public string? ActiveState { get; set; }

    public virtual BankAccount? Account { get; set; }
}
