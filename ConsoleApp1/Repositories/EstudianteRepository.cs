using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repositories
{
    public class EstudianteRepository
    {
        private readonly SchoolContext _context= new SchoolContext();

        public async Task guardarEstudiante(Student student)
        {   
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Student>> consultarEstudiantes()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> consultarEstudiante(int Id)
        {
            return await _context.Students.
                Where(x => x.StudentId == Id).
                FirstAsync();
        }

    }
}
