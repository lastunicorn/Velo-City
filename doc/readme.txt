Velo City 1.4.0
====================================================================================================

This is a console application tool that calculates the velocity of a scrum team.

The input information needed by the tool:
	- sprint information like start and end dates;
	- team members information like availability and vacation information;
	- official holidays.

This information is stored in a json file whose location is provided in the "appsettings.json"
configuration file from the installation directory.

The available commands:
	- sprints - Displays a list with the last n sprints.
	- sprint - Displays an analysis for the specified sprint.
	- vacations - Displays the vacation days for a team member.
	- team - Displays the composition of the team (team members).
	- db - Opens the database file in a text editor.