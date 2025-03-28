# FinanceTracker

**FinanceTracker** is a personal finance tracking web application built with .NET 9 and ASP.NET Core. It leverages Entity Framework Core with SQLite for data persistence, uses Bootstrap for a responsive UI, and integrates Chart.js for visual reporting. The application allows users to manage transactions (create, edit, delete), search and filter their entries, sort data by various criteria, and view analytical dashboards that display income, expenses, and balance trends. It also supports data export functionality (CSV).

## Table of Contents

- [Overview](#overview)
- [Tools & Technologies](#tools--technologies)
- [Features & Functionalities](#features--functionalities)
- [Project Structure](#project-structure)
- [Setup & Installation](#setup--installation)
- [Running the Application](#running-the-application)
- [Database Migrations](#database-migrations)
- [Usage](#usage)
- [Future Enhancements](#future-enhancements)
- [License](#license)

## Overview

FinanceTracker is designed for users who want a lightweight, yet powerful way to track personal finances. The application provides an intuitive interface to record transactions (both income and expense), analyze spending patterns through charts, and export data for further analysis.

## Tools & Technologies

- **.NET 9 / ASP.NET Core MVC**  
  The backbone of the application, offering a robust and scalable framework for building web applications.

- **Entity Framework Core (EF Core)**  
  Used for Object-Relational Mapping (ORM) and database interactions. EF Core simplifies data access with LINQ and supports code-first migrations.

- **SQLite**  
  A lightweight, file-based database that requires no additional server setup. Ideal for development and small-scale deployments.

- **Bootstrap**  
  Provides responsive and modern UI components to ensure the application looks good on desktop, tablet, and mobile devices.

- **Chart.js**  
  A JavaScript library for creating interactive charts. Integrated into the dashboard to visualize income vs. expense trends over time.

- **CSV Export**  
  Built-in functionality to export transaction data as a CSV file for external analysis.

## Features & Functionalities

### Transaction Management

- **CRUD Operations:**  
  - **Create:** Add new transactions including date, description, amount, and type (Income/Expense).
  - **Read:** View a list of all transactions in a searchable, filterable, and sortable table.
  - **Update:** Edit existing transactions.
  - **Delete:** Remove transactions after confirmation.

- **Search & Filter:**  
  Users can search transactions by description, and filter or sort results by date or amount.

- **Sorting:**  
  The table supports sorting by date and amount. Under the hood, a value conversion is used to handle sorting on decimal values by converting them to a double (using EF Core's value converters).

### Enhanced Reporting & Analytics

- **Dashboard Overview:**  
  Displays summary information:
  - **Total Income**
  - **Total Expense**
  - **Balance**

- **Interactive Charts:**  
  A Chart.js-powered line chart visualizes income vs. expense trends over time.

- **Data Export:**  
  Option to export the list of transactions as a CSV file for further analysis or record-keeping.

### Future Enhancements
  - **User Authentication** Integrate ASP.NET Core Identity to support multiple users and secure data access.
  - **Recurring Transactions** Add support for recurring transactions (e.g., monthly bills, salary).
  - **Advanced Reporting** Build additional reports and charts for deeper financial insights.
  - **Mobile Optimization** Enhance responsive design for improved mobile experience.


