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
        public async Task SortListOfPeople_ShouldReturnScore_5_ForSinglePersonWithLessThanMinIncomeAndWithoutDependent()
        {
            //Arrange
            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMinScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_7_ForSinglePersonWithLessThanMinIncomeAndWithOneDependent()
        {
            //Arrange
            List<DependentViewModel> dependents = new();
            dependents.Add(new DependentViewModel
            {
                FullName = "Dependent Full Name",
                Document = "456",
                BirthDate = DateTime.Now
            });

            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMinScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_7_ForSinglePersonWithLessThanMinIncomeAndWithTwoDependents()
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

            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMinScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_8_ForSinglePersonWithLessThanMinIncomeAndWithThreeDependents()
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

            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMinScore + AppSettings.ThreeOrMoreDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_3_ForSinglePersonWithMoreThanMinIncomeAndWithoutDependent()
        {
            //Arrange
            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMaxScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_5_ForSinglePersonWithMoreThanMinIncomeAndWithOneDependent()
        {
            //Arrange
            List<DependentViewModel> dependents = new();
            dependents.Add(new DependentViewModel
            {
                FullName = "Dependent Full Name",
                Document = "456",
                BirthDate = DateTime.Now
            });

            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMaxScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_5_ForSinglePersonWithMoreThanMinIncomeAndWithTwoDependents()
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

            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMaxScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_6_ForSinglePersonWithMoreThanMinIncomeAndWithThreeDependents()
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

            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue + 1,
                    Dependents = dependents
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMaxScore + AppSettings.ThreeOrMoreDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_5_ForMarriedPersonWithLessThanMinIncomeAndWithoutDependent()
        {
            //Arrange
            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMinScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_7_ForMarriedPersonWithLessThanMinIncomeAndWithOneDependent()
        {
            //Arrange
            List<DependentViewModel> dependents = new();
            dependents.Add(new DependentViewModel
            {
                FullName = "Dependent Full Name",
                Document = "456",
                BirthDate = DateTime.Now
            });

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMinScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_7_ForMarriedPersonWithLessThanMinIncomeAndWithTwoDependents()
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMinScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_8_ForMarriedPersonWithLessThanMinIncomeAndWithThreeDependents()
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMinScore + AppSettings.ThreeOrMoreDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_3_ForMarriedPersonWithMoreThanMinIncomeAndWithoutDependent()
        {
            //Arrange
            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMaxScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_5_ForMarriedPersonWithMoreThanMinIncomeAndWithOneDependent()
        {
            //Arrange
            List<DependentViewModel> dependents = new();
            dependents.Add(new DependentViewModel
            {
                FullName = "Dependent Full Name",
                Document = "456",
                BirthDate = DateTime.Now
            });

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMaxScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_5_ForMarriedPersonWithMoreThanMinIncomeAndWithTwoDependents()
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMaxScore + AppSettings.OneOrTwoDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_6_ForMarriedPersonWithMoreThanMinIncomeAndWithThreeDependents()
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMaxScore + AppSettings.ThreeOrMoreDependentsScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnScore_0_ForMarriedPersonWithMoreThanMaxIncomeAndWithThreeDependents()
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(0);
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
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

            PersonViewModel personViewModel = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("errors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Contains(ErrorMessage.BirthDateError, responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErroWhenTheTotalIncomeIsNotInformed()
        {
            //Arrange
            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = null
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
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
            PersonViewModel personViewModel = new()
            {
                FullName = applicationFullName,
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = -1
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
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

            PersonViewModel personViewModel = new()
            {
                FullName = "Applicant Full Name",
                Document = "123",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(AppSettings.IncomeMinScore);
            }
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnErrorWhenSpouseAlsoApplied()
        {
            //Arrange
            var applicationFullName = "Tatiane Oliveira";
            PersonViewModel personViewModel1 = new()
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
            PersonViewModel personViewModel2 = new()
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
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel1);
            personViewModelList.Add(personViewModel2);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
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
            PersonViewModel personViewModel1 = new()
            {
                FullName = "Dependent Full Name1",
                Document = "4561",
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

            PersonViewModel personViewModel2 = new()
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
                    TotalIncome = AppSettings.IncomeMaxValue + 1,
                    Dependents = dependents
                }
            };
            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel1);
            personViewModelList.Add(personViewModel2);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal(ErrorMessage.FamilyAlreadyHasApplicantError(applicationFullName), responseData.ToString());
        }

        [Fact]
        public async Task SortListOfPeople_ShouldReturnStoredList()
        {
            //Arrange
            PersonViewModel personViewModel1 = new()
            {
                FullName = "Applicant Full Name",
                Document = "123A",
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
            PersonViewModel personViewModel2 = new()
            {
                FullName = "Applicant Full Name",
                Document = "123C",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue,
                    Dependents = dependents1
                }
            };

            PersonViewModel personViewModel3 = new()
            {
                FullName = "Applicant Full Name",
                Document = "123E",
                FamilyData = new()
                {
                    TotalIncome = AppSettings.IncomeMinValue
                }
            };

            List<PersonViewModel> personViewModelList = new();
            personViewModelList.Add(personViewModel1);
            personViewModelList.Add(personViewModel2);
            personViewModelList.Add(personViewModel3);

            //Act
            var response = await _client.PostAsJsonAsync(uri, personViewModelList);
            var responseData = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            var PersonViewModelList = JsonConvert.DeserializeObject<List<PersonViewModel>>(responseData.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var control = 7;

            foreach (var PersonViewModel in PersonViewModelList)
            {
                PersonViewModel.Score.Should().Be(control);
                control -= 2;
            }
        }


        #endregion

    }
}
