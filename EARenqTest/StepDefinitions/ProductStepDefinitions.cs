using System;
using EaApplicationTest.Models;
using EaApplicationTest.Pages;
using FluentAssertions;
using Reqnroll;

namespace EARenqTest.StepDefinitions
{
    [Binding]
    public class ProductStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private readonly IHomePage _homePage;
        private readonly IProductPage _productPage;

        public ProductStepDefinitions(ScenarioContext scenarioContext, IHomePage homePage, IProductPage productPage)
        {
            _scenarioContext = scenarioContext;
            _homePage = homePage;
            _productPage = productPage;
        }


        [Given("I click the product menu")]
        public void GivenIClickTheProductMenu()
        {
            _homePage.ClickProduct();
        }

        [Given("I click the {string} link")]
        public void GivenIClickTheLink(string create)
        {
            _productPage.ClickCreateButton();
        }

        [Given("I create product with following details")]
        public void GivenICreateProductWithFollowingDetails(DataTable dataTable)
        {
            var product = dataTable.CreateInstance<Product>();
            _productPage.CreateProduct(product);
            _scenarioContext.Set(product);
        }

        [When("I click the Details link of the newly created product")]
        public void WhenIClickTheDetailsLinkOfTheNewlyCreatedProduct()
        {
            var product = _scenarioContext.Get<Product>();
            _productPage.PerformClickOnSpecialValue(product.Name, "Details");
        }

        [Then("I see all the product details are created as expected")]
        public void ThenISeeAllTheProductDetailsAreCreatedAsExpected()
        {
            var product = _scenarioContext.Get<Product>();
            _productPage.GetProductName().Should().BeEquivalentTo(product.Name);
        }
    }
}
