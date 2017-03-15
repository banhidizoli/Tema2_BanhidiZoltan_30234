create database UniversityDatabase;

use UniversityDatabase;

create table Student(
	ID int auto_increment not null primary key,
    Name varchar(40),
    BirthDate Date,
    Adress varchar(40));
    
create table Course(
	ID int auto_increment not null primary key,
    Name varchar(40),
    TeacherName varchar(40),
    StudyYear int);
    
create table StudentToCourse(
	StudentID int not null unique,
    CourseID int not null unique,
    Grade tinyint,
    primary key(StudentID, CourseID),
    foreign key (StudentID) references Student(ID),
    foreign key (CourseID) references Course(ID));
    
insert into Student(Name, BirthDate, Adress) values
	("Banhidi Zoltan", "1995-06-26", "Zalau, Closca 47");