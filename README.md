# 🏥 Clinic Management System – ASP.NET Core Web API

A comprehensive, enterprise-grade **Clinic Management System API** engineered using **ASP.NET Core Web API (.NET 8)** adhering strictly to **Clean Architecture** patterns. This solution automates complex healthcare workflows, including patient registry tracking, dynamic doctor specialty bindings, dual-layer validation pipelines, and secure room occupancy schedules.

---

## 🎯 Project Objective

This subsystem serves as a production-level medical workspace management portal designed to showcase:

* **Advanced Entity Configurations:** Fluent API declaration of composite primary keys and automatic GUID tracking utilizing native SQL Server `NEWID()` parameters.
* **Complex Healthcare Topologies:** Implementing decoupled Many-to-Many entity connections associating clinical physical workspaces with doctors' functional medical specialties.
* **Strict Cascade Policy Control:** Explicit architectural rules safeguarding sensitive patient profiles via cascading identity roots while strictly restricting transactional metadata deletion.

---

## 🧱 Architecture & Design Principles

The architectural solution segregates core production responsibilities across **4 decoupled layers**:

* **Domain Layer:** Contains core enterprise medical structural objects (`Doctor`, `Patient`, `Appointment`, `Specialty`, `Room`, `RoomSpecialty`) completely independent of external framework drivers.
* **Application Layer:** Manages input validation workflows, structural object mapping profiles, request/response contract definitions (DTOs), and business operational core rules.
* **Infrastructure Layer:** Drives Entity Framework Core workflows, operational database contexts (`AppDbContext`), data-tracking configurations, and Identity processing infrastructure.
* **Web API Layer:** Coordinates thin API controllers, thin routing schemes, global filtering logic, and custom exception interception middleware blocks.

### 💎 Advanced Architectural Decisions:

* **Native GUID Key Provisions:** Unique identifiers for clinic subjects (`Doctor` and `Patient`) are mapped dynamically using `.HasDefaultValueSql("NEWID()")` to securely offload database structural key generation onto the SQL Server instances.
* **Dynamic Relational Workspace Mapping:** The architectural mapping between medical `Room` configurations and `Specialty` bounds is securely resolved using an explicit join entity (`RoomSpecialty`) governed by a composite primary key layout.
* **Relational Schema Integrity Protection:** To safeguard critical healthcare history chains, deleting operational units like Doctors, Patients, or Rooms is blocked via `DeleteBehavior.Restrict` if active transactional `Appointment` schedules remain tied to their context.
* **Calculated Real-Time Domain Metrics:** Client output models like `ReadPatientDto` leverage specialized backend calculations to compute properties like `Age` dynamically relative to current application system dates, preventing analytical drift.

---

## 🛠️ Technologies

* **Framework:** ASP.NET Core Web API (.NET 8)
* **Database Engine & ORM:** Microsoft SQL Server + Entity Framework Core (Code-First)
* **Security Architecture:** ASP.NET Core Identity + Cryptographic JWT Bearer Token Authentication Channels
* **Object Mapping & Verification:** AutoMapper Profiles & FluentValidation Middleware Pipes
* **API Documentation Explorer:** Swagger / OpenAPI with Interactive Header Bearer Token Injectors
* **Error Pipeline Control:** Custom Global Exception Handling Middleware

---

## 🚀 Key Technical Features

### 🔐 1. Automated Identity Provisioning & Seeding

* **Self-Configuring Security Realms:** During the initial pipeline configuration phase, the host application triggers programmatic database seeds (`ContextSeed`) to inspect, map, and insert global Identity Roles (`Admin`, `Doctor`, `Patient`) along with a pre-configured root Admin profile out of the box.
* **Tenancy Claim Isolation:** Controller endpoints securely target authenticated identity records via direct token claim evaluations rather than looking them up through client-side query parameters, entirely removing cross-user data tampering surfaces.

### 🧩 2. Clean Architecture Extension Pipeline

To avoid bloat inside the presentation hosting tier (`Program.cs`), all system dependencies are cleanly abstracted into modular registration classes:

