using System;
using System.IO;

namespace Backend.TechChallenge.Api.Controllers
{
    public partial class UsersController
    {
        private StreamReader ReadUsersFromFile()
        {
            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);
            return reader;
        }

        private void WriteUserToFile(User user)
        {
            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";

            using (StreamWriter outputFile = new StreamWriter(path, append: true))
            {
                outputFile.WriteLine(user.Name + "," + user.Email + "," + user.Phone + "," + user.Address + "," + user.UserType + "," + user.Money);
            }
        }
    }
}
