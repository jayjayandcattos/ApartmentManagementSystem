Imports MySql.Data.MySqlClient

Public Class TenantsForm

    Public Property DashboardFormInstance As DashboardForm

    Dim connectionString As String = "Server=localhost;Database=oopapartment;Uid=root;Pwd=123456"
    Dim connection As New MySqlConnection(connectionString)

    ' Method to open the database connection
    Private Sub OpenConnection()
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
        Catch ex As Exception
            MessageBox.Show("Error connecting to database: " & ex.Message)
        End Try
    End Sub

    ' Method to close the database connection
    Private Sub CloseConnection()
        If connection.State = ConnectionState.Open Then
            connection.Close()
        End If
    End Sub

    ' Method to add a tenant to the database
    Private Sub AddTenantToDatabase(tenantName As String, email As String, contactNo As String, unitNo As String, registrationDate As String)
        Try
            OpenConnection()

            ' SQL query to insert a new tenant into the database
            Dim query As String = "INSERT INTO tbl_tenants (tenants_name, tenants_email, contact_number, tenants_unitnum, registration_date) VALUES (@TenantName, @Email, @ContactNo, @UnitNo, @RegistrationDate)"

            ' MySQL command to execute the query with parameters
            Dim cmd As New MySqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@TenantName", tenantName)
            cmd.Parameters.AddWithValue("@Email", email)
            cmd.Parameters.AddWithValue("@ContactNo", contactNo)
            cmd.Parameters.AddWithValue("@UnitNo", unitNo)
            cmd.Parameters.AddWithValue("@RegistrationDate", Convert.ToDateTime(registrationDate))

            cmd.ExecuteNonQuery()
            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error inserting tenant into database: " & ex.Message)
        End Try
    End Sub

    ' Method to update tenant information in the database
    Private Sub UpdateTenantInDatabase(tenantId As Integer, tenantName As String, email As String, contactNo As String, unitNo As String, registrationDate As String)
        Try
            OpenConnection()

            ' SQL query to update tenant information in the database
            Dim query As String = "UPDATE tbl_tenants SET tenants_name = @TenantName, tenants_email = @Email, contact_number = @ContactNo, tenants_unitnum = @UnitNo, registration_date = @RegistrationDate WHERE tenant_id = @TenantId"

            ' MySQL command to execute the query with parameters
            Dim cmd As New MySqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@TenantId", tenantId)
            cmd.Parameters.AddWithValue("@TenantName", tenantName)
            cmd.Parameters.AddWithValue("@Email", email)
            cmd.Parameters.AddWithValue("@ContactNo", contactNo)
            cmd.Parameters.AddWithValue("@UnitNo", unitNo)
            cmd.Parameters.AddWithValue("@RegistrationDate", Convert.ToDateTime(registrationDate))

            cmd.ExecuteNonQuery()
            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error updating tenant in database: " & ex.Message)
        End Try
    End Sub

    ' Method to delete tenant from the database
    Private Sub DeleteTenantFromDatabase(tenantId As Integer)
        Try
            OpenConnection()

            ' SQL query to delete tenant from the database
            Dim query As String = "DELETE FROM tbl_tenants WHERE tenant_id = @TenantId"

            ' MySQL command to execute the query with the parameter
            Dim cmd As New MySqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@TenantId", tenantId)

            cmd.ExecuteNonQuery()
            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error deleting tenant from database: " & ex.Message)
        End Try
    End Sub

    ' Method to load tenants from the database into the ListView
    Private Sub LoadTenantsFromDatabase()
        Try
            OpenConnection()

            ' SQL query to select all tenants
            Dim query As String = "SELECT * FROM tbl_tenants"
            Dim cmd As New MySqlCommand(query, connection)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            ' Clear the current ListView data
            ListView1.Items.Clear()

            ' Populate the ListView with data from the database
            While reader.Read()
                Dim item As New ListViewItem(reader("tenant_id").ToString()) ' tenant_id is the primary key
                item.SubItems.Add(reader("tenants_name").ToString()) ' tenants_name
                item.SubItems.Add(reader("tenants_email").ToString()) ' tenants_email
                item.SubItems.Add(reader("contact_number").ToString()) ' contact_number
                item.SubItems.Add(reader("tenants_unitnum").ToString()) ' tenants_unitnum
                item.SubItems.Add(Convert.ToDateTime(reader("registration_date")).ToString("yyyy-MM-dd")) ' registration_date

                ListView1.Items.Add(item)
            End While

            reader.Close()
            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error loading tenants from database: " & ex.Message)
        End Try
    End Sub

    ' Event to handle adding a new tenant
    Private Sub AddTenant_Click(sender As Object, e As EventArgs) Handles AddTenant.Click
        Dim popup As New Pop_Up_Add_Tenant()
        popup.StartPosition = FormStartPosition.CenterParent

        ' Pass the reference of DashboardForm to Pop_Up_Add_Tenant
        popup.DashboardFormInstance = Me.DashboardFormInstance

        ' Show the popup to add a new tenant
        If popup.ShowDialog() = DialogResult.OK Then
            Dim tenantName As String = popup.TenantName
            Dim email As String = popup.Email
            Dim contactNo As String = popup.ContactNo
            Dim unitNo As String = popup.UnitNo
            Dim registrationDate As String = popup.RegistrationDate

            ' Add the tenant to the database
            AddTenantToDatabase(tenantName, email, contactNo, unitNo, registrationDate)

            ' Reload tenants in the ListView
            LoadTenantsFromDatabase()

            If DashboardFormInstance IsNot Nothing Then
                DashboardFormInstance.RefreshDashboardData()
            End If
        End If
    End Sub

    ' Event to handle updating tenant information
    Private Sub Update_Click(sender As Object, e As EventArgs) Handles Update.Click
        If ListView1.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a tenant to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get the selected tenant's ID
        Dim selectedItem As ListViewItem = ListView1.SelectedItems(0)
        Dim tenantId As Integer = Convert.ToInt32(selectedItem.SubItems(0).Text) ' tenant_id is in the first column

        ' Create a popup to update the tenant's information
        Dim popup As New Pop_Up_Update_Tenant_Info()
        popup.StartPosition = FormStartPosition.CenterParent

        ' Populate the popup with the selected tenant's information
        popup.TenantName = selectedItem.SubItems(1).Text
        popup.Email = selectedItem.SubItems(2).Text
        popup.ContactNo = selectedItem.SubItems(3).Text
        popup.UnitNo = selectedItem.SubItems(4).Text
        popup.RegistrationDate = selectedItem.SubItems(5).Text

        ' Handle the update logic
        If popup.ShowDialog() = DialogResult.OK Then
            ' Update the tenant in the database
            UpdateTenantInDatabase(tenantId, popup.TenantName, popup.Email, popup.ContactNo, popup.UnitNo, popup.RegistrationDate)

            ' Update the ListView
            selectedItem.SubItems(1).Text = popup.TenantName
            selectedItem.SubItems(2).Text = popup.Email
            selectedItem.SubItems(3).Text = popup.ContactNo
            selectedItem.SubItems(4).Text = popup.UnitNo
            selectedItem.SubItems(5).Text = popup.RegistrationDate
        End If
    End Sub

    ' Event to handle deleting a tenant
    Private Sub Delete_Click(sender As Object, e As EventArgs) Handles Delete.Click
        If ListView1.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a tenant to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get the selected tenant's ID
        Dim selectedItem As ListViewItem = ListView1.SelectedItems(0)
        Dim tenantId As Integer = Convert.ToInt32(selectedItem.SubItems(0).Text) ' tenant_id is in the first column

        ' Confirm deletion
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this tenant?", "Delete Tenant", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' Delete tenant from the database
            DeleteTenantFromDatabase(tenantId)

            ' Remove tenant from the ListView
            ListView1.Items.Remove(selectedItem)

            If DashboardFormInstance IsNot Nothing Then
                DashboardFormInstance.RefreshDashboardData() ' Refresh the dashboard after deletion
            End If
        End If
    End Sub

    ' Event to handle form load and populate ListView
    Private Sub TenantsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Clear()
        ListView1.View = View.Details

        ' Define columns for the ListView
        ListView1.Columns.Add("Tenant ID", 70, HorizontalAlignment.Left)
        ListView1.Columns.Add("Name", 150, HorizontalAlignment.Left)
        ListView1.Columns.Add("Email", 200, HorizontalAlignment.Left)
        ListView1.Columns.Add("Contact No.", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Unit No.", 80, HorizontalAlignment.Left)
        ListView1.Columns.Add("Reg. Date", 100, HorizontalAlignment.Left)

        ' Load tenants from the database
        LoadTenantsFromDatabase()
    End Sub
End Class
