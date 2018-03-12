using System;
using System.Collections.Generic;


namespace Public.Common.MATLAB
{
    /// <summary>
    /// Contains functionality for basic MATLAB commands.
    /// </summary>
    public static class Commands
    {
        private static string NotFound = @"not found";


        public static void AddPath(this MatlabApplication matlabApplication, string path)
        {
            matlabApplication.AddPathToTop(path);
        }

        public static void AddPathToTop(this MatlabApplication matlabApplication, string path)
        {
            matlabApplication.Path(path);
        }

        public static void ChangeCurrentDirectory(this MatlabApplication matlabApplication, string directoryPath)
        {
            matlabApplication.CurrentDirectory(directoryPath);
        }

        public static void Cell(this MatlabApplication matlabApplication, string variableName, int rows, int columns = 1)
        {
            string command = $@"{variableName} = cell({rows.ToString()}, {columns.ToString()});";
            matlabApplication.Execute(command);
        }

        public static void Clc(this MatlabApplication matlabApplication)
        {
            string command = @"clc;";
            matlabApplication.Execute(command);
        }

        public static void Clear(this MatlabApplication matlabApplication, string variableName)
        {
            string command = $@"clear {variableName}";
            matlabApplication.Execute(command);
        }

        public static void Clear(this MatlabApplication matlabApplication)
        {
            string command = @"clear;";
            matlabApplication.Execute(command);
        }

        public static void CreateEmptryStructure(this MatlabApplication matlabApplication, string name)
        {
            string command = Matlab.GetCreateEmptyStructureCommand(name);
            matlabApplication.Execute(command);
        }

        public static string CurrentDirectory(this MatlabApplication matlabApplication)
        {
            string command = $@"{Matlab.AnswerVariableName} = cd;";
            matlabApplication.Execute(command);

            string currentDirectory = matlabApplication.GetString(Matlab.AnswerVariableName);
            return currentDirectory;
        }

        public static void CurrentDirectory(this MatlabApplication matlabApplication, string directoryPath)
        {
            string command = $@"cd('{directoryPath}');";
            matlabApplication.Execute(command);
        }

        public static bool IsAvailable(this MatlabApplication matlabApplication, string item)
        {
            string whichFirstOnly = matlabApplication.WhichFirstOnly(item);

            bool output = !whichFirstOnly.Contains(Commands.NotFound);
            return output;
        }

        /// <summary>
        /// Allows loading a single variable previously saved with the specify variable name save command. This is required to have both load and save agree on the name of the saved variable.
        /// </summary>
        public static void LoadSpecific(this MatlabApplication matlabApplication, string filePath, string variableName)
        {
            using (Variable loadSaveStruct = new Variable(matlabApplication, Matlab.LoadSaveFixedStructureName))
            using (Variable filePathVar = new Variable(matlabApplication, (object)filePath))
            {
                List<string> commands = new List<string>
                {
                    $@"load({filePathVar.Name});",
                    $@"{variableName} = {loadSaveStruct.Name}.{Matlab.LoadSaveFixedFieldName}"
                };

                matlabApplication.Execute(commands);
            }
        }

        public static void Load(this MatlabApplication matlabApplication, string filepath)
        {
            using (var filePathVar = new Variable(matlabApplication, (object)filepath))
            {
                string command = $@"load({filePathVar.Name});";
                matlabApplication.Execute(command);
            }
        }

        public static string[] Path(this MatlabApplication matlabApplication)
        {
            string command = $@"{Matlab.AnswerVariableName} = path;";
            matlabApplication.Execute(command);

            string pathConcatenated = matlabApplication.GetString(Matlab.AnswerVariableName);

            string[] output = pathConcatenated.Split(';');
            return output;
        }

        /// <summary>
        /// Adds a path to the top of the MATLAB search path.
        /// </summary>
        public static void Path(this MatlabApplication matlabApplication, string path)
        {
            string command = $@"path('{path}', path);";
            matlabApplication.Execute(command);
        }

        public static void Reshape(this MatlabApplication matlabApplication, string inputVariableName, string outputVariableName, int[] size)
        {
            using (var sizesTemp = new Variable(matlabApplication, size))
            {
                string command = $@"{outputVariableName} = reshape({inputVariableName}, {sizesTemp.Name});";
                matlabApplication.Execute(command);
            }
        }

        public static string Reshape(this MatlabApplication matlabApplication, string inputVariableName, int[] size)
        {
            string outputVariableName = Matlab.GetTemporaryVariableName();
            matlabApplication.Reshape(inputVariableName, outputVariableName, size);

            return outputVariableName;
        }

        /// <summary>
        /// Allows saving a single variable in MATLAB such that it can later be loaded into a specified variable name. 
        /// </summary>
        public static void SaveSpecific(this MatlabApplication matlabApplication, string filePath, string variableName)
        {
            using (Variable loadSaveStruct = new Variable(matlabApplication, Matlab.LoadSaveFixedStructureName))
            using (Variable filePathVar = new Variable(matlabApplication, (object)filePath))
            {
                List<string> commands = new List<string>
                {
                    $@"{loadSaveStruct.Name}.{Matlab.LoadSaveFixedFieldName} = {variableName}",
                    $@"save({filePathVar.Name}, '{loadSaveStruct.Name}');"
                };

                matlabApplication.Execute(commands);
            }
        }

        public static int[] Size(this MatlabApplication matlabApplication, string variableName)
        {
            string tempVariableName = @"SizeTempVariable";
            string creationCommand = $@"{tempVariableName} = size({variableName});";
            int[] output;
            using (Variable var = new Variable(matlabApplication, tempVariableName, creationCommand))
            {
                output = matlabApplication.GetIntegerArrayRow(tempVariableName);
            }

            return output;
        }

        public static void Reset(this MatlabApplication matlabApplication)
        {
            matlabApplication.Clear();
            matlabApplication.Clc();
        }

        /// <summary>
        /// Returns the all locations at which an item can be found. The item could be a function in a MATLAB code file, a method on a loaded Java class, a workspace variable, or a file (including its extension).
        /// </summary>
        /// <remarks>
        /// Always returns at least one element.
        /// </remarks>
        public static string[] Which(this MatlabApplication matlabApplication, string item, bool all = false)
        {
            string allSuffix = String.Empty;
            if (all)
            {
                allSuffix = @", '-all'";
            }

            string command = $@"{Matlab.AnswerVariableName} = which('{item}'{allSuffix});";
            matlabApplication.Execute(command);

            object answer = matlabApplication.GetData(Matlab.AnswerVariableName);

            string[] output;
            switch (answer)
            {
                case string answerString:
                    output = new string[] { answerString };
                    break;

                case object[,] cellArray:
                    int nItems = cellArray.GetLength(0);
                    output = new string[nItems];
                    for (int iItem = 0; iItem < nItems; iItem++)
                    {
                        output[iItem] = Convert.ToString(cellArray[iItem, 0]);
                    }
                    break;

                case null:
                default:
                    output = new string[] { Commands.NotFound };
                    break;
            }

            return output;
        }

        public static string WhichFirstOnly(this MatlabApplication matlabApplication, string item)
        {
            string[] locations = matlabApplication.Which(item);

            string output = locations[0];
            return output;
        }
    }
}
