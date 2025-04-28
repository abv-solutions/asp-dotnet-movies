# ASP.NET Movies App

Welcome to the **MovieApp** project — an ASP.NET Core MVC application that interacts with the TMDB API to display movies, details, images, casts, and allow user comments.

---

## 📂 Project Structure

- **asp-dotnet-movies/**
  - **MovieApp/** → Main project folder
    - `.env` → (Required) Environment variables file (API keys, etc.)
  - Other solution-related files

---

## ⚙️ Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 (or newer) **OR** VS Code with C# extensions
- Internet access (to reach the TMDB API)

---

## 📥 Cloning the Project

```bash
git clone https://github.com/abv-solutions/asp-dotnet-movies.git
```

---

## 🛠️ Setting up your Environment

1. **After cloning**, navigate to the project folder:

   ```bash
   cd asp-dotnet-movies/MovieApp
   ```

2. **Create a `.env` file** inside the `MovieApp/` directory.

   - You **must** have a `.env` file at this path:  
     ➔ `asp-dotnet-movies/MovieApp/.env`

   - I will provide this `.env` file separately by email together with the repository link.

   - The `.env` file should look like this:

     ```env
     API_KEY=your_tmdb_api_key_here
     BEARER_TOKEN=your_tmdb_bearer_token_here
     ```

3. **Restore dependencies**:

   ```bash
   dotnet restore
   ```

4. **Run the project**:

   ```bash
   dotnet run --project MovieApp
   ```

---

## 🧹 Notes

- The `.env` file is ignored from GitHub (`.gitignore`) for security reasons.
- Without a valid `.env` file, the project will not be able to fetch movie data from TMDB.
- When sharing the project internally, always separately share the `.env` file.

---

## ✨ Features

- Browse top-rated and latest movies
- Search movies by title or genre
- View detailed movie information (cast, images, description)
- Add and view comments for each movie
- Responsive and simple UI

---

## 🧑‍💻 Developer Tips

- The environment variables are loaded at application startup using the [DotNetEnv](https://www.nuget.org/packages/DotNetEnv) package.
- All external API communication is abstracted in the `Services/TmdbService.cs` file.
- Comments are managed via the `Repositories/CommentRepository.cs`.

---

## 📧 Questions?

If you encounter any issues or need support, please feel free to reach out by replying to the email you received.
