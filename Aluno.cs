using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Escola
{
    public class Aluno
    {
        public string Nome { get; set; }
        public string Matricula { get; set; }
        public List<double> Notas { get; set; }

        public double Media()
        {
            double somaNotas = 0;
            foreach (var nota in this.Notas)
                somaNotas += nota;

            return somaNotas / this.Notas.Count;
        }

        public string Situacao()
        {
            return (this.Media() > 6 ? "Aprovado" : "Reprovado");
        }

        public string NotasFormadata()
        {
            return string.Join(",", this.Notas);
        }

        private static List<Aluno> alunos = new List<Aluno>();
        public static List<Aluno> Todos()
        {
            if (File.Exists(Aluno.CaminhoJson()))
            {
                var conteudo = File.ReadAllText(Aluno.CaminhoJson());
                Aluno.alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }

            return Aluno.alunos;
        }

        private static string CaminhoJson()
        {
            return System.Configuration.ConfigurationManager.AppSettings["caminho_json"];
        }

        public static void Adicionar(Aluno aluno)
        {
            Aluno.alunos = Aluno.Todos();
            Aluno.alunos.Add(aluno);
            string caminho = @"E:\PersistenciadeDados\alunos.json";
            File.WriteAllText(Aluno.CaminhoJson(), JsonConvert.SerializeObject(Aluno.alunos));
        }
    }
}
