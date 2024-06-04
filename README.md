# Tasks:
Database diagram: <br>
![Capture5465465](https://github.com/AgrostemmaGithago/TEC-Internship/assets/129935966/a95a3f3f-9ee6-42c6-bf8c-cf67ffb5741b)



1. As a User I want to be able to delete a Person
2. As a User I need to be able to update a Person's Information
3. As a User I need to be able to create a new Person in the database
4. As a User I need to be able to add a new Salary
5. As a User I want to be able to edit/add new person's details

	Implementation: 
	- create PersonDetail Model according to the database diagram
	- add the PersonDetails database set to the APIDbContext
	- create a new migration using Package Manager Console <br>
	read: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#create-your-first-migration <br>
	Troubleshoot: <br>
	ERROR: "dotnet : Could not execute because the specified command or file was not found." Please run "dotnet tool install --global dotnet-ef --version 8.*" <br>
	ERROR: "No project was found. Change the current working directory or use the --project option". Please change the path to the ApiApp -  run "cd .\[_]\ApiApp" <br>
	- modify the View so I could update/add a Person's Details from the web app (Birthday and PersonCity)

6. As a User I want to be able to delete a Department <br>
7. As a developer I want to not change the port of the api in all of the Web Controllers <br>
   Details:  Ctrl + Shit + F in the entire solution and search for "HINT"
<br><br>
Are you bored yet? get a lot of points by: <br>
8. Adding Authentication to the app, you can implement as you please, but to give you some ideas: <br>
    	- create a Login View with admin username and password input fields. could be a popup or create the login in the Header of the app. <br>
    	- create an Admin Table with user information <br>
    	- I should not be able to do any of the RESTful api calls on person, persondetails, salary or department tables unless I am logged in <br>

9. Having too much time on your hands? Do any kind of improvements you wish. Just let us know via e-mail once you send the link with your repository what improvements you've done.
    
     
