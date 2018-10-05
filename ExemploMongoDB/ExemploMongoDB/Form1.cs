using ExemploMongoDB.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExemploMongoDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var database = cliente.GetDatabase("exemplo01");
            var collection = database.GetCollection<Professor>("professores");

            List<Materia> materias = new List<Materia>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++){
                materias.Add(new Materia() {
                    Nome = dataGridView2.Rows[i].Cells[0].Value.ToString(),
                    Hora = dateTimePicker1.Value
                });
            }

            Professor p = new Professor()
            {
                Nome = textBoxNome.Text,
                Salario = Convert.ToDouble(textBoxSalario.Text.Replace(',', '.')),
                Status = true,
                Materias = materias
            };
            textBoxNome.Text = "";
            textBoxSalario.Text = "";
            collection.InsertOne(p);
            AtualiazarTabela();
            textBoxNome.Focus();

       }

        private void Form1_Activated(object sender, EventArgs e)
        {
            AtualiazarTabela();
        }

        private void buttonApagar_Click(object sender, EventArgs e)
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var database = cliente.GetDatabase("exemplo01");
            var collection = database.GetCollection<Professor>("professores");
            int linhaSelecionada = dataGridView1.SelectedRows[0].Index;
            string objectId = dataGridView1.Rows[linhaSelecionada].Cells[0].Value.ToString() ;
            
            collection.DeleteOne(x => x.Id == new ObjectId(objectId));
        }

        private void textBoxBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                AtualiazarTabela();
            }
        }

        private void AtualiazarTabela()
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var database = cliente.GetDatabase("exemplo01");
            var collection = database.GetCollection<Professor>("professores");
            string busca = textBoxBusca.Text;
            IList<Professor> professores = collection.Find("{ Nome : /" + busca + "/}").ToList();
            dataGridView1.Rows.Clear();
            foreach (Professor professor in professores)
            {
                dataGridView1.Rows.Add(professor.Id, professor.Nome, professor.Salario, professor.Status);
            }
        }

        private void buttonAdicionar_Click(object sender, EventArgs e)
        {
            Materia m = new Materia()
            {
                Nome = textBoxMateria.Text,
                Hora  = dateTimePicker1.Value
            };
            textBoxMateria.Text = "";
            dateTimePicker1.Text = "";

            dataGridView2.Rows.Add(m.Nome, m.Hora);
        }

        private void buttonAlterar_Click(object sender, EventArgs e)
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var database = cliente.GetDatabase("exemplo01");
            var collection = database.GetCollection<Professor>("professores");
            ObjectId id = new ObjectId(textBoxAlterarId.Text);
            Professor p = new Professor()
            {
                Nome = textBoxAlterarNome.Text,
                Salario = Convert.ToDouble(textBoxAlterarSalario.Text.Replace(',', '.')),
                Status = radioButtonAtivo.Checked
            };

            var definicoes = Builders<Professor>.Update
                .Set(x => x.Nome, p.Nome)
                .Set(y => y.Salario, p.Salario)
                .Set(y => y.Status, p.Status);
            collection.UpdateOne(x => x.Id == id, definicoes);
            AtualiazarTabela();

        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var database = cliente.GetDatabase("exemplo01");
            var collection = database.GetCollection<Professor>("professores");
            int linhaSelecionada = dataGridView1.SelectedRows[0].Index;
            string objectId = dataGridView1.Rows[linhaSelecionada].Cells[0].Value.ToString();
            ObjectId id = new ObjectId(objectId);
            Professor professor = collection.Find(x => x.Id == id).First();
            textBoxAlterarId.Text = professor.Id.ToString();
            textBoxAlterarNome.Text = professor.Nome;
            textBoxAlterarSalario.Text = Convert.ToString(professor.Salario);

            if (professor.Status)
            {
                radioButtonAtivo.Checked = true;
            }
            else
            {
                radioButtonInativo.Checked = true;
            }
        }
    }
}
