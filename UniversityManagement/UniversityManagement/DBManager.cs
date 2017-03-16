﻿using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace UniversityManagement {
    class DBManager : IDbManager {
        private MySqlConnection connection;
        private string connectionString;

        public DBManager(string username, string password) {
            connectionString = "server=localhost; userid=" + username + 
                "; password=" + password + "; database=UniversityDatabase";
            connection = null;
        }

        public bool OpenConnection() {
            try {
                connection = new MySqlConnection(connectionString);
                connection.Open();
            } catch(Exception e) {
                return false;
            }
            return true;
        }

        public void CloseConnection() {
            if (connection != null) {
                connection.Close();
                connection = null;
            }
        }

        public bool AddStudent(string name, string birthDate, string adress) {
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "insert into Student(Name, BirthDate, Adress) values " +
                    "(@Name, @BirthDate, @Adress);";
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@BirthDate", birthDate);
                command.Parameters.AddWithValue("@Adress", adress);
                command.ExecuteNonQuery();
            } catch(Exception e) {
                return false;
            }           
            return true;            
        }

        public bool ExistsStudent(string name, string birthDate, string adress) {
            MySqlDataReader reader = null;
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select ID from Student where Name = @Name and " +
                    "BirthDate = @BirthDate and " + "Adress = @Adress; ";
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@BirthDate", birthDate);
                command.Parameters.AddWithValue("@Adress", adress);
                reader = command.ExecuteReader();
                if (reader.Read())
                    return true;
                else
                    return false;
            } catch(Exception e) {
                return false;
            } finally {
                if (reader != null)
                    reader.Close();
            }
        }

        public bool ExistsStudent(int id) {
            MySqlDataReader reader = null;
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from Student where ID = @ID; ";
                command.Parameters.AddWithValue("@ID", id);
                reader = command.ExecuteReader();
                return reader.Read();
            } catch(Exception e) {
                return false;
            } finally {
                reader.Close();
            }
        }

        public bool DeleteStudent(int id) {
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "delete from UniversityDatabase.Student where ID = @ID; ";
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
                return true;
            } catch(Exception e) {
                return false;
            }
        }

        public bool AlterStudent(int id, string newName, string newBirthDate, string newAdress) {
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "update Student set Name = @Name, BirthDate = @BirthDate, " +
                    "Adress = @Adress where ID = @ID; ";
                command.Parameters.AddWithValue("@Name", newName);
                command.Parameters.AddWithValue("@BirthDate", newBirthDate);
                command.Parameters.AddWithValue("@Adress", newAdress);
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
                return true;
            } catch(Exception e) {
                return false;
            }
        }

        public bool AddCourse(string name, string teacherName, int studyYear) {
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "insert into Course(Name, TeacherName, StudyYear) values " +
                    "(@Name, @TeacherName, @StudyYear)";
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@TeacherName", teacherName);
                command.Parameters.AddWithValue("@StudyYear", studyYear);
                command.ExecuteNonQuery();
            } catch(Exception e) {
                return false;
            }
            return true;
        }

        public bool ExistsCourse(string name) {
            MySqlDataReader reader = null;
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select id from Course where Name = @Name; ";
                command.Parameters.AddWithValue("@Name", name);
                reader = command.ExecuteReader();
                return reader.Read();
            } catch(Exception e) {
                return false;
            } finally {
                if (reader != null)
                    reader.Close();
            }
        }

        public bool ExistsCourse(int id) {
            MySqlDataReader reader = null;
            try {
                MySqlCommand command = new MySqlCommand("select * from Course where ID = @ID;", connection);
                command.Parameters.AddWithValue("@ID", id);
                reader = command.ExecuteReader();
                return reader.Read();
            } catch(Exception e) {
                return false;
            } finally {
                if (reader != null)
                    reader.Close();
            }
        }

        public bool DeleteCourse(int id) {
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = this.connection;
                command.CommandText = "delete from Course where ID = @ID; ";
                command.Prepare();
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
            } catch(Exception e) {
                return false;
            }
            return true;
        }

        public bool EnrollStudentToCourse(int studentID, int courseID) {
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "insert into StudentToCourse(StudentID, CourseID) " +
                    "values (@StudID, @CourseID); ";
                command.Parameters.AddWithValue("@StudID", studentID);
                command.Parameters.AddWithValue("@CourseID", courseID);
                command.ExecuteNonQuery();
                return true;
            } catch(Exception e) {
                return false;
            }
        }

        public bool ExistsStudentToCourse(int studentID, int courseID) {
            MySqlDataReader reader = null;
            try {
                MySqlCommand command = new MySqlCommand("select * from StudentToCourse where " +
                    "StudentID = @SID and CourseID = @CID;", connection);
                command.Parameters.AddWithValue("@SID", studentID);
                command.Parameters.AddWithValue("@CID", courseID);
                reader = command.ExecuteReader();
                return reader.Read();
            } catch(Exception e) {
                return false;
            } finally {
                if (reader != null)
                    reader.Close();
            }
        }

        public bool AddGrade(int studentId, int courseId, int grade) {
            try {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "update StudentToCourse set Grade = @Grade " +
                    "where StudentID = @StudentID and CourseID = @CourseID; ";
                command.Parameters.AddWithValue("@StudentID", studentId);
                command.Parameters.AddWithValue("@CourseID", courseId);
                command.ExecuteNonQuery();
            } catch(Exception e) {
                return false;
            }
            return true;
        }

        public DataTable getStudentsDataTable() {
            try {
                DataTable dataTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand("select ID, Name, BirthDate, Adress " + 
                    "from Student;", connection);
                adapter.Fill(dataTable);
                return dataTable;
            } catch( Exception e) {
                return null; 
            }
        }

    }
}