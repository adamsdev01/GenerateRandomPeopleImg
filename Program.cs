using Microsoft.Data.SqlClient;

string connectionString = "Server=ServerName;Database=AdventureWorks2022;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=True;";
string solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));

// Append the project folder name (adjust this if your project folder is named differently)
string projectDir = Path.Combine(solutionRoot, "GenerateRandomPeopleImg");

// Now build photo folder paths
string malePhotoDir = Path.Combine(projectDir, "EmployeePhotos", "M");
string femalePhotoDir = Path.Combine(projectDir, "EmployeePhotos", "F");

string[] malePhotos = Directory.GetFiles(malePhotoDir, "*.jpg");
string[] femalePhotos = Directory.GetFiles(femalePhotoDir, "*.jpg");

using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();

    string query = @"
    SELECT e.BusinessEntityID, e.Gender
    FROM HumanResources.Employee e
    WHERE e.Photo IS NULL";

    using SqlCommand cmd = new SqlCommand(query, conn);
    using SqlDataReader reader = cmd.ExecuteReader();

    List<(int Id, string Gender)> employees = new List<(int, string)>();

    while (reader.Read())
    {
        int id = reader.GetInt32(0);
        string gender = reader.GetString(1);
        employees.Add((id, gender));
    }

    reader.Close();

    Random rand = new Random();

    foreach (var emp in employees)
    {
        string[] photoList = emp.Gender == "M" ? malePhotos : femalePhotos;
        if (photoList.Length == 0) continue;

        string selectedPhoto = photoList[rand.Next(photoList.Length)];
        byte[] photoData = File.ReadAllBytes(selectedPhoto);

        using SqlCommand updateCmd = new SqlCommand(
            "UPDATE HumanResources.Employee SET Photo = @photo WHERE BusinessEntityID = @id", conn);
        updateCmd.Parameters.AddWithValue("@photo", photoData);
        updateCmd.Parameters.AddWithValue("@id", emp.Id);
        updateCmd.ExecuteNonQuery();

        Console.WriteLine($"Updated employee {emp.Id} with {Path.GetFileName(selectedPhoto)}");
    }
}
Console.WriteLine("Photo assignment complete.");
