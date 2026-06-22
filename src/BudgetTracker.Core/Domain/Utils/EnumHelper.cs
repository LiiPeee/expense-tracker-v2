using BudgetTracker.Core.Domain.Enum;


namespace BudgetTracker.Application
{
    public static class EnumHelper
    {
        public static int GetId<TEnum>(string enumName) where TEnum : struct, Enum
        {
            if (Enum.TryParse<TEnum>(enumName, out var enumValue))
            {
                return Convert.ToInt32(enumValue);
            }
            throw new Exception($"{enumName} is not supported.");
        }
        public static int GetRecurrence(string enumValue)
        {
            return enumValue switch
            {
                "NONE" => 1,
                "DAILY" => 2,
                "BIWEEKLY" => 3,
                "MONTHLY" => 4,
                "OCCASIONALLY" => 5
            };
        }

        public static int GetTypeContact(string enumValue)
        {
            return enumValue switch
            {
                "PERSONAL" => 1,
                "BUSINESS" => 2,
                _ => throw new NotImplementedException(),
            };
        }
        public static int GetTypeTransaction(string value)
        {
            return value switch
            {
                "EXPENSE" => 1,
                "INCOME" => 2,
                _ => throw new NotImplementedException(),
            };
        }
        public static long Category(string category)
        {
            switch (category)
            {
                case "Moradia":
                    return 1;
                case "Transporte":
                    return 2;
                case "Alimentação":
                    return 3;
                case "Saúde":
                    return 4;
                case "Educação":
                    return 5;
                case "Lazer":
                    return 6;
                case "Bens Pessoais":
                    return 7;
                case "Investimento":
                    return 8;
                case "Renda variável":
                    return 9;
                case "Benefícios":
                    return 10;
                case "Salário":
                    return 11;
                case "Conforto":
                    return 12;
                case "Outros":
                    return 13;
                default:
                    throw new KeyNotFoundException($"Category '{category}' is not supported.");
            }
        }
    }
}


