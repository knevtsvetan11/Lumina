# Lumina
Markdown
# Lumina - Cinema App 🎬

A full-stack web application for cinema management and ticket bookings. This project is built to demonstrate modern web development practices, clean architecture, and real-time features.

---

## 🚀 Tech Stack

| Layer | Technology |
| :--- | :--- |
| **Frontend** | Angular 17+ (Signals, RxJS, Angular Material) |
| **Backend** | .NET 8 Web API |
| **Real-time** | SignalR (Notifications & Presence) |
| **Database** | SQL Server + Entity Framework Core |
| **Auth** | JWT Bearer Tokens & ASP.NET Core Identity |

---

## ✨ Key Features
- **User Authentication:** Secure identity system with JWT-based login and registration.
- **Real-time Engine:** Live updates and notifications using SignalR.
- **Modern UI:** Responsive design built with Angular and Angular Material.
- **Database Migrations:** Fully managed database schema using EF Core.

---

## 🛠️ Getting Started

### 1. Database Setup
Run the migrations to create your local database:
```bash
cd API/ApiLumina
dotnet ef database update
2. Run Backend (API)
Start the .NET server:

Bash
dotnet run
3. Run Frontend (UI)
Install dependencies and start the Angular development server:

Bash
cd UI
npm install
npm start
Application will be available at: http://localhost:4200

🔒 Security & Configuration
To keep the repository secure, sensitive information like JWT Secret Keys is not committed to the code. For local development, please use .NET User Secrets:

Bash
dotnet user-secrets set "JwtSettings:Secret" "YOUR_LONG_RANDOM_SECRET_KEY"
👤 Author
Tsvetan Kanev

Junior Full-Stack Developer

📜 License
This project is for portfolio purposes. Feel free to explore the code!
