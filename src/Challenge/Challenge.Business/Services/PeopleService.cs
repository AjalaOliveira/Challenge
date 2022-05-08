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

        public Task<List<ApplicantDTO>> SortListOfPeople(List<ApplicantDTO> applicantList)
        {
            List<ApplicantDTO> result = new();
            foreach (var applicant in applicantList)
            {
                ValidateTotalIncome(applicant);
                ValidateApplicant(applicant);

                if (HasValidTotalIncome(applicant))
                    applicant.Score = CalculateScore(applicant);
                else
                    applicant.Score = 0;
                result.Add(applicant);
            }

            result = GetSortedList(result);
            return Task.FromResult(result);
        }

        private static void ValidateTotalIncome(ApplicantDTO applicant)
        {
            if (applicant.FamilyData.TotalIncome < 0)
                throw new Exception(ErrorMessage.InvalidaTotalIncomeError(applicant.FullName));
        }

        private static void ValidateApplicant(ApplicantDTO applicant)
        {
            if (!Applicants.Contains(applicant.Document))
            {
                Applicants.Add(applicant.Document);

                if (applicant.Spouse != default)
                {
                    if (!Applicants.Contains(applicant.Spouse.Document))
                        Applicants.Add(applicant.Spouse.Document);
                    else
                        ReturnError(applicant.FullName);
                }

                if (applicant.FamilyData.Dependents != default)
                {
                    foreach (var dependent in applicant.FamilyData.Dependents)
                    {
                        if (!Applicants.Contains(dependent.Document))
                            Applicants.Add(dependent.Document);
                        else
                            ReturnError(applicant.FullName);
                    }
                }
            }
            else
            {
                ReturnError(applicant.FullName);
            }
        }

        private static bool HasValidTotalIncome(ApplicantDTO applicant)
        {
            if (applicant.FamilyData.TotalIncome <= AppSettings.IncomeMaxValue)
                return true;
            return false;
        }

        private static void ReturnError(string applicantFullName)
        {
            throw new Exception(ErrorMessage.FamilyAlreadyHasApplicantError(applicantFullName));
        }

        private static int CalculateScore(ApplicantDTO applicant)
        {
            int score = 0;
            score += CalculateScoreByTotalIncome(applicant.FamilyData.TotalIncome);
            score += CalculateScoreByDependents(applicant.FamilyData.Dependents);
            return score;
        }

        private static int CalculateScoreByTotalIncome(decimal totalIncome)
        {
            if (totalIncome <= AppSettings.IncomeMinValue)
                return AppSettings.IncomeMinScore;
            else
                return AppSettings.IncomeMaxScore;
        }

        private static int CalculateScoreByDependents(List<DependentDTO> dependentList)
        {
            if (dependentList != default)
            {
                var validDependentsTotal = GetTotalValidDependentsByBirthDate(dependentList);
                if (validDependentsTotal > 0 && validDependentsTotal <= AppSettings.NumberOfDependentsControl)
                    return AppSettings.OneOrTwoDependentsScore;
                else if (validDependentsTotal > AppSettings.NumberOfDependentsControl)
                    return AppSettings.ThreeOrMoreDependentsScore;
                else
                    return 0;
            }
            return 0;
        }

        private static int GetTotalValidDependentsByBirthDate(List<DependentDTO> dependentList)
        {
            return dependentList.Where(dependent => dependent.BirthDate.Date > DateTime.Now.AddYears(-AppSettings.AdultPersonAge).Date).Count();
        }

        private static List<ApplicantDTO> GetSortedList(List<ApplicantDTO> applicantList)
        {
            return applicantList.OrderByDescending(applicant => applicant.Score).ToList();
        }
    }
}