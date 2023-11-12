<div align="center">
<img src="wwwroot/techInSight.png">
</div>

## Project Overview 💻
The **TechInSight** project is a feature-rich ASP.NET Core web application designed for managing a blogging platform. It encompasses user authentication, article creation, commenting, role-based authorization, and an admin panel for content management.

## Table of Contents

1. [Admin Panel](#admin-panel)
2. [Article Management](#article-management)
3. [User Authentication](#user-authentication)
4. [Rating System](#rating-system)
5. [Export to CSV](#export-to-csv)
6. [Installation](#installation)
    - [Installation Scripts](#installation-scripts)
    - [Code First Methodology](#code-first-methodology)
    - [Database Context and DbSet](#database-context-and-dbset)
7. [Usage](#usage)
    - [Let's say that you're the Administrator](#lets-say-that-youre-the-administrator)
8. [BlogASP API Documentation](#blogasp-api-documentation)
    - [Overview](#overview)
    - [Base URL](#base-url)
    - [API Endpoints](#api-endpoints)
        - [1. POST User](#1-post-user)
        - [2. PUT/PATCH Article](#2-putpatch-article)
        - [3. POST Article](#3-post-article)
        - [4. GET Articles](#4-get-articles)
        - [5. GET Users](#5-get-users)
        - [6. DELETE User](#6-delete-user)
        - [7. DELETE Article](#7-delete-article)
    - [Conclusion](#conclusion)


## Key Features 🔑 📊 📈

## Admin Panel

The Admin Panel provides a comprehensive interface for managing users and articles. Key features include:

- **User Management:**
  - View all users with their roles and creation dates.
  - Change user roles (Privileged, Public, Administrator, Disabled).
  - Disable users with the option to move them to a pending users section.

- **Article Management:**
  - View all articles with details such as title, category, stars, and status.
  - Disable or enable articles.
  - Edit article content.

- **Export to CSV:**
  - Export lists of users, pending users, articles, or all data to CSV files.

## Article Management

The ArticleController facilitates various operations related to articles, including:

- **Creation:**
  - Authors can create articles with titles, categories, and descriptions.
  - Automatic assignment of creation and update timestamps.

- **Details:**
  - View detailed information about a specific article.
  - Enable or disable article stars.
  - View related articles and most-starred articles.

- **Comments:**
  - Users can add comments to articles.
  - Comments display the author, creation, and update timestamps.

## User Authentication

The project employs ASP.NET Core Identity for user authentication. Key functionalities include:

- **Login:**
  - Users can log in with their credentials.
  - Authentication cookies are used for user sessions.

- **Logout:**
  - Users can log out, terminating their sessions.

- **Signup:**
  - New users can create accounts with unique usernames and emails.
  - Passwords are securely hashed.

## Rating System

The RatingService manages the article rating system:

- **Star Rating:**
  - Users can toggle stars on articles.
  - Unique user ratings prevent duplicate stars.

## Repository Pattern

The project utilizes the repository pattern for data access:

- **ArticleRepository:**
  - Manages article-related database operations.
  - Supports creation, editing, deletion, and retrieval of articles.

- **UserRepository:**
  - Handles user-related operations.
  - Supports user creation, editing, and retrieval.

- **CommentRepository:**
  - Manages comments for articles.

## Installation 🔧 🔨

## Installation Scripts

`Clone the repository`
```bash
git clone https://github.com/ClaudioRSGit/BlogASP.git
```

`Update Database`
```bash
update-database
```


### Code First Methodology
Our software employs the "Code First" methodology in database design. This approach involves defining the data model using C# classes, referred to as entities. The database schema is then automatically generated based on these classes when the application runs for the first time. This streamlines the process of database creation and management.
 
### Database Context and DbSet
In our application, the Database Context serves as the bridge between the application and the database, our DAL. It's represented by the DatabaseContext class derived from DbContext. DbSet, on the other hand, represents a collection of entities in the context, corresponding to a table in the database. We use DbSet to query and manipulate data.

## Usage

### Lets say that you're the Administrator

`login system`
<br><br>
    <img src="images/login.png" style="min-width: 80%; height: 80%; width: 80%;" alt="Logo">
 

> placeholder

# BlogASP API Documentation

## Overview

The **BlogASP API** provides a set of endpoints to interact with the blogging platform programmatically. This allows developers to integrate and automate various functionalities, enhancing the flexibility and extensibility of the system.

## Base URL
- Base URL: `https://localhost:7144/api`

## API Endpoints

### 1. **POST User**
Create a new user via API.

**Endpoint:**
- `POST /Users`

**Request Body:**
```json
{
    "Username": "user created via API",
    "Name": "API_user",
    "Email": "api@api.com",
    "Password": "Admin123!",
    "Role": "public"
}
```

### 2. **PUT/PATCH Article**
Update an existing article via API.

**Endpoint:**
- `PUT/PATCH /Articles/{id}`

**Request Body:**
```json
{
  "title": "Updated Title By API",
  "category": "Updated Category By API",
  "description": "Updated Description By API",
  "picture": "https://engineering.brevo.com/wp-content/uploads/2023/07/API.jpeg"
}
```

### 3. **POST Article**
Create a new article via API.

**Endpoint:**
- `POST /Articles`

**Request Body:**
```json
{
    "Title": "Machine Learning Applications in Healthcare",
    "Description": "Exploring the transformative impact of machine learning in healthcare, from diagnostics to personalized treatment.",
    "Category": "Data Science",
    "UserName": "byAPI",
    "isDisabled": false,
    "Stars": 0
}
```
### 4. **GET Articles**
Retrieve a list of all articles via API.

**Endpoint:**
- `GET /Articles`

**Request Link:**
```bash
    https://localhost:7144/api/Articles
```

### 5. **GET Users**
Retrieve a list of all users via API.

**Endpoint:**
- `GET /Users`

**Request Link:**
```bash
    https://localhost:7144/api/Users
```

### 6. **DELETE User**
Delete an existing user via API.


**Endpoint:**
- `DELETE /Users/{userId}`

**Request Body:**
```bash
https://localhost:7144/api/Users/{userId}
```

### 6. **DELETE Article**
Delete an existing article via API.

**Endpoint:**
- `DELETE /Articles/{articleId}`

**Request Body:**
```bash
https://localhost:7144/api/Articles/{articleId}
```

## Conclusion

The **TechInSight** project is a robust and user-friendly blogging platform with extensive features for both content creators and administrators. It ensures data integrity, security, and seamless user interactions.