<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&height=280&color=0:11998E,50:38EF7D,100:0BAB64&text=E-Learning%20Platform&fontColor=ffffff&fontSize=48&fontAlignY=38&desc=ASP.NET%20Core%208%20|%20Clean%20Architecture%20|%20CQRS&descAlignY=58&animation=fadeIn"/>
</p>

<h1 align="center">🎓 E-Learning Platform API</h1>

<h3 align="center">
Production-Ready Learning Management System built with ASP.NET Core 8
</h3>

<p align="center">
<img src="https://readme-typing-svg.herokuapp.com?font=Fira+Code&weight=600&size=22&pause=1000&color=00C853&center=true&vCenter=true&width=750&lines=Learning+Management+System;ASP.NET+Core+8;Clean+Architecture;CQRS+%2B+MediatR;Redis+Cache;Cloud+Storage;JWT+Authentication;RESTful+API"/>
</p>

<p align="center">

<a href="https://github.com/ziadyasserdev/ElearningPlatform">
<img src="https://img.shields.io/badge/Repository-181717?style=for-the-badge&logo=github&logoColor=white"/>
</a>

<a href="#">
<img src="https://img.shields.io/badge/API-Demo-00C853?style=for-the-badge"/>
</a>

<img src="https://img.shields.io/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>

<img src="https://img.shields.io/badge/Clean-Architecture-blue?style=for-the-badge"/>

<img src="https://img.shields.io/badge/CQRS-MediatR-success?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Redis-Cache-red?style=for-the-badge&logo=redis&logoColor=white"/>

<img src="https://img.shields.io/badge/License-MIT-orange?style=for-the-badge"/>

</p>

---

# 📖 Overview

The **E-Learning Platform API** is a production-ready Learning Management System (LMS) built using **ASP.NET Core 8** following **Clean Architecture** and **CQRS** principles.

The platform enables instructors to create, publish, and manage educational content while providing students with an interactive learning experience through enrollments, lessons, videos, assignments, exams, comments, and reviews.

Designed with scalability, maintainability, and performance in mind, the project incorporates enterprise-level backend practices and modern software architecture.

---

# ✨ Features

## 👨‍🏫 Instructor Management

- Create and manage courses
- Publish or unpublish courses
- Organize course content
- Track enrolled students
- Manage assignments and exams

---

## 👨‍🎓 Student Experience

- Browse available courses
- Enroll in courses
- Track learning progress
- Watch video lessons
- Submit assignments
- Take exams
- Rate and review courses

---

## 📚 Course Management

- Course CRUD Operations
- Featured Courses
- Popular Courses
- Latest Courses
- Course Publishing Workflow
- Course Search
- Pagination & Filtering

---

## 🗂️ Categories

- Create Categories
- Update Categories
- Delete Categories
- Search Categories
- Pagination
- Course Classification

---

## 📖 Sections & Lessons

- Course Sections
- Lesson Management
- Lesson Ordering
- Rich Content Organization
- Structured Learning Path

---

## 🎥 Video Content

- Video Upload
- Cloud Media Storage
- Streaming Support
- Video Progress Tracking
- Resume Watching
- Lesson Completion Tracking

---

## 📝 Assignments

- Assignment Creation
- Student Submission
- Submission Tracking
- Instructor Review
- Assignment Management

---

## 📑 Exams

- Exam Creation
- Question Management
- Student Attempts
- Score Calculation
- Exam Results

---

## 💬 Comments & Reviews

- Course Reviews
- Ratings
- Student Comments
- Content Moderation
- Review Management

---

## 📈 Student Progress

- Enrollment Tracking
- Video Progress
- Lesson Completion
- Course Completion
- Learning Statistics

---

## ☁️ Cloud Storage

- Secure Media Upload
- Cloud File Management
- Optimized Content Delivery
- Scalable Storage Solution

---

## ⚡ Performance

- Redis Cache
- Response Optimization
- Rate Limiting
- Pagination
- Efficient Database Queries

---

## 🔒 Security

- JWT Authentication
- ASP.NET Identity
- Role-Based Authorization
- Secure API Endpoints
- Input Validation
- FluentValidation Pipeline

---

## 🏗️ Enterprise Architecture

The project follows modern backend development practices including:

- Clean Architecture
- CQRS using MediatR
- Repository Pattern
- Result Pattern
- AutoMapper
- Entity Framework Core
- Redis Cache
- RESTful APIs
- Dependency Injection
- Validation Pipeline
- ---

# 🏗️ Architecture

The project follows **Clean Architecture** to ensure maintainability, scalability, testability, and separation of concerns.

```text
                 ┌──────────────────────┐
                 │     Presentation     │
                 │   ASP.NET Core API   │
                 └──────────┬───────────┘
                            │
                            ▼
                 ┌──────────────────────┐
                 │     Application      │
                 │ CQRS • MediatR       │
                 │ DTOs • Validators    │
                 │ Business Logic       │
                 └──────────┬───────────┘
                            │
                            ▼
                 ┌──────────────────────┐
                 │       Domain         │
                 │ Entities             │
                 │ Interfaces           │
                 │ Domain Rules         │
                 └──────────┬───────────┘
                            │
                            ▼
                 ┌──────────────────────┐
                 │   Infrastructure     │
                 │ EF Core              │
                 │ SQL Server           │
                 │ Redis Cache          │
                 │ Cloud Storage        │
                 │ Identity             │
                 └──────────────────────┘
```

