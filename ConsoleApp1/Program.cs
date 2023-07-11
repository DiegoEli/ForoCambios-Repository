using ConsoleApp1.Models;
using ConsoleApp1.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata;

class Program
{
    static async Task Main(string[] args)
    {
        //await agregarEstudianteAsync();
        //await consultarEstudiantesAsync();
        //await consultarEstudianteAsync();
        //await modificarEstudianteAsync();
        //await eliminarEstudianteAsync();
        //await consultarEstudiantesFuncionesAsync();
        //await guardarEstudianteYdireccionAsync();
        //await guardarEstudianteYdireccionTransactionAsync();
        //await consultarDireccionesAsync();
        //await consultarDireccionAsync();
        //await consultarDireccion2Async();
        //await guardarCursoAsyncAsync();
        //await guardarEstudianteCursoAsync();
        //await consultarAlumnosyCursosAsync();
        await guardarEstudianteAsync();

        //consultarAlumnosyCursos();
    }

    public static async Task guardarEstudianteAsync()
    {
        Console.WriteLine("Guardar Estudiantes desde la clase Repository");
        
        EstudianteRepository estudianteRepository = new EstudianteRepository();
        Student std = new Student();
        std.Name = "Lola";
        std.LastName = "Perez";

        await estudianteRepository.guardarEstudiante(std);
    }

    public static async Task guardarCursoAsync()
    {
        Console.WriteLine("Guardar Curso");

        CursoRepository cursoRepository = new CursoRepository();
        Course course = new Course();
        course.CourseName = "Tercero";

        await cursoRepository.guardarCurso(course);
    }

    public static async Task guardarEstudianteCursoAsync()
    {
        Console.WriteLine("Guardar EstudianteCurso");

        EstudianteCursoRepository estudianteCursoRepository = new EstudianteCursoRepository();
        StudentCourse stdCourse = new StudentCourse();
        /*
        Student std = new Student();
        std = context.Students.Find(1);
        
        Course course = new Course();
        course = context.Courses.Find(1);
        */
        stdCourse.CourseId = 3;//course.CourseId;
        stdCourse.StudentId = 3; //std.StudentId;
        /*
        stdCourse.Student = std;
        stdCourse.Course = course;
        */

        await estudianteCursoRepository.guardarEstudianteCurso(stdCourse);
    }

    public static async Task guardarEstudianteYdireccionAsync()
    {
        Console.WriteLine("Metodo agregar estudiante y direccion");

        EstudianteRepository estudianteRepository = new EstudianteRepository();
        EstudianteYdireccionRepository estudianteYdireccionRepository = new EstudianteYdireccionRepository();
        
        Student std = new Student();
        StudentAddress stdAddress = new StudentAddress();
        
        std.Name = "Ciri";
        
        await estudianteRepository.guardarEstudiante(std);

        stdAddress.Address1 = "direccion 1";
        stdAddress.Address2 = "direccion 2";
        stdAddress.StudentID = std.StudentId;
        stdAddress.City = "gye";
        stdAddress.State = "ecu";
        stdAddress.Student= std;
       
        await estudianteYdireccionRepository.guardarEstudianteYdireccion(stdAddress);
    }

    public static async Task guardarEstudianteYdireccionTransactionAsync()
    {
        Console.WriteLine("Metodo agregar estudiante y direccion");

        SchoolContext context = new SchoolContext();
        Student std = new Student();
        StudentAddress stdAddress = new StudentAddress();
        var dbContextTransaction = context.Database.BeginTransaction();
        
        try
        {
            std.Name = "Karina";
            context.Students.Add(std);
            context.SaveChanges();

            stdAddress.Address1 = "direccion 1";
            stdAddress.Address2 = "direccion 2";
            stdAddress.StudentID = std.StudentId;
            stdAddress.City = "gye";
            stdAddress.State = "ecu";

            context.StudentAddresses.Add(stdAddress);

            context.SaveChanges();
            dbContextTransaction.Commit();
            Console.WriteLine("Datos guardados con exito");


        }
        catch (Exception e)
        {
            dbContextTransaction.Rollback();
            Console.WriteLine("Error "+ e.ToString());
        }
       
    }

    public static async Task consultarDireccionesAsync()
    {
        Console.WriteLine("Consultar direcciones");
        //Console.WriteLine("Metodo consultar estudiante por Id");
        DireccionesRepository direccionesRepository = new DireccionesRepository();
        List<StudentAddress> listaDirecciones;
        listaDirecciones = await direccionesRepository.consultarDirecciones();
        
        foreach (var item in listaDirecciones)
        {
            Console.WriteLine("Codigo:"+ item.Student.StudentId +
                " Nombre: " + item.Student.Name + 
                " Direccion:" + item.Address1);
        }

    }

    public static async Task consultarDireccionAsync()
    {
        Console.WriteLine("Consultar direccion por Id");
        //Console.WriteLine("Metodo consultar estudiante por Id");
        DireccionesRepository direccionesRepository = new DireccionesRepository();
        StudentAddress address = new StudentAddress();
        address = await direccionesRepository.consultarDireccion(1);
        
        Console.WriteLine("Codigo: " + address.Student.StudentId +
                " Nombre: " + address.Student.Name +
                " Direccion: " + address.Address1);

    }

