using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Demo.Models
{
    /// <summary>
    /// Student Data Model
    /// </summary>
    [XmlType(TypeName = "student")]
    public class Student
    {
        /// <summary>
        /// Enumeration for Student Grade level
        /// </summary>
        public enum GradeEnum
        {
            PreSchool = -1,
            Kindergarten = 0,
            First = 1,
            Second,
            Third,
            Fourth,
            Fifth,
            Sixth,
            Seventh,
            Eighth,
            Freshmen,
            Sophomore,
            Junior,
            Senior,
            College
        } // end of enum

        [XmlAttribute(AttributeName = "id")]
        public int ID { get; set; }

        [XmlElement(ElementName = "lastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "firstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "dob")]
        public DateTime DOB { get; set; }

        [XmlElement(ElementName = "gpa")]
        public float GPA { get; set; }

        [XmlElement(ElementName = "grade")]
        public GradeEnum Grade { get; set; }

    } // end of class
} // end of namespace