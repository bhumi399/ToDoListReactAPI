Instructions to install few things to set up the environment for the project. 
-----------------------------------------------------------------------------
	For .NET API
		1. Install MS Visual Studio
		2. Install .NET 6 if it doesn't get installed with MS Visual Studio
		3. Install Postman - to test APIs

	For the database setup
		4. Install PostgreSQL version 17.2-2 for x64

	For front-end
		5. Install Npm version 10.8.2
		6. Install node version v18.20.5


Commands need to be run once before you start the project
---------------------------------------------------------
	1. For react dependencies
		in cmd --> in the project directory --> run command "npm install"

	2. For setup database and its schema (because we are using EF Core)
		To create the migration
			in cmd --> in the project directory --> run command "dotnet ef migrations add InitialCreate"
		To update/apply the migration
			in cmd --> in the project directory --> run command "dotnet ef database update"

List of APIs
------------
	1. https://localhost:7083/api/Todo/GetAllUsers
	2. https://localhost:7083/api/Todo/GetAllTasksByUserId/{taskId}
	3. https://localhost:7083/api/Todo/UpdateTaskStatus/{taskId}

To Execute the program
----------------------
	1. Run the startup project in MS Visual Studio
	2. In cmd --> in project directory/clientapp --> run command "npm start"	

