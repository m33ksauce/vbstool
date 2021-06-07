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
        static string parentPath;
        static string duplexPath;
        static int numberOfLines;
        static List<Student> students = new List<Student>();
        //static List<String> duplexFile;
        static String[] duplexFile;

        static void Main(string[] args)
        {
            ParseArgs(args);
            GenerateDestination();
            ParseCSV();
            prepForDuplex();
            PrintFiles();
        }

        static void ParseArgs(string[] args)
        {
            //fileName = "D:/VBS.csv";
            fileName = args[0];
        }
        static void GenerateDestination()
        {
            String directory = Path.GetDirectoryName(fileName);
            String fileNoExt = Path.GetFileNameWithoutExtension(fileName);
            parentPath = directory + fileNoExt + "-parent_cards.csv";
            duplexPath = directory + fileNoExt + "-duplex.csv";
        }

        static void ParseFile()
        {
            string[] lines = File.ReadAllLines(fileName)
                .Skip(1)
                .ToArray();

            foreach (string l in lines)
            {
                var student = ParseLine(l);
                students.Add(student);
            }
        }

        static void ParseCSV()
        {
            TextFieldParser csvParser = new TextFieldParser(fileName);
            csvParser.SetDelimiters(new string[] { "," });

            csvParser.ReadLine();
            numberOfLines = 1;
            while (!csvParser.EndOfData)
            {
                string[] fields = csvParser.ReadFields();
                Student student = Student.ParseFromLine(fields);
                students.Add(student);
                numberOfLines++;
            }


        }

        static Student ParseLine(string line)
        {
            var lineArray = line.Split(',');
            return Student.ParseFromLine(lineArray);
        }

        static void PrintFiles()
        {
            var parentGrouped = students
                .GroupBy(s => s.ParentName);
            var csvOutput = new System.Text.StringBuilder();
            var csvOutput2 = new System.Text.StringBuilder();
            csvOutput.AppendLine("Parent, Student1Name, Student1Crew, Student2Name, Student2Crew, Student3Name, Student3Crew, Student4Name, Student4Crew");
            foreach (var parent in parentGrouped)
            {
                csvOutput.AppendLine(parent.Aggregate(parent.Key, (str, nx) => $"{str}, {nx.PrintName()}"));
            }
            File.WriteAllText(parentPath, csvOutput.ToString());

            foreach (var s in duplexFile)
            {
                if (s==null)
                {
                    csvOutput2.AppendLine("blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank"); 
                }
                else {
                    csvOutput2.AppendLine(s);
                }
            }
            File.WriteAllText(duplexPath, csvOutput2.ToString());
        }
        static void prepForDuplex()
        {
            int labelsPerPage = 8;
            int currentLabel = 0;
            int currentPage = 0;
            string myLine;
            numberOfLines = (numberOfLines + (numberOfLines % labelsPerPage)) *2;
            duplexFile = new string[numberOfLines];

            System.IO.StreamReader file =
                new System.IO.StreamReader(@fileName);
            while ((myLine = file.ReadLine()) != null)
            {
                if (currentLabel % labelsPerPage == 1)
                {
                    currentPage++;
                }
                if (currentLabel == 0)
                {
                    duplexFile[currentLabel] = myLine; // Do not duplicate the headers.
                }
                else
                {

                    if (IsEven(currentLabel))
                    {
                        duplexFile[currentLabel + (((currentPage - 1) * labelsPerPage))] = myLine;
                        duplexFile[(currentLabel + ((((currentPage - 1) * labelsPerPage)))) + labelsPerPage-1] = myLine;

                    }
                    else
                    {
                        duplexFile[currentLabel + ((currentPage - 1) * labelsPerPage)] = myLine;
                        duplexFile[(currentLabel + ((((currentPage - 1) * labelsPerPage)))) + labelsPerPage + 1] = myLine;
                    }
                }
                currentLabel++;
                Console.WriteLine("Page: " + currentPage + " Label: "+ currentLabel);
            }




            static bool IsEven(int number)
            {
                return (number % 2 == 0);
            }
        }
    }
}