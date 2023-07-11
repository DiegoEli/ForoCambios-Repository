using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repositories
{
    public class CursoRepository
    {
        private readonly SchoolContext _context = new SchoolContext();

        public async Task guardarCurso(Course curso)
        {
            _context.Courses.Add(curso);
            await _context.SaveChangesAsync();
        }

    }
}
