using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExemploMongoDB.Model
{
    class Professor
    {
        public ObjectId Id { get; set; }
        public string Nome { get; set; }
        public double Salario { get; set; }
        public bool Status { get; set; }
        public IList<Materia> Materias { get; set; }

    }
}