| Service Extension Method | Registration Responsibility |
|--------------------------|-----------------------------|
| `AddSwaggerExtension` | Pre-configures Swagger schemas along with interactive JWT token input fields |
| `AddInfrastructureServices` | Binds database context pipelines and core repository infrastructure operations |
| `AddApplicationServices` | Resolves core medical application workflow dependencies and structural service layers |
| `AddIdentityServices` | Provisions baseline identity authentication blocks and user validation workflows |
| `AddAuthService` | Evaluates incoming JWT structural claims, token lifecycles, and encryption keys |

**Clean `Program.cs` Layout Blueprint:**

```csharp

builder.Services.AddSwaggerExtension();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices();
builder.Services.AddAuthService(builder.Configuration);
builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();
// Seeding and Pipeline configurations...
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


## ✅ FluentValidation Strategy

All input payloads are strictly vetted with developer-friendly error messages before hitting the business layer:

| DTO | Validation Rules Applied |
|-----|--------------------------|
| `RegisterUserDto` | Username (3–50 chars), valid Email syntax, strong Password complexity, PhoneNumber required |
| `LoginDto` | Valid Email structure required, non-empty Password input block |
| `CreateDoctorDto` | Valid `UserId`, Name required (max 100 chars), Phone (max 12 chars), MedicalLicense required, valid `SpecialtyId` |
| `UpdateDoctorDto` | Non-empty Name, valid Phone limits, updated MedicalLicense string, mandatory `SpecialtyId` value |
| `CreatePatientDto` | Valid `UserId`, Name required (max 100 chars), Phone (max 12 chars), BloodType syntax, past-bounded `DateOfBirth` |
| `UpdatePatientDto` | Updated Profile Name, Phone metadata edits, BloodType tracking, ChronicDiseases optional string |
| `CreateAppointmentDto` | Valid `PatientId`, active `DoctorId`, functional `RoomId`, future-bounded `AppointmentDate`, Duration > 0 |
| `UpdateAppointmentDto` | Optional future `AppointmentDate` adjustments, optional Duration, Doctor, Patient, or Room overrides |
| `CreateDoctorCertificateDto` | CertificateName required, IssuingOrganization details, past-bounded GraduationDate, valid `DoctorId` binding |
| `UpdateDoctorCertificateDto` | Optional qualification Title parameters, updated Issuing entity logs, adjustable Graduation dates |
| `CreateSpecialtyDto` | Specialty unique Name is mandatory (3–100 chars max) |
| `UpdateSpecialtyDto` | Valid Specialty ID verification tracking, mandatory updated Name constraint values |
| `CreateRoomDto` | Distinct clinical physical RoomNumber parameter required (max 10 chars layout) |
| `UpdateRoomDto` | Target database key verification, updated RoomNumber tracking parameters |
| `CreateRoomSpecialtyDto` | Relational mapping validating physical `roomId` directly against structural `specialtyId` constraints |
| `UpdateRoomSpecialtyDto` | Valid physical `roomId`, tracking old relationship key mappings to safely replace with new specialty |

---

## 🚨 Global Exception Handling

The centralized exception logging middleware intercept application-level validation errors globally, returning consistent JSON schemas:

| Exception | HTTP Status | When Used / Operational Rule |
|-----------|-------------|------------------------------|
| `NotFoundException` | 404 | Thrown whenever a targeted Doctor, Patient, Specialty, Room, or Appointment identifier fails database lookups |
| `BadRequestException` | 400 | Dispatched during operational booking room overlaps, scheduling conflicts, or failed business invariant states |
| `UnauthorizedException` | 401 | Executed when an operation fails secure data tenancy ownership validation or security role evaluation checks |


## 🔑 Core API Endpoints

### 🔐 Authentication & Accounts

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Registers a new clinic system user profile securely | ❌ No |
| POST | `/api/auth/login` | Validates credentials & returns a secure JWT Token along with claims | ❌ No |

### 🩺 Doctor Registry & Credentials

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/doctors` | Extracts paginated doctors returning active `ReadDoctorDto` structures | ✅ Yes |
| GET | `/api/doctors/{id}` | Locates an isolated doctor by database GUID returning dynamic fields | ✅ Yes |
| POST | `/api/doctors` | Provisions a fresh doctor profile binding entity fields and nested certificates | ✅ Yes (Admin) |
| PUT | `/api/doctors/{id}` | Modifies core professional details mapped to a single medical profile | ✅ Yes (Admin/Doctor) |
| DELETE | `/api/doctors/{id}` | Wipes out a doctor registry (Cascades account, restricts core appointments) | ✅ Yes (Admin) |
| POST | `/api/doctors/certificates` | Appends a fresh certified academic qualification onto an active doctor dossier | ✅ Yes (Admin/Doctor) |
| PUT | `/api/doctors/certificates/{id}` | Amends qualification text titles or organizational issuance dates | ✅ Yes (Admin/Doctor) |

