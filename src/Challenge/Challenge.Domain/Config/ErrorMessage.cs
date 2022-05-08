namespace Challenge.Domain.Config
{
    public static class ErrorMessage
    {
        public const string FullNameError = "O campo 'fullName' é obrigatório!";
        public const string DocumentError = "O campo 'document' é obrigatório!";
        public const string BirthDateError = "O campo 'birthDate' é obrigatório!";
        public const string TotalIncomeError = "O campo 'totalIncome' é obrigatório!";
        public const string FamilyDataError = "O campo 'familyData' é obrigatório!";
        public const string SpouseError = "O campo 'spouse' é obrigatório!";

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