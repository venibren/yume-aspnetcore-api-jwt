# ASP.NET Core Web API w/ JWT

A pragmatic ASP.NET Core Web API starter with custom auth middleware. It uses a fully custom user model, role-based policies, and JWT auth with MFA options (TOTP, email, backup codes). Designed to be extended without fighting ASP Identity defaults.

- .NET 8 SDK
- PostgreSQL
- MongoDB

## Roadmap

- Complete role policy logic & assignment
- Complete TOTP & Backup functionality
- Complete JWT refresh
- Connect Secrets Manager
- Connect Email Service
- Connect DataDog Logging
- Connect S3 for file upload & management
- Set up Docker
- Complete factories
- Complete test containers
- Set up client entity configuration

## Features & Goals

*Note certain features are still in progress and may not be fully supported

- Custom user model and role-policy authorization
- JWT access and refresh tokens
- MFA options*:
  - TOTP
  - Email codes
  - Backup codes
- Email service*
- AWS integrations*:
  - Secrets Manager
  - Parameter Store
  - S3

### User Sample

```json
{
  "id": "853444ad-2dca-4c11-a762-9750d8f5d825",
  "email": "alina@example.com",
  "emailVerified": false,
  "username": "alina",
  "nickname": "Ali",
  "discriminator": "0420",
  "createdDate": "2025-01-01T12:00:00Z",
  "updatedDate": "2025-01-01T12:00:00Z",
  "isActive": true,
  "customization": {
    "id": "691e8080-f8d6-4c80-916e-a6ada2b9ad5f",
    "userId": "853444ad-2dca-4c11-a762-9750d8f5d825",
    "avatar": "https://cdn.example.com/u/853444ad-2dca-4c11-a762-9750d8f5d825/avatar.png",
    "banner": "https://cdn.example.com/u/853444ad-2dca-4c11-a762-9750d8f5d825/banner.jpg",
    "description": "In hac habitasse platea dictumst. Nulla non nulla at tortor tincidunt vestibulum. Nulla facilisi. Vivamus et laoreet risus. Morbi vulputate eleifend rutrum. Nulla vitae rutrum tortor, ut ultrices erat. Aliquam varius, nisl.",
    "theme": "Light"
  },
  "auth": {
    "id": "ee69e8d7-9a71-476a-a6d3-aad2873872ea",
    "userId": "853444ad-2dca-4c11-a762-9750d8f5d825",
    "passwordHash": "<REDACTED>",
    "mfaEmailEnabled": false,
    "mfaTotpEnabled": true,
    "mfaBackupEnabled": true,
    "mfaTotp": {
      "id": "7231e85d-b5c3-4a3f-a463-d07cbb25d82f",
      "userId": "ee69e8d7-9a71-476a-a6d3-aad2873872ea",
      "secretHash": "<REDACTED>",
      "createdDate": "2025-01-01T12:00:00Z"
    },
    "mfaBackup": [
      {
        "id": "56f0446b-a3ce-4f2b-9fef-ff4c2abca399",
        "userId": "ee69e8d7-9a71-476a-a6d3-aad2873872ea",
        "codeHash": "<REDACTED>",
        "createdDate": "2025-01-01T12:00:00Z",
        "activatedDate": null
      },
      ...
    ],
    "history": [
      {
        "id": "3ae80075-7f00-4107-8ca7-35956b2428e4",
        "userId": "ee69e8d7-9a71-476a-a6d3-aad2873872ea",
        "timestamp": "",
        "success": true,
        "reason": null,
        "ipAddress": "192.168.0.1",
        "agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64)..."
      },
      ...
    ]
  }
}
```