### 👤 Patient Medical Database

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/patients` | Extracts patient records mapping names, fields, and computed runtime `Age` | ✅ Yes (Admin/Doctor) |
| GET | `/api/patients/{id}` | Locates a single patient database record profile parsing historical metadata | ✅ Yes |
| POST | `/api/patients` | Registers a new patient medical file profile into the clinic ecosystem | ✅ Yes (Admin/Staff) |
| PUT | `/api/patients/{id}` | Overrides personal profiling parameters or chronic condition tracking logs | ✅ Yes |

### 📅 Appointment Operations

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/appointments` | Extracts paginated medical bookings displaying names, rooms, and string status | ✅ Yes |
| POST | `/api/appointments` | Schedules a reservation ensuring synchronization between rooms, patients, and doctors | ✅ Yes |
| PUT | `/api/appointments/{id}` | Process structural scheduling modifications or assignment updates fluidly | ✅ Yes |
| PATCH | `/api/appointments/{id}/status` | Updates the reservation progression state (`EnAppointmentStatus`) via clean text paths | ✅ Yes |
| DELETE | `/api/appointments/{id}` | Drops a selected transaction booking permanently from clinic tables | ✅ Yes (Admin/Staff) |

### 🏢 Clinic Infrastructure (Rooms & Specialties)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/specialties` | Fetches functional medical categories and department specialties | ✅ Yes |
| POST | `/api/specialties` | Registers a fresh clinic operational department or medical specialty | ✅ Yes (Admin) |
| PUT | `/api/specialties/{id}` | Overrides existing department naming parameters inside data tables | ✅ Yes (Admin) |
| GET | `/api/rooms` | Extracts physical workspace setups and outpatient clinical rooms | ✅ Yes |
| POST | `/api/rooms` | Creates a new physical clinic workspace identifier number | ✅ Yes (Admin) |
| POST | `/api/rooms/specialties` | Links physical operating workspaces directly to functional medical specialties | ✅ Yes (Admin) |
| PUT | `/api/rooms/specialties` | Safely replaces old workspace specialty bindings with fresh parameters | ✅ Yes (Admin) |

📄 Server-Side Pagination Schema

### Request

```http
GET /api/appointments?pageNumber=1&pageSize=10&doctorId=guid-value&status=Scheduled

{
  "data": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 42,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true
}

⚙️ Local Installation & Database Initialization
1. Verification of Connection Properties
Review configuration boundaries located within your presentation tier project layer (appsettings.json). Ensure a target connection configuration points smoothly to a local SQL engine node instance:
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\;Database=ClinicManagement_DB;Trusted_Connection=True;TrustServerCertificate=True;"
}

2. Execution of EF Core Migration Scripts
Fire up your Package Manager Console (PMC) inside Visual Studio. Ensure your Default Project points straight to your Infrastructure assembly location, then execute:
# Compile schema configurations into tracking files
Add-Migration InitialClinicSetupSetup -Project Infrastructure

# Push structural context updates directly onto your running SQL Server instance
Update-Database

3. Execution Framework
Fire up the server application engine instance by running through Visual Studio (F5) or via terminal tooling paths:
dotnet run --project ClinicManagement.API

Navigate your local browsing pipeline targets to /swagger to instantly track, authenticate, and explore your live data interfaces.

🧑‍💻 Author
Mohammad Al-Mohammad – Backend Developer – ASP.NET Core Specialist