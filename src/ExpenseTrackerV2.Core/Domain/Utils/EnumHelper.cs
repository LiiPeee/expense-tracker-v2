using ExpenseTrackerV2.Core.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Application
{
    public static class EnumHelper
    {
        public static int GetId<TEnum>(string enumName) where TEnum :struct, Enum
        {
            if(Enum.TryParse<TEnum>(enumName, out var enumValue))
            {
                return Convert.ToInt32(enumValue);
            }
            throw new Exception($"{enumName} is not supported.");
        }
        public static int GetId<TEnum>(TEnum enumValue) where TEnum : struct, Enum 
        {
            return Convert.ToInt32(enumValue);
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
                case "Renda Variável":
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
