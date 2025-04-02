# IMDB Drinks - Ratings Module

A self-contained MVVM-based Ratings & Reviews module for the IMDB Drinks application. This module is designed to be independent, maintainable, and easily integrated with the main product system.

## Features

- **MVVM Architecture**: Implements the Model-View-ViewModel pattern for clear separation of concerns.
- **User Reviews**: Allows users to submit and view reviews for different drinks.
- **Rating System**: Enables users to rate drinks on a predefined scale.

## Getting Started

### Installing MySQL

The Ratings Module requires a MySQL database for storing and retrieving ratings data. Follow these steps to install and configure MySQL:

#### Install MySQL Server
  - Download the installer from the [MySQL Downloads page](https://dev.mysql.com/downloads/installer/).
  - Run the installer and choose the *Custom* setup.
  - Install MySql Server and MySql Workbench
  - Leave the configuration for MySql Server as default and make sure you use an easy to remember password
  - Open MySql Workbench and connect to your server
  - Create a new schema(database), preferably name it 'imdb'

### Creating Tables

After creating the the database, you need to set up the necessary tables.
  ```

CREATE TABLE `ratings` (
  `RatingID` int NOT NULL AUTO_INCREMENT,
  `ProductID` int NOT NULL,
  `UserID` int NOT NULL,
  `RatingValue` varchar(45) DEFAULT NULL,
  `RatingDate` datetime DEFAULT NULL,
  `IsActive` tinyint DEFAULT NULL,
  PRIMARY KEY (`RatingID`),
  UNIQUE KEY `ratingid_UNIQUE` (`RatingID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


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


  