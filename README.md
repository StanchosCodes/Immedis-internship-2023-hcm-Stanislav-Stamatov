# Human Capital Management System
# Immedis-internship-2023-hcm-Stanislav-Stamatov
This is a web project for the Immedis intership 2023 with ASP.NET Core MVC + Web Api.
Human Capital Management is an online app for managing teams and projects in a company. It is meant for regular employees and managers of teams.

# 📐 Technical description
The app have two options for a member. The first is as a regular employee and the second is as admin which is meant for the managers of the teams.
When first registered, the user is an employee and only an admin can make him admin.
The employees can see all projects, all employees, all departments and all towns which they have colleagues from. They can see all the projects they work on and the department they work in on a separate page. The employee can update its own profile information.
The managers can update and delete the projects, the departments, the towns, the employees profiles and there own profiles. They can add more roles and assign employees from one project to another.
The app uses custom made Authentication and Authorization system. For the password crypting it is used the Bcrypt package. It works with one-way crypting and it validates the crypted version of the password on login. In the database is stored only the hash of the password with salt.

# 🎨 Front-end
For the front-end part it's used bootstrap 5 and sweet alert modals with javascript for alerts and confirmation messages. There is one background for all pages. The images are retrieved from links which are given when a project, town or employee is beeing added. When a user is logged in there is a greeting message in the top-right corner. The logged in and not logged in users have different navigation menu options as well as the employees and admins.

# 📋 Used technologies

Entity Framework Core 6
ASP.NET Core 6
ASP.NET Web API
Swagger UI
HTML5
CSS3
Bootstrap
Limonte-sweetalert2
JQuery
JavaScript
SOLID Principles
MVC Design Pattern
SQL Server - for Development

# 💼 Database
When migrations are applied the database is seeded only with four employees, two of them are with employee role and the other two are admins. There are four departments, four projects and five towns. All realations are applied. When the app is up and running you can register a new user if you wish and explore.

# Database Diagram
![FinalDiagram](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/114e4b31-7767-4ae7-b09e-26d3950e3ff4)

# Home Page before Login
![HomePageNotLogin](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/1c2a3ed5-1ce3-4d2e-b30c-6fbc53b72df5)

# Home Page after Login
![HomePageLogedInUser](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/e0b5bbd9-804f-4010-ae29-2c6db1d80e8f)

# Register page
![RegisterPage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/6df1cc15-bb82-41cc-ab0d-e57106859e45)

# Login page
![LoginPage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/16914038-cdb0-4e40-b115-fd28244fa9b1)

# Employee Details page
![EmployeeDetailsPage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/50708715-9917-4ce8-bce1-e84005a47690)

# Edit Employee page
![EditEmployeePage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/b54f5c5b-7943-4797-bb88-7fbe50eb97b6)

# All Projects you are assigned to
![AllAssignedProjects](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/f2766423-aa65-40e8-918b-7f20e890d9b3)

# Employee pages
### All Departments and Department Details pages
<p>
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/c1b1071d-4dbe-4697-8fec-bdd4ea137637" width="49%" />
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/0b60a34f-8d51-4caa-8b5d-a3fb381b9ba3" width="49%" />
</p>

### All Towns and Town Details pages
<p>
  <img src="https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/06c28c8a-aeb3-4d40-91c1-2a23c07d50c5" width="49%" />
  <img src="https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/599ab75c-bd87-44f4-a13a-a84a52bcbb2e" width="49%" />
</p>

### All Projects and Project Details pages
<p>
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/7fc80bfa-98c1-499d-a662-44772b1c4995" width="49%" />
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/efa48608-2f7d-41c4-a27d-6f7c62b19a9c" width="49%" />
</p>

### All Roles page
![AllRoles](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/c8c36ac1-3724-470a-9650-19ef4234ac39)

# Admin pages
### Admin profile different from the current logged in admin
![DifferentAdminProfilePage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/7756adcf-ac35-4ee8-8a8a-ecd5102b41cc)


### All Departments page
![AllDepartmentsPage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/d236742b-89b2-4070-a124-85adad2bba54)

### All Employees in the current Department page
![AllEmployeesInDepartmentPage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/3b3878de-dbc0-41f9-a0e1-4e0059034219)

### Assign Employee from one Project to another
![MoveEmployeesFromDepartmentsPage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/f68f8d16-acc3-4560-9fea-699cab576899)

### Department Details and Edit pages
<p>
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/9dc63471-0112-4531-ba38-d6f1f19a089c" width="49%" />
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/bc07fd90-c753-4ab5-a45d-c8dc6c289add" width="49%" />
</p>

### Project pages

All Projects and Project Details pages
<p>
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/2e849097-05d4-4b68-9af8-be5b5da46caf" width="49%" />
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/d8b007e2-0346-4312-9d2c-f547d12e4e46" width="49%" />
</p>

Add and Edit Project pages
<p>
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/f34eb26f-1d5b-435a-9909-a31337110d7c" width="49%" />
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/267f6346-7f33-46eb-80fe-c65b679ace2d" width="49%" />
</p>

### Town pages

All Towns and Town Details pages
<p>
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/42b295bb-b315-493b-8a8c-950535e8b60b" width="49%" />
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/b8a57bf3-d3ee-4e95-b3e0-f55e665f1cba" width="49%" />
</p>

Add and Edit Town pages
<p>
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/2623c86b-da7f-4349-b575-aaf8b5f7edc3" width="49%" />
  <img src"https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/49f01b2c-d408-437f-96b9-24c0be97032a" width="49%" />
</p>

### Roles pages

All Roles page
![AllRolesPage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/4213ba60-fe09-49aa-afff-bbdc5ba70f18)

Create Role page
![CreateRolePage](https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/12fc3045-275a-47ae-bf97-50d358c9afa9)

# ✔️ Sweet alert messages
For all edit, add, delete pages there is a sweet alert message.

<p>
  <img src="https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/455387b3-6225-49e3-b9db-83865212a172" width="49%" />
  <img src="https://github.com/StanchosCodes/Immedis-internship-2023-hcm-Stanislav-Stamatov/assets/102748080/4d6bc9ca-04d7-451f-b03b-6b054f1dca4c" width="49%" />
</p>

# 🧑‍💻 Author
[Stanislav Stamatov](https://www.linkedin.com/in/stanislav-stamatov-402647255)

# 👍 Feedback would be appreciated
If you like my project give it a star. ⭐
