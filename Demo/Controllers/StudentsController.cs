using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Demo.Models;
using System.Collections;
using System.Linq.Expressions;

namespace Demo.Controllers
{
    /// <summary>
    /// Controller for Students
    /// </summary>
    public class StudentsController : ApiController
    {
        /// <summary>
        /// Gets the full path to the XML document containing the student data
        /// </summary>
        private string FilePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/App_Data/student.xml");
            }
        } // end of method

        #region CRUD methods

        /// <summary>
        /// Get using paging
        /// Collection retrieval version of Get. Because our 
        /// dataset can be quite large we will need to limit 
        /// the results.This can be done with optional URL 
        /// parameters and the IQueryable interface 
        /// to provide paging support
        /// </summary>
        /// <param name="page">(int) page to retrieve</param>
        /// <param name="count">(int) number of items to retrieve</param>
        /// <returns>List of type student enuemerable list of students</returns>
        public List<Student> Get(int page = 0, int count = 50)
        {
            var data = StudentList.Load(FilePath).AsQueryable();
            return data.Skip(page * count).Take(count).ToList();
        } // end of method

        /// <summary>
        /// Get by id
        /// Get method that returns a specific Student. 
        /// This method will also use Linq to filter the
        /// results, but the IQueryable interface is not required
        /// </summary>
        /// <param name="id">(int) id of the student to retrieve</param>
        /// <returns>(Student) object</returns>
        public Student Get(int id)
        {
            var data = StudentList.Load(FilePath);
            return data.Where(s => s.ID == id).FirstOrDefault();
        } // end of method

        /// <summary>
        /// Post
        /// The Post method will represent Add. This is a bit 
        /// more complicated than Get as we have to check for duplicate
        /// Students and then save any changes. If there is a duplicate 
        /// we must throw an HttpException.
        /// </summary>
        /// <param name="value">(Student) object to create</param>
        public void Post([FromBody]Student value)
        {
            var data = StudentList.Load(FilePath);
            Student existing = data.Where(s => s.ID == value.ID).FirstOrDefault();

            if (existing == null)
            {
                data.Add(value);
                data.Save(FilePath);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }

        } // end of method

        /// <summary>
        /// Put
        /// Put represents the Update method. Just like the 
        /// Post action, the data is contained in the body 
        /// of the request. Unlike Post, however, if the 
        /// item exists then it is replaced. If the item 
        /// doesn’t exist then it is added.
        /// </summary>
        /// <param name="id">(int) id of the student to update</param>
        /// <param name="value">(Student) value to update</param>
        public void Put(int id, [FromBody]Student value)
        {
            var data = StudentList.Load(FilePath);
            Student existing = data.Where(s => s.ID == id).FirstOrDefault();
            if (existing == null)
            {
                data.Add(value);
            }
            else
            {
                data.RemoveAll(s => s.ID == id);
                data.Add(value);
            }
            data.Save(FilePath);
        } // end of Put method

        /// <summary>
        /// Delete
        /// Delete will remove a Student record that has the matching 
        /// ID but it will not fail if the ID is non-existent
        /// </summary>
        /// <param name="id">(integer) id of the student</param>
        public void Delete(int id)
        {
            var data = StudentList.Load(FilePath);
            Student existing = data.Where(s => s.ID == id).FirstOrDefault();
            if (existing != null)
            {
                data.RemoveAll(s => s.ID == id);
            }
            data.Save(FilePath);
        } // end of method

        #endregion CRUD methods

    } // end of class
} // end of namespace
