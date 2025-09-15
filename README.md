# EigenvalueFinder

EigenvalueFinder is a math web application that computes eigenvalues and eigenvectors of real matrices using the QR Algorithm. It consists of a Web API, frontend, and a numerical core powered by MathNet.Numerics.

---

## 📦 Features

- Calculate eigenvalues and eigenvectors via the QR algorithm
- Visual frontend and programmatic Web API
- NUnit unit-tested core methods

---

## 🔧 Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/downloads)

### 1. Clone the Repository

```bash
git clone https://github.com/Roiqk7/EigenvalueFinder.git
cd EigenvalueFinder
```

### 2. Restore Dependencies and Build

```bash
cd src/
dotnet restore
dotnet build
```

---

## ▶️ Running the Application

### Run the API

To run both the backend API and the frontend in one simple step, use the provided `run.sh` script in the root directory of the project:

```
./run.sh
```

This will automatically start the backend server on `https://localhost:5001` and the frontend server on `http://localhost:8080`.

Now visit `http://localhost:8080` to view the page.

Press `Ctrl+C` to shut down both services.

<details>
<summary><b>Address already in use error</b></summary>
<br>

If you see an error like `OSError: [Errno 48] Address already in use`, it means a previous process is still running  and occupying the necessary port. To fix this, you need to manually kill the old process.

1. **Find the processes**: Use the `lsof` command to find the process IDs (PIDs) using the ports.

```
# For port 5001  
lsof -i :5001  

# For port 8080  
lsof -i :8080  
```

2. **Kill the processes**: Use the `kill` command with the PIDs you found.

```
kill <PID>  
```

If that doesn't work, you can use `kill -9 <PID>` to force the termination.

3. **Run the script again**: After you've cleared the ports, you can rerun `./run.sh`.

</details>

<details>
<summary><b>The ASP.NET Core developer certificate is not trusted</b></summary>
<br>

This error can be fixed by running this command:

```
dotnet dev-certs https --trust
```

*Password may be required by the OS*

</details>

<details>
<summary><b>Load Failed Error</b></summary>
<br>

This error indicates that the front end and back end failed to communicate properly.  
Please ensure you’ve followed all usage instructions before retrying.
</details>

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

## 💬 Feedback and Contributions

- Use the [Issues](https://github.com/YOUR_USERNAME/EigenvalueFinder/issues) tab to report bugs or suggest improvements.
- Pull requests are welcome!

---

## 🖼️ Gallery

![pic2](/.images/pic2.jpg)
![pic3](/.images/pic3.jpg)

---

## 📚 Technologies

- .NET 8 SDK
- ASP.NET Core
- MathNet.Numerics
- NUnit 3
- Swagger (via Swashbuckle.AspNetCore)

---

## 📜 License

MIT License – see `LICENSE`

## 🤖 AI used for:
- Commenting 
- Documentation
- Most frontend code
- Helped debugging
- Assited with writting tests
