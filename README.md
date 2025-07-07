# EigenvalueFinder

EigenvalueFinder is a math web application that computes eigenvalues and eigenvectors of real matrices using the QR Algorithm. It consists of a Web API, frontend, and a numerical core powered by MathNet.Numerics.

---

## 📦 Features

- Calculate eigenvalues and eigenvectors via the QR algorithm
- Visual frontend and programmatic Web API
- OpenAPI documentation with Swagger
- NUnit unit-tested core algorithm

---

## 🔧 Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/downloads)

### 1. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/EigenvalueFinder.git
cd EigenvalueFinder
```

### 2. Restore Dependencies and Build

```bash
cd src/EigenvalueFinder/
dotnet restore
dotnet build
```

---

## ▶️ Running the Application

### Run the API

This project is a Web API backend hosted using ASP.NET Core. Start it with:

```bash
dotnet run --project EigenvalueFinder.WebAPI
```

By default, this will start the server on:

- `https://localhost:5001`

### Run the Frontend

The frontend is a static HTML/JS/CSS page located in:

```text
EigenvalueFinder.Web/wwwroot/index.html
```

You can open this file directly in a browser, or serve it using a static file server:

```bash
cd EigenvalueFinder.Web/wwwroot
python3 -m http.server 8080
```

Visit `http://localhost:8080`.

> Optionally, static files can be served directly from the WebAPI by copying them into its `wwwroot/` directory.

---

## 🧪 Running Unit Tests

To run the test suite:

```bash
dotnet test
```

This will run all unit tests in the `EigenvalueFinder.Tests` project using NUnit3.

---

## 🧮 What the program *doesn't* yet support

- Accurate eigenpairs:
	- The program currently assigns the columns of the accumulated `Q` matrix as eigenvectors, which is correct but then randomly assigns them to eigenvalues.
	- For future development, consider implementing [Inverse Iteration](https://en.wikipedia.org/wiki/Inverse_iteration) or verifying eigenvectors using matrix identity: `A * v = λ * v`.

---

## 📤 Publishing for Deployment

To publish the Web API project for deployment:

```bash
dotnet publish EigenvalueFinder.WebAPI --configuration Release --output ./publish
```

The output folder (`./publish`) will contain a self-contained deployment-ready version.

---

## 💬 Feedback and Contributions

- Use the [Issues](https://github.com/YOUR_USERNAME/EigenvalueFinder/issues) tab to report bugs or suggest improvements.
- Pull requests are welcome!

---

## 🖼️ Gallery

*Coming soon: demo screenshots or GIFs.*

---

## 📚 Technologies

- .NET 8 SDK
- ASP.NET Core
- MathNet.Numerics
- NUnit 3
- Swagger (via Swashbuckle.AspNetCore)

---

## 📜 License

MIT License – see `LICENSE.md`
