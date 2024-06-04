# Tasks:
Database diagram: <br>

1. As a User I want to be able to delete a Person
2. As a User I need to be able to update a Person's Information
3. As a User I need to be able to create a new Person in the database
4. As a User I need to be able to add a new Salary
5. As a User I want to be able to edit/add new person's details
6. As a User I want to be able to delete a Department <br>
7. As a developer I want to not change the port of the api in all of the Web Controllers <br>
8. Adding Authentication to the app, you can implement as you please, but to give you some ideas: <br>
9. Having too much time on your hands? Do any kind of improvements you wish. Just let us know via e-mail once you send the link with your repository what improvements you've done.
    
V1
    The user can perform all CRUD (Create, Read, Update, Delete) operations on Person entities. (Tasks: 1, 2, 3)
    The user can create and update person information. (Tasks: 3, 4 combined)
    The user can perform all CRUD operations on Salary entities. (Task: 4)
    The user can perform all CRUD operations on Department entities. (Task: 6)
    The API port is configured in the appsettings.json file (Task: 7).
    (Task: 8)
    Token-based authentication using the Bearer scheme is implemented. A token is generated upon user login, and all operations require authorization (user logged in). 
    Login and registration functionalities are implemented for Admin users.

V2 (Improvements over V1)
    (Task 9)
    Combined the Person and Person Details tables as they represent the same entity.
    Streamlined Admin information by keeping only necessary fields.
    Implemented logout functionality for Admin users.
    Added logging for the API application.

