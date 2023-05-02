VeloCity 1.17.0
====================================================================================================

This is a tool that calculates the velocity of a scrum team and helps planning the sprints.
It has two interfaces: a command line interface and a GUI created in WPF.


Client data
----------------------------------------------------------------------------------------------------

The client data is stored in a json file whose location is provided in the "appsettings.json".
For the moment, this file is  read-only and must be created/edited by hand.


Command Line Interface
----------------------------------------------------------------------------------------------------

The console is accessible by typing "velo" in a console.


Desktop GUI Interface
----------------------------------------------------------------------------------------------------

The GUI is in a beta version and is missing some of the features.


Features
----------------------------------------------------------------------------------------------------

Among the most important fetures, we can mention:

	- Make an analysis for the next sprint (the "sprint" command) to calculate the expected velocity
	  and the most probable number of story points that the team will be able to burn.
	  [Console and WPF]
	
	- See an overview of the last n sprints (the "sprints" command) with charts for velocity and
	  other values.
	  [Console]
	
	- Inspect the team's composition (the "team" command) for the current sprint or another moment
	  in time.
	  [Console and WPF (only for current sprint)]
	
	- Inspect the list of vacations for each team member (the "vacations" command).
	  [Console and WPF]
	
	- etc.