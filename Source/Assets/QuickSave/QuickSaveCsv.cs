////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave.Core.Storage;

namespace CI.QuickSave
{
    public class QuickSaveCsv
    {
        /// <summary>
        /// Returns the number of rows in the csv file
        /// </summary>
        public int RowCount => _values.Count;

        private readonly List<List<string>> _values;

        private QuickSaveCsv(List<List<string>> items)
        {
            _values = items;
        }

        public QuickSaveCsv()
        {
            _values = new List<List<string>>();
        }

        public static QuickSaveCsv Load(string path) => Load(path, new QuickSaveSettings());

        public static QuickSaveCsv Load(string path, QuickSaveSettings settings)
        {
            var lines = FileAccess.LoadLines(path);

            if (lines == null)
            {
                throw new QuickSaveException("File does not exist");
            }

            List<List<string>> values = new List<List<string>>();

            try
            {
                foreach (var line in lines)
                {
                    values.Add(new List<string>());

                    var cells = line.Split(',');

                    foreach (var cell in cells)
                    {
                        values.Last().Add(cell);
                    }
                }
            }
            catch
            {
                throw new QuickSaveException("Failed to load file");
            }

            return new QuickSaveCsv(values);
        }

        public void Save(string path) => Save(path, new QuickSaveSettings());

        public void Save(string path, QuickSaveSettings settings) 
        {
            var lines = _values.Select(x => string.Join(",", x)).ToList();

            if (!FileAccess.SaveLines(path, lines))
            {
                throw new QuickSaveException("Failed to save file");
            }
        }

        public int GetCellCount(int row)
        {
            if (_values.Count <= row)
            {
                throw new QuickSaveException("The specified row does not exist");
            }

            return _values[row].Count;
        }

        public string GetCell(int row, int column)
        {
            if (_values.Count <= row || _values[row].Count <= column)
            {
                throw new QuickSaveException("The specified cell does not exist");
            }

            return _values[row][column];
        }

        public T GetCell<T>(int row, int column)
        {
            if (_values.Count <= row || _values[row].Count <= column)
            {
                throw new QuickSaveException("The specified cell does not exist");
            }

            return (T)Convert.ChangeType(_values[row][column], typeof(T));
        }

        public void SetCell(int row, int column, string value)
        {
            if (_values.Count <= row)
            {
                while (_values.Count <= row)
                {
                    _values.Add(new List<string>());
                }
            }

            if (_values[row].Count <= column)
            {
                while (_values[row].Count <= column)
                {
                    _values[row].Add(string.Empty);
                }
            }

            _values[row][column] = value;
        }

        public void SetCell<T>(int row, int column, T value) => SetCell(row, column, Convert.ToString(value));

        public void DeleteRow(int row)
        {
            if (_values.Count > row)
            {
                _values.RemoveAt(row);
            }
        }
    }
}