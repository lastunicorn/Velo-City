Velo City 1.10.0
====================================================================================================

This is a tool that calculates the velocity of a scrum team and helps planning the sprints.
It has two interfaces: a command line interface and a GUI created in WPF.


Client data
----------------------------------------------------------------------------------------------------

The client data is stored in a json file whose location is provided in the "appsettings.json".
For the moment, this file is  read-only and must be created/edited by hand.

This json contains three sections:
	- sprints - information about the existing sprints, including the start and end dates of each
	            one;
	- team members - information like availability and vacation information;
	- official holidays - a list of official holidays for each team member.


Command Line Interface
----------------------------------------------------------------------------------------------------

The console is accessible by typing "velo" in a console.


Desktop GUI Interface
----------------------------------------------------------------------------------------------------

The GUI is in a beta version and is missing some of the features. They will be added in later
versions.


Features
----------------------------------------------------------------------------------------------------

Among the most important fetures, we can mention:

	- Make an analysis for the next sprint (the "sprint" command) to calculate the expected velocity
	  and the most probable number of story points that the team will be able to burn.
	
	- See an overview of the last n sprints (the "sprints" command) with charts for velocity and
	  other values.
	
	- Inspect the team's composition (the "team" command) for the current sprint or another moment
	  in time.
	
	- Inspect the list of vacations for each team member (the "vacations" command).
	
	- etc.