---

# 🛠️ Tech Stack

## Programming Language

<p>

<img src="https://skillicons.dev/icons?i=cs"/>

</p>

---

## Backend Technologies

<p>

<img src="https://skillicons.dev/icons?i=dotnet"/>

<img src="https://skillicons.dev/icons?i=redis"/>

<img src="https://skillicons.dev/icons?i=mysql"/>

</p>

<p>

<img src="https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Entity_Framework_Core-68217A?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Clean_Architecture-blue?style=for-the-badge"/>

<img src="https://img.shields.io/badge/CQRS-MediatR-success?style=for-the-badge"/>

<img src="https://img.shields.io/badge/JWT-Authentication-black?style=for-the-badge"/>

<img src="https://img.shields.io/badge/ASP.NET_Identity-purple?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Redis_Cache-red?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Cloud_Storage-4285F4?style=for-the-badge"/>

<img src="https://img.shields.io/badge/REST_API-success?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Rate_Limiting-orange?style=for-the-badge"/>

<img src="https://img.shields.io/badge/FluentValidation-green?style=for-the-badge"/>

<img src="https://img.shields.io/badge/AutoMapper-yellow?style=for-the-badge"/>

</p>

---

## Tools

<p>

<img src="https://skillicons.dev/icons?i=git"/>

<img src="https://skillicons.dev/icons?i=github"/>

<img src="https://skillicons.dev/icons?i=visualstudio"/>

<img src="https://skillicons.dev/icons?i=postman"/>

</p>

---

# 📂 Project Structure

```text
ElearningPlatform
│
├── ElearningPlatform.API
│
├── ElearningPlatform.Application
│
├── ElearningPlatform.Domain
│
├── ElearningPlatform.Infrastructure
│
└── ElearningPlatform.Persistence
```

---

# 📦 Core Modules

| Module | Description |
|---------|-------------|
| 👤 Authentication | User registration, login, JWT authentication, refresh tokens, and role-based authorization. |
| 🏷 Categories | Organize and classify courses into categories with search and pagination support. |
| 📚 Courses | Complete course management including creation, publishing, updates, search, featured courses, and popularity tracking. |
| 📖 Sections | Organize courses into structured sections for better learning flow. |
| 📄 Lessons | Manage lessons, learning materials, and lesson ordering within sections. |
| 🎥 Videos | Upload educational videos, track progress, stream content, and manage cloud-based media. |
| 📝 Assignments | Create assignments, manage submissions, and review student work. |
| 📑 Exams | Build exams, manage questions, evaluate attempts, and calculate scores. |
| 🎓 Enrollments | Student enrollment management with course access control and progress tracking. |
| 💬 Comments | Interactive discussions between students and instructors. |
| ⭐ Reviews | Course ratings, reviews, moderation, and feedback management. |
| 📊 Analytics | Platform insights, enrollment statistics, learning progress, and course performance reports. |

---

# 🚀 Platform Highlights

## 🎓 Smart Learning Experience

Students can:

- Browse courses
- Enroll instantly
- Watch videos
- Complete lessons
- Submit assignments
- Take exams
- Track their learning journey

---

## 👨‍🏫 Instructor Dashboard

Instructors can:

- Create Courses
- Publish Courses
- Upload Videos
- Create Assignments
- Create Exams
- Monitor Students
- Review Progress

---

## 📈 Learning Progress Tracking

Track:

- Video Progress

- Lesson Completion

- Course Completion

- Enrollment Status

- Student Performance

---

## ⚡ High Performance

Optimized using:

- Redis Cache

- Efficient Queries

- Pagination

- Response Caching

- Rate Limiting

---

## ☁️ Cloud Media Storage

Supports:

- Video Upload

- Secure File Storage

- Optimized Media Delivery

- Scalable Storage Architecture

---

## 🔒 Security

✔ JWT Authentication

✔ ASP.NET Identity

✔ Role-Based Authorization

✔ Secure API Endpoints

✔ Validation Pipeline

---

# 🌟 Why This Project?

This project demonstrates enterprise-level backend development practices and showcases the implementation of a scalable Learning Management System (LMS).

Key engineering practices include:

- Clean Architecture

- CQRS with MediatR

- Dependency Injection

- Entity Framework Core

- Redis Cache

- RESTful API Design

- Validation Pipeline

- Cloud Storage Integration

- Role-Based Authorization

- Performance Optimization

- Scalable Software Architecture

------

# 🚀 Getting Started

Follow these steps to run the project locally.

## 1️⃣ Clone the Repository

```bash
git clone https://github.com/ziadyasserdev/ElearningPlatform.git
```

```bash
cd ElearningPlatform
```

---

## 2️⃣ Configure the Database

Update the connection string inside:

