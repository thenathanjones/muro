Feature: Displaying project status
	As an interested person
	I want to be shown the project status
	So I can see how the projects are tracking

@wip
Scenario: All builds are shown at once
	Given there are 3 builds monitored
	When I load the status page
	Then I should see all 3 builds

@wip
Scenario: Successful builds are indicated as such
	Given I have a successful build
	When I load the status page
	Then I should see a successful build
