using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repositories
{
    public class EstudianteydireccionesRepository
    {
        private readonly SchoolContext _context = new SchoolContext();

        public async Task guardarEstudianteDireccion(StudentAddress stdAddress)
        {
            _context.StudentAddresses.Add(stdAddress);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentAddress>> consultarDirecciones()
        {
            return await _context.StudentAddresses
                .Include(x => x.Student)
                .ToListAsync();
        }

        public async Task<StudentAddress> consultarDireccion(int Id)
        {
            return await _context.StudentAddresses.
                Where(x => x.StudentID == Id).
                Include(x => x.Student).
                FirstAsync();
        }

        public async Task<StudentAddress> consultarDireccion2(int Id)
        {
            StudentAddress address = new StudentAddress();
            address = await _context.StudentAddresses
                .SingleAsync(x => x.StudentID == Id);

            _context.Entry(address)
                .Reference(x => x.Student)
                .Load();

            return address;
        }

    }
}
