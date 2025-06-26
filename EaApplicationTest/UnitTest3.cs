using AutoFixture.Xunit2;
using EaApplicationTest.Models;
using EaApplicationTest.Pages;

namespace EaApplicationTest;

public class UnitTest3
{
    private readonly IHomePage _homePage;
    private readonly IProductPage _productPage;

    public UnitTest3(IHomePage homePage, IProductPage productPage)
    {
        _homePage = homePage;
        _productPage = productPage;
    }
        
    [Theory]
    [AutoData]
    public void CreateProduct(Product product)
    {
        //Click the Create link
        _homePage.ClickProduct();

        //Create product
        _productPage.ClickCreateButton();
        _productPage.CreateProduct(product);
        _productPage.PerformClickOnSpecialValue(product.Name, "Details");

        Thread.Sleep(5000);
    }
}