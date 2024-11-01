Feature: Auth and Recycling Operations

  Scenario: User Registration
    Given I have the following user details
      | Username      | Password       | Role  |
      | authTest       | testing22    | User  |
    When I register a new user
    Then the response status should be 201

  Scenario: User Login
    Given I have valid user credentials
      | Username      | Password       |
      | authTest       | testing22    |
    When I login with these credentials
    Then the response status should be 200
    And I should receive a JWT token


  @RequireAuth
  Scenario: Add a Recycling Report
    Given I have a valid JWT token
    And I have the following recycling report
      | Material | Quantity |
      | Plastic  | 5        |
    When I submit a recycling report
    Then the response status should be 201
