using System.Text.Json;



namespace StudentManagement
{
    public partial class Form1 : Form
    {
        // StudentList is now visible all functions of the Form
        // Scope
        // Class level variable
        List<Student> StudentList = new List<Student>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnTestStudents_Click(object sender, EventArgs e)
        {
            var st1 = new Student();
            st1.Firstname = "Xander";
            st1.Lastname = "Wemmers";
            st1.City = "Utrecht";
            st1.DateOfBirth = new DateTime(1974, 2, 7);   // february 7, 1974
            st1.Gender = GenderType.Male;

            var st2 = new Student
            {
                Firstname = "Bill",
                Lastname = "Gates",
                City = "Seattle",
                DateOfBirth = new DateTime(1955, 10, 28),
                Gender = GenderType.Male
            };

            var st3 = new Student
            {
                Firstname = "Steve",
                Lastname = "Jobs",
                City = "San Fransisco",
                DateOfBirth = new DateTime(1955, 2, 24),
                Gender = GenderType.Male
            };

            StudentList.Add(st1);
            StudentList.Add(st2);
            StudentList.Add(st3);

            //
            //var st4 = new Student();
            //StudentList.Add(st4);

            UpdateUI();
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            var s = new Student();

            // Trim will remove spaces at the beginning and end of a string
            s.Firstname = txtFirstname.Text.Trim();
            s.Lastname = txtLastname.Text.Trim();
            s.City = cbxCity.Text.Trim();

            s.DateOfBirth = dtpDateOfBirth.Value.Date; // Only the Date part is interesting. Time is set to zero

            if (rbMale.Checked)
                s.Gender = GenderType.Male;

            if (rbFemale.Checked)
                s.Gender = GenderType.Female;

            if (rbUnknown.Checked)
                s.Gender = GenderType.Unknown;

            StudentList.Add(s);

            // After adding the student, clear the form:
            txtFirstname.Text = "";
            txtLastname.Text = "";
            cbxCity.Text = "";
            dtpDateOfBirth.Value = DateTime.Today;
            //dtpDateOfBirth.Value = new DateTime(1980, 1, 1);

            rbMale.Checked = false;
            rbFemale.Checked = false;
            rbUnknown.Checked = false;

            UpdateUI();
        }

        void UpdateUI()
        {
            // When using the DataSource property the list should always be refreshed
            // by setting it to null
            dgvStudents.DataSource = null;
            dgvStudents.DataSource = StudentList;

            // Show the number of users in the label just above the datagridview
            if (StudentList.Count == 1)
                lblStudentCount.Text = $"There is {StudentList.Count} student";
            else
                lblStudentCount.Text = $"There are {StudentList.Count} students";

            // Query all the cities in the List
            // Use LINQ to write a query
            // LINQ looks very much like SQL (select, from, where, orderby )

            var query = from s in StudentList
                        where !string.IsNullOrEmpty(s.City)
                        orderby s.City
                        select s.City;

            // Execute the query by calling ToList()

            // Distinct will un-duplicate
            var cities = query.Distinct().ToList();

            // Show all the cities in the combobox cbxCity
            cbxCity.DataSource = cities;
        }

        private void saveJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Serialization: take the objects in memory and create a string in JSON format

            // System.Text.Json is the namespace

            var dialog = new SaveFileDialog();
            dialog.ShowDialog();

            // Check if the user has canceled the dialog
            // If user canceled, then the FileName property is empty string
            if (dialog.FileName == "")
                return;

            // On top of this file check the namespace of the JsonSerializer class on top of this file
            string jsondata = JsonSerializer.Serialize(StudentList);
            File.WriteAllText(dialog.FileName, jsondata);
        }

        private void openJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();

            if (dialog.FileName == "")
                return;

            string jsondata = File.ReadAllText(dialog.FileName);

            StudentList = JsonSerializer.Deserialize<List<Student>>(jsondata)!;
            UpdateUI();
        }

        private void btnSearchWomen_Click(object sender, EventArgs e)
        {
            // Use LINQ to search the data
            var query = from student in StudentList
                        where student.Gender == GenderType.Female
                        && student.Age > 30
                        select student;

            var allWomen = query.ToList();


            dgvStudents.DataSource = allWomen;

            lblStudentCount.Text += $" I found {allWomen.Count} students";

        }

        private void btnOldestMan_Click(object sender, EventArgs e)
        {
            var query = from s in StudentList
                        where s.Gender == GenderType.Male
                        orderby s.Age descending
                        select s;

            Student? oldestMan = query.FirstOrDefault();

            // It is very well possible that no man is found. Check for null!
            // Possible null reference

            if (oldestMan == null)
                MessageBox.Show("There are no men!");
            else
                MessageBox.Show($"{oldestMan.Fullname} is the oldest man.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // searching by using == is an exact match and is case sensitive

            // Now a partial match is enough and is case insensitive

            var query = from s in StudentList
                        where s.Firstname.ToLower().Contains(txtSearch.Text.ToLower())
                        || s.Lastname.ToLower().Contains(txtSearch.Text.ToLower())
                        || s.Age.ToString() == txtSearch.Text
                        select s;

            var result = query.ToList();

            dgvStudents.DataSource = result;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // TextChanged is an event for the TextBox
            // Now the query is executed with every letter you type
            // (also when using backspace)

            var query = from s in StudentList
                        where s.Firstname.ToLower().Contains(txtSearch.Text.ToLower())
                        || s.Lastname.ToLower().Contains(txtSearch.Text.ToLower())
                        || s.Age.ToString() == txtSearch.Text
                        select s;

            // C# is a strongly typed language. Each variable has a type and when comparing
            // for instance age to txtSearch.Text you first need to convert the int to string

            var result = query.ToList();

            dgvStudents.DataSource = result;
        }

        private void btnSearchByCity_Click(object sender, EventArgs e)
        {
            // Lambda expressions are like LINQ queries but different syntax  =>

            var query = from s in StudentList
                        where s.City == cbxCity.Text
                        select s;

            // Where is filtering, like in JavaScript the function filter(...)
            var utrechtStudents = StudentList.Where(s => s.City == cbxCity.Text).ToList();

            dgvStudents.DataSource = utrechtStudents;

            var oldest = StudentList.OrderByDescending(x => x.Age).First();

            MessageBox.Show(oldest.Fullname);

            // Select is not filtering students, but selecting the property you need
            List<string> names = StudentList.Select(s => s.Firstname).ToList();

            List<string?> cities = StudentList.Where(s => !string.IsNullOrEmpty(s.City))
                                              .OrderBy(s => s.City)
                                              .Select(s => s.City)
                                              .ToList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        // .NET is a part of the Windows OS

        // In Linux .NET is called Mono

        // for Android (and iPhone) use C# with Xamarin


    }
}