Feature: Product

Create a new product

@mytag
Scenario: Create product and verify the details
	Given I click the product menu
	And I click the "Create" link
	And I create product with following details
	| Name       | Description        | Price | ProductType |
	| Headphones | Noise cancellation | 300   | PERIPHARALS |
	When I click the Details link of the newly created product
	Then I see all the product details are created as expected