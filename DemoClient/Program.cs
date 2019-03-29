using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Net;

namespace DemoClient
{
    /// <summary>
    /// Test Client 
    /// Console Application to test the students web api
    /// CRUD methods are tested for the api
    /// </summary>
    class Program
    {
        // Base Address for the API and controller
        const string BASE_ADDR = "http://localhost:64611/api/students/";
        // random number to make IDs
        static Random random = new Random(); 

        /// <summary>
        /// Main Entry method
        /// </summary>
        /// <param name="args">No arguments used</param>
        static void Main(string[] args)
        {
            #region Test Get Student by ID

            Console.WriteLine(new string('=', 20));
            Console.WriteLine("Testing Get Student by ID");
            Console.WriteLine(new string('=', 20));

            var result = ClientHelper.Get<Student>(BASE_ADDR, SerializationModesEnum.Json, "{0}", 136655918);
            Console.WriteLine(result.Result);

            #endregion Test Get Student by ID

            #region Test Get Students by Page

            Console.WriteLine(new string('=', 20));
            Console.WriteLine("Testing Get Student List by Page");
            Console.WriteLine(new string('=', 20));

            var results = ClientHelper.Get<List<Student>>(BASE_ADDR, SerializationModesEnum.Json, "?page={0}&count={1}", 5, 5);

            foreach (var s in results.Result)
            {
                Console.WriteLine(s);
            }

            #endregion Test Get Students by Page

            #region Test Post/Add New Student

            Console.WriteLine(new string('=', 20));
            Console.WriteLine("Testing Post/Add new Student");
            Console.WriteLine(new string('=', 20));

            Student add = new Student
            {
                ID = 12346788,
                LastName = "West",
                FirstName = "Kathleen",
                Grade = Student.GradeEnum.College,
                DOB = new DateTime(1981, 01, 19),
                GPA = 3.99f
            };

            add.ID = random.Next(1, 20000);          
            Console.WriteLine(add);

            Console.WriteLine("Try to POST new Student by ID...");
            // Post New Student
            PostStudent(add);

            // Verify POST with Get
            Console.WriteLine("Verify POST operation by getting student information...");
            result = ClientHelper.Get<Student>(BASE_ADDR, SerializationModesEnum.Json, "{0}", add.ID);
            Console.WriteLine(result.Result);

            Console.WriteLine("Try to POST Duplicate Student by ID...");

            // Post Duplicate Student
            PostStudent(add);

            #endregion Test Post/Add New Student

            #region Test Put/Modify Student
            
            Student put = new Student
            {
                ID = add.ID,
                LastName = add.LastName,
                FirstName = add.FirstName,
                Grade = add.Grade,
                DOB = add.DOB,
                GPA = add.GPA
            };

            Console.WriteLine(new string('=', 20));
            Console.WriteLine("Testing Put/Modify Student");
            Console.WriteLine(new string('=', 20));

            // Existing Put Test
            Console.WriteLine(put);
            Console.WriteLine("Existing Student name is now: Jane Doe");
            put.FirstName = "Jane";
            put.LastName = "Doe";
            Console.WriteLine(put);
            Console.WriteLine("Try put/update operation on existing student...");
            PutStudent(put);

            // Verify Update with Get
            Console.WriteLine("Verify put/update operation by getting student information...");
            result = ClientHelper.Get<Student>(BASE_ADDR, SerializationModesEnum.Json, "{0}", put.ID);
            Console.WriteLine(result.Result);

            // Non-existing Put Test
            put.ID = random.Next(1, 20000);
            Console.WriteLine("New Student ID is: {0}", put.ID);
            Console.WriteLine(put);
            Console.WriteLine("Try put/update operation on NEW student...");
            PutStudent(put);

            // Verify Update with Get
            Console.WriteLine("Verify put/update operation by getting student information...");
            result = ClientHelper.Get<Student>(BASE_ADDR, SerializationModesEnum.Json, "{0}", put.ID);
            Console.WriteLine(result.Result);

            #endregion Test Put/Modify Student

            #region Test Delete Student

            Student delete = new Student
            {
                ID = add.ID,
                LastName = add.LastName,
                FirstName = add.FirstName,
                Grade = add.Grade,
                DOB = add.DOB,
                GPA = add.GPA
            };

            Console.WriteLine(new string('=', 20));
            Console.WriteLine("Testing Delete Student");
            Console.WriteLine(new string('=', 20));

            // Existing delete Test
            Console.WriteLine(delete);
            Console.WriteLine("Try delete operation on this existing student...");
            DeleteStudent(delete);
            Console.WriteLine("Try to get the previous deleted student... Expected: null");
            result = ClientHelper.Get<Student>(BASE_ADDR, SerializationModesEnum.Json, "{0}", delete.ID);

            // Non-existing student delete test
            // Existing delete Test
            Console.WriteLine(delete);
            Console.WriteLine("Try delete operation on this already deleted student...");
            DeleteStudent(delete);
            Console.WriteLine("Try to get the previous deleted student... Expected: null");
            result = ClientHelper.Get<Student>(BASE_ADDR, SerializationModesEnum.Json, "{0}", delete.ID);

            #endregion Test Delete Student

            // Wait for the User Input
            Console.WriteLine("Press <Enter> to Quit...");
            Console.ReadLine();

        } // end of main method

        #region Helper methods for my testing

        /// <summary>
        /// PostStudent
        /// Processes helper class call inputs/outputs for testing
        /// </summary>
        /// <param name="add">(Student) student object to add/post</param>
        private static void PostStudent(Student add)
        {
            var addResult = ClientHelper.Post<Student, object>(BASE_ADDR, SerializationModesEnum.Json, add, string.Empty);

            if (addResult.StatusCode == HttpStatusCode.Conflict)
            {
                Console.WriteLine("Duplicate Student Conflict: The POST was unsuccessful.");
            }
            else if (!((int)addResult.StatusCode >= 200 && (int)addResult.StatusCode < 300))
            {

                Console.WriteLine("Error encountered: {0}", addResult.Error);
            }
            else
            {
                Console.WriteLine("The POST call was successful");
            }
        } // end of method

        /// <summary>
        /// PutStudent
        /// Processes helper class call inputs/outputs for testing
        /// </summary>
        /// <param name="put">(Student) student object</param>
        private static void PutStudent(Student put)
        {
            var putResult = ClientHelper.Put<Student, object>(BASE_ADDR, SerializationModesEnum.Json, put, "{0}", put.ID);

            if (!((int)putResult.StatusCode >= 200 && (int)putResult.StatusCode < 300))
            {
                Console.WriteLine("Error encountered: {0}", putResult.Error);
            }

            else
            {
                Console.WriteLine("The PUT call was successful");
            }
        } // end of method

        /// <summary>
        /// Delete Student
        /// Processes helper class call inputs/outputs for testing
        /// </summary>
        /// <param name="delete"></param>
        private static void DeleteStudent(Student delete)
        {
            var deleteResult = ClientHelper.Delete<Student, object>(BASE_ADDR, SerializationModesEnum.Json, delete, "{0}", delete.ID);

            if (!((int)deleteResult.StatusCode >= 200 && (int)deleteResult.StatusCode < 300))
            {
                Console.WriteLine("Error encountered: {0}", deleteResult.Error);
            }

            else
            {
                Console.WriteLine("The DELETE call was successful");
            }
        } // end of method

        #endregion Helper methods for my testing

    } // end of class
} // end of namespace
