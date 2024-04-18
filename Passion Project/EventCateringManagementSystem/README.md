# Event Catering Management System

## Description
The Event Catering Management System is a web application designed to streamline the process of managing events, menus, and food items for catering businesses. It utilizes .NET Entity Framework to interact with a database consisting of three main tables: Event, Menu, and Food, along with a bridging table called MenuxFood.

## Tables

1. **Event:**
   - This table stores information about events such as event ID, event name, date, time, location, and any additional details.

2. **Menu:**
   - The Menu table contains details about different menus offered by the catering service, including menu ID, name, description, and price.

3. **Food:**
   - The Food table holds information about individual food items available for catering, such as food ID, name, description, and price.

4. **MenuxFood (Bridging Table):**
   - This bridging table establishes a many-to-many relationship between menus and food items. It links menu IDs with corresponding food IDs to indicate which food items are included in each menu.

## Technologies Used
- .NET Framework
- Entity Framework
- C#
- HTML/CSS
- JavaScript
- SQL Server

## Features
- **Event Management:**
  - Create, read, update, and delete events.
  - View event details including date, time, location, and associated menus.
  
- **Menu Management:**
  - Add, edit, and remove menus.
  - Include food items in menus and set prices.
  
- **Food Management:**
  - Maintain a database of food items with details such as name, description, and price.
  
- **Integration with Entity Framework:**
  - Utilizes .NET Entity Framework for seamless interaction with the SQL Server database.

## Installation
1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Restore NuGet packages if necessary.
4. Build and run the application.

## Contributing
- Contributions are welcome! Feel free to fork the repository and submit pull requests with your enhancements or bug fixes.

## Authors
- Suyash Kulkarni
   - Worked on Event and Menu module.
- Deep Patel
   - Worked on Food module.
- We together worked on the relation part between menu and food and how to design it and what functions we will need to show what.

### GitHub Repository
https://github.com/Suyash0028/http5226.git

### Self Evaluation

1. **MVP Completion**: 100/100
2. **Additional Features**: 80/100
3. **Administrative/Login Functionality**: 60/100
4. **README Documentation**: 100/100

### Project Reflection

During the development of the Event Catering Management System, we encountered various challenges and gained valuable insights. One of the significant learning experiences was understanding the intricacies of integrating .NET Entity Framework with the SQL Server database to efficiently manage data. Additionally, collaborating on the design and implementation of the many-to-many relationship between menus and food items provided us with a deeper understanding of database management.

Despite careful planning, there were deviations from our original development plan as we encountered unforeseen technical hurdles. However, through effective communication and problem-solving, we were able to adapt and implement alternative solutions to overcome these challenges.

Looking back, there are areas where we could have improved our approach, such as more thorough testing and documentation throughout the development process. In future projects, we aim to prioritize these aspects to ensure a smoother and more efficient development cycle.

Overall, working on the Event Catering Management System was a rewarding experience that allowed us to enhance our technical skills and collaborate effectively as a team.
