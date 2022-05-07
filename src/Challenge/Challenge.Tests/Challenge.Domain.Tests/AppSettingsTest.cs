using Challenge.Domain.Config;
using Xunit;

namespace Challenge.Domain.Tests
{
    public class AppSettingsTest
    {
        [Fact]
        public void AppSettings_ShouldReturnIncomeMinValue()
        {
            var result = AppSettings.IncomeMinValue;
            Assert.Equal(900.00M, result);
        }

        [Fact]
        public void AppSettings_ShouldReturnIncomeMinScore()
        {
            var result = AppSettings.IncomeMinScore;
            Assert.Equal(5, result);
        }

        [Fact]
        public void AppSettings_ShouldReturnIncomeMaxValue()
        {
            var result = AppSettings.IncomeMaxValue;
            Assert.Equal(1500.00M, result);
        }

        [Fact]
        public void AppSettings_ShouldReturnIncomeMaxScore()
        {
            var result = AppSettings.IncomeMaxScore;
            Assert.Equal(3, result);
        }

        [Fact]
        public void AppSettings_ShouldReturnNumberOfDependentsControl()
        {
            var result = AppSettings.NumberOfDependentsControl;
            Assert.Equal(2, result);
        }

        [Fact]
        public void AppSettings_ShouldReturnOneOrTwoDependentsScore()
        {
            var result = AppSettings.OneOrTwoDependentsScore;
            Assert.Equal(2, result);
        }

        [Fact]
        public void AppSettings_ShouldReturnThreeOrMoreDependentsScore()
        {
            var result = AppSettings.ThreeOrMoreDependentsScore;
            Assert.Equal(3, result);
        }

        [Fact]
        public void AppSettings_ShouldReturnAdultPersonAge()
        {
            var result = AppSettings.AdultPersonAge;
            Assert.Equal(18, result);
        }
    }
}