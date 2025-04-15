# Inp0st_org Console Application

## Description
This is a console application for managing clients, deliverers, and packages using MongoDB as the database. The project is developed as part of a school subject on object-oriented programming.

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)

## Getting Started

### Running the Application with .NET CLI

1. **Clone the repository from GitHub:**
    ```sh
    git clone https://github.com/ZSK-octoTeam/Inp0st_org.git
    ```

2. **Enter the project directory:**
    ```sh
    cd Inp0st_org
    ```

3. **Create a .env file in the root directory with the necessary environment variables:**
    ```sh
    DATABASE_USER=your_database_user
    DATABASE_PASSWORD=your_database_password
    ```

4. **Run the application using .NET CLI:**
    ```sh
    dotnet run --project Inp0st_org
    ```

### Running the Application with Docker (Build Locally)

1. **Clone the repository from GitHub:**
    ```sh
    git clone https://github.com/ZSK-octoTeam/Inp0st_org.git
    ```

2. **Enter the project directory:**
    ```sh
    cd Inp0st_org
    ```

3. **Create a .env file in the root directory with the necessary environment variables:**
    ```sh
    DATABASE_USER=your_database_user
    DATABASE_PASSWORD=your_database_password
    ```

4. **Build the Docker image:**
    ```sh
    docker build -t Inp0st_org_image .
    ```

5. **Run the Docker container:**
    ```sh
    docker run -it --name Inp0st_org_container --env-file .env Inp0st_org_image
    ```

### Running the Application with Docker (Prebuilt Image)

1. **Pull the prebuilt Docker image:**
    ```sh
    docker pull guc10/inp0st_org
    ```

2. **Create a .env file in the directory where you will run the container:**
    ```sh
    DATABASE_USER=your_database_user
    DATABASE_PASSWORD=your_database_password
    ```

3. **Run the Docker container:**
    ```sh
    docker run -it --name Inp0st_org_container --env-file .env guc10/inp0st_org
    ```

## Usage
Once the application is running, follow the on-screen prompts to log in and navigate through the menu options to manage clients, deliverers, and packages.

## License
This project is licensed under the MIT License.
