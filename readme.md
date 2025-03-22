# Inpost_org Console Application

## Description
This is a console application for managing clients, deliverers, and packages using MongoDB as the database. The project is developed as part of a school subject on object-oriented programming.

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)

## Getting Started

### Running the Application with Docker

1. **Clone the repository:**
    ```sh
    git clone https://github.com/ZSK-octoTeam/Inpost_org.git
    ```

2. **Enter the project directory:**
    ```sh
    cd Inpost_org
    ```

3. **Create a .env file in root directory with the necessary environment variables:**
   ```sh
   DATABASE_USER=your_database_user
   DATABASE_PASSWORD=your_database_password
   ```

4. **Build the Docker image:**
    ```sh
    docker build -t inpost_org_image .
    ```

5. **Run the Docker container:**
    ```sh
    docker run -it --name inpost_org_container --env-file .env inpost_org_image
    ```

## Usage
Once the application is running, follow the on-screen prompts to log in and navigate through the menu options to manage clients, deliverers, and packages.

## License
This project is licensed under the MIT License.
