Feature: Login

As an authenticated user
When I enter my details
So that I can see the home page
If you see this, this is the OLD version!!!

@Login
Scenario: I can login successfully
	Given I navigate to the site
	When I enter the correct user details
	Then I can view the home page
