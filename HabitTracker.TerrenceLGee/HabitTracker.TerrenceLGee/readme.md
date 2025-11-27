# Habit Tracker App

C# application written on Ubuntu 25.10 using JetBrains Rider.

Console application to track/log habits to help you stay on top of the things most important to you.

Created following the curriculum of [C# Academy](https://www.thecsharpacademy.com/)

# Project Requirements:

- [x] This is an application where you’ll log occurrences of a habit.
- [x] This habit can't be tracked by time (I changed this as to allow the user to track by different units of measurement)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [x] Follow the DRY Principle, and avoid code repetition.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works. 

# Features
- Creates a SQLite database if it does not already exist when starting the program.
- Creates the relevant tables in the database if they do not already exist.
- Multi-user has the ability to add a simple user so that more than one person may track their habits.
- Allows a user to Add, Update, Delete, and View their habits as well as View a report on a particular habit.
- Also allows the user to add an optional comment for each habit logged.
- Implements logging to a file in the project's directory in case of errors, while also handling exceptions.

# Challenges Faced When Implementing This Project
- SQLite. Putting together queries to extract the needed information from the database.
- ADO.NET. For example, how to deal with possible NULL values being both Inserted and also Retrieved from the database.
- The reporting functionality. What to report on and also how to deal with a case for example, if the user logged the quantity of the more than one unit of measurement of a habit (i.e. Mintutes/Hours);
- Coding the application in general. There is always a desire to create clean, readable and self documenting code.
- Separation of concerns. Tried to separate different functionalities into their own classes/namespaces while also using them together in order to have a fully functioning application.

# What Was Learned Implementing This Project
- The importance of planning out a project before writing a single line of code. In planning this project I realized how essential it is to plan out all classes, properties etc. before writing a single line of code as this makes actual coding much easier as you already have a blueprint of what to code.
- The importance of reading the documentation. I spent quite a few hours going back and forth between implementing a feature and reading documentation on how to do so, everything from SQLite queries to how to use ADO.NET. This was one of the most important parts of this project as knowing how to read documentation and find the answers you are looking for is crucial for success.
- The importance of external libraries to help with implementing your vision for an application, particularly Spectre.Console for the actual console portion of this application.

# Areas To Improve Upon
- The ability to write more concise and reusable methods and classes.
- The ability to add more complexity into an application as time goes on, while not writing bloated or unnecessary code.
- Most importantly to continue to improve logic and problem-solving abilities and skills and get deeper into the C# programming language and the .NET ecosystem.

# Technologies Used
- [Spectre.Console](https://spectreconsole.net/)
- [ADO.NET](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview)
- [Serilog](https://serilog.net/)
- [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration)

# Some Resources Used
- [C# Academy](https://www.thecsharpacademy.com/)
- [SQLite Documentation](https://sqlite.org/docs.html)
- [Microsoft's Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Stack Overflow](https://stackoverflow.com/questions)
- [C# Forums](https://csharpforums.net/forums/c-general-discussion.79/)




