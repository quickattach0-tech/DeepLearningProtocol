# v3.2 Release Notes

**Released:** January 25, 2026  
**Version:** 3.2  
**Status:** Production Ready ✅

---

## 🎉 v3.2 Release: SignalR + Docker Improvements

This release adds a built-in SignalR server endpoint for real-time notifications and fixes containerization issues by updating the Docker runtime to the ASP.NET image and exposing port 80.

---

## ✨ What's New in v3.2

### 🌐 SignalR Server
- **Endpoint:** `/hub/notifications`
- **Purpose:** Real-time notifications to connected clients (web, desktop)
- **Basic API:** Clients can call `SendNotification` to broadcast to all connected clients

### 🐳 Docker Improvements
- **Runtime:** Switched to `mcr.microsoft.com/dotnet/aspnet:10.0-alpine` for proper ASP.NET hosting
- **Port:** Exposes port 80 for HTTP and SignalR connections
- **Healthcheck:** Uses `/health` endpoint for container health

### 📦 Installer & Distribution
- **Installer Version:** 3.2
- **Installers:** Windows, Linux (systemd), macOS (launch agents)
- **CI/CD:** Releases now default to `v3.2` and include installers in artifacts

---

## 🔧 Migration Notes
- Docker images built from this release are web-ready and support SignalR clients.
- If you previously used a container that executed the application as a console app only, the container will now expose HTTP port 80 — ensure firewall rules are adjusted as needed.

---

## 🛠️ Upgrade Steps
1. Pull the `v3.2` release artifacts from GitHub
2. For Docker: rebuild images or pull the updated images
3. For server installations: run the installer for your platform

---

## Troubleshooting
- If the container fails health checks, check `/health` endpoint and application logs
- Ensure no other process is binding to port 80 inside the host

---

For full details, see `INSTALLATION_GUIDE.md` and `RELEASE_DISTRIBUTION_POLICY.md`.
