using MicroJson;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Usage_Data_Parser
{


    public partial class Form1 : Form
    {

        #region Constant Fields

        #endregion

        #region Fields

        private SqlConnection connection;
        private List<ParsedJSONFile> parsedFiles = new List<ParsedJSONFile>();
        private List<TreeNode> selectedDataFiles = new List<TreeNode>();
        private TreeNode oldNode;
        private string rootNodePath = "";
        
        #endregion

        #region Constructors

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateTablesFromDatabase();

            Text += " v" + Version.getVersion(); // Add the version number to the window title bar

            string lastOpenedUsageDataPath = Properties.Settings.Default.usageDataPath; // Fetch the last usage data folder from settings

            if (lastOpenedUsageDataPath != null && lastOpenedUsageDataPath != "")
            {
                treeView1.BeginUpdate();
                ListDirectory(treeView1, lastOpenedUsageDataPath);
                treeView1.EndUpdate();
            }

            //Establish Connection to database
            connection = EstablishConnectionToSqlDatabase();

        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rootNodePath != "" & rootNodePath != null)
            {
                Properties.Settings.Default.usageDataPath = rootNodePath;
                Properties.Settings.Default.Save();
            }
        }

        private void TreeNode_Selected(object sender, TreeViewEventArgs e)
        {
            //ClearDataGrids();
            label1.Text = "";
            label2.Text = "";
            if (oldNode != null)
            {
                Font normalFont = new Font(treeView1.Font, FontStyle.Regular);
                oldNode.NodeFont = normalFont;
            }
            Font boldFont = new Font(treeView1.Font, FontStyle.Bold);

            var node = treeView1.SelectedNode;
            node.NodeFont = boldFont;
            oldNode = node;

            var tag = (string)node.Tag;
            string fullPath = tag.ToString();

            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(fullPath);

            // If the user selects a file
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                string _fullpath = node.Tag.ToString();
                string hashString = GenerateSHA256(_fullpath);
                foreach (DataGridViewRow row in dataGridViewHandConfig.Rows)
                {
                    if (row.Cells[0].Value.Equals(hashString))
                    {
                        dataGridViewHandConfig.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridViewHandConfig.ClearSelection();
                        row.Selected = true;
                        dataGridViewHandConfig.FirstDisplayedScrollingRowIndex = dataGridViewHandConfig.SelectedRows[0].Index;
                        break;
                    }
                }
            }
            else // If the user selects a folder get all the child nodes and add them to a list
            {
                label1.Text = "Folder selected";
                List<TreeNode> childNodes = node.GetAllTreeNodes();

                selectedDataFiles.Clear();
                foreach (TreeNode childNode in childNodes)
                {
                    bool isDataFile = childNode.Tag.ToString().Contains("_DAT");
                    if (isDataFile)
                    {
                        selectedDataFiles.Add(childNode);
                    }
                }

                int numberOfSelectedDataFiles = selectedDataFiles.Count;
                label1.Text += " contains " + numberOfSelectedDataFiles.ToString() + " data file";
                if (numberOfSelectedDataFiles > 1)
                {
                    label1.Text += "s";
                }
            }
        }

        private void folderSelectButton_Click(object sender, EventArgs e)
        {
            using (var directoryDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select Folder"
            })
            {
                CommonFileDialogResult result = directoryDialog.ShowDialog();

                if (result == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(directoryDialog.FileName))
                {
                    rootNodePath = directoryDialog.FileName;
                    treeView1.BeginUpdate();
                    ListDirectory(treeView1, rootNodePath);
                    treeView1.EndUpdate();
                }
            }
        }

        private void importSelected_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int numberOfSelectedDataFiles = selectedDataFiles.Count;
            Console.WriteLine("number of selected files: {0}", numberOfSelectedDataFiles);
            if (numberOfSelectedDataFiles > 0)
            {
                List<TreeNode> newFiles = new List<TreeNode>();
                List<String> newHashes = new List<String>();
                List<String> touchPoints = new List<String>();
                List<String> newHandNumbers = new List<String>();
                                
                for (int i = 0; i < numberOfSelectedDataFiles; i++)
                {
                    TreeNode _node = selectedDataFiles[i];
                    string _fullpath = _node.Tag.ToString();
                    DateTime dataCollectionDate;
                    string handNumber;

                    // Try to get the session collection date and hand number, else skip it
                    try
                    {
                        dataCollectionDate = GetDataCollectionDate(_node);
                    }
                    catch (System.NullReferenceException)
                    {
                        continue;
                    }
                    try
                    {
                        handNumber = GetHandNumber(_node);
                    }
                    catch (System.NullReferenceException)
                    {
                        continue;
                    }

                    // Compute the hash
                    string hashString = GenerateSHA256(_fullpath);
                    Console.WriteLine("hash: {0}", hashString);

                    // Search to see if the record already exists
                    bool inDatabase = IsInDataBase(hashString);
                    Console.WriteLine("is in database: {0}", inDatabase.ToString());

                    // If there is a duplicate of a record in the selected files, it won't be in the database yet
                    // so each will pass the check. Therefore they list to add is also checked for duplicates
                    bool isAlreadyInUpload = newHashes.Contains(hashString);
                    Console.WriteLine("is in list to add: {0}", isAlreadyInUpload.ToString());

                    // If it's not in the database or already set to be inserted
                    if (!inDatabase && !isAlreadyInUpload)
                    {
                        newHashes.Add(hashString);
                        newHandNumbers.Add(handNumber);
                        newFiles.Add(selectedDataFiles[i]);

                        // Check to see if this new entry is from an existing touch point.
                        bool sessionFromExistingTouchPoint = IsTouchPointInDatabase(handNumber, dataCollectionDate);

                        string touchPoint;
                        if (!sessionFromExistingTouchPoint) // If there are no existing touch points for this session's hand and date, make one.
                        {
                            // Check to see what the highest existing touch point index is
                            int? highestExistingIndex = GetHighestTouchPointIndexFromDatabase(handNumber);

                            int newTouchPointIndex;
                            if (highestExistingIndex != null) // If there aren't any for the hand
                            {
                                newTouchPointIndex = 1; // Start on 1 as the send out date is 0
                            }
                            else
                            {
                                newTouchPointIndex = (int)highestExistingIndex + 1; // else use the latest one + 1
                            }
                            touchPoint = handNumber + "_" + newTouchPointIndex.ToString(); // make this this first touch point after the send out (i.e. touch point 1). 
                            AddTouchPointToDatabase(dataCollectionDate, handNumber, touchPoint, newTouchPointIndex);
                        }
                        else
                        {
                            int sessionTouchPoint = GetSessionTouchPointFromDatabase(handNumber, dataCollectionDate);
                            touchPoint = handNumber + "_" + sessionTouchPoint.ToString(); // Set it to the matching touchpoint
                        }
                        touchPoints.Add(touchPoint);
                    }
                }

                // Add the files identified as new
                int numberOfNewFiles = newFiles.Count;
                for (int i = 0; i < numberOfNewFiles; i++)
                {
                    label1.Text = "Adding session " + i.ToString() + " of " + numberOfNewFiles.ToString() + ".";
                    TreeNode _node = newFiles[i];
                    string _fullpath = _node.Tag.ToString();
                    ParsedJSONFile file = ParseUsageDataFile(_node, _fullpath);
                    InsertSessionToSQLdatabase(newHashes[i], touchPoints[i], newHandNumbers[i], file);
                }
                Console.WriteLine("number of new files: {0}", numberOfNewFiles);
                if (numberOfNewFiles > 0)
                {
                    label1.Text = numberOfNewFiles.ToString() + " sessions added to the database.";
                }
                else
                {
                    label1.Text = "No new sessions in selection. No sessions added to the database.";
                }
            }
            Cursor.Current = Cursors.Default;
            UpdateTablesFromDatabase();
        }

        private void ExportToExcel_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode;
            var tag = (string)node.Tag;
            string fullPath = tag.ToString();

            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(fullPath);

            if (!attr.HasFlag(FileAttributes.Directory)) // is a file
            {
                ParsedJSONFile jsonParsedData;

                string jsonInputData = File.ReadAllText(fullPath);
                try
                {
                    jsonParsedData = new JsonSerializer().Deserialize<ParsedJSONFile>(jsonInputData);
                }
                catch (ParserException ex)
                {
                    DodgyJsonParser dodgyJsonParser = new DodgyJsonParser();
                    jsonParsedData = dodgyJsonParser.parseDodgyFile(jsonInputData);
                }

                Excel._Application excel = new Excel.Application();
                Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                Excel._Worksheet worksheet = null;

                try
                {
                    worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                    workbook.Sheets.Add(Count: 5);

                    worksheet = (Excel.Worksheet)workbook.Sheets[1];

                    worksheet.Name = node.Text;

                    worksheet.Cells[1, 1] = "Serial Number";
                    worksheet.Cells[1, 2] = "Firmware Version";
                    worksheet.Cells[1, 3] = "Chirality";
                    worksheet.Cells[1, 4] = "nMotors";
                    worksheet.Cells[1, 5] = "Session Number";
                    worksheet.Cells[1, 6] = "Reset Cause";
                    worksheet.Cells[1, 7] = "ON time";
                    worksheet.Cells[1, 8] = "Active Time";

                    if (jsonParsedData != null)
                    {
                        if (jsonParsedData.handConfig != null)
                        {
                            worksheet.Cells[2, 1] = jsonParsedData.handConfig.serialNum;
                            worksheet.Cells[2, 2] = jsonParsedData.handConfig.fwVer;
                            worksheet.Cells[2, 3] = jsonParsedData.handConfig.chirality;
                            worksheet.Cells[2, 4] = jsonParsedData.handConfig.nMotors;
                        }

                        if ((jsonParsedData.time != null) && (jsonParsedData.resetCause != null))
                        {
                            worksheet.Cells[2, 5] = jsonParsedData.sessionN;
                            worksheet.Cells[2, 6] = jsonParsedData.resetCause;
                            worksheet.Cells[2, 7] = jsonParsedData.time.onTime;
                            worksheet.Cells[2, 8] = jsonParsedData.time.activeTime;
                        }

                        if (jsonParsedData.grip != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[2];//.Select();
                            worksheet.Name = "Grip";

                            worksheet.Cells[1, 1] = "Grip Group";
                            worksheet.Cells[1, 2] = "Grip Type";
                            worksheet.Cells[1, 3] = "Duration";

                            int row = 1;

                            foreach (Group group in jsonParsedData.grip.gripGroups)
                            {
                                foreach (GripChild grip in group.grips)
                                {
                                    row++;
                                    worksheet.Cells[row, 1] = group.n;
                                    worksheet.Cells[row, 2] = grip.name;
                                    worksheet.Cells[row, 3] = grip.duration;
                                }
                            }
                        }

                        if (jsonParsedData.battery != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[3];//.Select();
                            worksheet.Name = "Battery";

                            worksheet.Cells[1, 1] = "Type";
                            worksheet.Cells[1, 2] = "N";
                            worksheet.Cells[1, 3] = "Battery Voltage";
                            worksheet.Cells[1, 4] = "Duration";

                            worksheet.Cells[2, 1] = "Min";
                            worksheet.Cells[2, 2] = jsonParsedData.battery.min.n;
                            worksheet.Cells[2, 3] = jsonParsedData.battery.min.battV;
                            worksheet.Cells[2, 4] = jsonParsedData.battery.min.duration;

                            worksheet.Cells[3, 1] = "Max";
                            worksheet.Cells[3, 2] = jsonParsedData.battery.max.n;
                            worksheet.Cells[3, 3] = jsonParsedData.battery.max.battV;
                            worksheet.Cells[3, 4] = jsonParsedData.battery.max.duration;

                            worksheet.Cells[5, 2] = "n";
                            worksheet.Cells[5, 3] = "Battery Voltage";
                            worksheet.Cells[5, 4] = "Duration";

                            int row = 5;

                            foreach (BattSample batt in jsonParsedData.battery.battSamples)
                            {
                                row++;

                                worksheet.Cells[row, 2] = batt.n;
                                worksheet.Cells[row, 3] = batt.battV;
                                worksheet.Cells[row, 4] = batt.duration;
                            }
                        }

                        if (jsonParsedData.temp != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[4];//.Select();
                            worksheet.Name = "Temp";

                            worksheet.Cells[1, 1] = "Type";
                            worksheet.Cells[1, 2] = "N";
                            worksheet.Cells[1, 3] = "Temperature (celcius)";
                            worksheet.Cells[1, 4] = "Duration";

                            worksheet.Cells[2, 1] = "Min";
                            worksheet.Cells[2, 2] = jsonParsedData.temp.minTemp.n;
                            worksheet.Cells[2, 3] = jsonParsedData.temp.minTemp.tempC;
                            worksheet.Cells[2, 4] = jsonParsedData.temp.minTemp.duration;

                            worksheet.Cells[3, 1] = "Max";
                            worksheet.Cells[3, 2] = jsonParsedData.temp.maxTemp.n;
                            worksheet.Cells[3, 3] = jsonParsedData.temp.maxTemp.tempC;
                            worksheet.Cells[3, 4] = jsonParsedData.temp.maxTemp.duration;

                            worksheet.Cells[5, 2] = "n";
                            worksheet.Cells[5, 3] = "Temperature (celcius)";
                            worksheet.Cells[5, 4] = "Duration";

                            int row = 5;

                            foreach (TempSample temp in jsonParsedData.temp.tempSamples)
                            {
                                row++;

                                worksheet.Cells[row, 2] = temp.n;
                                worksheet.Cells[row, 3] = temp.tempC;
                                worksheet.Cells[row, 4] = temp.duration;
                            }
                        }

                        if (jsonParsedData.magFlux != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[5];//.Select();
                            worksheet.Name = "Magnetic Flux";

                            worksheet.Cells[1, 1] = "Axis";
                            worksheet.Cells[1, 2] = "Max";
                            worksheet.Cells[1, 3] = "Units";
                            worksheet.Cells[1, 4] = "Duration";

                            worksheet.Cells[2, 1] = "X";
                            worksheet.Cells[2, 2] = jsonParsedData.magFlux.X.max;
                            worksheet.Cells[2, 3] = jsonParsedData.magFlux.unit;
                            worksheet.Cells[2, 4] = jsonParsedData.magFlux.X.duration;

                            worksheet.Cells[3, 1] = "Y";
                            worksheet.Cells[3, 2] = jsonParsedData.magFlux.Y.max;
                            worksheet.Cells[3, 3] = jsonParsedData.magFlux.unit;
                            worksheet.Cells[3, 4] = jsonParsedData.magFlux.Y.duration;
                        }

                        if (jsonParsedData.accel != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[6];//.Select();
                            worksheet.Name = "Acceleration";

                            worksheet.Cells[1, 1] = "Axis";
                            worksheet.Cells[1, 2] = "Max";
                            worksheet.Cells[1, 3] = "Units";
                            worksheet.Cells[1, 4] = "Duration";

                            worksheet.Cells[2, 1] = "X";
                            worksheet.Cells[2, 2] = jsonParsedData.accel.X.max;
                            worksheet.Cells[2, 3] = jsonParsedData.accel.unit;
                            worksheet.Cells[2, 4] = jsonParsedData.accel.X.duration;

                            worksheet.Cells[3, 1] = "Y";
                            worksheet.Cells[3, 2] = jsonParsedData.accel.Y.max;
                            worksheet.Cells[3, 3] = jsonParsedData.accel.unit;
                            worksheet.Cells[3, 4] = jsonParsedData.accel.Y.duration;

                            worksheet.Cells[4, 1] = "Z";
                            worksheet.Cells[4, 2] = jsonParsedData.accel.Z.max;
                            worksheet.Cells[4, 3] = jsonParsedData.accel.unit;
                            worksheet.Cells[4, 4] = jsonParsedData.accel.Z.duration;
                        }
                    }

                    //Getting the location and file name of the excel to save from user.
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveDialog.FilterIndex = 2;

                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        workbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Export Successful");
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                }
                finally
                {
                    excel.Quit();
                    workbook = null;
                    excel = null;
                }
            }
        }

        #endregion

        #region Methods

        private void UpdateTablesFromDatabase()
        {
            // TODO: This line of code loads data into the 'heroUsageDataDataSet.touchPoints' table. You can move, or remove it, as needed.
            touchPointsTableAdapter.Fill(heroUsageDataDataSet.touchPoints);
            // TODO: This line of code loads data into the 'heroUsageDataDataSet.sessions' table. You can move, or remove it, as needed.
            sessionsTableAdapter.Fill(heroUsageDataDataSet.sessions);
        }

        private int GetSessionTouchPointFromDatabase(string _handNumber, DateTime _dataCollectionDate)
        {
            connection.Open();
            SqlDataReader touchPointIdReader;
            string touchPointIDQuery = "SELECT TouchPointIndex FROM HeroUsageData.dbo.touchPoints WHERE HandNumber = @HandNumber AND Date = @Date";
            SqlCommand sqlCommand = new SqlCommand(touchPointIDQuery, connection);
            sqlCommand.Parameters.AddWithValue("@HandNumber", (object)_handNumber ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Date", (object)_dataCollectionDate.Date ?? DBNull.Value);
            touchPointIdReader = sqlCommand.ExecuteReader();
            int touchPointIndex = -1;
            while (touchPointIdReader.Read())
            {
                touchPointIndex = touchPointIdReader.GetValue(0) != DBNull.Value ? (int)touchPointIdReader.GetValue(0) : -1;
            }
            touchPointIdReader.Close();
            sqlCommand.Dispose();
            connection.Close();
            return touchPointIndex;
        }

        private void AddTouchPointToDatabase(DateTime dataCollectionDate, string handNumber, string touchPoint, int newTouchPointIndex)
        {
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            string addTouchPoint = "insert into touchPoints (TouchPointID, HandNumber, TouchPointIndex, Date) values (@TouchPointID, @HandNumber, @TouchPointIndex, @Date)";
            SqlCommand insertTouchPoint = new SqlCommand(addTouchPoint, connection);
            insertTouchPoint.Parameters.AddWithValue("@TouchPointID", touchPoint); // Cannot be null
            insertTouchPoint.Parameters.AddWithValue("@HandNumber", (object)handNumber ?? DBNull.Value);
            insertTouchPoint.Parameters.AddWithValue("@TouchPointIndex", (object)newTouchPointIndex);
            insertTouchPoint.Parameters.AddWithValue("@Date", (object)dataCollectionDate.Date ?? DBNull.Value);
            adapter.InsertCommand = insertTouchPoint;
            adapter.InsertCommand.ExecuteNonQuery();
            adapter.Dispose();
            insertTouchPoint.Dispose();
            connection.Close();
        }

        private int? GetHighestTouchPointIndexFromDatabase(string handNumber)
        {
            connection.Open();
            SqlDataReader touchPointIdReader;
            string numberOfMatchingTouchPointsQuery = "SELECT max(TouchPointIndex) FROM touchPoints WHERE HandNumber = @HandNumber";
            SqlCommand sqlCommand = new SqlCommand(numberOfMatchingTouchPointsQuery, connection);
            sqlCommand.Parameters.AddWithValue("@HandNumber", (object)handNumber ?? DBNull.Value);
            touchPointIdReader = sqlCommand.ExecuteReader();
            int? largestTouchPointIndex = null;
            while (touchPointIdReader.Read())
            {
                largestTouchPointIndex = touchPointIdReader.GetValue(0) != DBNull.Value ? (int)touchPointIdReader.GetValue(0) : -1;
            }
            touchPointIdReader.Close();
            sqlCommand.Dispose();
            connection.Close();
            return largestTouchPointIndex;
        }

        private bool IsTouchPointInDatabase(string _handNumber, DateTime _dataCollectionDate)
        {
            connection.Open();
            SqlDataReader touchPointIdReader;
            string numberOfMatchingTouchPointsQuery = "SELECT COUNT(*) FROM HeroUsageData.dbo.touchPoints WHERE HandNumber = @HandNumber AND Date = @Date";
            SqlCommand sqlCommand = new SqlCommand(numberOfMatchingTouchPointsQuery, connection);
            sqlCommand.Parameters.AddWithValue("@HandNumber", (object)_handNumber ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Date", (object)_dataCollectionDate.Date ?? DBNull.Value);
            touchPointIdReader = sqlCommand.ExecuteReader();
            int existingTouchPoint = 0;
            while (touchPointIdReader.Read())
            {
                existingTouchPoint = touchPointIdReader.GetValue(0) != DBNull.Value ? (int)touchPointIdReader.GetValue(0) : -1;
            }
            touchPointIdReader.Close();
            sqlCommand.Dispose();
            connection.Close();
            switch (existingTouchPoint)
            {
                case 0:
                    return false;
                case 1:
                    return true;
                default:
                    WarningException duplicateEntries = new WarningException("Duplicate touch points in Database");
                    Console.WriteLine(duplicateEntries.ToString());
                    return true;
            }
        }

        private bool IsInDataBase(string hashString)
        {
            connection.Open();
            SqlDataReader sessionIdReader;
            string numberOfMatchingSessionsQuery = "SELECT COUNT(1) FROM sessions WHERE SessionID = @SessionID";
            SqlCommand command = new SqlCommand(numberOfMatchingSessionsQuery, connection);
            command.Parameters.AddWithValue("@SessionID", hashString);
            sessionIdReader = command.ExecuteReader();
            String numberOfMatchingSessionHashes = "";
            while (sessionIdReader.Read())
            {
                numberOfMatchingSessionHashes += sessionIdReader.GetValue(0) + "\n";
            }
            int numberOfMatchingRecords = Int32.Parse(numberOfMatchingSessionHashes);
            sessionIdReader.Close();
            connection.Close();

            switch (numberOfMatchingRecords)
            {
                case 0:
                    return false;
                case 1:
                    return true;
                default:
                    WarningException duplicateEntries = new WarningException("Duplicate Entries in Database");
                    Console.WriteLine(duplicateEntries.ToString());
                    return true;  // TODO;
            }

        }

        private static string GenerateSHA256(string _fullpath)
        {
            byte[] hash;
            using (FileStream stream = File.OpenRead(_fullpath))
            {
                var sha265 = SHA256.Create();
                hash = sha265.ComputeHash(stream);
            }

            // Convert hash to string for human readability
            string hashString = "";
            foreach (byte b in hash)
            {
                hashString += b.ToString("x2");
            }

            return hashString;
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name) { Tag = directoryInfo.FullName };
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Nodes.Add(new TreeNode(file.Name) { Tag = file.FullName });
            return directoryNode;
        }      

        private ParsedJSONFile ParseUsageDataFile(TreeNode node, string fullPath)
        {
            ParsedJSONFile _jsonParsedData;

            string jsonInputData = File.ReadAllText(fullPath);
            try
            {
                _jsonParsedData = new JsonSerializer().Deserialize<ParsedJSONFile>(jsonInputData);
            }
            catch (ParserException ex)
            {
                DodgyJsonParser dodgyJsonParser = new DodgyJsonParser();
                _jsonParsedData = dodgyJsonParser.parseDodgyFile(jsonInputData);
            }

            label1.Text = node.FullPath;
            if (_jsonParsedData.sessions > 1)
            {
                label2.Text = "Multiple Sessions detected in this file. Please check file manually";
                label2.Font = new System.Drawing.Font(label2.Font, System.Drawing.FontStyle.Bold);
            }
            else if (_jsonParsedData.dodgyFile)
            {
                label2.Text = "Null Character Detected in File. This file may need checking manually as well.";
                label2.Font = new System.Drawing.Font(label2.Font, System.Drawing.FontStyle.Regular);
            }
            else
            {
                label2.Text = "";
            }

            return _jsonParsedData;

        }

        private string GetHandNumber(TreeNode node)
        {
            TreeNode parent;
            try
            {
                parent = node.Parent;
            }
            catch (System.NullReferenceException)
            {
                throw;
            }

            string text = parent.Text;
            string pattern = @"^H[0-9]{1,5}$";
            Regex rx = new Regex(pattern);
            if (rx.IsMatch(text))
            {
                return text;
            }
            else
            {
                return GetHandNumber(parent);
            }
        }

        private DateTime GetDataCollectionDate(TreeNode node)
        {
            TreeNode parent;
            try
            {
                parent = node.Parent;
            }
            catch (System.NullReferenceException)
            {
                throw;
            }
            
            try
            {
                return DateTime.Parse(parent.Text);
            }
            catch (System.FormatException)
            {
                return GetDataCollectionDate(parent);
            }
        }

        private static SqlConnection EstablishConnectionToSqlDatabase()
        {
            string connectionString;
            SqlConnection connection;
            connectionString = @"Data Source=localhost\SQLEXPRESS; Database=HeroUsageData; Integrated Security=True;";
            connection = new SqlConnection(connectionString);
            return connection;
        }

        private void InsertSessionToSQLdatabase(string hashString, string touchPoint, string handNumber, ParsedJSONFile session)
        {
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "Insert into sessions (" +
                "SessionID, " +
                "CollectedInTouchPoint, " +
                "SessionNumber, " +
                "HandNumber, " + 
                "serialNumber, " +
                "firmwareVersion, " +
                "chirality, " +
                "nMotors, " +
                "resetCause, " +
                "activeTime, " +
                "onTime, " +
                "BatteryMinV, " +
                "BatteryMaxV, " +
                "TempMinC, " +
                "TempMaxC," +
                "MagMaxX, " +
                "MagMaxY, " +
                "AccelMaxX, " +
                "AccelMaxY," +
                "AccelMaxz) " +
                " values (" +
                "@SessionID, " +
                "@CollectedInTouchPoint, " +
                "@SessionNumber, " + 
                "@HandNumber, " + 
                "@serialNumber, " +
                "@firmwareVersion, " +
                "@chirality, " +
                "@nMotors, " +
                "@resetCause, " +
                "@activeTime, " +
                "@onTime, " +
                "@BatteryMinV, " +
                "@BatteryMaxV, " +
                "@TempMinC, " +
                "@TempMaxC ," +
                "@MagMaxX, " +
                "@MagMaxY, " +
                "@AccelMaxX, " +
                "@AccelMaxY, " +
                "@AccelMaxZ)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@SessionID", hashString); // Cannot be null
            command.Parameters.AddWithValue("@CollectedInTouchPoint", (object)touchPoint ?? DBNull.Value);
            command.Parameters.AddWithValue("@SessionNumber", session.sessionN); // Cannot be null
            command.Parameters.AddWithValue("@HandNumber", (object)handNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@serialNumber", session.handConfig != null ? (object)session.handConfig.serialNum : DBNull.Value);
            command.Parameters.AddWithValue("@firmwareVersion", session.handConfig != null ? (object)session.handConfig.fwVer : DBNull.Value);
            command.Parameters.AddWithValue("@chirality", session.handConfig != null ? (object)session.handConfig.chirality : DBNull.Value);
            command.Parameters.AddWithValue("@nMotors", session.handConfig != null? (object)session.handConfig.nMotors : DBNull.Value);
            command.Parameters.AddWithValue("@resetCause", (object)session.resetCause ?? DBNull.Value);
            command.Parameters.AddWithValue("@activeTime", session.time != null && TimeSpan.TryParse(session.time.activeTime, out TimeSpan activeTime) ? (object)activeTime : DBNull.Value);
            command.Parameters.AddWithValue("@onTime", session.time != null && TimeSpan.TryParse(session.time.onTime, out TimeSpan onTime) ? (object)onTime : DBNull.Value);
            command.Parameters.AddWithValue("@BatteryMinV", session.battery != null && Single.TryParse(session.battery.min.battV, out float battMin) ? (object)battMin : DBNull.Value);
            command.Parameters.AddWithValue("@BatteryMaxV", session.battery != null && Single.TryParse(session.battery.max.battV, out float battMax) ? (object)battMax : DBNull.Value);
            command.Parameters.AddWithValue("@TempMinC", session.temp != null && Single.TryParse(session.temp.minTemp.tempC, out float minTemp) ? (object)minTemp : DBNull.Value);
            command.Parameters.AddWithValue("@TempMaxC", session.temp != null && Single.TryParse(session.temp.maxTemp.tempC, out float maxTemp) ? (object)maxTemp : DBNull.Value);
            command.Parameters.AddWithValue("@MagMaxX", session.magFlux != null && Single.TryParse(session.magFlux.X.max, out float maxMagX) ? (object)maxMagX : DBNull.Value);
            command.Parameters.AddWithValue("@MagMaxY", session.magFlux != null && Single.TryParse(session.magFlux.Y.max, out float maxMagY) ? (object)maxMagY : DBNull.Value);
            command.Parameters.AddWithValue("@AccelMaxX", session.accel != null && Single.TryParse(session.accel.X.max, out float maxAccX) ? (object)maxAccX : DBNull.Value);
            command.Parameters.AddWithValue("@AccelMaxY", session.accel != null && Single.TryParse(session.accel.Y.max, out float maxAccY) ? (object)maxAccY : DBNull.Value);
            command.Parameters.AddWithValue("@AccelMaxZ", session.accel != null && Single.TryParse(session.accel.Z.max, out float maxAccZ) ? (object)maxAccZ : DBNull.Value);
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            connection.Close();
        }
        #endregion
    }

    public static class TreeViewExtensions
    {
        public static List<TreeNode> GetAllTreeNodes(this TreeNode _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode child in _self.Nodes)
            {
                result.Add(child);
                result.AddRange(child.GetAllTreeNodes());
            }
            return result;
        }
    }
}