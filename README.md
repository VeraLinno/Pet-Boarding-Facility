# Eksam (8h) – Pet Boarding Facility Management System

A comprehensive ASP.NET Core web application for managing a pet boarding facility. The system handles pet registrations, kennel assignments, medication schedules, feeding restrictions, and photo updates for pet owners.

## Features

### Core Functionality
- **Pet Management** - Register and manage pets with species, size, and behavioral attributes
- **Owner Management** - Track customer information and premium status
- **Kennel Management** - Manage kennel availability, sizes, and zones
- **Stay Tracking** - Record pet boarding stays with start/end dates

### Business Rules
- **Kennel Assignment Rules** - Automated validation for pet-kennel compatibility based on:
  - Size matching (pet size vs kennel capacity)
  - Aggression compatibility (aggressive pets get their own kennel)
  - Species compatibility (certain species cannot be adjacent)
  - Kennel availability status

### Daily Operations
- **Medication Scheduling** - Create and manage medication schedules for pets
- **Feeding Plans** - Set feeding times and ingredient restrictions
- **Photo Updates** - Send daily photos to pet owners
- **Dashboard** - Real-time overview of boarding pets, medications due, and kennel status

### Communication
- **Photo Sharing** - Mark photos as sent to owners
- **Medication Logging** - Track when medications are administered
- **Notification System** - Email alerts for missed critical medications

## Project Structure

### Domain Layer (`Domain/`)
Contains the core business entities, enums, and rules:
- **Entities**: `Pet`, `Owner`, `Kennel`, `Stay`, `Medication`, `MedicationLog`, `PhotoUpdate`, `FeedingSchedule`, `Incompatibility`
- **Enums**: `PetSpecies`, `PetSize`, `KennelStatus`, `KennelSize`, `KennelZone`, `UpdateType`, `PremiumLevel`
- **Rules**: `KennelAssignmentRules` - Business logic for kennel assignments

### Application Layer (`Application/`)
Services and use-case implementations:
- `MedicationService` - Medication scheduling and management
- `KennelAllocationService` - Kennel assignment with rule validation
- Repository abstractions for data access

### Data Access Layer (`DAL/`)
Entity Framework Core implementation:
- `AppDbContext` - Database context with entity configurations
- `IRepository` - Generic repository interface
- `ConfigRepoEF` - Entity Framework repository implementation
- Migrations for database schema management

### Infrastructure Layer (`Infrastructure/`)
Concrete implementations:
- `EFRepositories` - Repository implementations

### Web Application (`WebApp/`)
ASP.NET Core Razor Pages application:
- **Pages**: Dashboard, Pets, Owners, Kennels, Medications, Incompatibility
- **API**: Minimal API endpoints for programmatic access
- **UI**: Bootstrap-styled responsive interface

## Getting Started

### Prerequisites
- .NET 8.0 or later
- SQLite database

### Running the Application

```sh
cd WebApp
dotnet run --urls "http://localhost:5000"
```

The application will be available at http://localhost:5000

### Database Setup

```sh
# Add a new migration
dotnet ef migrations --startup-project WebApp --project DAL add <MigrationName>

# Apply migrations
dotnet ef database update --startup-project WebApp --project DAL

# Drop and recreate database
dotnet ef database drop --startup-project WebApp --project DAL
```

## Web Interface

### Dashboard
- Overview of all boarding pets
- Medications due today with Given button to mark administration
- Available and occupied kennel counts
- Photo status with Photo Sent button

### Pets
- View all registered pets
- Add new pets with species, size, and behavioral attributes
- Edit pet information
- Assign to kennels
- View medication schedules

### Kennels
- View all kennels with status (Available, Occupied, Cleaning)
- Assign pets to kennels (with rule validation)
- Mark kennels for cleaning

### Medications
- Create medication schedules for pets
- Edit existing medication details
- Delete medications
- View medications grouped by pet

### Owners
- Register pet owners
- Track premium customer status
- View owner details and associated pets

### Incompatibility
- Define species incompatibilities
- Prevent incompatible pets from being in adjacent kennels

## Tools

### Entity Framework Tools
```sh
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

### ASP.NET Scaffolding
```sh
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool update --global dotnet-aspnet-codegenerator
```

## Chat prompts:

Role:
You are KiloCode Architect Mode — a senior software architect and technical lead.
You prioritize delivery over theory, clarity over cleverness, and working software over abstractions.

### You are building a Pet Boarding Facility Management System using:
ASP.NET Core Razor Pages
Entity Framework Core
SQL Server
Simple CRUD
No real notifications, no background jobs, no external services
All “alerts” are UI-only simulations.

### DELIVERY RULES (NON-NEGOTIABLE)

Follow the Priority Tiers strictly
Do not jump ahead
Do not gold-plate
Do not introduce unnecessary patterns
If Tier 1 is incomplete, stop
You must explicitly state which Tier you are working on in every response.

PRIORITY TIERS
## Tier 1 — Minimum Viable Product (Hours 1–3)

### You MUST deliver:
Clean database schema
EF Core migrations that run without errors
CRUD for all core entities
Relationships working correctly
Simple list + detail Razor Pages

### Application runs without runtime errors

❌ No business rules
❌ No advanced validation
❌ No search

## Tier 2 — Core Features

### You MUST add:
Simple search (single-field, basic LIKE)

### Basic business rules:
Dog vs Cat zone enforcement
Aggressive dogs → isolation kennels
2-hour cleaning lock after checkout
Form validation:
Required fields
Valid enum selections

## Tier 3 — Full Requirements

### You MUST add:
Advanced filtering combinations
Full-text search across relevant fields

### All business rules:
Incompatible pets never adjacent
Medication schedules enforced visually
Missed medication flagged
All validation rules using FluentValidation

## Tier 4 — Polish
### Optional enhancements:
Toast notifications (UI only)
Loading indicators
Keyboard navigation
Edge-case handling
Code cleanup
Documentation comments

## TECHNICAL REQUIREMENTS

### Clean separation:
Razor Pages (UI)
Services (business logic)
Repositories / DbContext (data access)
async/await for all data operations
Data Annotations for simple validation
FluentValidation for complex rules
Proper DB indexes for search fields
Query logging enabled in development
No N+1 query problems
User-friendly errors in UI
Technical details logged server-side

## DOMAIN CONSTRAINTS
Cats and dogs are in physically separate zones
Aggressive dogs must be isolated
Certain pets can be marked “Never Adjacent”

### Kennels have states:
Available
Occupied
Cleaning (locked 2 hours)
Blocked

### Medication schedules:
Fixed times (e.g., 8am / 6pm)
Staff checklist
Missed meds highlighted visually

### Owner updates:
Standard: once daily
Premium: twice daily + fake livestream link

## OUTPUT EXPECTATIONS

### Clearly state:
Current Tier
What is being implemented
What is explicitly deferred
Provide:
Razor Page structure
Entity models
DbContext setup
Validation examples

### Prefer:
Explicit code
Simple patterns
Predictable behavior

### Avoid:
Over-engineering
CQRS
MediatR
Microservices
Domain-driven buzzwords

