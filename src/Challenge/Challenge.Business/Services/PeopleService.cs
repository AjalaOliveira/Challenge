using Challenge.Business.Interfaces;
using Challenge.Domain.Config;
using Challenge.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Business.Services
{
    public class PeopleService : IPeopleService
    {
        private static List<string> Applicants;

        public PeopleService()
        {
            Applicants = new();
        }

        public Task<List<PersonDTO>> SortListOfPeople(List<PersonDTO> personDTO)
        {
            List<PersonDTO> result = new();
            foreach (var item in personDTO)
            {
                ValidateTotalIncome(item);
                ValidateApplicant(item);

                if (HasValidTotalIncome(item))
                    item.Score = CalculateScore(item);
                else
                    item.Score = 0;
                result.Add(item);
            }

            result = GetSortedList(result);
            return Task.FromResult(result);
        }

        private static void ValidateTotalIncome(PersonDTO person)
        {
            if (person.FamilyData.TotalIncome < 0)
                throw new Exception($"Nome: {person.FullName}. O valor da renda total da família não pode ser menor que zero!");
        }

        private static void ValidateApplicant(PersonDTO person)
        {
            if (!Applicants.Contains(person.Document))
            {
                Applicants.Add(person.Document);

                if (person.Spouse != default)
                {
                    if (!Applicants.Contains(person.Spouse.Document))
                        Applicants.Add(person.Spouse.Document);
                    else
                        ReturnError(person.FullName);
                }

                if (person.FamilyData.Dependents != default)
                {
                    foreach (var dependent in person.FamilyData.Dependents)
                    {
                        if (!Applicants.Contains(dependent.Document))
                            Applicants.Add(dependent.Document);
                        else
                            ReturnError(person.FullName);
                    }
                }
            }
            else
            {
                ReturnError(person.FullName);
            }
        }

        private static bool HasValidTotalIncome(PersonDTO person)
        {
            if (person.FamilyData.TotalIncome <= AppSettings.IncomeMaxValue)
                return true;
            return false;
        }

        private static void ReturnError(string fullName)
        {
            throw new Exception($"Nome: {fullName}. Já existe um pretendente dessa família!");
        }

        private static int CalculateScore(PersonDTO person)
        {
            int score = 0;
            score += CalculateScoreByTotalIncome(person.FamilyData.TotalIncome);
            score += CalculateScoreByDependents(person.FamilyData.Dependents);
            return score;
        }

        private static int CalculateScoreByTotalIncome(decimal totalIncome)
        {
            if (totalIncome <= AppSettings.IncomeMinValue)
                return AppSettings.IncomeMinScore;
            else
                return AppSettings.IncomeMaxScore;
        }

        private static int CalculateScoreByDependents(List<DependentDTO> dependents)
        {
            if (dependents != default)
            {
                var validDependentsTotal = GetTotalValidDependentsByBirthDate(dependents);
                if (validDependentsTotal > 0 && validDependentsTotal <= AppSettings.NumberOfDependentsControl)
                    return AppSettings.OneOrTwoDependentsScore;
                else if (validDependentsTotal > AppSettings.NumberOfDependentsControl)
                    return AppSettings.ThreeOrMoreDependentsScore;
                else
                    return 0;
            }
            return 0;
        }

        private static int GetTotalValidDependentsByBirthDate(List<DependentDTO> dependents)
        {
            return dependents.Where(dependent => dependent.BirthDate.Date > DateTime.Now.AddYears(-AppSettings.AdultPersonAge).Date).Count();
        }

        private static List<PersonDTO> GetSortedList(List<PersonDTO> personDTOList)
        {
            return personDTOList.OrderByDescending(personDTOList => personDTOList.Score).ToList();
        }
    }
}