# Event Catering Management System


## Description:
The Event Catering Management System is a web application designed to streamline the process of managing events, menus, and food items for catering businesses. It utilizes .NET Entity Framework to interact with a database consisting of three main tables: Event, Menu, and Food, along with a bridging table called MenuxFood.

## Tables:

1. **Event:**
   - This table stores information about events such as event ID, event name, date, time, location, and any additional details.

2. **Menu:**
   - The Menu table contains details about different menus offered by the catering service, including menu ID, name, description, and price.

3. **Food:**
   - The Food table holds information about individual food items available for catering, such as food ID, name, description, and price.

4. **MenuxFood (Bridging Table):**
   - This bridging table establishes a many-to-many relationship between menus and food items. It links menu IDs with corresponding food IDs to indicate which food items are included in each menu.

## Technologies Used:
- .NET Framework
- Entity Framework
- C#
- HTML/CSS
- JavaScript
- SQL Server

## Features:
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
  
## Installation:
1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Restore NuGet packages if necessary.
4. Build and run the application.

## Contributing:
- Contributions are welcome! Feel free to fork the repository and submit pull requests with your enhancements or bug fixes.

## Authors:
- Suyash Kulkarni
   - Worked on Event and Menu module.
- Deep Patel
   - Worked on Food module.
- We together worked on the relation part between menu and food and how to design it and what functions we will need to show what.
