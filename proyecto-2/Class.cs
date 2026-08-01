/*
-- Eliminar tablas si existen para recrearlas limpias (en orden inverso por las llaves foráneas)
DROP TABLE IF EXISTS Incidente;
DROP TABLE IF EXISTS Asistencia;
DROP TABLE IF EXISTS Comprobante;
DROP TABLE IF EXISTS Inscripcion;
DROP TABLE IF EXISTS Curso;
DROP TABLE IF EXISTS Horario;
DROP TABLE IF EXISTS EspacioFisico;
DROP TABLE IF EXISTS Participante;
DROP TABLE IF EXISTS Tutor;
DROP TABLE IF EXISTS Usuario;

CREATE TABLE Usuario (
    UsuarioID INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioRed VARCHAR(50) NOT NULL UNIQUE,
    NombreCompleto VARCHAR(100) NOT NULL,
    Rol VARCHAR(30) NOT NULL,
    Activo TINYINT(1) NOT NULL DEFAULT 1
);

CREATE TABLE Tutor (
    TutorID INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(80) NOT NULL,
    Identificacion VARCHAR(20) NOT NULL UNIQUE,
    Telefono VARCHAR(20) NOT NULL,
    Email VARCHAR(100) NULL
);

CREATE TABLE Participante (
    ParticipanteID INT AUTO_INCREMENT PRIMARY KEY,
    TutorID INT NOT NULL,
    Identificacion VARCHAR(20) NOT NULL UNIQUE,
    Nombre VARCHAR(80) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Alergias VARCHAR(250) NULL,
    ContactoEmergenciaNombre VARCHAR(80) NULL,
    ContactoEmergenciaTelefono VARCHAR(20) NULL,
    FOREIGN KEY (TutorID) REFERENCES Tutor(TutorID)
);

CREATE TABLE EspacioFisico (
    EspacioID INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(60) NOT NULL,
    Tipo VARCHAR(30) NOT NULL,
    CapacidadMaxima INT NOT NULL
);

CREATE TABLE Horario (
    HorarioID INT AUTO_INCREMENT PRIMARY KEY,
    DiaSemana VARCHAR(15) NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL
);

CREATE TABLE Curso (
    CursoID INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(80) NOT NULL,
    Categoria VARCHAR(40) NOT NULL,
    CupoMaximo INT NOT NULL,
    InstructorID INT NOT NULL,
    EspacioID INT NOT NULL,
    HorarioID INT NOT NULL,
    FOREIGN KEY (InstructorID) REFERENCES Usuario(UsuarioID),
    FOREIGN KEY (EspacioID) REFERENCES EspacioFisico(EspacioID),
    FOREIGN KEY (HorarioID) REFERENCES Horario(HorarioID)
);

CREATE TABLE Inscripcion (
    InscripcionID INT AUTO_INCREMENT PRIMARY KEY,
    ParticipanteID INT NOT NULL,
    CursoID INT NOT NULL,
    FechaInscripcion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Activa',
    FOREIGN KEY (ParticipanteID) REFERENCES Participante(ParticipanteID),
    FOREIGN KEY (CursoID) REFERENCES Curso(CursoID)
);

CREATE TABLE Comprobante (
    ComprobanteID INT AUTO_INCREMENT PRIMARY KEY,
    InscripcionID INT NOT NULL UNIQUE,
    NumeroComprobante VARCHAR(20) NOT NULL UNIQUE,
    FechaEmision DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (InscripcionID) REFERENCES Inscripcion(InscripcionID)
);

CREATE TABLE Asistencia (
    AsistenciaID INT AUTO_INCREMENT PRIMARY KEY,
    InscripcionID INT NOT NULL,
    Fecha DATE NOT NULL,
    Estado VARCHAR(15) NOT NULL,
    RegistradoPor INT NOT NULL,
    FOREIGN KEY (InscripcionID) REFERENCES Inscripcion(InscripcionID),
    FOREIGN KEY (RegistradoPor) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE Incidente (
    IncidenteID INT AUTO_INCREMENT PRIMARY KEY,
    ParticipanteID INT NOT NULL,
    CursoID INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Descripcion VARCHAR(500) NOT NULL,
    RegistradoPor INT NOT NULL,
    FOREIGN KEY (ParticipanteID) REFERENCES Participante(ParticipanteID),
    FOREIGN KEY (CursoID) REFERENCES Curso(CursoID),
    FOREIGN KEY (RegistradoPor) REFERENCES Usuario(UsuarioID)
);
 */