```text
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ElearningPlatformDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

## 3️⃣ Configure Redis

Update your Redis configuration.

```json
"Redis": {
    "ConnectionString": "localhost:6379"
}
```

---

## 4️⃣ Configure Cloud Storage

Add your cloud storage credentials.

Example:

```json
"CloudStorage": {
    "Provider": "Cloudinary",
    "CloudName": "...",
    "ApiKey": "...",
    "ApiSecret": "..."
}
```

---

## 5️⃣ Apply Migrations

```bash
dotnet ef database update
```

---

## 6️⃣ Run the Project

```bash
dotnet run
```

---

## 7️⃣ Open Swagger

```text
https://localhost:xxxx/swagger
```

---

# 📡 API Modules

The API is organized into feature-rich modules following **Clean Architecture** and **CQRS** principles.

| Module | Description |
|---------|-------------|
| 🔐 Authentication | User registration, login, JWT authentication, refresh tokens, and role-based authorization. |
| 👤 Users | User profile management, permissions, and account operations. |
| 🏷 Categories | Manage course categories with CRUD operations, filtering, searching, and pagination. |
| 📚 Courses | Create, update, publish, archive, feature, search, and manage complete course lifecycle. |
| 📖 Sections | Organize course content into structured sections. |
| 📄 Lessons | Manage lessons, learning materials, ordering, and lesson visibility. |
| 🎥 Videos | Upload videos, manage cloud media, stream content, and track watching progress. |
| 🎓 Enrollments | Student enrollment workflows, enrollment status, and learning access management. |
| 📝 Assignments | Assignment creation, submissions, grading workflow, and submission tracking. |
| 📑 Exams | Question management, exam attempts, score calculation, and evaluation. |
| 💬 Comments | Student discussions, instructor interaction, moderation, and engagement. |
| ⭐ Reviews | Ratings, reviews, moderation, review history, and course feedback. |
| 📈 Progress Tracking | Monitor lesson completion, video progress, and overall course completion. |
| ⚡ Redis Cache | Improve application performance through distributed caching. |
| ☁️ Cloud Storage | Store and deliver videos and educational resources securely. |
| 📊 Reports & Analytics | Course performance, enrollments, engagement metrics, and learning analytics. |

---

# 🎯 Key Highlights

### 📚 Learning Management System

A complete backend solution for managing online education platforms.

---

### 🎥 Video Learning

- Cloud Media Storage

- Video Streaming

- Video Progress Tracking

- Resume Watching

---

### 🎓 Student Experience

- Browse Courses

- Enroll in Courses

- Watch Lessons

- Submit Assignments

- Take Exams

- Review Courses

---

### 👨‍🏫 Instructor Experience

- Create Courses

- Publish Courses

- Upload Videos

- Manage Assignments

- Build Exams

- Monitor Students

---

### ⚡ Performance Optimization

- Redis Cache

- Pagination

- Efficient Queries

- Rate Limiting

- Optimized Responses

---

### 🔒 Security

- JWT Authentication

- ASP.NET Identity

- Role-Based Authorization

- Secure REST APIs

- FluentValidation

---

# 📸 API Preview

<p align="center">

<img src="images/swagger-home.png" width="900"/>

</p>

> Replace this image with your Swagger homepage screenshot.

---

# 📈 Future Improvements

- Docker Support

- Kubernetes Deployment

- CI/CD Pipeline

- Unit Testing

- Integration Testing

- Event-Driven Architecture

- Microservices

- Azure Deployment

- AI-Based Course Recommendations

- Real-Time Notifications using SignalR

---

# 🤝 Contributing

Contributions are always welcome.

If you'd like to improve the project:

1. Fork the repository

2. Create a feature branch

3. Commit your changes

4. Push your branch

5. Open a Pull Request

---

# 📄 License

This project is licensed under the MIT License.

---

# 👨‍💻 Author

## Zeyad Yasser

Backend .NET Developer

Passionate about building scalable backend systems using ASP.NET Core, Clean Architecture, and modern software engineering principles.

<p align="center">

<a href="mailto:ziadyasser.dev@gmail.com">
<img src="https://img.shields.io/badge/Email-EA4335?style=for-the-badge&logo=gmail&logoColor=white"/>
</a>

<a href="https://www.linkedin.com/in/ziad-yasser-6155b828b">
<img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"/>
</a>

<a href="https://github.com/ziadyasserdev">
<img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white"/>
</a>

<a href="https://wa.me/201033724845">
<img src="https://img.shields.io/badge/WhatsApp-25D366?style=for-the-badge&logo=whatsapp&logoColor=white"/>
</a>

</p>

<p align="center">

📧 ziadyasser.dev@gmail.com &nbsp;|&nbsp;
📱 +20 103 372 4845 &nbsp;|&nbsp;
📍 Egypt

</p>

---

<p align="center">
<img src="https://capsule-render.vercel.app/api?type=waving&height=120&section=footer&color=0:11998E,50:38EF7D,100:0BAB64"/>
</p>

<h3 align="center">

⭐ If you found this project useful, don't forget to leave a star! ⭐

</h3>
