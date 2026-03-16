**Authentication System – Implemented Features**

**Framework**

* ASP.NET Core Web API (.NET 8)

---

## Core Authentication

Implemented endpoints:

```text
POST   /api/auth/register
POST   /api/auth/login
GET    /api/auth/me   (protected)
POST   /api/auth/refresh
```

Capabilities:

```text
User registration
User login
JWT access token generation
Protected endpoint using JWT
Current user identity endpoint (me)
```

---

## Token System

Two-token authentication model implemented.

```text
Access Token
Refresh Token
```

Access Token:

```text
JWT
Contains user claims
Used for API authentication
Expires in 15 minutes (900 seconds)
```

Claims inside token:

```text
UserId
Username
Email
Role
RoleId
Issuer
Audience
Expiration
```

Refresh Token:

```text
Random cryptographic string
Stored in database
Linked to user
Expires in 7 days
Used to generate new access tokens
```

Database table added:

```text
RefreshTokens
```

Fields:

```text
Id
Token
UserId
Expires
IsRevoked
```

---

## Authentication Response Structure

Login/Register response returns:

```json
{
  "token": "access_token",
  "refreshToken": "refresh_token",
  "expiresIn": 900,
  "user": {
    "id": 1,
    "username": "user",
    "email": "user@email.com",
    "roleId": 2,
    "role": "User"
  }
}
```

Purpose:

```text
Avoids immediate /me request
Client already knows authenticated user
```

---

## Security Implementations

Password Security:

```text
BCrypt password hashing
Password verification during login
```

Validation:

```text
DTO model validation using DataAnnotations
Automatic validation using ApiController
Custom validation error response
```

Security Checks:

```text
Email format validation
Password complexity validation
Email uniqueness check during registration
Role existence verification
```

Authorization:

```text
JWT Bearer authentication
Role-based authorization support
Protected endpoints using [Authorize]
```

---

## Roles System

Separate controller implemented:

```text
RolesController
```

Capabilities:

```text
Create role
Get roles
Update role
Delete role
Role-based authorization
```

Database structure:

```text
Users
Roles
RefreshTokens
```

Relationship:

```text
User → Role (many-to-one)
```

---

## Protected Endpoint

Current authenticated user:

```text
GET /api/auth/me
```

Function:

```text
Reads claims from JWT
Retrieves user from database
Returns user identity and role
```

---

## Refresh Token Flow

Token lifecycle:

```text
Login/Register
↓
Client receives AccessToken + RefreshToken
↓
Client calls APIs using AccessToken
↓
AccessToken expires (15 minutes)
↓
Client calls /api/auth/refresh using RefreshToken
↓
Server returns new AccessToken
```

Important rule:

```text
Refresh token is called automatically by the frontend/client application, not the backend.
```

Server responsibility:

```text
Validate refresh token
Generate new access token
Return updated authentication response
```

Client responsibility:

```text
Detect expired access token
Call refresh endpoint
Store new access token
Retry original request
```

---

## Current Authentication Capabilities

System now supports:

```text
JWT authentication
Role-based authorization
Password hashing
DTO validation
Custom validation responses
Protected API endpoints
Refresh token authentication
Token expiration management
User identity retrieval
Role management
```

---

## Authentication Layer Status

Implemented authentication architecture corresponds to a **production-style JWT + refresh-token authentication system**.
