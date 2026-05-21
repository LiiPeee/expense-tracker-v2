using BudgetTracker.Core.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static string Category(string category)
        {
            switch (category)
            {
                case "Alimentação":
                    return Categories.ALIMENTACAO.ToString();
                case "Conforto":
                    return Categories.CONFORTO.ToString();
                case "Moradia":
                    return Categories.MORADIA.ToString();
                case "Transporte":
                    return Categories.TRANSPORTE.ToString();
                case "Saúde":
                    return Categories.SAUDE.ToString();
                case "Educação":
                    return Categories.EDUCACAO.ToString();
                case "Lazer":
                    return Categories.LAZER.ToString();
                case "Bens Pessoais":
                    return Categories.BENS_PESSOAIS.ToString();
                case "Investimento":
                    return Categories.INVESTIMENTO.ToString();
                case "Outros":
                    return Categories.OUTROS.ToString();
                case "Renda variável":
                    return Categories.RENDA_VARIAVEL.ToString();
                case "Benefícios":
                    return Categories.BENEFICIOS.ToString();
                case "Salário":
                    return Categories.SALARIO.ToString();
                default:
                    throw new Exception($"{category} não é uma categoria suportada.");
            }
        }
    }
}