    public static async Task consultarDireccion2Async()
    {
        Console.WriteLine("Consultar direccion por Id, metodo 2");
        
        DireccionesRepository direccionesRepository = new DireccionesRepository();
        StudentAddress address = new StudentAddress();
        address = await direccionesRepository.consultarDireccion2(1);

        /*
        context.Entry(address)
          .Collection(x => x.Student)
          .Load();
        */

        Console.WriteLine("Codigo: " + address.Student.StudentId +
                " Nombre: " + address.Student.Name +
                " Direccion: " + address.Address1);


    }

    public static async Task consultarAlumnosyCursosAsync()
    {
        Console.WriteLine("Consultar un Alumnos y sus cursos con Include");

        AlumnosyCursosRepository alumnosyCursosRepository = new AlumnosyCursosRepository();
        List< StudentCourse> std;
        std = await alumnosyCursosRepository.consultarAlumnosyCursos(13);

        Console.WriteLine("Cursos del estudiante " + std[0].StudentId + " " + std[0].Student.Name);

        foreach (var item in std)
        {
            Console.WriteLine("Curso: " + item.CourseId + " " + item.Course.CourseName);
        }


    }

    public static async Task consultarAlumnosyCursos2Async()
    {
        Console.WriteLine("Consultar un Alumno y sus cursos");

        SchoolContext context = new SchoolContext();
        Student std = new Student();
        std = context.Students
            .Single(x => x.StudentId == 3);

        context.Entry(std)
            .Reference(x => x.StudentAddress)
            .Load();

        context.Entry(std)
          .Collection(x => x.StudentCourse )
          .Load();

        for (int i = 0; i < 3; i++)
        {
            context.Entry(std.StudentCourse[i])
              .Reference(x => x.Course)
              .Load();
        }

        Console.WriteLine("Cursos del estudiante " + std.StudentId + " " + std.Name);
        
        foreach (var item in std.StudentCourse)
        {
            Console.WriteLine("Curso: " + item.CourseId + " " + item.Course.CourseName);
        }


    }
    //agregar estudiante
    public static async Task agregarEstudianteAsync()
    {
        Console.WriteLine("Metodo agregar estudiante");
        
        EstudianteRepository estudianteRepository = new EstudianteRepository();
        Student std = new Student();
        std.Name = "Pedro";
        await estudianteRepository.agregarEstudiante();
      
        Console.WriteLine("Codigo: "+ std.StudentId + " Nombre: "+ std.Name);

    }

    public static async Task consultarEstudiantesAsync()
    {
        Console.WriteLine("Metodo consultar estudiantes");

        EstudianteRepository estudianteRepository = new EstudianteRepository();
        List<Student> listEstudiantes = await estudianteRepository.consultarEstudiantes() ;

        foreach (var item in listEstudiantes)
        {
            Console.WriteLine("Codigo: " + item.StudentId + " Nombre: " + item.Name);
        }
        
    }

    public static async Task consultarEstudianteAsync()
    {
        Console.WriteLine("Metodo consultar estudiante por Id");

        EstudianteRepository estudianteRepository = new EstudianteRepository();
        Student std = new Student();
        std = estudianteRepository.consultarEstudiante(11);

       Console.WriteLine("Codigo: " + std.StudentId + " Nombre: " + std.Name);
      
    }

    public static async Task modificarEstudianteAsync()
    {
        Console.WriteLine("Metodo modificar estudiante");

        SchoolContext context = new SchoolContext();
        Student std = new Student();
        std = context.Students.Find(1);

        std.Name = "Anahi";
        context.SaveChanges();
        Console.WriteLine("Codigo: " + std.StudentId + " Nombre: " + std.Name);

    }

    public static async Task eliminarEstudianteAsync()
    {
        Console.WriteLine("Metodo eliminar estudiante");
        
        SchoolContext context = new SchoolContext();
        Student std = new Student();
        std = context.Students.Find(5);
        context.Remove(std);
        context.SaveChanges();
        Console.WriteLine("Codigo: " + std.StudentId + " Nombre: " + std.Name);

    }

    public static async Task consultarEstudiantesFuncionesAsync()
    {
        Console.WriteLine("Metodo consultar estudiantes con el uso de funciones");
        SchoolContext context = new SchoolContext();
        List<Student> listEstudiantes;

        Console.WriteLine("Cantidad de registros: " + context.Students.Count());
        Student std = context.Students.First();

        Console.WriteLine("Primer elemento de la tabla:" +  std.StudentId +"-" +std.Name);

        //listEstudiantes = context.Students.Where(s => s.StudentId > 2 && s.Name == "Anita").ToList();

        //listEstudiantes = context.Students.Where(s => s.Name == "Patty" || s.Name == "Anita").ToList();

        listEstudiantes = context.Students.Where(s => s.Name.StartsWith("A")).ToList();

        foreach (var item in listEstudiantes)
        {
            Console.WriteLine("Codigo: " + item.StudentId + " Nombre: " + item.Name);
        }

        /*
        var query = context.Students.GroupBy( s => s.Name) 
        .Select(g => new
        {
            Nombre = g.Key,
            Cantidad = g.Count()
        }). ToList();

        foreach (var result in query)
        {
            Console.WriteLine($"Nombre: {result.Nombre}, Cantidad: {result.Cantidad}");
        }
        */

    }

}

