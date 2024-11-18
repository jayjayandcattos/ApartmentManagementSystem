Imports MySql.Data.MySqlClient

Public Class Pop_Up_Edit_Apartment
    Public Property UnitId As String
    Public Property UnitNo As String
    Public Property Description As String
    Public Property Price As String
    Public Property Status As String


    Public Property DashboardFormInstance As DashboardForm

    ' Connection string for database connection
    Dim connectionString As String = "Server=localhost;Database=oopapartment;Uid=root;Pwd=123456"
    Dim connection As New MySqlConnection(connectionString)

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles Save.Click
        ' Validate inputs
        If String.IsNullOrWhiteSpace(TextBox2.Text) OrElse String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) OrElse ComboBox1.SelectedItem Is Nothing Then
            MessageBox.Show("Please fill all the fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Update the apartment details in the database
        UnitNo = TextBox2.Text
        Description = TextBox3.Text
        Price = TextBox4.Text
        Status = ComboBox1.SelectedItem.ToString()

        ' Update database with the new values
        UpdateApartmentInDatabase()

        If DashboardFormInstance IsNot Nothing Then
            DashboardFormInstance.RefreshDashboardData()
        End If

        ' Close the form with OK result
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Pop_Up_Edit_Apartment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Pre-fill the fields with the existing data
        TextBox2.Text = UnitNo
        TextBox3.Text = Description
        TextBox4.Text = Price

        ' Set the ComboBox for the status (Vacant or Occupied)
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(New String() {"Vacant", "Occupied"})
        ComboBox1.SelectedItem = Status
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        ' Close the form without saving changes
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

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

    ' Method to close the database connection
    Private Sub CloseConnection()
        If connection.State = ConnectionState.Open Then
            connection.Close()
        End If
    End Sub

    ' Method to update the apartment in the database
    Private Sub UpdateApartmentInDatabase()
        Try
            OpenConnection()

            ' SQL query to update the apartment details
            Dim query As String = "UPDATE tbl_rooms SET unit_number = @UnitNo, unit_description = @Description, unit_price = @Price, status = @Status WHERE unit_id = @UnitId"

            ' MySQL command with parameters to prevent SQL injection
            Dim cmd As New MySqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@UnitId", UnitId) ' Use the UnitId property
            cmd.Parameters.AddWithValue("@UnitNo", UnitNo)
            cmd.Parameters.AddWithValue("@Description", Description)
            cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(Price)) ' Ensure the price is decimal
            cmd.Parameters.AddWithValue("@Status", Status)

            ' Execute the update command
            cmd.ExecuteNonQuery()

            ' Close the connection
            CloseConnection()

            MessageBox.Show("Apartment updated successfully.")

        Catch ex As Exception
            MessageBox.Show("Error updating apartment: " & ex.Message)
        End Try
    End Sub

End Class
