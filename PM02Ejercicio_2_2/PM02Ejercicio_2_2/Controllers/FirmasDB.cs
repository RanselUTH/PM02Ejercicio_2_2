using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PM02Eje2_2.Models;

namespace PM02Eje2_2.Controllers
{
    public class FirmasDB
    {
        readonly SQLiteAsyncConnection db;
        public FirmasDB() { }

        public FirmasDB(String pathbasedatos)
        {
            db = new SQLiteAsyncConnection(pathbasedatos);
            db.CreateTableAsync<Firmas>();
        }

        public Task<List<Firmas>> ListViewFirmas()
        {
            return db.Table<Firmas>().ToListAsync();
        }

        public Task<Int32> GuardarFirmas(Firmas firma)
        {
            if (firma.Id != 0)
            {
                return db.UpdateAsync(firma);
            }
            else
            {
                return db.InsertAsync(firma);
            }
        }

        public Task<Firmas> BuscarFirma(int fcodigo)
        {
            return db.Table<Firmas>()
                .Where(i => i.Id == fcodigo)
                .FirstOrDefaultAsync();
        }

        public Task<Int32> EliminarFirma(Firmas firma)
        {
            return db.DeleteAsync(firma);
        }

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
