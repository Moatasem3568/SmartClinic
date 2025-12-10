-- التحقق من وجود قاعدة البيانات وإنشائها إذا لم تكن موجودة
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'clinic')
BEGIN
    CREATE DATABASE clinic;
    PRINT 'تم إنشاء قاعدة البيانات clinic بنجاح';
END
ELSE
BEGIN
    PRINT 'قاعدة البيانات clinic موجودة بالفعل';
END
GO

USE clinic;
GO

IF OBJECT_ID('Appointments', 'U') IS NOT NULL
BEGIN
    PRINT 'جاري حذف جدول Appointments...';
    DROP TABLE Appointments;
    PRINT 'تم حذف جدول Appointments';
END
GO

IF OBJECT_ID('Patients', 'U') IS NOT NULL
BEGIN
    PRINT 'جاري حذف جدول Patients...';
    DROP TABLE Patients;
    PRINT 'تم حذف جدول Patients';
END
GO

IF OBJECT_ID('Doctors', 'U') IS NOT NULL
BEGIN
    PRINT 'جاري حذف جدول Doctors...';
    DROP TABLE Doctors;
    PRINT 'تم حذف جدول Doctors';
END
GO

IF OBJECT_ID('Users', 'U') IS NOT NULL
BEGIN
    PRINT 'جاري حذف جدول Users...';
    DROP TABLE Users;
    PRINT 'تم حذف جدول Users';
END
GO

-- إنشاء الجداول من جديد
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL
);

PRINT 'تم إنشاء جدول Users';

INSERT INTO Users (UserName, PasswordHash)
VALUES (N'admin', N'1234');

PRINT 'تم إضافة المستخدم الافتراضي';

CREATE TABLE Doctors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Specialty NVARCHAR(100) NULL
);

PRINT 'تم إنشاء جدول Doctors';

CREATE TABLE Patients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Phone NVARCHAR(50) NULL,
    Address NVARCHAR(200) NULL,
    Notes NVARCHAR(MAX) NULL
);

PRINT 'تم إنشاء جدول Patients';

CREATE TABLE Appointments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Notes NVARCHAR(MAX) NULL,
    CONSTRAINT FK_Appointments_Patients FOREIGN KEY (PatientId) 
        REFERENCES Patients(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Appointments_Doctors FOREIGN KEY (DoctorId) 
        REFERENCES Doctors(Id) ON DELETE CASCADE
);

PRINT 'تم إنشاء جدول Appointments';

PRINT 'تم إنشاء جميع الجداول بنجاح!';
-- إضافة أطباء افتراضيين
INSERT INTO Doctors (Name, Specialty)
VALUES 
    (N'د. أحمد محمد', N'طب عام'),
    (N'د. سارة خالد', N'أمراض باطنية'),
    (N'د. علي حسن', N'جراحة');

-- إضافة مرضى افتراضيين
INSERT INTO Patients (FullName, Phone, Address)
VALUES 
    (N'محمد عبدالله', N'0551234567', N'تعز - الحصب '),
    (N'فاطمة سعيد', N'0509876543', N'تعز - حي العسكري');

PRINT 'تم إضافة بيانات افتراضية';