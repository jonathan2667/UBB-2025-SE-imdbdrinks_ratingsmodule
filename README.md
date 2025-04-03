# IMDB Drinks - Ratings Module

A self-contained MVVM-based Ratings & Reviews module for the IMDB Drinks application. This module is designed to be independent, maintainable, and easily integrated with the main product system.

## Features

- **MVVM Architecture**: Implements the Model-View-ViewModel pattern for clear separation of concerns.
- **User Reviews**: Allows users to submit and view reviews for different drinks.
- **Rating System**: Enables users to rate drinks on a predefined scale.

### Important Information

It is important to know that a rating can have multiple reviews, but not the inverse.

You must first select a rating and then add a review — really important.

To see the reviews associated with each rating, select a new rating in the rating list view.


## Demo

[Watch demo video](./imdbdrinks_ratingsmodule/Assets/ISS_imdb.mp4)


## Getting Started

### Installing MySQL

The Ratings Module requires a MySQL database for storing and retrieving ratings data. Follow these steps to install and configure MySQL:

#### Install MySQL Server

1. Download the MySQL installer from the [MySQL Downloads page](https://dev.mysql.com/downloads/installer/). *(Installer size: 2.4M from the official MySQL site)*
2. Run the installer and choose the **Custom** setup.
3. Install **MySQL Server** (version 8.0.41) and **MySQL Workbench** (version 8.0.40).
   - In MySQL Workbench, click the green button to push **MySQL Server** and **MySQL Workbench** on the right side.
   - For the password, it is important to use `1234`, otherwise you will have to change the SQL connections credentials in the project. 
4. Once MySQL Workbench is open, locate your SQL connection and click to open it.
5. On the left panel in MySQL Workbench, right-click the **Schemas** section and select **Create Schema** (by pressing right-click) and preferably name it 'imdb'.
6. Open a new SQL query tab (or use an existing one). Double-click the `imdb` database schema on the left so that the query executes in the correct context, then paste and execute the SQL code provided below.
7. Refresh the schema view. You should now see two new tables.
8. Update your password in `MainWindow.xaml.cs`; if your database is not named `imdb` modify the schema name as needed.
9. To reset your database, first drop the `reviews` table, then drop the `ratings` table using an SQL query.

### Creating Tables

After creating the the database, you need to set up the necessary tables.
  ```

CREATE TABLE `ratings` (
  `RatingID` int NOT NULL AUTO_INCREMENT,
  `ProductID` int NOT NULL,
  `UserID` int NOT NULL,
  `RatingValue` double DEFAULT NULL,
  `RatingDate` datetime DEFAULT NULL,
  `IsActive` tinyint DEFAULT NULL,
  PRIMARY KEY (`RatingID`),
  UNIQUE KEY `ratingid_UNIQUE` (`RatingID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE `reviews` (
  `ReviewID` int NOT NULL AUTO_INCREMENT,
  `RatingID` int NOT NULL,
  `UserID` int DEFAULT NULL,
  `Content` text,
  `CreationDate` datetime DEFAULT NULL,
  `IsActive` tinyint DEFAULT NULL,
  PRIMARY KEY (`ReviewID`),
  KEY `RatingID_idx` (`RatingID`),
  CONSTRAINT `RatingID` FOREIGN KEY (`RatingID`) REFERENCES `ratings` (`RatingID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

  ```

### Update the connection string

In the "MainWindow.xaml.cs" file update the string "connection" with your credentials


### AI Integration

Please contact the owner for the API key :)


  
