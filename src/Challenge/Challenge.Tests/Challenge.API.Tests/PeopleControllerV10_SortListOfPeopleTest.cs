using Challenge.API.ViewModels;
using Challenge.Domain.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.API.Tests
{
    public class PeopleControllerV10_SortListOfPeopleTest
    {
        private readonly string uri = "/People/v1.0/SortListOfPeople";
        private readonly HttpClient _client;

        public PeopleControllerV10_SortListOfPeopleTest()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient();
        }

        #region HAPPY TESTS FOR LISTS WITH ONE APPLICANT ONLY

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_5_ForApplicantWithLessThanMinIncomeAndWithoutDependent()
        {
            //Arrange
            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(AppSettings.IncomeMinScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_7_ForApplicantWithLessThanMinIncomeAndWithOneDependent()
        {
            //Arrange
            List<DependentViewModel> dependents = new();
            dependents.Add(new DependentViewModel
            {
                FullName = "Dependent Full Name",
                Document = "456",
                BirthDate = DateTime.Now
            });

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(AppSettings.IncomeMinScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_7_ForApplicantWithLessThanMinIncomeAndWithTwoDependents()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            DependentViewModel dependent2 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4562",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);
            dependents.Add(dependent2);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(AppSettings.IncomeMinScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_8_ForApplicantWithLessThanMinIncomeAndWithThreeDependents()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            DependentViewModel dependent2 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4562",
                BirthDate = DateTime.Now
            };

            DependentViewModel dependent3 = new()
            {
                FullName = "Dependent Full Name3",
                Document = "4563",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);
            dependents.Add(dependent2);
            dependents.Add(dependent3);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(AppSettings.IncomeMinScore + AppSettings.ThreeOrMoreDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_3_ForApplicantWithMoreThanMinIncomeAndWithoutDependent()
        {
            //Arrange
            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(AppSettings.IncomeMaxScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_5_ForApplicantWithMoreThanMinIncomeAndWithOneDependent()
        {
            //Arrange
            List<DependentViewModel> dependents = new();
            dependents.Add(new DependentViewModel
            {
                FullName = "Dependent Full Name",
                Document = "456",
                BirthDate = DateTime.Now
            });

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(AppSettings.IncomeMaxScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_5_ForApplicantWithMoreThanMinIncomeAndWithTwoDependents()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            DependentViewModel dependent2 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4562",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);
            dependents.Add(dependent2);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(AppSettings.IncomeMaxScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_6_ForApplicantWithMoreThanMinIncomeAndWithThreeDependents()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            DependentViewModel dependent2 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4562",
                BirthDate = DateTime.Now
            };

            DependentViewModel dependent3 = new()
            {
                FullName = "Dependent Full Name3",
                Document = "4563",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);
            dependents.Add(dependent2);
            dependents.Add(dependent3);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(AppSettings.IncomeMaxScore + AppSettings.ThreeOrMoreDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_0_ForApplicantWithMoreThanMaxIncomeAndWithThreeDependents()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            DependentViewModel dependent2 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4562",
                BirthDate = DateTime.Now
            };

            DependentViewModel dependent3 = new()
            {
                FullName = "Dependent Full Name3",
                Document = "4563",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);
            dependents.Add(dependent2);
            dependents.Add(dependent3);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMaxValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(0);
            }
        }

        #endregion

        #region BAD TESTS TO CHECK MODEL DEFINITIONS

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheApplicantFullNameIsNotInformed()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = string.Empty,
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("errors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Contains(ErrorMessage.FullNameError, responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheApplicantDocumentIsNotInformed()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = string.Empty,
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("errors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Contains(ErrorMessage.DocumentError, responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheSpouseFullNameIsNotInformed()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = string.Empty,
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("errors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Contains(ErrorMessage.FullNameError, responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheSpouseDocumentIsNotInformed()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = string.Empty
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("errors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Contains(ErrorMessage.DocumentError, responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheDependentFullNameIsNotInformed()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = string.Empty,
                Document = "4561",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name1",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "456"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("errors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Contains(ErrorMessage.FullNameError, responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheDependentDocumentIsNotInformed()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = string.Empty,
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Dependent Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "456"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("errors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Contains(ErrorMessage.DocumentError, responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheDependentBirthDateIsNotInformed()
        {
            //Arrange
            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4651",
                BirthDate = null
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Dependent Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "456"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("errors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Contains(ErrorMessage.BirthDateError, responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheTotalIncomeIsNotInformed()
        {
            //Arrange
            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = null
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("errors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Contains(ErrorMessage.TotalIncomeError, responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheTotalIncomeIsInvalid()
        {
            //Arrange
            var applicationFullName = "Applicant Full Name";
            ApplicantViewModel applicantViewModel = new()
            {
                FullName = applicationFullName,
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = -1
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal(ErrorMessage.InvalidaTotalIncomeError(applicationFullName), responseData.ToString());
        }

        #endregion

        #region TESTS TO CHECK THE BUSINESS RULES

        [Fact]
        public async Task SortListOfPeople_ShouldReturn_5_AndNotConsiderDependentAdultAsAValidDependent()
        {
            //Arrange
            List<DependentViewModel> dependents = new();
            dependents.Add(new DependentViewModel
            {
                FullName = "Dependent Full Name",
                Document = "456",
                BirthDate = DateTime.Now.AddYears(-AppSettings.AdultPersonAge)
            });

            ApplicantViewModel applicantViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(AppSettings.IncomeMinScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErrorWhenSpouseAlsoApplied()
        {
            //Arrange
            var applicationFullName = "Tatiane Oliveira";
            ApplicantViewModel applicantViewModel1 = new()
            {
                FullName = "Ajala Oliveira",
                Document = "123",
                Spouse = new()
                {
                    FullName = applicationFullName,
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMaxValue + 1
                }
            };
            ApplicantViewModel applicantViewModel2 = new()
            {
                FullName = applicationFullName,
                Document = "1231",
                Spouse = new()
                {
                    FullName = "Ajala Oliveira",
                    Document = "123"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMaxValue + 1
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel1);
            applicantViewModelList.Add(applicantViewModel2);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal(ErrorMessage.FamilyAlreadyHasApplicantError(applicationFullName), responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErrorWhenSomeDependentAlsoApplied()
        {
            //Arrange
            var applicationFullName = "Applicant Full Name";
            ApplicantViewModel applicantViewModel1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMaxValue + 1
                }
            };

            DependentViewModel dependent1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
                BirthDate = DateTime.Now
            };

            List<DependentViewModel> dependents = new();
            dependents.Add(dependent1);

            ApplicantViewModel applicantViewModel2 = new()
            {
                FullName = applicationFullName,
                Document = "123",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1232"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMaxValue + 1,
                    Dependents = dependents
                }
            };
            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel1);
            applicantViewModelList.Add(applicantViewModel2);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal(ErrorMessage.FamilyAlreadyHasApplicantError(applicationFullName), responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnSortedList()
        {
            //Arrange
            ApplicantViewModel applicantViewModel1 = new()
            {
                FullName = "Applicant Full Name",
                Document = "123A",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231A"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1
                }
            };

            List<DependentViewModel> dependents1 = new();
            dependents1.Add(new DependentViewModel
            {
                FullName = "Dependent Full Name",
                Document = "456B",
                BirthDate = DateTime.Now
            });
            ApplicantViewModel applicantViewModel2 = new()
            {
                FullName = "Applicant Full Name",
                Document = "123C",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231B"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents1
                }
            };

            ApplicantViewModel applicantViewModel3 = new()
            {
                FullName = "Applicant Full Name",
                Document = "123E",
                Spouse = new()
                {
                    FullName = "Spouse Full Name",
                    Document = "1231C"
                },
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue
                }
            };

            List<ApplicantViewModel> applicantViewModelList = new();
            applicantViewModelList.Add(applicantViewModel1);
            applicantViewModelList.Add(applicantViewModel2);
            applicantViewModelList.Add(applicantViewModel3);

            //Act
            var response = await _client.PostAsJsonAsync(uri, applicantViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var applicantList = JsonConvert.DeserializeObject<List<ApplicantViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var control = 7;

            foreach (var applicant in applicantList)
            {
                applicant.Score.Should().Be(control);
                control -= 2;
            }
        }

        #endregion

    }
}
