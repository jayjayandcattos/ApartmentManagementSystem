Imports MySql.Data.MySqlClient

Public Class PaymentsForm

    Dim connection As New MySqlConnection("Server=localhost;Database=oopapartment;Uid=root")

    ' Method to open the database connection
    Private Sub OpenConnection()
        Try
            ' Open the connection if it's not already open
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
        Catch ex As Exception
            ' Show error message if connection fails
            MessageBox.Show("Error connecting to database: " & ex.Message)
        End Try
    End Sub

    ' Method to close the database connection
    Private Sub CloseConnection()
        If connection.State = ConnectionState.Open Then
            connection.Close()
        End If
    End Sub

    ' Method to load payment data for tenants
    Private Sub LoadPaymentsForTenants()
        Try
            ' Open the connection
            OpenConnection()

            ' SQL query to retrieve tenant payment information, including tenant details and payment status
            Dim query As String = "SELECT t.tenant_id, t.tenants_name, t.registration_date, " &
                                  "IFNULL(p.monthly_rate, 0) AS monthly_rate, IFNULL(p.balance, 0) AS balance " &
                                  "FROM tbl_tenants t LEFT JOIN tbl_payments p ON t.tenant_id = p.tenant_id"


            Dim cmd As New MySqlCommand(query, connection)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            ' Clear current data in the ListView
            ListView1.Items.Clear()

            ' Populate the ListView with tenant payment data
            While reader.Read()
                Dim item As New ListViewItem(reader("tenant_id").ToString()) ' Tenant ID
                item.SubItems.Add(reader("tenants_name").ToString()) ' Tenant Name
                item.SubItems.Add(Convert.ToDateTime(reader("registration_date")).ToString("yyyy-MM-dd")) ' Registration Date
                item.SubItems.Add(Convert.ToDecimal(reader("monthly_rate")).ToString("C")) ' Monthly Rate (formatted as currency)
                item.SubItems.Add(Convert.ToDecimal(reader("balance")).ToString("C")) ' Outstanding Balance (formatted as currency)

                ListView1.Items.Add(item)
            End While

            reader.Close()
            CloseConnection()

        Catch ex As Exception
            ' Show an error message if an exception occurs
            MessageBox.Show("Error loading payments: " & ex.Message)
        End Try
    End Sub

    ' Handle the button click event for taking a fee (popup window)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim popup As New Pop_Up_Take_Fee()

        popup.StartPosition = FormStartPosition.CenterParent
        popup.ShowDialog()
    End Sub

    ' Handle form load event to initialize the ListView and load payment data
    Private Sub PaymentsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Clear the ListView
        ListView1.Clear()

        ' Set ListView view to Details
        ListView1.View = View.Details

        ' Define columns for the ListView
        ListView1.Columns.Add("#", 50, HorizontalAlignment.Left)
        ListView1.Columns.Add("Name", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Date Registered", 150, HorizontalAlignment.Left)
        ListView1.Columns.Add("Monthly Rate", 120, HorizontalAlignment.Left)
        ListView1.Columns.Add("Outstanding Balance", 200, HorizontalAlignment.Left)

        ' Load payment data linked to tenants
        LoadPaymentsForTenants()
    End Sub

    ' Paint event handler for Panel2 (empty, no implementation needed here)
    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint
        ' Empty event handler
    End Sub

End Class
