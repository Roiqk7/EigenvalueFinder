# Developer Documentation

Welcome to the internal developer documentation for **EigenvalueFinder**.

This guide is intended for contributors and maintainers of the project.

---

## 📚 Table of Contents

- [Project Overview](#project-overview)
- [Technologies Used](#technologies-used)
- [Development Prerequisites](#development-prerequisites)
- [Building and Running](#building-and-running)
- [Testing](#testing)
- [Algorithm Summary](#algorithm-summary)
- [Swagger & API Testing](#swagger--api-testing)
- [Known Limitations](#known-limitations)
- [Contribution Guidelines](#contribution-guidelines)

---

## 📦 Project Overview

The project is structured as a multi-project .NET solution:

| Project                     | Description                                                   |
|----------------------------|---------------------------------------------------------------|
| `EigenvalueFinder.Core`    | Core mathematical logic (QR algorithm, matrix utilities)      |
| `EigenvalueFinder.WebAPI`  | ASP.NET Core backend API exposing eigenvalue calculations     |
| `EigenvalueFinder.Web`     | Static web frontend (HTML/CSS/JS)                             |
| `EigenvalueFinder.Tests`   | NUnit3-based test suite for `Core` logic                      |

---

## 🛠️ Technologies Used

- **.NET 8 SDK**
- **ASP.NET Core 8**
- **MathNet.Numerics**
- **NUnit 3**
- **Swagger / Swashbuckle**
- **JetBrains Rider** (recommended IDE)
- **Git**

---

## 💻 Development Prerequisites

1. **.NET SDK 8**
	- Install: [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
	- Verify:
	  ```bash
	  dotnet --version
	  ```

2. **Git**
	- Install: [https://git-scm.com/](https://git-scm.com/)
	- Verify:
	  ```bash
	  git --version
	  ```

---

## 🔃 Building and Running

### Build Solution

```bash
dotnet build
```

### Run Web API

```bash
dotnet run --project EigenvalueFinder.WebAPI
```

### Serve Frontend

```bash
cd EigenvalueFinder.Web/wwwroot
python3 -m http.server
```

---

## 🧪 Testing

Run the test project:

```bash
dotnet test
```

This runs `EigenvalueFinder.Tests` using NUnit.

---

## 📐 Algorithm Summary

### QR Algorithm

**Input:** Matrix $A \in \mathbb{R}^{n \times n}$.

1.  Initialize $A_0 := A$, $i := 0$.
2.  **while** termination condition is not met **do**
3.  Construct the QR decomposition of matrix $A_i$, i.e., $A_i = QR$.
4.  Update $A_{i+1} := RQ$.
5.  Increment $i := i + 1$.
6.  **end while**

**Output:** Matrix $A_i$.

### QR Decomposition Algorithm

**Input:** Matrix $A \in \mathbb{R}^{m \times n}$.

1.  Initialize $Q := I_m$, $R := A$.
2.  **for** $j := 1$ to $\min(m, n)$ **do**
3.  Set $x := R(j:m, j)$. (This typically means the subvector of $R$ from row $j$ to $m$ in column $j$).
4.  **if** $x \neq \|x\|_2 e_1$ **then**
5.  Update $x := x - \|x\|_2 e_1$.
6.  Construct Householder matrix $H(x) := I_{m-j+1} - \frac{2}{x^T x} xx^T$.
7.  Construct $H := \begin{pmatrix} I_{j-1} & 0 \\ 0 & H(x) \end{pmatrix}$.
8.  Update $R := HR$, $Q := QH$.
9.  **end if**
10. **end for**

**Output:** Diagonal of $A_i$ approximates eigenvalues and columns of $Q_i$ approximate the eigenvectors.

---

## 📋 Swagger & API Testing

After running the WebAPI, navigate to:

```
https://localhost:5001/swagger/index.html
```

You'll find an interactive documentation where you can test the `POST` endpoint for sending matrices and receiving computed eigenvalues.

> 🔄 Note: Cross-Origin Requests are allowed from `http://localhost:63343` by default. You can modify this in `WebAPI.cs` if you're hosting the frontend elsewhere.

---

## ⚠️ Known Limitations

- Provided eigenpairs are not guaranteed to form an eigenpair.
- Floating-point instability may occur on ill-conditioned matrices but `MathNet.Numerics` should take care of it.

---

## 🤝 Contribution Guidelines

- Fork and clone the repository
- Create a new branch for each feature/fix
- Use clear commit messages
- Run `dotnet test` before submitting PRs
- Update this documentation if relevant

---

## 🧾 License

MIT – Free to use, modify, and distribute with attribution
