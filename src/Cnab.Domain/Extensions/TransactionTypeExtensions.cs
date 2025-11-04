using Cnab.Domain.Enums;

namespace Cnab.Domain.Extensions;

public static class TransactionTypeExtensions
{
    public static bool IsRevenue(this TransactionType type)
    {
        return type switch
        {
            TransactionType.Debito => true,
            TransactionType.Credito => true,
            TransactionType.RecebimentoEmprestimo => true,
            TransactionType.Vendas => true,
            TransactionType.RecebimentoTed => true,
            TransactionType.RecebimentoDoc => true,
            _ => false
        };
    }

    public static bool IsExpense(this TransactionType type)
    {
        return !IsRevenue(type);
    }

    public static int GetSign(this TransactionType type)
    {
        return IsRevenue(type) ? 1 : -1;
    }
}

