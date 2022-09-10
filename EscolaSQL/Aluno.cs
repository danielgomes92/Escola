using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
        public static List<Aluno> TodosJson()                       //JSON
        {
            if (File.Exists(Aluno.CaminhoJson()))
            {
                var conteudo = File.ReadAllText(Aluno.CaminhoJson());
                Aluno.alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }

            return Aluno.alunos;
        }

        //SQL
        public static List<Aluno> TodosSql()                       //SQL
        {
            using (var cnn = new SqlConnection(Aluno.stringConexaoSql()))
            {
                using (var cmd = new SqlCommand("select * from alunos", cnn)) // cmd = Command
                {
                    using (SqlDataReader dr = cmd.ExecuteReader()) // dr = Data Reader
                    {
                        while (dr.Read()) // enquanto estiver lendo vai ser transformado na lista de aluno
                        {
                            var aluno = new Aluno();
                            aluno.Nome = dr["nome"].ToString();
                            aluno.Matricula = dr["matricula"].ToString();
                        }
                    }
                }
            }

            return Aluno.alunos;
        }
        private static string stringConexaoSql()
        {
            return System.Configuration.ConfigurationManager.AppSettings["conexao_sql"];
        }
        //SQL

        private static string CaminhoJson()
        {
            return System.Configuration.ConfigurationManager.AppSettings["caminho_json"];
        }

        public static void AdicionarJson(Aluno aluno)       //JSON
        {
            Aluno.alunos = Aluno.TodosJson();                        //JSON
            Aluno.alunos.Add(aluno);
            string caminho = @"E:\PersistenciadeDados\alunos.json";
            File.WriteAllText(Aluno.CaminhoJson(), JsonConvert.SerializeObject(Aluno.alunos));
        }
    }
}
