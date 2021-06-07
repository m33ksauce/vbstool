using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace VBSTool
{
    class Program
    {
        static string fileName;
        static List<Student> students = new List<Student>();

        static void Main(string[] args)
        {
            ParseArgs(args);
            ParseCSV();
            PrintByParent();
        }

        static void ParseArgs(string[] args)
        {
            fileName = args[0];
        }

        static void ParseFile() {
            string[] lines = File.ReadAllLines(fileName)
                .Skip(1)
                .ToArray();
            foreach (string l in lines) {
                var student = ParseLine(l);
                students.Add(student);
            }
        }

        static void ParseCSV()
        {
            TextFieldParser csvParser = new TextFieldParser(fileName);
            csvParser.SetDelimiters(new string[] { "," });

            csvParser.ReadLine();

            while (!csvParser.EndOfData)
            {
                string[] fields = csvParser.ReadFields();
                Student student = Student.ParseFromLine(fields);
                students.Add(student);
            }

        }

        static Student ParseLine(string line)
        {
            var lineArray = line.Split(',');
            return Student.ParseFromLine(lineArray);
        }

        static void PrintByParent() {
            var parentGrouped = students
                .GroupBy(s => s.ParentName);

            foreach(var parent in parentGrouped)
            {
                Console.WriteLine(parent.Aggregate(parent.Key, (str, nx) => $"{str} {nx.PrintName()}"));
            }
        }
    }
}