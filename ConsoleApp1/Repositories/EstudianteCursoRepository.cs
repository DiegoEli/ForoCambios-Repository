using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repositories
{
    public class EstudianteCursoRepository
    {
        private readonly SchoolContext _context = new SchoolContext();

        public async Task guardarEstudianteCurso(StudentCourse stdCourse)
        {
            _context.StudentCourses.Add(stdCourse);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentCourse>> consultarAlumnosyCursos(int Id)
        {
            return await _context.StudentCourses
                .Where(x => x.StudentId == Id)
                .Include(x => x.Course)
                .Include(x => x.Student)
                .ToListAsync();
        }

    }
}
