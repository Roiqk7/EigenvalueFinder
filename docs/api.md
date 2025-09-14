# EigenvalueFinder Web API Documentation

This document describes the API endpoint(s) provided by the `EigenvalueFinder.WebAPI` ASP.NET Core backend. The API exposes a single endpoint for calculating the eigenvalues and eigenvectors of a matrix using the QR algorithm.

---

## 📍 Base URL

During development, the API runs on:

```
https://localhost:5001
```

Ensure the backend is running via:

```bash
dotnet run --project EigenvalueFinder.WebAPI
```

---

## 🚀 Endpoint: `/api/eigenvalue/calculate`

### Method: `POST`

Calculates eigenvalues and eigenvectors of a matrix.

### Request Body

Send a matrix (2D array of numbers) in JSON format:

```json
{
  "matrix": [
    [4.0, 1.0],
    [2.0, 3.0]
  ]
}
```

### Request Model

| Field  | Type               | Description                |
|--------|--------------------|----------------------------|
| matrix | `List<List<double>>` | Square matrix to analyze |

---

### Response

```json
{
  "eigenpairs": [
    {
      "eigenvalue": {
        "real": 5.0,
        "imaginary": 0.0
      },
      "eigenvector": [
        { "real": 1.0, "imaginary": 0.0 },
        { "real": 0.5, "imaginary": 0.0 }
      ]
    },
    ...
  ]
}
```

### Response Model

| Field        | Type                         | Description                        |
|--------------|------------------------------|------------------------------------|
| eigenpairs   | `List<Eigenpair>`            | List of eigenvalue/vector pairs    |

Each `Eigenpair` contains:

- `eigenvalue`: complex number
