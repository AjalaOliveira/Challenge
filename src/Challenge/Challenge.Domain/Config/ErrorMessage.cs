namespace Challenge.Domain.Config
{
    public static class ErrorMessage
    {
        public const string FullNameError = "O campo 'nomeCompleto' é obrigatório!";
        public const string DocumentError = "O campo 'numeroDocumento' é obrigatório!";
        public const string BirthDateError = "O campo 'dataNascimento' é obrigatório!";
        public const string TotalIncomeError = "O campo 'rendaFamiliar' é obrigatório!";
        public const string FamilyDataError = "O campo 'dadosFamilia' é obrigatório!";

        public static string FamilyAlreadyHasApplicantError(string applicantFullName)
        {
            return $"Pretendente: {applicantFullName}. Já existe um pretendente dessa família!";
        }

        public static string InvalidaTotalIncomeError(string applicantFullName)
        {
            return $"Pretendente: {applicantFullName}. O valor da renda total da família não pode ser menor que zero!";
        }
    }
}