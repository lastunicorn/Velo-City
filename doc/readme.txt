Velo City 1.9.0
====================================================================================================

This is a console application tool that calculates the velocity of a scrum team.

The input information needed by the tool:
	- sprint information like start and end dates;
	- team members information like availability and vacation information;
	- official holidays.

This information is stored in a json file whose location is provided in the "appsettings.json"
configuration file from the installation directory.

Among the most important use cases we can mention:

	- Make an analysis for the next sprint (the "sprint" command) to calculate the expected velocity
	  and the most probable number of story points that the team will be able to burn.
	
	- See an overview of the last 6 sprints (the "sprints" command) with charts for velocity and
	  other values.
	
	- Inspect the team's composition (the "team" command) for the current sprint or another moment
	  in time.
	
	- Inspect the list of vacations for each team member (the "vacations" command).
	
	- etc.