namespace Challenge.Domain.Config
{
    public static class AppSettings
    {
        public static readonly decimal IncomeMinValue = 900.00M;
        public static readonly int IncomeMinScore = 5;

        public static readonly decimal IncomeMaxValue = 1500.00M;
        public static readonly int IncomeMaxScore = 3;

        public static readonly int NumberOfDependentsControl = 2;
        public static readonly int OneOrTwoDependentsScore = 2;
        public static readonly int ThreeOrMoreDependentsScore = 3;

        public static readonly int AdultPersonAge = 18;
    }
}