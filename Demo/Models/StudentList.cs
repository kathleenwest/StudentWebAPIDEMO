using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Serialization;

namespace Demo.Models
{
    /// <summary>
    /// StudentList
    /// Serializes and Deserializes data to/from an
    /// xml file
    /// </summary>
    [XmlRoot(ElementName = "students")]
    public class StudentList : List<Student>
    {
        /// <summary>
        /// Load
        /// Loads data fom an xml file into memory
        /// </summary>
        /// <param name="filename">(string) name of file to load xml</param>
        /// <returns></returns>
        public static StudentList Load(string filename)
        {
            StudentList list = new StudentList();
            XmlSerializer ser = new XmlSerializer(typeof(Models.StudentList));
            using (StreamReader reader = new StreamReader(filename))
            {
                if (reader != null)
                {
                    list = ser.Deserialize(reader) as Models.StudentList;
                }
            }
            return list;
        } // end of Load method

        /// <summary>
        /// Save
        /// Saves and writes the data to XML file
        /// </summary>
        /// <param name="filename">(string) name of file to write data</param>
        public void Save(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Models.StudentList));
            using (StreamWriter writer = new StreamWriter(filename))
            {
                ser.Serialize(writer, this);
            }
        } // end of Save method

    } // end of class
} // end of namespace