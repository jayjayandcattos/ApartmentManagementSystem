Imports MySql.Data.MySqlClient

Public Class Pop_Up_Add_Tenant
    ' Properties to hold tenant details
    Public Property TenantName As String
    Public Property Email As String
    Public Property ContactNo As String
    Public Property UnitNo As String
    Public Property RegistrationDate As String

    Public Property DashboardFormInstance As DashboardForm
    Public Property ApartmentsFormInstance As ApartmentsForm  ' Reference to ApartmentsForm

    ' Connection string for database connection
    Dim connectionString As String = "Server=localhost;Database=oopapartment;Uid=root"
    Dim connection As New MySqlConnection(connectionString)

    ' Method to open database connection
    Private Sub OpenConnection()
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
        Catch ex As Exception
            MessageBox.Show("Error connecting to database: " & ex.Message)
        End Try
    End Sub

    ' Method to close database connection
    Private Sub CloseConnection()
        If connection.State = ConnectionState.Open Then
            connection.Close()
        End If
    End Sub

    ' Load available unit numbers into ComboBox1
    Private Sub LoadAvailableUnits()
        Try
            OpenConnection()

            ' Query to fetch available unit numbers
            Dim query As String = "SELECT unit_id, unit_number FROM tbl_rooms WHERE status = 'Vacant'"
            Dim adapter As New MySqlDataAdapter(query, connection)
            Dim table As New DataTable()

            adapter.Fill(table)

            ' Bind the DataTable to the ComboBox
            ComboBox1.DataSource = table
            ComboBox1.DisplayMember = "unit_number" ' This is what will be displayed in the ComboBox
            ComboBox1.ValueMember = "unit_id"      ' This can be used to get the selected unit ID

            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error loading available units: " & ex.Message)
        End Try
    End Sub

    ' Set status to "Occupied" once a unit number is selected
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem IsNot Nothing Then
            UnitNo = ComboBox1.SelectedItem.ToString()

            ' Update apartment status to "Occupied" when a unit is selected
            If ApartmentsFormInstance IsNot Nothing Then
                ApartmentsFormInstance.UpdateApartmentStatus(UnitNo, "Occupied")
            End If
        End If
    End Sub

    ' Save tenant details and update apartment status
    Private Sub SaveTenantButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
       String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
       String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
       ComboBox1.SelectedValue Is Nothing Then
            MessageBox.Show("Please fill in all required fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        TenantName = TextBox1.Text
        Email = TextBox2.Text
        ContactNo = TextBox3.Text
        UnitNo = ComboBox1.Text
        Dim UnitId As Integer = Convert.ToInt32(ComboBox1.SelectedValue)
        RegistrationDate = DateTimePicker1.Value.ToString("yyyy-MM-dd")

        Try
            OpenConnection()

            ' Save tenant details to the database
            Dim insertTenantQuery As String = "INSERT INTO tbl_tenants (tenants_name, tenants_email, contact_number, tenants_unitnum, registration_date) " &
                                          "VALUES (@Name, @Email, @ContactNo, @UnitId, @RegDate)"
            Dim cmd As New MySqlCommand(insertTenantQuery, connection)
            cmd.Parameters.AddWithValue("@Name", TenantName)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@ContactNo", ContactNo)
            cmd.Parameters.AddWithValue("@UnitId", UnitId)
            cmd.Parameters.AddWithValue("@RegDate", RegistrationDate)
            cmd.ExecuteNonQuery()

            ' Update the unit status to "Occupied"
            Dim updateStatusQuery As String = "UPDATE tbl_rooms SET status = 'Occupied' WHERE unit_id = @UnitId"
            Dim statusCmd As New MySqlCommand(updateStatusQuery, connection)
            statusCmd.Parameters.AddWithValue("@UnitId", UnitId)
            statusCmd.ExecuteNonQuery()

            MessageBox.Show("Tenant added and unit status updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Refresh ListView and ComboBox
            RefreshComboBoxAndListView()

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error saving tenant details: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' Cancel button click handler
    Private Sub CancelTenantButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ' Method to load tenants into ListView
    Private Sub LoadTenantListView()
        Try
            OpenConnection()

            ' Query to fetch tenants
            Dim query As String = "SELECT tenants_name, tenants_unitnum, contact_number FROM tbl_tenants"
            Dim adapter As New MySqlDataAdapter(query, connection)
            Dim table As New DataTable()

            adapter.Fill(table)

            ' Clear the existing items in ListView
            TenantListView.Items.Clear()

            ' Loop through the data and add it to the ListView
            For Each row As DataRow In table.Rows
                Dim item As New ListViewItem(row("tenants_name").ToString())
                item.SubItems.Add(row("tenants_unitnum").ToString())
                item.SubItems.Add(row("contact_number").ToString())
                TenantListView.Items.Add(item)
            Next

            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error loading tenant list: " & ex.Message)
        End Try
    End Sub

    ' Refresh ListView and ComboBox after adding a tenant
    Private Sub RefreshComboBoxAndListView()
        ' Load available units again
        LoadAvailableUnits()

        ' Reload the tenant list in the ListView
        LoadTenantListView()
    End Sub

    ' Load data when the form is loaded
    Private Sub Pop_Up_Add_Tenant_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load available units into the ComboBox
        LoadAvailableUnits()

        ' Optionally, if you want to set a default status:
        ' ComboBox1.SelectedIndex = 0  ' Set to first available unit
    End Sub

    Private TenantListView As New ListView()
End Class
