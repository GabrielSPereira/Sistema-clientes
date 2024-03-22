using System;

public class CPFDuplicadoException : Exception
{
    public CPFDuplicadoException() : base("CPF duplicado. Por favor, insira um CPF único.")
    {
    }
}
