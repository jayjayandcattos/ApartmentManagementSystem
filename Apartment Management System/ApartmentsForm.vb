Imports MySql.Data.MySqlClient

Public Class ApartmentsForm

    Public Property DashboardFormInstance As DashboardForm

    Dim connectionString As String = "Server=localhost;Database=oopapartment;Uid=root"
    Dim connection As New MySqlConnection(connectionString)

    Private Sub OpenConnection()
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
        Catch ex As Exception
            MessageBox.Show("Error connecting to database: " & ex.Message)
        End Try
    End Sub

    ' Method to close the connection
    Private Sub CloseConnection()
        If connection.State = ConnectionState.Open Then
            connection.Close()
        End If
    End Sub

    Private Sub AddApartmentToDatabase(unitNo As String, description As String, price As String, status As String)
        Try
            ' Open the connection
            OpenConnection()

            ' SQL query to insert the new apartment (no need to include unit_id because it's auto-increment)
            Dim query As String = "INSERT INTO tbl_rooms (unit_number, unit_description, unit_price, status) VALUES (@UnitNo, @Description, @Price, @Status)"

            ' MySQL command with parameters to prevent SQL injection
            Dim cmd As New MySqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@UnitNo", unitNo)
            cmd.Parameters.AddWithValue("@Description", description)
            cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(price))  ' Ensure the price is a decimal
            cmd.Parameters.AddWithValue("@Status", status)

            ' Execute the command
            cmd.ExecuteNonQuery()

            ' Close the connection
            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error inserting apartment into database: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadApartmentsFromDatabase()
        Try
            ' Open the connection
            OpenConnection()

            ' SQL query to select all apartments
            Dim query As String = "SELECT * FROM tbl_rooms"

            ' MySQL command to fetch data
            Dim cmd As New MySqlCommand(query, connection)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            ' Clear the current ListView
            ListView1.Items.Clear()

            ' Read the data and populate the ListView
            While reader.Read()
                Dim item As New ListViewItem(reader("unit_id").ToString()) ' unit_id is the primary key
                item.SubItems.Add(reader("unit_number").ToString()) ' unit_number
                item.SubItems.Add(reader("unit_description").ToString()) ' unit_description
                item.SubItems.Add(reader("unit_price").ToString()) ' unit_price
                item.SubItems.Add(reader("status").ToString()) ' status
                ListView1.Items.Add(item)
            End While

            ' Close the connection and the reader
            reader.Close()
            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error loading apartments from database: " & ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListView1.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an apartment to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedItem As ListViewItem = ListView1.SelectedItems(0)

        Dim popup As New Pop_Up_Edit_Apartment()
        popup.StartPosition = FormStartPosition.CenterParent

        popup.UnitId = selectedItem.SubItems(0).Text
        popup.UnitNo = selectedItem.SubItems(1).Text
        popup.Description = selectedItem.SubItems(2).Text
        popup.Price = selectedItem.SubItems(3).Text
        popup.Status = If(selectedItem.SubItems.Count > 4, selectedItem.SubItems(4).Text, "Vacant") ' Default to 'Vacant'

        popup.DashboardFormInstance = Me.DashboardFormInstance

        If popup.ShowDialog() = DialogResult.OK Then
            selectedItem.SubItems(0).Text = popup.UnitId
            selectedItem.SubItems(1).Text = popup.UnitNo
            selectedItem.SubItems(2).Text = popup.Description
            selectedItem.SubItems(3).Text = popup.Price
            selectedItem.SubItems(4).Text = popup.Status
        End If
    End Sub

    Private Sub DeleteApartmentFromDatabase(unitId As String)
        Try
            ' Open the connection
            OpenConnection()

            ' SQL query to delete an apartment
            Dim query As String = "DELETE FROM tbl_rooms WHERE unit_id = @UnitId"

            ' MySQL command with parameters
            Dim cmd As New MySqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@UnitId", Convert.ToInt32(unitId)) ' Ensure unitId is treated as an integer

            ' Execute the delete command
            cmd.ExecuteNonQuery()

            ' Close the connection
            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error deleting apartment from database: " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim unitNo As String = TextBox2.Text
        Dim description As String = TextBox3.Text
        Dim price As String = TextBox4.Text

        ' Validate inputs
        If String.IsNullOrWhiteSpace(unitNo) OrElse String.IsNullOrWhiteSpace(description) OrElse
           String.IsNullOrWhiteSpace(price) Then
            MessageBox.Show("Please fill all the fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Add a new apartment (no need to pass unitId as it's auto-incremented by MySQL)
        AddApartmentToDatabase(unitNo, description, price, "Vacant")

        ' Reload apartments to refresh the ListView
        LoadApartmentsFromDatabase()

        ' Clear inputs for the next entry
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox2.Focus()

        If DashboardFormInstance IsNot Nothing Then
            DashboardFormInstance.RefreshDashboardData()
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox2.Clear()  ' Clear Unit Number
        TextBox3.Clear()  ' Clear Description
        TextBox4.Clear()  ' Clear Price

        ' Optionally, set focus to the first textbox to start fresh
        TextBox2.Focus()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Check if any item is selected in the ListView
        If ListView1.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an apartment to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get the selected apartment's unit_id
        Dim selectedItem As ListViewItem = ListView1.SelectedItems(0)
        Dim unitId As String = selectedItem.SubItems(0).Text ' unit_id is in the first column

        ' Ask the user for confirmation before deleting
        Dim confirmResult As DialogResult = MessageBox.Show("Are you sure you want to delete this apartment?", "Delete Apartment", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirmResult = DialogResult.Yes Then
            ' Delete the apartment from the database
            DeleteApartmentFromDatabase(unitId)

            ' Remove the apartment from the ListView
            ListView1.Items.Remove(selectedItem)

            MessageBox.Show("Apartment deleted successfully.")
        End If

        If DashboardFormInstance IsNot Nothing Then
            DashboardFormInstance.RefreshDashboardData()
        End If

    End Sub

    Private Sub ApartmentsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Clear()
        ListView1.View = View.Details

        ' Add columns to the ListView
        ListView1.Columns.Add("Unit ID", 70)
        ListView1.Columns.Add("Unit No", 70)
        ListView1.Columns.Add("Description", 120)
        ListView1.Columns.Add("Price", 100)
        ListView1.Columns.Add("Status", 80)

        ' Load apartment data immediately after the form loads
        LoadApartmentsFromDatabase()
    End Sub

End Class
