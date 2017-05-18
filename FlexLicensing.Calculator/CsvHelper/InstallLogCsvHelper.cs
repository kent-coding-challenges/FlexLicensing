using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using FlexLicensing.Calculator.Enums;
using FlexLicensing.Calculator.Models;

namespace FlexLicensing.Calculator.Database
{
    /// <summary>
    ///     Provides functionalities to import Install Log CSV files quickly.
    /// </summary>
    public class InstallLogCsvHelper
    {
        /// <summary>
        ///     The input file path of the CSV file.
        /// </summary>
        public string InputFilePath { get; private set; }

        /// <summary>
        ///     The application ID to be imported from the CSV file.
        /// </summary>
        public int ApplicationID { get; set; }
        
        private int _batchSize;

        /// <summary>
        ///     The batch size which will decide how many rows should be inserted at any one time.
        /// </summary>
        /// <remarks>
        ///     Ideally, a small value will make the Import process longer,
        ///      but if this value is too large, then it might result in Out of Memory exception.
        /// </remarks>
        public int BatchSize
        {
            get
            {
                // Return min batch size as 1.
                return _batchSize > 0 ? _batchSize : 1;
            }
            set
            {
                _batchSize = value;
            }
        }

        private FlexDbContext dbContext;
        private string tableName;
        private string tempTableName;

        /// <summary>
        ///     Create a new instance of CsvImportHelper.
        /// </summary>
        /// <param name="csvFilePath">
        ///     The path of the CSV file to import.
        /// </param>
        /// <param name="applicationID"></param>
        public InstallLogCsvHelper(string csvFilePath, int applicationID)
        {
            ApplicationID = applicationID;
            InputFilePath = csvFilePath;

            dbContext = new FlexDbContext();
            tableName = (dbContext as IObjectContextAdapter)
                .ObjectContext.CreateObjectSet<InstallLog>().EntitySet.Name;

            // Initialize temp table name, appended with DateTime.Now to ms precision.
            tempTableName = $"Temp{ tableName }_{ DateTime.Now.ToString("yyyyMMddhhmmssffffff") }";

            // Set default batch size.
            BatchSize = 1000;

            ValidateInputFilePath();
        }
        
        /// <summary>
        ///     Import specified CSV, removing all duplicates.
        /// </summary>
        public void Import()
        {
            try
            {
                CreateTempTable();
                ImportToTempTable();
                CopyDistinctTempTableToDb();
            } catch(Exception ex)
            {
                throw ex;
            }
            finally {
                DropTempTable();
            }
        }

        private void CreateTempTable()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE { tempTableName } (");
            sb.AppendLine("[ComputerID] INT,");
            sb.AppendLine("[UserID] INT,");
            sb.AppendLine("[ApplicationID] INT,");
            sb.AppendLine("[ComputerType] INT );");

            string query = sb.ToString();
            dbContext.Database.ExecuteSqlCommand(query);
        }

        private void DropTempTable()
        {
            string query = $"DROP TABLE { tempTableName }";
            dbContext.Database.ExecuteSqlCommand(query);
        }

        private void ImportToTempTable()
        {
            string connectionString = dbContext.Database.Connection.ConnectionString;
            var dt = NewDataTable();
            int count = 0;

            using (var reader = new StreamReader(InputFilePath))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create the bulk copy object.
                    var sqlBulkCopy = NewSqlBulkCopy(connection);

                    // Read first line to ignore CSV headers.
                    reader.ReadLine();

                    // Loop through the CSV and load each set of n records into a DataTable
                    // Then execute query when batch size is reached.
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] columns = line.Split(',');
                        AddToDataTable(dt, columns);

                        count++;
                        if (count % BatchSize == 0)
                        {
                            // Execute batch insertion.
                            CommitDataTable(sqlBulkCopy, connection, dt);

                            // Refresh bulk copy object to force garbage collection before proceeding to next batch.
                            sqlBulkCopy = NewSqlBulkCopy(connection);
                        }
                    }

                    // Execute insertion for last batch.
                    CommitDataTable(sqlBulkCopy, connection, dt);
                }
            }
        }

        private SqlBulkCopy NewSqlBulkCopy(SqlConnection connection)
        {
            var sqlBulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = tempTableName
            };

            // Set column mappings.
            var mapping = new SqlBulkCopyColumnMapping();
            sqlBulkCopy.ColumnMappings.Add("ComputerID", "ComputerID");
            sqlBulkCopy.ColumnMappings.Add("UserID", "UserID");
            sqlBulkCopy.ColumnMappings.Add("ApplicationID", "ApplicationID");
            sqlBulkCopy.ColumnMappings.Add("ComputerType", "ComputerType");

            return sqlBulkCopy;
        }

        private void CopyDistinctTempTableToDb()
        {
            string columns = "ApplicationID, UserID, ComputerID, ComputerType";

            var sb = new StringBuilder();
            sb.AppendLine($"TRUNCATE TABLE { tableName };");
            sb.AppendLine($"INSERT INTO { tableName }");
            sb.AppendLine($"({ columns })");
            sb.AppendLine($"SELECT DISTINCT { columns } FROM { tempTableName }");

            string query = sb.ToString();
            dbContext.Database.ExecuteSqlCommand(query);
        }
        
        private DataTable NewDataTable()
        {
            DataTable dt = new DataTable();

            // Add the columns in the temp table.
            dt.Columns.Add("ComputerID", typeof(int));
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("ApplicationID", typeof(int));
            dt.Columns.Add("ComputerType", typeof(int));

            return dt;
        }

        private void AddToDataTable(DataTable dt, string[] line)
        {
            DataRow row = dt.NewRow();
            int applicationID = Convert.ToInt32(line[2]);
            if(applicationID == ApplicationID)
            {
                row["ApplicationID"] = applicationID;
                row["ComputerID"] = Convert.ToInt32(line[0]);
                row["UserID"] = Convert.ToInt32(line[1]);

                var computerType = (ComputerType)Enum.Parse(typeof(ComputerType), line[3], true);
                row["ComputerType"] = computerType;

                dt.Rows.Add(row);
            }
        }

        private void CommitDataTable(SqlBulkCopy sqlBulkCopy, SqlConnection sqlConnection, DataTable dt)
        {
            sqlBulkCopy.WriteToServer(dt);

            // Force garbage-collection of dt before starting with next batch.
            dt.Clear();
            dt.Dispose();
            dt = NewDataTable();
        }

        private void ValidateInputFilePath()
        {
            if (!File.Exists(InputFilePath))
            {
                string error = "Input file path cannot be found.";
                throw new FileNotFoundException(error, "FileInputPath");
            }
        }
    }
}
