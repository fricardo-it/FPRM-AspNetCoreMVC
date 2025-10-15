# 🏢 FP Rental Management (FPRM)

**Course:** 420-DW4-AS – Web Server Applications Development II  
**Session:** Winter 2024  
**Group:** 7270  
**Author:** [Francisco Ricardo Andraschko](#)  
**Teacher:** Quang Hoang Cao  
**Date:** April 25, 2024

---

## 📌 Project Description

**FPRM (FP Rental Management)** is a property rental management system developed using **ASP.NET Core MVC**.  
Its main goal is to **simplify and optimize the rental process** for property owners, managers, and tenants through an intuitive, role-based interface.

### 👤 Roles & Functionalities

#### 🛠️ Administrators / Owners
- Full user management (add/edit/delete)
- Control of all buildings, apartments, and rentals
- Management of internal messages exchanged between users

#### 🧑‍💼 Property Managers
- CRUD operations for buildings and apartments
- Review and approve appointment requests
- Communicate through internal messaging
- Forward events to property owners when necessary

#### 🧑 Tenants
- Browse available properties and apartments
- Schedule visits to properties
- Track appointments and communicate with managers

Built-in **authentication** and **access control** ensure a secure, personalized experience for each user.

---

## 🧭 Project Phases

### 1. 📊 **Analysis**
Defined and interpreted customer requirements to support a structured solution design.

- **Property Owners / Admins**: Manage user accounts, site administration  
- **Property Managers**: Manage buildings, apartments, appointments, and communication  
- **Tenants**: Create accounts, view apartments, make appointments, send messages

---

### 2. 🧱 **Design**
Used the **Code First** approach with Entity Framework to design the database schema directly in the development environment.

**Database Schema Overview**:

- `Building` (Id, Name, Address, Description, etc.)  
- `Apartment` (Id, BuildingId, PostalCode, Type, RentAmount, etc.)  
- `User` (Id, Name, Email, Role, etc.)  
- `Appointment` (Id, ApartmentId, TenantId, Date, Status, etc.)  
- `Message` (Id, SenderId, ManagerId, Timestamp, Content, etc.)  
- `Rental` (Id, ApartmentId, TenantId, RentDates, MonthlyRent, etc.)

---

### 3. 💻 **Implementation**

**Technologies & Tools**
- **Back-End:** ASP.NET Core MVC, C#, Entity Framework (Code First), SQL Server  
- **Front-End:** HTML, CSS, JavaScript, Bootstrap  
- **Architecture:** MVC pattern, Dependency Injection, Middlewares, Pipelines  
- **Security:** Identity with Roles & Claims

This stack ensures a powerful, secure, scalable, and API-friendly application.  
The **Code First** methodology keeps business rules aligned with the evolving database structure.

---

### 4. 🧪 **Testing**

| Feature | Role | Result |
|---------|------|--------|
| Authentication | All | ✅ Successful login/logout |
| User Management | Admin | ✅ CRUD works |
| Building/Apartment CRUD | Manager | ✅ Works, with minor image collection issue |
| Appointments | Manager/Tenant | ✅ Schedule, Accept/Reject, Status updates |
| Messaging | All | ✅ Working, except Admin reply not implemented |
| Public Navigation | Unlogged | ✅ Filters work; minor UI checkbox issue |

---

## 📝 Conclusion

This project consolidated key concepts learned during the AEC IT Programming course:

- **MVC Pattern**
- **Dependency Injection**
- **Middlewares & Pipelines**
- **Entity Framework (Code First)**
- **Authentication & Authorization with Roles and Claims**

It provided hands-on experience with **real-world technologies** and practices used in professional software development.  
As a result, this project strengthened my understanding of backend/frontend integration, database design, and scalable architectures.

---

## 📚 References

- [Microsoft Documentation](https://docs.microsoft.com/)  
- [ASP.NET Core – Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/)  
- [LINQ – Microsoft Learn](https://learn.microsoft.com/pt-br/dotnet/csharp/linq/)  
- [Eduardo Pires (MVP)](https://www.desenvolvedor.io)  
- [Stack Overflow](https://stackoverflow.com/)  
- [Bootstrap](https://getbootstrap.com/)  
- [Free HTML Templates](https://free-css.com/)

---

## 🧠 Author

**Francisco Ricardo Andraschko**  
Student – AEC IT Programming, Canada  
GitHub: [@francisco-ricardo](#)  
📅 April 2024

---

## 📌 License

This project was developed for academic purposes. You are free to explore and use it as a learning resource.
